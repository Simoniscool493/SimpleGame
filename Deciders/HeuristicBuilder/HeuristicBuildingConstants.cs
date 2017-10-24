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

        public const int ConditionsToAddToHeuristicFromInput = 2;

        public const double OddsOfRemovingHeuristicWhenMutating = 0.6;
        public const double OddsOfChangingHeuristicOutputWhenMutating = 0.3;
        public const double OddsOfAddingNewHeuristicWhenMutating = 0.2;
        public const double OddsOfShufflingWhenMutating = 0.05;

        public const int MaxAllowedGensWithNoHeuristicUses = 4;

        public const int NumStepsToMutateChildDecider = 2;

    }
}
