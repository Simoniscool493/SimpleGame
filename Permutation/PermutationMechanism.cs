using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Permutation
{
    public class PermutationMechanism
    {
        public int[] PermutationCounter;
        private List<List<int>> _paramaterList;
        
        public virtual int GetNumberOfValues(int currentPlaceInList)
        {
            return _paramaterList.ElementAt(currentPlaceInList).Count();
        }

        public PermutationMechanism(List<List<int>> paramaterList)
        {
            _paramaterList = paramaterList;
            PermutationCounter = new int[paramaterList.Count];
        }

        public PermutationMechanism() { }

        public bool TryIncrement(int orderOfMagnitide)
        {
            if (orderOfMagnitide >= PermutationCounter.Length)
            {
                return false;
            }

            PermutationCounter[orderOfMagnitide]++;
            if (PermutationCounter[orderOfMagnitide] >= GetNumberOfValues(orderOfMagnitide))
            {
                PermutationCounter[orderOfMagnitide] = 0;
                return TryIncrement(orderOfMagnitide + 1);
            }

            return true;
        }

        public int[] GetCurrentConfiguration()
        {
            List<int> output = new List<int>();

            for(int i=0;i<_paramaterList.Count;i++)
            {
                var point = PermutationCounter[i];
                var selectionAtThisPoint = _paramaterList.ElementAt(i).ElementAt(point);
                output.Add(selectionAtThisPoint);
            }

            return output.ToArray();
        }

        public void Reset()
        {
            for (int i = 0; i < PermutationCounter.Count(); i++)
            {
                PermutationCounter[i] = 0;
            }
        }

        public List<int[]> GetAllPermutations()
        {
            Reset();
            var permutations = new List<int[]>();

            do
            {
                permutations.Add(GetCurrentConfiguration());

            } while (TryIncrement(0));

            return permutations;
        }
    }
}
