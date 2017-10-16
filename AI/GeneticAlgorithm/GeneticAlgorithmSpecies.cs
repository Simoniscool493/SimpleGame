using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.AI.GeneticAlgorithm
{
    [Serializable()]
    public class GeneticAlgorithmSpecies : IDiscreteDecider
    {
        public bool IsScored = false;
        public IDiscreteDecider BaseDecider;
        public int Score;

        private DeciderType _deciderType;

        public DiscreteIOInfo IOInfo => BaseDecider.IOInfo;

        public GeneticAlgorithmSpecies(IDiscreteDecider matrix,DeciderType deciderType)
        {
            BaseDecider = matrix;
            _deciderType = deciderType;
        }

        public override string ToString()
        {
            return Score.ToString();
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2,double mutationRate,Random r)
        {
            if ((_deciderType == DeciderType.Matrix)||(_deciderType == DeciderType.LazyMatrix))
            {
                return MatrixCross((IDecisionMatrix)BaseDecider, (IDecisionMatrix)(species2.BaseDecider), mutationRate, r);
            }
            else if(_deciderType == DeciderType.Random)
            {
                return new GeneticAlgorithmSpecies(new RandomDiscreteDecider(r,IOInfo), DeciderType.Random);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private GeneticAlgorithmSpecies MatrixCross(IDecisionMatrix matrix1, IDecisionMatrix matrix2, double mutationRate, Random r)
        {
            if (!((matrix1.IOInfo.InputInfo.PayloadType == matrix2.IOInfo.InputInfo.PayloadType) && matrix1.IOInfo.OutputInfo.PayloadType == matrix2.IOInfo.OutputInfo.PayloadType))
            {
                throw new Exception();
            }

            var outputValues = matrix1.IOInfo.OutputInfo.PayloadType.GetEnumValues();
            var childMatrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();


            foreach (var key in matrix1.GetKeys())
            {
                if (r.NextDouble() < mutationRate)
                {
                    var value = outputValues.GetValue(r.Next(0, outputValues.Length));
                    var valueAsIntArray = new int[] { ((int)value) };
                    childMatrix[key] = new DiscreteDataPayload(matrix1.IOInfo.OutputInfo.PayloadType, valueAsIntArray);
                }
                else if (r.NextDouble() > 0.5)
                {
                    childMatrix[key] = matrix1.Decide(key);
                }
                else
                {
                    childMatrix[key] = matrix2.Decide(key);
                }
            }

            if (matrix1 is LazyDecisionMatrix)
            {
                foreach (var key in matrix2.GetKeys())
                {
                    if (!matrix1.ContainsKey(key))
                    {
                        childMatrix[key] = matrix2.Decide(key);
                    }
                }

                return new GeneticAlgorithmSpecies(new LazyDecisionMatrix(childMatrix, ((LazyDecisionMatrix)(matrix1)).IOInfo), DeciderType.LazyMatrix);
            }

            return new GeneticAlgorithmSpecies(new DecisionMatrix(childMatrix,IOInfo), DeciderType.Matrix);
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return BaseDecider.Decide(input);
        }

        public void SaveToFile(string fileName)
        {
            Stream saver = File.OpenWrite(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saver, this);
            saver.Close();
        }

    }
}
