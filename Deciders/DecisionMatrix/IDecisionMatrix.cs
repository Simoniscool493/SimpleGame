using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.DecisionMatrix
{
    interface IDecisionMatrix : IDiscreteDecider
    {
         Type InputType { get; }
         Type OutputType { get; }

         IEnumerable<DiscreteDataPayload> GetKeys();
    }
}
