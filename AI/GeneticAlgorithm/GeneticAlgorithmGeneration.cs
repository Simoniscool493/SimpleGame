using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Deciders.Discrete;
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

        public IEnumerable<GeneticAlgorithmSpecies> GenerationWithoutWorst
        {
            get
            {
                var sortredGen = ThisGeneration.OrderBy(species => species.Score).ToList();
                sortredGen.Remove(sortredGen.ElementAt(0));
                return sortredGen;
            }

        }


        public GeneticAlgorithmSpecies BestSpecies
        {
            get
            {
                int highestScore = 0;
                GeneticAlgorithmSpecies best = ThisGeneration[0];

                foreach (GeneticAlgorithmSpecies s in ThisGeneration)
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

        public List<GeneticAlgorithmSpecies> ThisGeneration = new List<GeneticAlgorithmSpecies>();

        public Generation(int maxSize, double mutationRate, Random r)
        {
            _maxSize = maxSize;
            MutationRate = mutationRate;
            _r = r;
        }

        public void PopulateWithRandoms(DiscreteIOInfo gameIOInfo, DeciderType deciderType)
        {
            while (ThisGeneration.Count < _maxSize)
            {
                IDiscreteDecider startingDecider = null;

                switch (deciderType)
                {
                    case DeciderType.Matrix:
                        startingDecider = DecisionMatrix.GetRandomIOMapping(_r, gameIOInfo);
                        break;
                    case DeciderType.LazyMatrix:
                        startingDecider = DecisionMatrix.GetLazyIOMapping(_r, gameIOInfo);
                        break;
                    case DeciderType.Random:
                        startingDecider = new RandomDiscreteDecider(_r, gameIOInfo);
                        break;
                }

                Add(new GeneticAlgorithmSpecies(startingDecider, deciderType));
            }
        }

        public void ScoreGeneration(IDiscreteGameManager game, IDiscreteGameState state, int numOfTimesToTestASpecies)
        {
            foreach (var species in ThisGeneration)
            {
                if (!species.IsScored)
                {
                    if (numOfTimesToTestASpecies == 1)
                    {
                        species.Score = game.Score(species, state);

                        if(species.Score>2070)
                        {

                        }

                        species.IsScored = true;
                        state.Reset();
                    }
                    else
                    {
                        var scoreList = new List<int>();

                        for (int i = 0; i < numOfTimesToTestASpecies; i++)
                        {
                            scoreList.Add(game.Score(species, state));
                            state.Reset();
                        }

                        species.Score = scoreList.Sum() / numOfTimesToTestASpecies;
                    }
                }
            }

        }

        public void Kill(int numToKill)
        {
            var sortredGen = ThisGeneration.OrderBy(species => species.Score);

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

        private GeneticAlgorithmSpecies GetNewSpeciesFromSpeciesInThisGeneration()
        {
            //var parent1 = ThisGeneration[_r.Next(0, ThisGeneration.Count())];
            //var parent2 = ThisGeneration[_r.Next(0, ThisGeneration.Count())];

            var parent1 = GetSpeciesBasedOnScoreWeighing(HighestOrLowest.Highest);
            var parent2 = GetSpeciesBasedOnScoreWeighing(HighestOrLowest.Highest);

            if(MutationRate < 0)
            {
                return parent1.Cross(parent2, _r.NextDouble(), _r);
            }
            else
            {
                return parent1.Cross(parent2, MutationRate, _r);
            }
        }

        private GeneticAlgorithmSpecies GetSpeciesBasedOnScoreWeighing(HighestOrLowest highestOrLowest)
        {
            // totalWeight is the sum of all brokers' weight
            var scoredSpecies = ThisGeneration.Where(s => s.IsScored);
            int randomNumber = _r.Next(0, scoredSpecies.Select(s => s.Score).Sum());

            GeneticAlgorithmSpecies selectedSpecies = null;
            foreach (GeneticAlgorithmSpecies sp in scoredSpecies)
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

        private void Add(GeneticAlgorithmSpecies species)
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
