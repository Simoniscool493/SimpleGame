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
        private double _mutationRate;

        public List<GeneticAlgorithmSpecies> ThisGeneration = new List<GeneticAlgorithmSpecies>();

        public Generation(int maxSize,double mutationRate,Random r)
        {
            _maxSize = maxSize;
            _mutationRate = mutationRate;
            _r = r;
        }

        public void PopulateWithRandoms(DiscreteIOInfo gameIOInfo,DeciderType deciderType)
        {
            while(ThisGeneration.Count < _maxSize)
            {
                IDiscreteDecider startingDecider = null;

                switch(deciderType)
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

                Add(new GeneticAlgorithmSpecies(startingDecider,deciderType));
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

        public GeneticAlgorithmSpecies GetBestSpecies()
        {
            int highestScore = 0;
            GeneticAlgorithmSpecies best = ThisGeneration[0];

            foreach(GeneticAlgorithmSpecies s in ThisGeneration)
            {
                if(s.Score>highestScore)
                {
                    highestScore = s.Score;
                    best = s;
                }
            }

            return best;
        }

        public void Kill(int numToKill)
        {
            var sortredGen = ThisGeneration.OrderBy(species => species.Score);
            var lisfOfSpeciesToKill = sortredGen.Take(numToKill);
            ThisGeneration.RemoveAll(species => lisfOfSpeciesToKill.Contains(species));
        }

        public void Multiply()
        {
            while(ThisGeneration.Count()<_maxSize)
            {
                var newSpecies = GetNewSpeciesFromSpeciesInThisGeneration();
                ThisGeneration.Add(newSpecies);
            }
        }

        public int GetAverageScore()
        {
            return (int)Math.Round(ThisGeneration.Where(sp=>sp.IsScored).Select((sp) => sp.Score).Average());
        }

        private GeneticAlgorithmSpecies GetNewSpeciesFromSpeciesInThisGeneration()
        {
            var parent1 = ThisGeneration[_r.Next(0, ThisGeneration.Count())];
            var parent2 = ThisGeneration[_r.Next(0, ThisGeneration.Count())];

            return parent1.Cross(parent2, _mutationRate,_r);
        }

        private void Add(GeneticAlgorithmSpecies species)
        {
            ThisGeneration.Add(species);
        }
    }
}
