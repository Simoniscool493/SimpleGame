﻿using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads;
using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.GeneticAlgorithm
{
    class Generation
    {
        private DiscreteDataPayloadInfo _inputInfo;
        private DiscreteDataPayloadInfo _outputInfo;

        private Random _r = new Random();
        private int _maxSize;
        private double _mutationRate;

        private List<GeneticAlgorithmSpecies> _theGeneration = new List<GeneticAlgorithmSpecies>();

        public Generation(int maxSize,double mutationRate)
        {
            _maxSize = maxSize;
            _mutationRate = mutationRate;
        }

        public void Add(DecisionMatrix matrix)
        {
            _theGeneration.Add(new GeneticAlgorithmSpecies(matrix));
        }

        public void ScoreGeneration(IDiscreteGame r,IGameState s)
        {
            foreach (var matrix in _theGeneration)
            {
                if(!matrix.IsScored)
                {
                    var score = r.Score(matrix, s);
                    matrix.Score = score;
                    matrix.IsScored = true;
                }
            }
        }

        public GeneticAlgorithmSpecies GetBestSpecies()
        {
            int highestScore = 0;
            GeneticAlgorithmSpecies best = _theGeneration[0];

            foreach(GeneticAlgorithmSpecies s in _theGeneration)
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
            var sortredGen = _theGeneration.OrderBy(g => g.Score);
            var maricesToKill = sortredGen.Take(numToKill);
            _theGeneration.RemoveAll(g=>maricesToKill.Contains(g));
        }

        public void Multiply()
        {
            while(_theGeneration.Count()<_maxSize)
            {
                var newSpecies = GetNewSpeciesFromSpeciesInThisGeneration();
                _theGeneration.Add(newSpecies);
            }
        }

        private GeneticAlgorithmSpecies GetNewSpeciesFromSpeciesInThisGeneration()
        {
            var parent1 = _theGeneration[_r.Next(0, _theGeneration.Count())];
            var parent2 = _theGeneration[_r.Next(0, _theGeneration.Count())];

            var child = GeneticAlgorithmSpecies.Cross(parent1, parent2, _mutationRate);


        }
    }
}
