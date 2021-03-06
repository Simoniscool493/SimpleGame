﻿using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.HeuristicBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.Discrete.HeuristicBuilder
{
    class HeuristicFactory
    {
        public static Heuristic CreateRandom(Random r, DiscreteIOInfo ioInfo, int numConditions, int numExceptions)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput, ioInfo);

            h.AddConditions(numConditions, r);
            return h;
        }

        public static Heuristic CreateHeuristicRandomlyFromThisInput(Random r, DiscreteIOInfo ioInfo, IDiscreteDataPayload input, int numConditions)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput, ioInfo);

            if (numConditions > input.Data.Length)
            {
                numConditions = input.Data.Length;
            }

            for (int i = 0; i < numConditions; i++)
            {
                top:
                var position = r.Next(0, ioInfo.InputInfo.PayloadLength);

                if (h.Conditions[position] != -1)
                {
                    goto top;
                }

                h.Conditions[position] = input.Data[position];
            }

            return h;
        }

        public static Heuristic CreateExactHeuristicFromThisInput(Random r, DiscreteIOInfo ioInfo, IDiscreteDataPayload input)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput, ioInfo);

            for (int i = 0; i < ioInfo.InputInfo.PayloadLength; i++)
            {
                h.Conditions[i] = input.Data[i];
            }

            return h;
        }
    }
}
