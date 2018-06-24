using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame
{
    public class DeciderRandomSeedRange
    {
        public bool IsSingle => RangeStartInclusive == RangeEndInclusive;
        public int RangeSize => RangeEndInclusive - RangeStartInclusive + 1;
        public int RangeStartInclusive;
        public int RangeEndInclusive;

        public DeciderRandomSeedRange(int start,int? end = null)
        {
            RangeStartInclusive = start;

            if (end==null)
            {
                RangeEndInclusive = start;
            }
            else
            {
                RangeEndInclusive = end.Value;
            }
        }
    }
}
