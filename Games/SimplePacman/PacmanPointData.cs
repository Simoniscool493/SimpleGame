using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman
{
    enum PacmanPointData 
    {
        Nothing = 99,
        Pellet = 1,
        Path = 0,
        Ghost = 66,
        Wall = 10,
        BigPellet = 2,
        ScaredGhost = 67
    }
}
