using SimpleGame.Deciders;
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
        private Random _r = new Random();
        private int _maxSize;
        private double _mutationRate;

        public class Species
        {
            public bool IsScored = false;
            public DecisionMatrix Matrix;
            public int Score;

            public Species(DecisionMatrix matrix)
            {
                Matrix = matrix;
            }

            public override string ToString()
            {
                return Score.ToString();
            }
        }

        private List<Species> _theGeneration = new List<Species>();

        public Generation(int maxSize,double mutationRate)
        {
            _maxSize = maxSize;
            _mutationRate = mutationRate;
        }

        public void Add(DecisionMatrix matrix)
        {
            _theGeneration.Add(new Species(matrix));
        }

        public void ScoreGeneration(FoodEatingGameRunner r,FoodEatingGameBoard b)
        {
            foreach (var matrix in _theGeneration)
            {
                if(!matrix.IsScored)
                {
                    var score = r.RunPlayerOnBoard(new AiGridPlayer(new MatrixDecider(matrix.Matrix)), b, 0, 0);
                    matrix.Score = score;
                    matrix.IsScored = true;
                }
            }
        }

        public void RunSample(FoodEatingGameBoard b,int timerLength)
        {
            new FoodEatingGameRunner(false, true, timerLength).RunPlayerOnBoard(new AiGridPlayer(new MatrixDecider(GetBestSpecies().Matrix)),b,0,0);
        }

        public Species GetBestSpecies()
        {
            int highestScore = 0;
            Species best = _theGeneration[0];

            foreach(Species s in _theGeneration)
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
                var newSpecies = GetNewSpecies();
                _theGeneration.Add(newSpecies);
            }
        }

        private Species GetNewSpecies()
        {
            var parent1 = _theGeneration[_r.Next(0, _theGeneration.Count())];
            var parent2 = _theGeneration[_r.Next(0, _theGeneration.Count())];
            var child = new Dictionary<Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>, Direction>();

            var itemsAtPoint = typeof(ItemAtPoint).GetEnumValues();
            var numDirections = typeof(Direction).GetEnumValues().Length;

            for (int a = 0; a < itemsAtPoint.Length; a++)
            {
                for (int b = 0; b < itemsAtPoint.Length; b++)
                {
                    for (int c = 0; c < itemsAtPoint.Length; c++)
                    {
                        for (int d = 0; d < itemsAtPoint.Length; d++)
                        {
                            if(_r.Next()>(1-_mutationRate))
                            {
                                child[new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d)] = (Direction)_r.Next(0, numDirections);
                            }
                            else if(_r.Next()>0.5)
                            {
                                child[new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d)] = parent1.Matrix.Get(new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d));
                            }
                            else
                            {
                                child[new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d)] = parent2.Matrix.Get(new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d));
                            }
                        }
                    }
                }
            }

            return new Species(new DecisionMatrix(child));
        }
    }
}
