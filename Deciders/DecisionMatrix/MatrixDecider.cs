using System;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class MatrixDecider : IDiscreteDecider
    {
        protected IDecisionMatrix matrix;
        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => matrix.NumGenes;

        public MatrixDecider(IDecisionMatrix d,DiscreteIOInfo ioInfo)
        {
            matrix = d;
            IOInfo = ioInfo;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return matrix.Decide(input);
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
