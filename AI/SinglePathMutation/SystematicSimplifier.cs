using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.AI.SinglePathMutation
{
    class SystematicSimplifier
    {
        private IDiscreteGameManager _game;
        private int _numRandomSeeds;
        private int _baseRandomSeed;

        public SystematicSimplifier(IDiscreteGameManager game, int numRandomSeeds,int baseRandomSeed)
        {
            _game = game;
            _numRandomSeeds = numRandomSeeds;
            _baseRandomSeed = baseRandomSeed;
        }

        public DeciderSpecies SimplifySpeciesAsMuchAsPossible(DeciderSpecies bestSpecies)
        {
            int stepsBack = 0;
            HeuristicBuildingDecider curbest = (HeuristicBuildingDecider)bestSpecies.BaseDecider;

            while (true)
            {
                bool hadSingleCondition;
                var newDecider = GetDeciderWithoutLastConditionXStepsBack(curbest, stepsBack, out hadSingleCondition);

                if (hadSingleCondition)
                {
                    stepsBack++;
                }

                if (newDecider == null)
                {
                    break;
                }

                var score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, newDecider, _numRandomSeeds,_baseRandomSeed);

                if (score >= bestSpecies.Score && newDecider.TotalComplexity <= curbest.TotalComplexity)
                {
                    curbest = newDecider;
                }
                else
                {
                    stepsBack++;
                }
            }

            var species = new DeciderSpecies(curbest);
            species.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, species, _numRandomSeeds,_baseRandomSeed);

            return species;
        }

        private HeuristicBuildingDecider GetDeciderWithoutLastConditionXStepsBack(HeuristicBuildingDecider curBestFound, int steps, out bool hadSingleCondition)
        {
            steps++;
            hadSingleCondition = false;
            int heuristicNumber = curBestFound.Heuristics.Count - 1;
            int conditionNumber = curBestFound.Heuristics.Last().Conditions.Length - 1;
            while (curBestFound.Heuristics[heuristicNumber].Conditions[conditionNumber] == -1)
            {
                conditionNumber--;

                if(conditionNumber==0)
                {
                    break;
                }
            }

            while (true)
            {
                if (curBestFound.Heuristics[heuristicNumber].Conditions[conditionNumber] != -1)
                {
                    steps--;

                    if (steps == 0)
                    {
                        break;
                    }
                }

                conditionNumber--;

                if (conditionNumber <= 0)
                {
                    if (heuristicNumber == 0)
                    {
                        return null;
                    }

                    heuristicNumber--;
                    conditionNumber = curBestFound.Heuristics[heuristicNumber].Conditions.Length - 1;
                }
            }

            var newDecider = curBestFound.CloneWithAllHeuristics();
            newDecider.Heuristics[heuristicNumber].Conditions[conditionNumber] = -1;
            if (newDecider.Heuristics[heuristicNumber].Conditions.Count(c => c != -1) == 1)
            {
                hadSingleCondition = true;
            }

            return newDecider;
        }
    }
}
