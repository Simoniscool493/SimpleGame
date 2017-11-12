using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Deciders.Discrete.DecisionMatrix;
using SimpleGame.Games;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class Generation
    {
        private Random _r;
        private int _maxSize;
        public double MutationRate;

        public int AverageScore => (int)Math.Round(GenerationWithoutWorst.Where(sp => sp.IsScored).Select((sp) => sp.Score).Average());
        public int WorstScore => (GenerationWithoutWorst.Where(sp => sp.IsScored).Select((sp) => sp.Score).Min());

        public IEnumerable<DeciderSpecies> GenerationWithoutWorst
        {
            get
            {
                var sortredGen = ThisGeneration.OrderBy(species => species.Score).ToList();
                sortredGen.Remove(sortredGen.ElementAt(0));
                return sortredGen;
            }

        }


        public DeciderSpecies BestSpecies
        {
            get
            {
                int highestScore = 0;
                DeciderSpecies best = ThisGeneration[0];

                foreach (DeciderSpecies s in ThisGeneration)
                {
                    if (s.Score > highestScore)
                    {
                        highestScore = s.Score;
                        best = s;
                    }
                }

                return best;
            }
        }

        public List<DeciderSpecies> ThisGeneration = new List<DeciderSpecies>();

        public Generation(int maxSize, double mutationRate, Random r)
        {
            _maxSize = maxSize;
            MutationRate = mutationRate;
            _r = r;
        }

        public void PopulateWithRandoms(DiscreteIOInfo gameIOInfo, DiscreteDeciderType deciderType)
        {
            while (ThisGeneration.Count < _maxSize)
            {
                IDiscreteDecider startingDecider = null;

                switch (deciderType)
                {
                    case DiscreteDeciderType.BasicMatrix:
                        startingDecider = DecisionMatrixFactory.GetRandomIOMapping(_r, gameIOInfo);
                        break;
                    case DiscreteDeciderType.LazyMatrix:
                        startingDecider = DecisionMatrixFactory.GetLazyIOMapping(_r, gameIOInfo);
                        break;
                    case DiscreteDeciderType.Random:
                        startingDecider = new RandomDiscreteDecider(_r, gameIOInfo);
                        break;
                    case DiscreteDeciderType.HeuristicBuilder:
                        startingDecider = new HeuristicBuildingDecider(_r, gameIOInfo);
                        break;
                }

                Add(new DeciderSpecies(startingDecider));
            }
        }

        public void ScoreGeneration(IDiscreteGameManager game, IDiscreteGameState state, int numOfTimesToTestASpecies)
        {
            if(numOfTimesToTestASpecies == 1)
            {
                foreach (var species in ThisGeneration)
                {
                    if (!species.IsScored)
                    {
                        species.Score = game.Score(species, state);
                        state.Reset();
                        species.IsScored = true;
                    }
                }
            }
            else
            {
                foreach (var species in ThisGeneration)
                {
                    var scoreList = new List<int>();

                    for (int i = 0; i < numOfTimesToTestASpecies; i++)
                    {
                        scoreList.Add(game.Score(species, state));
                        state.Reset();
                    }

                    species.Score = scoreList.Sum() / numOfTimesToTestASpecies;
                    species.IsScored = true;
                }
            }
        }

        public void Kill(int numToKill)
        {
            var sortredGen = ThisGeneration.OrderBy(species => species.Score).ToArray();

            for(int i=0;i<numToKill;i++)
            {
                ThisGeneration.Remove(sortredGen.ElementAt(i));
            }
        }

        public void Multiply()
        {
            while (ThisGeneration.Count() < _maxSize)
            {
                var newSpecies = GetNewSpeciesFromSpeciesInThisGeneration();
                ThisGeneration.Add(newSpecies);
            }
        }

        private DeciderSpecies GetNewSpeciesFromSpeciesInThisGeneration()
        {
            var parent1 = GetSpeciesBasedOnScoreWeighing(HighestOrLowest.Highest);
            var parent2 = GetSpeciesBasedOnScoreWeighing(HighestOrLowest.Highest);

            if(MutationRate < 0)
            {
                return (DeciderSpecies)(parent1.CrossMutate(parent2, _r.NextDouble(), _r));
            }
            else
            {
                return (DeciderSpecies)(parent1.CrossMutate(parent2, MutationRate, _r));
            }
        }

        private DeciderSpecies GetSpeciesBasedOnScoreWeighing(HighestOrLowest highestOrLowest)
        {
            var scoredSpecies = ThisGeneration.Where(s => s.IsScored);
            int totalWeights = scoredSpecies.Select(s => s.Score).Sum();
            int randomNumber = _r.Next(0, totalWeights);

            if(totalWeights==0)
            {
                return ThisGeneration.First();
            }

            DeciderSpecies selectedSpecies = null;
            foreach (DeciderSpecies sp in scoredSpecies)
            {
                if (randomNumber < sp.Score)
                {
                    selectedSpecies = sp;
                    break;
                }

                randomNumber = randomNumber - sp.Score;
            }

            return selectedSpecies;
        }

        private void Add(DeciderSpecies species)
        {
            ThisGeneration.Add(species);
        }

        private enum HighestOrLowest
        {
            Highest,
            Lowest
        }
    }
}
