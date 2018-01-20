using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Permutation
{
    class DiscreteDataPayloadPermutator : PermutationMechanism
    {
        public int NumberOfEnumValues;
        private Array _enumValues;

        public override int GetNumberOfValues(int currentPlaceInList)
        {
            return NumberOfEnumValues;
        }

        public DiscreteDataPayloadPermutator(DiscreteDataPayloadInfo enumInfo)
        {
            throw new NotImplementedException();

            /*
            _enumValues = enumInfo.PayloadType.GetEnumValues();
            PermutationCounter = new int[enumInfo.PayloadLength];
            NumberOfEnumValues = enumInfo.PayloadType.GetEnumValues().Length;
            */
        }

        public DiscreteDataPayload GetAsEnum(Type enumType)
        {
            List<int> output = new List<int>();

            for (int i = 0; i < PermutationCounter.Length; i++)
            {
                var enumNumber = PermutationCounter[i];
                var value = (int)_enumValues.GetValue(enumNumber);
                output.Add(value);
            }

            return new DiscreteDataPayload(output.ToArray());
        }
    }
}
