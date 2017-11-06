using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.AI.DecisionTrees
{
    class DecisionTreeLearner
    {
        /*private static int NumberOfPoints = 50;
        private static int NumberOfTreesToGenerate = 40;
        private static int NumberOfPointsToMutateForTrainingData = 30;
        private static int TreeDepth = 5;

        private static int GridWidth = 50;
        private static int GridHeight = 50;

        class DataPoint
        {
            public int X;
            public int Y;
            public int ColorNumber;

            public DataPoint(int x, int y, int colorNumber)
            {
                X = x;
                Y = y;
                ColorNumber = colorNumber;
            }

            public override string ToString()
            {
                return X + " " + Y;
            }
        }

        class SplitDecision
        {
            public SidesOfGraph Sides;
            public int Number;
            public bool IsX;

            public SplitDecision(SidesOfGraph sides, int number, bool isX)
            {
                Sides = sides;
                Number = number;
                IsX = isX;
            }
        }

        class TreeNode
        {
            public TreeNode LeftNode;
            public TreeNode RightNode;
            public double NumberToCompare;
            public bool IsX;
            public bool IsEnd;
            public int ColorToGoTo = -1;

            public TreeNode(TreeNode left, TreeNode right, double number, bool isX)
            {
                LeftNode = left;
                RightNode = right;
                NumberToCompare = number;
                IsX = isX;
                IsEnd = false;
            }

            public TreeNode(int color)
            {
                ColorToGoTo = color;
                IsEnd = true;
            }

            public int GetColorNumberFor(int x, int y)
            {
                if (IsEnd)
                    return ColorToGoTo;

                if (IsX)
                    if (x < NumberToCompare)
                        return LeftNode.GetColorNumberFor(x, y);
                    else
                        return RightNode.GetColorNumberFor(x, y);
                else
                    if (y < NumberToCompare)
                    return LeftNode.GetColorNumberFor(x, y);
                else
                    return RightNode.GetColorNumberFor(x, y);

                throw new Exception();
            }
        }

        class SidesOfGraph
        {
            public GraphSection FirstSide;
            public GraphSection SecondSide;
            public bool IsHorizontal;

            public SidesOfGraph(GraphSection first, GraphSection second, bool isHorizontal)
            {
                FirstSide = first;
                SecondSide = second;
                IsHorizontal = isHorizontal;
            }

            public double GetDifferenceInEntropy(GraphSection section)
            {
                var oldEntropy = section.GetEntropy();
                var newEntropy1 = FirstSide.GetEntropy();
                var newEntropy2 = SecondSide.GetEntropy();

                var oddsOfFirstSide = (double)FirstSide.Count() / section.Count();
                var oddsOfSecondSide = (double)SecondSide.Count() / section.Count();

                var output = oddsOfFirstSide * (oldEntropy - newEntropy1) + oddsOfSecondSide * (oldEntropy - newEntropy2);
                return output;
            }
        }

        class GraphSection : List<DataPoint>
        {
            public double GetEntropy()
            {
                if (Count == 0)
                    return 0;

                var freqReds = GetFrequencyOfColorNumber(0);
                var freqGreens = GetFrequencyOfColorNumber(1);
                var freqBlues = GetFrequencyOfColorNumber(2);

                double total = 0;

                if (freqReds != 0)
                {
                    total += (freqReds * (Math.Log(1 / (freqReds), 2)));
                }
                if (freqGreens != 0)
                {
                    total += (freqGreens * (Math.Log(1 / (freqGreens), 2)));
                }
                if (freqBlues != 0)
                {
                    total += (freqBlues * (Math.Log(1 / (freqBlues), 2)));
                }

                return total;
            }

            public double GetFrequencyOfColorNumber(int colorNumber)
            {
                return ((double)this.Where(p => p.ColorNumber == colorNumber).Count()) / this.Count();
            }

            public int GetMostFrequentColorNumber()
            {
                if (Count == 0)
                    return -1;

                var freqReds = GetFrequencyOfColorNumber(0);
                var freqGreens = GetFrequencyOfColorNumber(1);
                var freqBlues = GetFrequencyOfColorNumber(2);

                if (freqReds > freqGreens && freqReds > freqBlues)
                    return 0;
                else if (freqGreens > freqBlues)
                    return 1;
                else
                    return 2;
            }

            public SidesOfGraph SplitX(double number)
            {
                GraphSection firstSide = new GraphSection();
                GraphSection secondSide = new GraphSection();

                foreach (var point in this)
                {
                    if (point.X <= number)
                    {
                        firstSide.Add(point);
                    }
                    else
                    {
                        secondSide.Add(point);
                    }
                }

                return new SidesOfGraph(firstSide, secondSide, true);
            }

            public SidesOfGraph SplitY(double number)
            {
                GraphSection firstSide = new GraphSection();
                GraphSection secondSide = new GraphSection();

                foreach (var point in this)
                {
                    if (point.Y <= number)
                    {
                        firstSide.Add(point);
                    }
                    else
                    {
                        secondSide.Add(point);
                    }
                }

                return new SidesOfGraph(firstSide, secondSide, false);
            }

            public SplitDecision GetBestSplits()
            {
                double bestDifference = 0;
                var currentBestNumberToSplitAt = 0;
                SidesOfGraph currentBestSplits = null;
                var isBestX = false;

                foreach (var point in this)
                {
                    var splitsX = SplitX(point.X);
                    var splitsY = SplitY(point.Y);

                    var bestDifferenceForX = splitsX.GetDifferenceInEntropy(this);
                    var bestDifferenceForY = splitsY.GetDifferenceInEntropy(this);

                    if (bestDifferenceForY > bestDifference)
                    {
                        bestDifference = bestDifferenceForY;
                        currentBestSplits = splitsY;
                        currentBestNumberToSplitAt = point.Y;
                        isBestX = false;
                    }

                    if (bestDifferenceForX > bestDifference)
                    {
                        bestDifference = bestDifferenceForX;
                        currentBestSplits = splitsX;
                        currentBestNumberToSplitAt = point.X;
                        isBestX = true;
                    }
                }

                if (currentBestSplits == null)
                {
                    return null;
                }

                if (currentBestSplits.FirstSide.Count == 0 || currentBestSplits.SecondSide.Count == 0)
                {
                    throw new Exception();
                }

                return new SplitDecision(currentBestSplits, currentBestNumberToSplitAt, isBestX);
            }

            public TreeNode BuildDecisionTree(int depth, int maxDepth)
            {
                if(this.Count==0)
                {
                    throw new Exception();
                }

                if (depth > maxDepth || this.Count==1)
                {
                    return new TreeNode(GetMostFrequentColorNumber());
                }
                else
                {
                    var splits = GetBestSplits();

                    if(splits==null)
                    {
                        return new TreeNode(GetMostFrequentColorNumber());
                    }
                    var leftNode = splits.Sides.FirstSide.BuildDecisionTree(depth + 1, maxDepth);
                    var rightNode = splits.Sides.SecondSide.BuildDecisionTree(depth + 1, maxDepth);
                    return new TreeNode(leftNode, rightNode, splits.Number, splits.IsX);
                }
            }

            public GraphSection GetDeepCopy()
            {
                var copy = new GraphSection();

                foreach (var point in this)
                {
                    copy.Add(new DataPoint(point.X, point.Y, point.ColorNumber));
                }

                return copy;
            }

            public GraphSection GetCopyWithNoise(double mutationRate, Random r)
            {
                var copy = GetDeepCopy();
                var numberOfPointstoMutate = (int)(mutationRate * copy.Count());

                for (int i = 0; i < numberOfPointstoMutate; i++)
                {
                    var pointToChange = copy[r.Next(0, copy.Count)];
                    int numberToChangeTo;

                    if (r.Next(0, 2) == 1)
                    {
                        numberToChangeTo = r.Next(0, GridWidth);
                        pointToChange.X = numberToChangeTo;
                    }
                    else
                    {
                        numberToChangeTo = r.Next(0, GridHeight);
                        pointToChange.Y = numberToChangeTo;
                    }
                }

                return copy;
            }
        }

        class ListOfTrees : List<TreeNode>
        {
            public Dictionary<Tuple<int, int>, int> GetAverageOfTrees()
            {
                var output = new Dictionary<Tuple<int, int>, int>();

                for (int y = 0; y < GridHeight; y++)
                {
                    for (int x = 0; x < GridWidth; x++)
                    {
                        output[new Tuple<int, int>(x, y)] = GetAverageOfTreesForOnePoint(x, y);
                    }
                }

                return output;
            }

            public int GetAverageOfTreesForOnePoint(int x, int y)
            {
                var numReds = 0;
                var numGreens = 0;
                var numBlues = 0;

                foreach (var tree in this)
                {
                    var colorValue = tree.GetColorNumberFor(x, y);
                    if (colorValue == 0)
                        numReds++;
                    else if (colorValue == 1)
                        numGreens++;
                    else if (colorValue == 2)
                        numBlues++;
                }

                if (numReds > numGreens && numReds > numBlues)
                    return 0;
                else if (numGreens > numBlues)
                    return 1;
                else
                    return 2;
            }
        }

        public static char GetColorFromColorNumber(int colorNumber)
        {
            if (colorNumber == 0)
                return '#';
            if (colorNumber == 1)
                return '@';
            if (colorNumber == 2)
                return '8';
            throw new Exception();
        }

        public static void Demonstrate()
        {
            var learner = new DecisionTreeLearner();

            var data = new GraphSection()
            {
                new DataPoint(0,0,0),new DataPoint(1,0,1),
                new DataPoint(0,1,2),new DataPoint(1,1,3),
            };

            data = GetRandomGraph(new Random(), 20);

            var tree = data.BuildDecisionTree(0, 10);

            tree.GetColorNumberFor(0, 0);
        }

        private static GraphSection GetRandomGraph(Random r, int length)
        {
            var output = new GraphSection();

            for (int i = 0; i < length; i++)
            {
                var x = r.Next(0, GridWidth);
                var y = r.Next(0, GridHeight);
                var color = r.Next(0, 2+1);

                output.Add(new DataPoint(x, y, color));
            }

            return output;
        }

        private static void PrintTreeNode(TreeNode t,int width,int height)
        {
        }*/
    }
}

