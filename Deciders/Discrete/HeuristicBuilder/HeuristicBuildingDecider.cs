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
using SimpleGame.Deciders.Discrete.HeuristicBuilder;
using SimpleGame.AI;

namespace SimpleGame.Deciders
{
    [Serializable()]
    public class HeuristicBuildingDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo { get; }

        public int NumGenes => Heuristics.Count;
        public int NumConditions => Heuristics.Sum(h => h.Conditions.Count(c => c != -1));
        public int TotalComplexity => NumConditions;

        public bool ShouldMakeNewHeuristics = true;

        public List<Heuristic> Heuristics;

        private Random _r;
        
        public int NumConditionsToBuildFrom { get; }

        public HeuristicBuildingDecider(Random r, DiscreteIOInfo ioInfo,int numCondidionsToBuildFrom)
        {
            Heuristics = new List<Heuristic>();
            IOInfo = ioInfo;
            _r = r;

            NumConditionsToBuildFrom = numCondidionsToBuildFrom;
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider decider2, double mutationRate, Random r)
        {
            return HeuristicDeciderFactory.CrossMutate(this, (HeuristicBuildingDecider)decider2, mutationRate, r);
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            return HeuristicDeciderFactory.GetMutated(this, mutationRate, r);
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
        {
            var h = GetHeuristicFromListFor(input);

            if (h == null)
            {
                if(ShouldMakeNewHeuristics)
                {
                    if(NumConditionsToBuildFrom<1)
                    {
                        h = HeuristicFactory.CreateExactHeuristicFromThisInput(_r, IOInfo, input);
                    }
                    else
                    {
                        h = HeuristicFactory.CreateHeuristicRandomlyFromThisInput(_r, IOInfo, input, NumConditionsToBuildFrom);
                    }

                    Heuristics.Add(h);
                }
                else
                {
                    return new DiscreteDataPayload(IOInfo.OutputInfo.GetRandomInstance(_r).SingleItem);
                }
            }

            DiscreteDataPayload decision = new DiscreteDataPayload(h.ExpectedOutput);

            return decision;
        }


        private static Dictionary<int, int> choices = new Dictionary<int, int>();

        public Heuristic GetHeuristicFromListFor(IDiscreteDataPayload input)
        {
            var inputData = input.Data;
            var inputDataLength = input.Data.Length;

            foreach (var h in Heuristics)
            {
                for (int i = 0; i < inputDataLength; i++)
                {
                    if (input.Data[i] != h.Conditions[i] && h.Conditions[i] != -1)
                    {
                        goto CheckNext;
                    }
                }

                h.UseCount++;
                return h;

                CheckNext: continue;
            }

            return null;
        }

        public void AddRandomHeuristics(int numToAdd)
        {
            for (int i = 0; i < numToAdd; i++)
            {
                var newHeuristic = HeuristicFactory.CreateRandom(_r, IOInfo,
                    HeuristicBuildingConstants.ConditionsToAddToRandomHeuristic,
                    HeuristicBuildingConstants.ExceptionsToAddToRandomHeuristic);

                var position = _r.Next(0, Heuristics.Count);
                Heuristics.Insert(position, newHeuristic);
            }
        }

        public void RemoveRandomHeuristics(int numToTake)
        {
            if (numToTake > Heuristics.Count)
            {
                Heuristics.Clear();
                return;
            }

            for (int i = 0; i < numToTake; i++)
            {
                var position = _r.Next(0, Heuristics.Count);
                Heuristics.RemoveAt(position);
            }
        }

        public void RemoveRandomConditions(double numToRemove, Random r)
        {
            List<Heuristic> toRemove = new List<Heuristic>();

            for (int i = 0; i < numToRemove; i++)
            {
                var elementToTake = r.Next(0, Heuristics.Count);
                var h = Heuristics.ElementAt(elementToTake);

                if (h.Conditions.Length == 0)
                {
                    toRemove.Add(h);
                    break;
                }

                var conToRemove = r.Next(0, h.Conditions.Length);
                h.Conditions[conToRemove] = -1;

                if (h.Conditions.Length == 0)
                {
                    toRemove.Add(h);
                }
            }

            foreach (var h in toRemove)
            {
                Heuristics.Remove(h);
            }
        }

        public HeuristicBuildingDecider CloneWithAllHeuristics()
        {
            var decider = new HeuristicBuildingDecider(_r,IOInfo,NumConditionsToBuildFrom);

            decider.Heuristics = new List<Heuristic>();

            foreach (Heuristic h in Heuristics)
            {
                decider.Heuristics.Add(h.GetCopy(false));
            }

            return decider;
        }

        public void PostGenerationProcessing()
        {
            foreach (var h in Heuristics)
            {
                h.UseCount = 0;
            }
        }

        public string GetFullStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var h in Heuristics)
            {
                string data = "";

                for (int i = 0; i < h.Conditions.Length; i++)
                {
                    data = data + " " + h.Conditions[i];

                }
                data = data + '\t';

                data = data + h.ExpectedOutput.ToString();

                sb.AppendLine(data);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return " Genes: " + NumGenes.ToString() + " Complexity: " + TotalComplexity.ToString();
        }
    }
}
