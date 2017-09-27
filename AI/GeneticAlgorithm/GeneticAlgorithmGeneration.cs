using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Games;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.AI.GeneticAlgorithm
{
    class Generation
    {
        private Random _r = new Random();
        private int _maxSize;
        private double _mutationRate;

        private List<GeneticAlgorithmSpecies> _thisGeneration = new List<GeneticAlgorithmSpecies>();

        public Generation(int maxSize,double mutationRate)
        {
            _maxSize = maxSize;
            _mutationRate = mutationRate;
        }

        public void PopulateWithRandoms(Random r,DiscreteIOInfo gameIOInfo,bool isLazy)
        {
            while(_thisGeneration.Count < _maxSize)
            {
                IDecisionMatrix randomMatrix = null;

                if(isLazy)
                {
                    randomMatrix = DecisionMatrix.GetLazyIOMapping(r, gameIOInfo);
                }
                else
                {
                    randomMatrix = DecisionMatrix.GetRandomIOMapping(r, gameIOInfo);
                }

                Add(new GeneticAlgorithmSpecies(randomMatrix));
            }
        }

        public void ScoreGeneration(IDiscreteGameManager game,IDiscreteGameState state)
        {
            foreach (var species in _thisGeneration)
            {
                if(!species.IsScored)
                {
                    species.Score = game.Score(species, state);
                    species.IsScored = true;
                    state.Reset();
                }
            }
        }

        public GeneticAlgorithmSpecies GetBestSpecies()
        {
            int highestScore = 0;
            GeneticAlgorithmSpecies best = _thisGeneration[0];

            foreach(GeneticAlgorithmSpecies s in _thisGeneration)
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
            var sortredGen = _thisGeneration.OrderBy(species => species.Score);
            var lisfOfSpeciesToKill = sortredGen.Take(numToKill);
            _thisGeneration.RemoveAll(species => lisfOfSpeciesToKill.Contains(species));
        }

        public void Multiply()
        {
            while(_thisGeneration.Count()<_maxSize)
            {
                var newSpecies = GetNewSpeciesFromSpeciesInThisGeneration();
                _thisGeneration.Add(newSpecies);
            }
        }

        private GeneticAlgorithmSpecies GetNewSpeciesFromSpeciesInThisGeneration()
        {
            var parent1 = _thisGeneration[_r.Next(0, _thisGeneration.Count())];
            var parent2 = _thisGeneration[_r.Next(0, _thisGeneration.Count())];

            return GeneticAlgorithmSpecies.Cross(parent1, parent2, _mutationRate,_r);
        }

        private void Add(GeneticAlgorithmSpecies species)
        {
            _thisGeneration.Add(species);
        }
    }
}
