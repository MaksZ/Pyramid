using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    /// <summary>
    /// Helper class to iterate through line numbers
    /// </summary>
    internal struct LineIterator
    {
        private IReadOnlyList<LineNumber> items;
        private int index;

        public LineNumber Current { get; private set; }

        public LineNumber Next => GetRightToCurrent();

        public bool CanIterate => index < items.Count;

        public LineIterator(IReadOnlyList<LineNumber> source)
        {
            items = source;
            index = 0;
            Current = items[0];
        }

        public bool MoveNext()
        {
            if (++index >= items.Count) return false;

            Current = items[index];
            return true;
        }

        private LineNumber GetRightToCurrent()
        {
            var j = index + 1;
            return j == items.Count ? null : items[j];
        }
    }
}
