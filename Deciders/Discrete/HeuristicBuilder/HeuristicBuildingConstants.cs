using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    class HeuristicBuildingConstants
    {
        public const int ConditionsToAddToRandomHeuristic = 2;
        public const int ExceptionsToAddToRandomHeuristic = 1;

        public const int ConditionsToAddToHeuristicFromInput = 8;

        public const int MaxAllowedGensWithNoHeuristicUses = 5;
        public const int NumStepsToMutateChildDecider = 2;
    }
}
