using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.AI.SinglePathMutation
{
    [Serializable]
    public enum StatusTypeNumber
    {
        OverallStatus = 0,
        Mutate,
        RemoveHeuristics,
        RemoveConditions,
        GreedySearch,
        AddExceptions
    }
}
