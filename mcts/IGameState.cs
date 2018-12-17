using System;
using System.Collections.Generic;
using System.Text;

namespace mcts
{
    public interface IGameState
    {
        void InitGameState();

        IGameState Clone();

        void DoMove(int move);

        // get all possible moves from this state
        int[] GetMoves();

        // get the game result from the viewpoint of playerNo
        int GetResult(int playerNo);

        string Display();

        int GetPlayerJustMoved();

        int GetRemaingChips();
    }
}
