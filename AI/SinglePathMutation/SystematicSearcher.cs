using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.AI.SinglePathMutation
{
    class SystematicSearcher
    {
        private IDiscreteGameManager _game;
        private int _numRandomSeeds;
        private int _baseRandomSeed;
        private Action<string> _log;

        public SystematicSearcher(IDiscreteGameManager game,int numRandomSeeds,int baseRandomSeed,Action<string> log)
        {
            _game = game;
            _numRandomSeeds = numRandomSeeds;
            _baseRandomSeed = baseRandomSeed;
            _log = log;
        }

        public DeciderSpecies SystematicSearch(DeciderSpecies oldBestSpecies,Random r,bool shouldLog)
        {
            if(shouldLog)
            {
                _log("Checking single heursitcs one-by-one...");
            }

            var listToTest = new List<HeuristicBuildingDecider> { (HeuristicBuildingDecider)oldBestSpecies.BaseDecider };

            listToTest = GetListOfChangedDeciders(listToTest);
            var betterScores = new Dictionary<HeuristicBuildingDecider, int>();

            foreach (var decider in listToTest)
            {
                var score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, decider, _numRandomSeeds,_baseRandomSeed);

                if (score > oldBestSpecies.Score)
                {
                    betterScores[decider] = score;
                }
            }

            if (betterScores.Any())
            {
                var bestScore = betterScores.Max(kvp => kvp.Value);

                var bestScoresByComplexity = betterScores.Where(kvp => kvp.Value == bestScore).OrderBy(kvp => kvp.Key.TotalComplexity);
                var newDecider = bestScoresByComplexity.First().Key;

                var newBestSpecies = new DeciderSpecies(newDecider);
                newBestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, newDecider, _numRandomSeeds,_baseRandomSeed);

                if(shouldLog)
                {
                    _log("New best score found: " + newBestSpecies.Score);
                }
                return newBestSpecies;
            }

            return null;
        }

        private List<HeuristicBuildingDecider> GetListOfChangedDeciders(List<HeuristicBuildingDecider> listOfOriginals)
        {
            var possibleValues = ((DiscreteDataPayloadInfo)(listOfOriginals.First().IOInfo.OutputInfo)).AllPossibleValues();
            var output = new List<HeuristicBuildingDecider>();

            foreach (var decider in listOfOriginals)
            {
                var usesToIndexesOrdered = GetOrderedUsesToIndexes(new DeciderSpecies(decider));

                for (int i = 0; i < decider.Heuristics.Count; i++)
                {
                    for (int j = 0; j < possibleValues.Count; j++)
                    {
                        var deciderWithOneChangedHeuristic = decider.CloneWithAllHeuristics();
                        deciderWithOneChangedHeuristic.Heuristics.ElementAt(usesToIndexesOrdered.ElementAt(i).Value).ExpectedOutput = possibleValues.ElementAt(j).SingleItem;

                        output.Add(deciderWithOneChangedHeuristic);
                    }
                }
            }

            return output;
        }

        private List<KeyValuePair<int, int>> GetOrderedUsesToIndexes(DeciderSpecies sp)
        {
            SimpleGameTester.SetRandomSuccessTesting(_game, sp, _numRandomSeeds,_baseRandomSeed);
            var baseDecider = ((HeuristicBuildingDecider)sp.BaseDecider);

            List<KeyValuePair<int, int>> usesToIndexes = new List<KeyValuePair<int, int>>();

            for (int h = 0; h < baseDecider.Heuristics.Count; h++)
            {
                usesToIndexes.Add(new KeyValuePair<int, int>(baseDecider.Heuristics.ElementAt(h).UseCount, h));
            }

            var usesToIndexesOrdered = usesToIndexes.OrderBy(uti => uti.Key).Reverse().ToList();

            return usesToIndexesOrdered;
        }
    }
}
