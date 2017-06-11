using System;

namespace SimpleGame
{
    class ManualGridPlayer : IGridPlayer
    {
        Random r = new Random();

        public Direction GetDirection(ItemAtPoint[] upDownLeftright)
        {

            while(true)
            {
                var keyPressed = Console.ReadKey();

                switch (keyPressed.KeyChar)
                {
                    case 'w':
                        return Direction.Up;
                    case 'a':
                        return Direction.Left;
                    case 's':
                        return Direction.Down;
                    case 'd':
                        return Direction.Right;
                }
            }
        }
    }
}
