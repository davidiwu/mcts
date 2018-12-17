using System;
using System.Collections.Generic;
using System.Text;

namespace mcts
{
    public class NimState : IGameState
    {
        private int _playerJustMoved;
        private int _chips;

        public void InitGameState()
        {
            _playerJustMoved = 2;
        }

        public NimState(int chips)
        {
            InitGameState();
            _chips = chips;
        }

        public IGameState Clone()
        {
            var nim = new NimState(_chips) {_playerJustMoved = _playerJustMoved};

            return nim;
        }

        public void DoMove(int move)
        {
            if (move >= 1 && move <= 3)
            {
                _chips -= move;
                _playerJustMoved = 3 - _playerJustMoved;
            }
            
        }

        // get all possible moves from this state
        public int[] GetMoves()
        {
            int min = _chips >= 3 ? 3 : _chips;
            int[] results = new int[min];
            for (int i = 1; i <= min; i++)
            {
                results[i - 1] = i;
            }

            return results;
        }

        public int GetResult(int playerNo)
        {
            if (_chips == 0)
            {
                return _playerJustMoved == playerNo ? 1 : 0;
            }

            throw new Exception("game is not finished");
        }

        public string Display()
        {
            return $"Chips: {_chips}, Just Played by: {_playerJustMoved}";
        }

        public int GetPlayerJustMoved()
        {
            return _playerJustMoved;
        }

        public int GetRemaingChips()
        {
            return _chips;
        }
    }
}
