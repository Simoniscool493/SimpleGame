using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Iris
{
    class IrisDataInstance : IDiscreteGameState
    {
        public static string IrisPath = @"C:\Users\Simon\Desktop\College\FinalYearProject\Code\GamesToTestOn\Iris\IrisData.txt";

        public struct IrisFeatures
        {
            public double Feature1;
            public double Feature2;
            public double Feature3;
            public double Feature4;

            public IrisSpecies Species;

            public IrisFeatures(double f1, double f2, double f3, double f4,IrisSpecies species)
            {
                Feature1 = f1;
                Feature2 = f2;
                Feature3 = f3;
                Feature4 = f4;

                Species = species;
            }
        }

        public static List<IrisFeatures> allData;


        public List<IrisFeatures> trainingData;
        public List<IrisFeatures> evalData;

        public IrisDataInstance(Random r)
        {
            if (allData == null)
            {
                allData = LoadFromFile(IrisPath);
            }

            trainingData = allData.ToList();
            evalData = allData.ToList();

            for (int i=0;i<((int)(allData.Count/2.0));i++)
            {
                int position = r.Next(0, trainingData.Count);
                trainingData.RemoveAt(position);
            }

            evalData.RemoveAll(p => trainingData.Contains(p));
        }

        public List<IrisFeatures> LoadFromFile(string filePath)
        {
            var list = new List<IrisFeatures>();

            var raw = new StreamReader(filePath).ReadToEnd();
            var lines = raw.Split('\n');

            foreach(var line in lines)
            {
                var points = line.Split('\t');
                var point = new IrisFeatures(
                    double.Parse(points[0]),
                    double.Parse(points[1]),
                    double.Parse(points[2]),
                    double.Parse(points[3]),
                    (IrisSpecies)Enum.Parse(typeof(IrisSpecies), points[4].TrimEnd()));

                list.Add(point);
            }

            return list;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }
    }
}
