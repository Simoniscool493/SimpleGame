using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class DecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();

        private Dictionary<DiscreteDataPayload,DiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }

        public DecisionMatrix(Dictionary<DiscreteDataPayload, DiscreteDataPayload> matrix,DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = IOInfo;

        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return _theMatrix[input];
        }

        public Dictionary<DiscreteDataPayload,DiscreteDataPayload>.KeyCollection GetKeys()
        {
            return _theMatrix.Keys;
        }

        public bool ContainsKey(DiscreteDataPayload d)
        {
            return _theMatrix.ContainsKey(d);
        }

        public static IDecisionMatrix GetRandomIOMapping(Random r,DiscreteIOInfo IOInfo)
        {
            var permutator = new PermutationMechanism(IOInfo.InputInfo);
            var matrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();
            var isRunning = true;

            while(isRunning)
            {
                var input = permutator.GetAsEnum(IOInfo.InputInfo.PayloadType);
                var randomOutput = IOInfo.OutputInfo.GetRandomInstance(r);
                matrix[input] = randomOutput;

                isRunning = permutator.TryIncrement(0);
            }

            return new DecisionMatrix(matrix,IOInfo);
        }

        public static IDecisionMatrix GetLazyIOMapping(Random r, DiscreteIOInfo IOInfo)
        {
            var matrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();
            return new LazyDecisionMatrix(matrix,IOInfo);
        }

        class PermutationMechanism
        {
            private Array _enumValues;
            public int[] PermutationCounter;
            public int NumberOfEnumValues;

            public PermutationMechanism(DiscreteDataPayloadInfo enumInfo)
            {
                _enumValues = enumInfo.PayloadType.GetEnumValues();
                PermutationCounter = new int[enumInfo.PayloadLength];
                NumberOfEnumValues = enumInfo.PayloadType.GetEnumValues().Length;
            }

            public DiscreteDataPayload GetAsEnum(Type enumType)
            {
                List<int> output = new List<int>();

                for(int i=0;i<PermutationCounter.Length;i++)
                {
                    var enumNumber = PermutationCounter[i];
                    var value = (int)_enumValues.GetValue(enumNumber);
                    output.Add(value);
                }

                return new DiscreteDataPayload(enumType, output.ToArray());
            }

            public bool TryIncrement(int orderOfMagnitide)
            {
                if(orderOfMagnitide>=PermutationCounter.Length)
                {
                    return false;
                }

                PermutationCounter[orderOfMagnitide]++;
                if(PermutationCounter[orderOfMagnitide]>=NumberOfEnumValues)
                {
                    PermutationCounter[orderOfMagnitide] = 0;
                    return TryIncrement(orderOfMagnitide + 1);
                }

                return true;
            }
        }

    }
}
