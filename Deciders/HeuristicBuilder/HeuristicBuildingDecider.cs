using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.HeuristicBuilder;

namespace SimpleGame.Deciders
{
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
            HeuristicBuildingDecider child = new HeuristicBuildingDecider(r, IOInfo);

            var parent1Heuristics = Heuristics;
            var parent2Heuristics = ((HeuristicBuildingDecider)species2.BaseDecider).Heuristics;

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

            return new GeneticAlgorithmSpecies(child);
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            DiscreteDataPayload decision = DecideBasedOnHeuristics(input);

            if(decision == null)
            {
                Heuristic heuristic = Heuristic.CreateHeuristicForThisInput(_r, IOInfo, input,HeuristicBuildingConstants.ConditionsToAddToHeuristicFromInput);
                Heuristics.Add(heuristic);

                return new DiscreteDataPayload(IOInfo.OutputInfo.PayloadType, heuristic.ExpectedOutput);
            }

            return decision;
        }

        public DiscreteDataPayload DecideBasedOnHeuristics(DiscreteDataPayload input)
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
                return new DiscreteDataPayload(IOInfo.OutputInfo.PayloadType, heuristicToUse.ExpectedOutput);
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

        public void SaveToFile(string filename)
        {
            throw new NotImplementedException();
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

    }
}
