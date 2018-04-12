using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Permutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.Discrete.DecisionMatrix
{
    static class DecisionMatrixFactory
    {
        public static IDecisionMatrix MatrixCrossMutate(IDecisionMatrix matrix1, IDecisionMatrix matrix2, double mutationRate, Random r)
        {
            if (!((matrix1.IOInfo.InputInfo == matrix2.IOInfo.InputInfo) && matrix1.IOInfo.OutputInfo == matrix2.IOInfo.OutputInfo))
            {
                throw new Exception("Cannot cross matrixes of two different payload types");
            }

            var childMatrix = new Dictionary<IDiscreteDataPayload, IDiscreteDataPayload>();

            foreach (var key in matrix1.GetKeys())
            {
                if (r.NextDouble() < mutationRate)
                {
                    childMatrix[key] = matrix1.IOInfo.OutputInfo.GetRandomInstance(r);
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

                return new LazyDecisionMatrix(childMatrix, matrix1.IOInfo);
            }

            return new BasicDecisionMatrix(childMatrix, matrix1.IOInfo);
        }

        public static IDecisionMatrix GetRandomIOMapping(Random r, DiscreteIOInfo IOInfo)
        {
            throw new NotImplementedException();

            /*var permutator = new DiscreteDataPayloadPermutator((DiscreteDataPayloadInfo)(IOInfo.InputInfo));
            var matrix = new Dictionary<IDiscreteDataPayload, IDiscreteDataPayload>();
            var isRunning = true;

            while (isRunning)
            {
                var input = permutator.GetAsEnum(IOInfo.InputInfo.GetType());
                var randomOutput = IOInfo.OutputInfo.GetRandomInstance(r);
                matrix[input] = randomOutput;

                isRunning = permutator.TryIncrement(0);
            }

            return new BasicDecisionMatrix(matrix, IOInfo);*/
        }

        public static IDecisionMatrix GetLazyIOMapping(Random r, DiscreteIOInfo IOInfo)
        {
            var matrix = new Dictionary<IDiscreteDataPayload, IDiscreteDataPayload>();
            return new LazyDecisionMatrix(matrix, IOInfo);
        }
    }
}
