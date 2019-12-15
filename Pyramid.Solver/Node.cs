using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    internal class Node
    {
        public int Index { get; }
        public int Value { get; }
        public int SubTotal { get; }

        public Node Parent { get; }

        public Node (int index, int value, Node parent = null)
        {
            Index = index;
            Value = value;
            SubTotal = value + (parent?.SubTotal ?? 0);
            Parent = parent;
        }

        public static Node Root(int value) => new Node(0, value);
    }
}
