using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcts
{
    public class Node
    {
        public int Move { get; set; }
        public Node ParentNode { get; set; }
        public List<Node> ChildNodes { get; set; }
        public int Wins { get; set; }
        public int Visits { get; set; }
        public List<int> UntriedMoves { get; set; }
        public int PlayerJustMoves { get; set; }

        private const double Ucb1Constant = 2.0;

        public Node(int move, Node parent, IGameState state)
        {
            Move = move;
            ParentNode = parent;
            ChildNodes = new List<Node>();
            Wins = 0;
            Visits = 0;
            UntriedMoves = state.GetMoves().ToList();
            PlayerJustMoves = state.GetPlayerJustMoved();
        }

        // use the UCB1 formula to select a child node
        public Node UctSelectChild()
        {
            var sorted = ChildNodes.OrderByDescending(Ucb1Formula);
            return sorted.FirstOrDefault();
        }

        // UCB1: Upper confidence bound 1
        private double Ucb1Formula(Node node)
        {
            var result = (double)node.Wins / node.Visits 
                         + Math.Sqrt(Ucb1Constant * Math.Log(Visits) / node.Visits);

            return result;

        }

        public Node AddChild(int move, IGameState state)
        {
            var n = new Node(move, this, state);
            UntriedMoves.Remove(move);
            ChildNodes.Add(n);

            return n;
        }

        public void Update(int result)
        {
            Visits += 1;
            Wins += result;
        }

        public string Display()
        {
            return $"[Player {PlayerJustMoves} made Move: {Move}, Win/Visit: {Wins}/{Visits}={(double) Wins / Visits}]";
        }

        public string ChildrenToString()
        {
            var str = new StringBuilder();
            foreach (var childNode in ChildNodes)
            {
                str.AppendLine(childNode.Display());
            }

            return str.ToString();
        }

        public string IndentString(int indent)
        {
            var str = new StringBuilder();
            str.Append('\n');
            for (int i = 0; i < indent; i++)
            {
                str.Append("| ");
            }

            return str.ToString();

        }

        public string TreeToString(int indent)
        {
            var tree = new StringBuilder();
            tree.Append(IndentString(indent));
            tree.Append(Display());
            foreach (var childNode in ChildNodes)
            {
                tree.Append(childNode.TreeToString(indent + 1));
            }

            return tree.ToString();
        }
    }
}
