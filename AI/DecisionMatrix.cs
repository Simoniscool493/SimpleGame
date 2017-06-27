﻿using SimpleGame.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleGame.DataPayloads;

namespace SimpleGame
{
    class DecisionMatrix : IDiscreteDecider
    {
        private static Random r = new Random();
        private Dictionary<DiscreteDataPayload,DiscreteDataPayload> _theMatrix;

        public DecisionMatrix(Dictionary<DiscreteDataPayload, DiscreteDataPayload> matrix)
        {
            _theMatrix = matrix;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return _theMatrix[input];
        }

        public IEnumerable<DiscreteDataPayload> GetKeys()
        {
            return _theMatrix.Keys;
        }

        public static DecisionMatrix GetRandomIOMapping(DiscreteDataPayloadInfo inputInfo,DiscreteDataPayloadInfo outputInfo)
        {
            throw new Exception();
        }
    }
}
