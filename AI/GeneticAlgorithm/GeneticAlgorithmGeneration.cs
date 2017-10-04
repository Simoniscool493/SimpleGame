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
    class Generation
    {
        private Random _r;
        private int _maxSize;
        private double _mutationRate;

        private List<GeneticAlgorithmSpecies> _thisGeneration = new List<GeneticAlgorithmSpecies>();

        public Generation(int maxSize,double mutationRate,Random r)
        {
            _maxSize = maxSize;
            _mutationRate = mutationRate;
            _r = r;
        }

        public void PopulateWithRandoms(DiscreteIOInfo gameIOInfo,DeciderType deciderType)
        {
            while(_thisGeneration.Count < _maxSize)
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
            foreach (var species in _thisGeneration)
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

            return parent1.Cross(parent2, _mutationRate,_r);
        }

        private void Add(GeneticAlgorithmSpecies species)
        {
            _thisGeneration.Add(species);
        }
    }
}
