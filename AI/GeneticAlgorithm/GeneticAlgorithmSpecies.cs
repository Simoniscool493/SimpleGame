using SimpleGame.DataPayloads;
using SimpleGame.Deciders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.AI.GeneticAlgorithm
{
    class GeneticAlgorithmSpecies : MatrixDecider
    {
        public bool IsScored = false;
        public DecisionMatrix Matrix => matrix;
        public int Score;

        public GeneticAlgorithmSpecies(DecisionMatrix matrix)
            : base(matrix)
        {
        }

        public override string ToString()
        {
            return Score.ToString();
        }

        public static GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species1,GeneticAlgorithmSpecies species2,double mutationRate,Random r)
        {
            var childMatrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();

            foreach(var key in species1.Matrix.GetKeys())
            {
                if(r.Next() >mutationRate)
                {
                }
            }
        }
    }
}
