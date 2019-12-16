using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    internal class LineNumber
    {
        public int Index { get; }
        public int Value { get; }
        public int SubTotal { get; }

        public LineNumber Parent { get; }

        public LineNumber (int index, int value, LineNumber parent = null)
        {
            Index = index;
            Value = value;
            SubTotal = value + (parent?.SubTotal ?? 0);
            Parent = parent;
        }

        public static LineNumber Root(int value) => new LineNumber(0, value);
    }
}
