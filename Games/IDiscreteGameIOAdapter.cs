﻿using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games
{
    public interface IDiscreteGameIOAdapter
    {
        IDiscreteDataPayload GetOutput(IDiscreteGameState genericState);

        void SendInput(IDiscreteGameState genericState, IDiscreteDataPayload input);

    }
}
