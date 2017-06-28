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
            if(!((species1.Matrix.InputType == species2.Matrix.InputType)&& species1.Matrix.OutputType == species2.Matrix.OutputType))
            {
                throw new Exception();
            }

            var outputValues = species1.Matrix.OutputType.GetEnumValues();

            var childMatrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();

            foreach(var key in species1.Matrix.GetKeys())
            {
                if(r.Next() > mutationRate)
                {
                    var value = outputValues.GetValue(r.Next(0, outputValues.Length));
                    var valueAsIntArray = (int[])value;
                    childMatrix[key] = new DiscreteDataPayload(species1.Matrix.OutputType,valueAsIntArray);
                }
                else if(r.Next()>0.5)
                {
                    childMatrix[key] = species1.Matrix.Decide(key);
                }
                else
                {
                    childMatrix[key] = species2.Matrix.Decide(key);
                }
            }

            return new GeneticAlgorithmSpecies(new DecisionMatrix(childMatrix));
        }
    }
}
