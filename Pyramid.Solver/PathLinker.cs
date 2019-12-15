using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    /// <summary>
    /// Helper class to link path nodes
    /// </summary>
    internal struct PathLinker
    {
        private readonly List<Node> _pathEndings;
        private int _index;
        private Node _current;

        public PathLinker(List<Node> pathEndings)
        {
            _pathEndings = pathEndings;
            _index = 0;
            _current = pathEndings[0];
        }

        internal bool TryLocatePath(int itemIndex, out Node path)
        {
            path = null;

            if (_index == _pathEndings.Count) return false;

            do
            {
                var nodeIndex = _current.Index;

                if (itemIndex < nodeIndex) return false;

                // Line j-1:    N(i-1) N(i)
                // Line  j :        N(i)

                if (itemIndex - 1 == nodeIndex)
                {
                    var i = _index + 1;

                    if (i < _pathEndings.Count)
                    {
                        path = _pathEndings[i];

                        if (path.Index == itemIndex && path.SubTotal >= _current.SubTotal)
                            return true;
                    }

                    path = _current;

                    return true;
                }

                if (itemIndex == nodeIndex)
                {
                    if (_index > 0)
                    {
                        path = _pathEndings[_index - 1];

                        if (path.Index == itemIndex - 1 && path.SubTotal > _current.SubTotal)
                            return true;
                    }

                    path = _current;

                    return true;
                }

                if (++_index == _pathEndings.Count) return false;

                _current = _pathEndings[_index];

            } while (true);
        }
    }
}
