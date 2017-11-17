using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads.DiscreteData
{
    public interface IDiscreteDataPayload
    {
        int[] Data { get; }

        int SingleItem { get; }
    }
}
