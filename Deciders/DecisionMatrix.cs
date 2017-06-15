using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleGame
{
    class DecisionMatrix
    {
        private Dictionary<Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>, Direction> _theMatrix;

        protected DecisionMatrix(Dictionary<Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>, Direction> matrix)
        {
            _theMatrix = matrix;
        }

        public static DecisionMatrix GetRandomMatrix()
        {
            var r = new Random();
            var randomMatrix = new Dictionary<Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>, Direction>();

            var itemsAtPoint = typeof(ItemAtPoint).GetEnumValues();
            var numDirections = typeof(Direction).GetEnumValues().Length;

            for (int a=0;a<itemsAtPoint.Length;a++)
            {
                for (int b = 0; b < itemsAtPoint.Length; b++)
                {
                    for (int c = 0; c < itemsAtPoint.Length; c++)
                    {
                        for (int d = 0; d < itemsAtPoint.Length; d++)
                        {
                            randomMatrix[new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>((ItemAtPoint)a, (ItemAtPoint)b, (ItemAtPoint)c, (ItemAtPoint)d)] = (Direction)r.Next(0, numDirections);
                        }
                    }
                }
            }

            return new DecisionMatrix(randomMatrix);
        }

        public Direction Decide(ItemAtPoint[] upDownLeftRight)
        {
            return _theMatrix[new Tuple<ItemAtPoint, ItemAtPoint, ItemAtPoint, ItemAtPoint>(upDownLeftRight[0], upDownLeftRight[1], upDownLeftRight[2], upDownLeftRight[3])];
        }
    }
}
