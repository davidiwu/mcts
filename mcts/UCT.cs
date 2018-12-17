using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcts
{
    // Upper Confidence Bound 1 applied to trees
    public class UCT
    {
        public int Search(IGameState rootState, int itermax)
        {
            Node rootNode = new Node(0, null, rootState);

            for (int i = 0; i < itermax; i++)
            {
                var node = rootNode;
                var state = rootState.Clone();

                // Select
                node = SelectNextNodeToPlay(node, state);

                // Expand
                node = ExpandChildNodesAfterPlay(node, state);

                // Rollout
                RolloutAllPossibleMoves(state);

                // Backpropagate
                BackpropagatePlayResults(node, state);
            }

            Console.Write(rootNode.ChildrenToString());

            var selectedNode = rootNode.ChildNodes.OrderByDescending(c => c.Visits).FirstOrDefault();

            var result = selectedNode.Move;

            return result;
        }

        private static void BackpropagatePlayResults(Node node, IGameState state)
        {
            while (node != null)
            {
                var result = state.GetResult(node.PlayerJustMoves);
                node.Update(result);
                node = node.ParentNode;
            }
        }

        private static void RolloutAllPossibleMoves(IGameState state)
        {
            while (state.GetMoves().Any())
            {
                var temp = state.GetMoves();
                if (temp.Length == 0)
                    break;
                state.DoMove(temp.OrderBy(x => Guid.NewGuid()).FirstOrDefault());
            }
        }

        private static Node ExpandChildNodesAfterPlay(Node node, IGameState state)
        {
            if (node.UntriedMoves.Any())
            {
                var m = node.UntriedMoves.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                state.DoMove(m);
                node = node.AddChild(m, state);
            }

            return node;
        }

        private static Node SelectNextNodeToPlay(Node node, IGameState state)
        {
            while (node.UntriedMoves.Count == 0 && node.ChildNodes.Any())
            {
                node = node.UctSelectChild();
                state.DoMove(node.Move);
            }

            return node;
        }

        public void PlayUctGame(int chips)
        {
            var state = new NimState(chips);

            while (state.GetMoves().Any())
            {
                var temp = state.GetMoves();
                if(temp.Length == 0)
                    break;

                int move = 0;
                if (state.GetPlayerJustMoved() == 1)
                {
                    move = Search(state, 1000);
                }
                else
                {
                    move = Search(state, 100);
                }

                state.DoMove(move);
                Console.WriteLine($"Player {state.GetPlayerJustMoved()} decided to make moves {move}, remaining chip {state.GetRemaingChips()}");

            }

            DisplayResult(state);
        }

        private static void DisplayResult(NimState state)
        {
            var result = state.GetResult(state.GetPlayerJustMoved());

            if (result == 1 || result == 0)
            {
                Console.WriteLine($"Player {state.GetPlayerJustMoved()} : result {result}");
            }
            else
            {
                Console.WriteLine("Nobody wins");
            }
        }
    }
}
