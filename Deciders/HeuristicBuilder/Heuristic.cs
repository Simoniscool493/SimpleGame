using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using System.Diagnostics;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    public struct Heuristic
    {
        public int PositionInPayload;
        public int ExpectedInput;
        public int ExpectedOutput;

        private DiscreteIOInfo IOInfo;

        public Heuristic(int position,int exInput,int exOutput,DiscreteIOInfo ioInfo)
        {
            PositionInPayload = position;
            ExpectedInput = exInput;
            ExpectedOutput = exOutput;

            IOInfo = ioInfo;
        }

        public void Mutate(Random r)
        {
            var outputEnumTypes = IOInfo.OutputInfo.PayloadType.GetEnumValues();
            var newExpectedOutput = (int)outputEnumTypes.GetValue(r.Next(0, outputEnumTypes.Length));

            ExpectedOutput = newExpectedOutput;
        }

        public static Heuristic CreateRandom(Random r, DiscreteIOInfo ioInfo)
        {
            var position = r.Next(0, ioInfo.InputInfo.PayloadLength);

            var inputEnumTypes = ioInfo.InputInfo.PayloadType.GetEnumValues();
            var outputEnumTypes = ioInfo.OutputInfo.PayloadType.GetEnumValues();

            var expectedInput = (int)inputEnumTypes.GetValue(r.Next(0,inputEnumTypes.Length));
            var expectedOutput = (int)outputEnumTypes.GetValue(r.Next(0, outputEnumTypes.Length));

            return new Heuristic(position, expectedInput, expectedOutput,ioInfo);

        }

        public override string ToString()
        {
            var positionName = IOInfo.InputInfo.PositionNames[PositionInPayload];

            var inputName = Enum.GetName(IOInfo.InputInfo.PayloadType, ExpectedInput);
            var outputName = Enum.GetName(IOInfo.OutputInfo.PayloadType, ExpectedOutput);

            return $"{positionName} = {inputName} => {outputName}";
        }
    }
}
