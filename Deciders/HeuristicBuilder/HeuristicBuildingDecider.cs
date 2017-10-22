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
            throw new NotImplementedException();
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            DiscreteDataPayload decision = DecideBasedOnHeuristics(input);

            if(decision == null)
            {
                return IOInfo.OutputInfo.GetDefualtInstance();
            }

            return decision;
        }

        public DiscreteDataPayload DecideBasedOnHeuristics(DiscreteDataPayload input)
        {
            foreach(var h in Heuristics)
            {
                if(input.Data[h.PositionInPayload] == h.ExpectedInput)
                {
                    return new DiscreteDataPayload(IOInfo.OutputInfo.PayloadType, h.ExpectedOutput);
                }
            }

            return null;
        }

        public void AddRandomHeuristics(int numToAdd)
        {
            for(int i=0;i<numToAdd;i++)
            {
                var newHeuristic = Heuristic.CreateRandom(_r, IOInfo);
                Heuristics.Add(newHeuristic);
            }
        }

        public void SaveToFile(string filename)
        {
            throw new NotImplementedException();
        }

        public HeuristicBuildingDecider GetSingleMutated()
        {
            var decider = this.CloneAllHeuristics();

            var giveTakeOrChange = _r.NextDouble();

            if(giveTakeOrChange<0.70)
            {
                if (decider.Heuristics.Any())
                {
                    var geneNumToChange = _r.Next(0, decider.Heuristics.Count);

                    if (giveTakeOrChange < 0.60)
                    {
                        decider.Heuristics.RemoveAt(geneNumToChange);
                    }
                    else
                    {
                        var geneToChange = decider.Heuristics.ElementAt(geneNumToChange);
                        geneToChange.Mutate(_r);
                    }
                }
            }
            else if(giveTakeOrChange<0.90)
            {
                decider.AddRandomHeuristics(1);
            }
            else
            {
                Shuffle(decider.Heuristics, _r);
            }

            return decider;
        }

        public HeuristicBuildingDecider CloneAllHeuristics()
        {
            var decider = new HeuristicBuildingDecider(this._r,this.IOInfo);
            decider.Heuristics = new List<Heuristic>(Heuristics);

            return decider;
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
