using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.Discrete
{
    class DiscreteDeciderLoader
    {
        public static IDiscreteDecider LoadFromFile(String fileName)
        {
            if (File.Exists(fileName))
            {
                Stream saver = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                var decider = (IDiscreteDecider)deserializer.Deserialize(saver);
                saver.Close();

                return decider;
            }

            throw new Exception("File not found");
        }
    }
}
