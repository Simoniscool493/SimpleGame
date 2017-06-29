using SimpleGame.DataPayloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games
{
    interface IDiscreteDecider
    {
        DiscreteDataPayload Decide(DiscreteDataPayload input);
    }
}
