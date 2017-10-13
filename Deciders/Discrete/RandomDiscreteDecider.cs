using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.Deciders.Discrete
{
    class RandomDiscreteDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo { get; }
        private Random _r;

        public RandomDiscreteDecider(Random r,DiscreteIOInfo ioInfo)
        {
            IOInfo = ioInfo;
            _r = r;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return IOInfo.OutputInfo.GetRandomInstance(_r);
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
