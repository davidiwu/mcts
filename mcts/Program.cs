using System;

namespace mcts
{
    class Program
    {
        static void Main(string[] args)
        {
            var uct = new UCT();
            uct.PlayUctGame(20);
        }
    }
}
