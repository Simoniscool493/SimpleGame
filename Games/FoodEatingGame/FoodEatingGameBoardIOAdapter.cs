using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameBoardIOAdapter : IDiscreteGameIOAdapter
    {
        public DiscreteDataPayload GetOutput(IDiscreteGameState genericState)
        {
            return new DiscreteDataPayload(typeof(ItemAtPoint),((FoodEatingGameBoard)genericState).GetPlayerData().Cast<int>().ToArray());
        }

        public void SendInput(IDiscreteGameState genericState, DiscreteDataPayload input)
        {
            var inputToSend = (Direction)input.SingleItem;

            ((FoodEatingGameBoard)genericState).MovePlayer(inputToSend);
        }
    }
}
