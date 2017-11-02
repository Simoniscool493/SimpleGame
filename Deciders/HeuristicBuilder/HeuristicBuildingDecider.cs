using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.HeuristicBuilder;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.Deciders
{
    [Serializable()]
    public class HeuristicBuildingDecider : IDiscreteDecider
    {
        public static int maxGenes = 100;

        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => Heuristics.Count;

        public List<Heuristic> Heuristics;

        private Random _r;

        public HeuristicBuildingDecider(Random r,DiscreteIOInfo ioInfo)
        {
            Heuristics = new List<Heuristic>();
            IOInfo = ioInfo;
            _r = r;
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r)
        {
            var parent1Heuristics = Heuristics;
            var parent2Heuristics = ((HeuristicBuildingDecider)species2.BaseDecider).Heuristics;

            return CrossWithMatrixLogic(this, ((HeuristicBuildingDecider)species2.BaseDecider), mutationRate, r);
            ///TODO: this is the old cross
            /*HeuristicBuildingDecider child = new HeuristicBuildingDecider(r, IOInfo);


            foreach(var h in parent1Heuristics)
            {
                child.Heuristics.Add(h);
            }

            foreach(var h in parent2Heuristics)
            {
                child.Heuristics.Add(h);
            }

            Shuffle(child.Heuristics,r);

            if(_r.NextDouble()< mutationRate)
            {
                child = child.GetMutated(HeuristicBuildingConstants.NumStepsToMutateChildDecider);
            }

            while (child.Heuristics.Count > maxGenes)
            {
                var numToKill = r.Next(0, child.Heuristics.Count);
                child.Heuristics.RemoveAt(numToKill);
            }

            return new GeneticAlgorithmSpecies(child);*/


        }

        private GeneticAlgorithmSpecies CrossWithMatrixLogic(HeuristicBuildingDecider decider1, HeuristicBuildingDecider decider2, double mutationRate, Random r)
        {
            var heuristics1 = decider1.Heuristics;
            var heuristics2 = decider2.Heuristics;

            var outputValues = IOInfo.OutputInfo.EnumValues;
            var childDecider = new HeuristicBuildingDecider(r, IOInfo);
            var childHeuristics = childDecider.Heuristics;

            foreach (var h in heuristics1)
            {
                if (r.NextDouble() < mutationRate)
                {
                    var value = outputValues.GetValue(r.Next(0, outputValues.Length));
                    var valueAsIntArray = new int[] { ((int)value) };

                    var mutatedH = new Heuristic((int)value, IOInfo);
                    mutatedH.Conditions = new List<Tuple<int, int>>(h.Conditions);
                    mutatedH.Exceptions = new List<Tuple<int, int>>(h.Exceptions);
                    mutatedH.UseCount = h.UseCount;

                    childHeuristics.Add(mutatedH);
                }
                else if (r.NextDouble() > 0.5)
                {
                    var parentH = decider1.GetExactHeuristicOrRandomForThisInput(h.RecreatePayloadWithConditions());

                    var childH = new Heuristic(parentH.ExpectedOutput, IOInfo);
                    childH.Conditions = new List<Tuple<int, int>>(parentH.Conditions);
                    childH.Exceptions = new List<Tuple<int, int>>(parentH.Exceptions);
                    childH.UseCount = parentH.UseCount;
                    childDecider.Heuristics.Add(childH);
                }
                else
                {
                    var parentH = decider2.GetExactHeuristicOrRandomForThisInput(h.RecreatePayloadWithConditions());

                    var childH = new Heuristic(parentH.ExpectedOutput, IOInfo);
                    childH.Conditions = new List<Tuple<int, int>>(parentH.Conditions);
                    childH.Exceptions = new List<Tuple<int, int>>(parentH.Exceptions);
                    childH.UseCount = parentH.UseCount;
                    childDecider.Heuristics.Add(childH);
                }
            }

            /*foreach (var key in matrix2.GetKeys())
            {
                if (!matrix1.ContainsKey(key))
                {
                    childMatrix[key] = matrix2.Decide(key);
                }
            }*/

            return new GeneticAlgorithmSpecies(childDecider);

        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            var h = GetHeuristicFromListFor(input);

            if(h==null)
            {
                //h = Heuristic.CreateHeuristicRandomlyFromThisInput(_r, IOInfo, input, HeuristicBuildingConstants.ConditionsToAddToHeuristicFromInput);
                h = GetExactHeuristicOrRandomForThisInput(input);
                Heuristics.Add(h);
            }
            DiscreteDataPayload decision = new DiscreteDataPayload(IOInfo.OutputInfo.PayloadType, h.ExpectedOutput);

            return decision;
        }

        public Heuristic GetExactHeuristicOrRandomForThisInput(DiscreteDataPayload input)
        {
            Heuristic decidedHeuristic = GetHeuristicFromListFor(input);
            if (decidedHeuristic != null)
            {
                return decidedHeuristic;
            }
            else
            {
                Heuristic h = Heuristic.CreateExactHeuristicFromThisInput(_r, IOInfo, input);
                //Heuristic heuristic = Heuristic.CreateHeuristicRandomlyFromThisInput(_r, IOInfo, input, HeuristicBuildingConstants.ConditionsToAddToHeuristicFromInput);

                Heuristics.Add(h);
                return h;
            }
        }

        public Heuristic GetHeuristicFromListFor(DiscreteDataPayload input)
        {
            Heuristic heuristicToUse = null;

            foreach(var h in Heuristics)
            {
                foreach(var con in h.Conditions)
                {
                    if(input.Data[con.Item1] != con.Item2)
                    {
                        goto CheckNext;
                    }
                }

                foreach (var ex in h.Exceptions)
                {
                    if (input.Data[ex.Item1] == ex.Item2)
                    {
                        goto CheckNext;
                    }
                }

                heuristicToUse = h;
                break;

                CheckNext: continue;
            }

            if(heuristicToUse != null)
            {
                heuristicToUse.UseCount++;
                return heuristicToUse;
            }

            return null;
        }

        public void AddRandomHeuristics(int numToAdd)
        {
            for(int i=0;i<numToAdd;i++)
            {
                var newHeuristic = Heuristic.CreateRandom(_r, IOInfo,
                    HeuristicBuildingConstants.ConditionsToAddToRandomHeuristic,
                    HeuristicBuildingConstants.ExceptionsToAddToRandomHeuristic);

                var position = _r.Next(0, Heuristics.Count);
                Heuristics.Insert(position, newHeuristic);
            }
        }

        public void SaveToFile(string fileName)
        {
            Stream saver = File.OpenWrite(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saver, this);
            saver.Close();
        }

        public HeuristicBuildingDecider GetMutated(int steps)
        {
            HeuristicBuildingDecider decider = this.CloneWithAllHeuristics();

            for(int i=0;i<steps;i++)
            {
                if(_r.NextDouble() < HeuristicBuildingConstants.OddsOfRemovingHeuristicWhenMutating)
                {
                    var geneNumToChange = _r.Next(0, decider.Heuristics.Count);
                    decider.Heuristics.RemoveAt(geneNumToChange);
                }

                if(_r.NextDouble() < HeuristicBuildingConstants.OddsOfChangingHeuristicOutputWhenMutating)
                {
                    var geneNumToChange = _r.Next(0, decider.Heuristics.Count);
                    var geneToChange = decider.Heuristics.ElementAt(geneNumToChange);
                    geneToChange.Mutate(_r);
                }

                if(_r.NextDouble() < HeuristicBuildingConstants.OddsOfAddingNewHeuristicWhenMutating)
                {
                    decider.AddRandomHeuristics(1);
                }

                if (_r.NextDouble() < HeuristicBuildingConstants.OddsOfShufflingWhenMutating)
                {
                    Shuffle(decider.Heuristics, _r);
                }
            } 

            return decider;
        }

        public HeuristicBuildingDecider CloneWithAllHeuristics()
        {
            var decider = new HeuristicBuildingDecider(this._r,this.IOInfo);

            decider.Heuristics = new List<Heuristic>();

            foreach(Heuristic h in Heuristics)
            {
                var newHeuristic = new Heuristic(h.ExpectedOutput, IOInfo);
                newHeuristic.Exceptions = new List<Tuple<int, int>>(h.Exceptions);
                newHeuristic.Conditions = new List<Tuple<int, int>>(h.Conditions);

                decider.Heuristics.Add(newHeuristic);
            }

            return decider;
        }

        public void PostGenerationProcessing()
        {
            //TODO put this back maybe
            return;
            List<Heuristic> toRemove = new List<Heuristic>();
            foreach(var h in Heuristics)
            {
                if(h.UseCount==0)
                {
                    h.ConsecutiveGensNotUsed++;

                    if(h.ConsecutiveGensNotUsed >= HeuristicBuildingConstants.MaxAllowedGensWithNoHeuristicUses)
                    {
                        toRemove.Add(h);
                    }
                }
                else
                {
                    h.ConsecutiveGensNotUsed = 0;
                }
            }

            Heuristics = Heuristics.Except(toRemove).ToList();
        }


        public static void Shuffle(IList<Heuristic> list,Random r)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                Heuristic value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public string GetRaw()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var h in Heuristics)
            {
                string data = "";
                foreach(var c in h.Conditions)
                {
                    data = data + " " + c.Item2;
                }
                data = data + '\t';

                data = data + h.ExpectedOutput.ToString();

                sb.AppendLine(data);
            }

            return sb.ToString();
        }
    }
}
