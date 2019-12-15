using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    public class PyramidSolver
    {
        public Result Solve(IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException();

            var iter = source.GetEnumerator();

            if (!iter.MoveNext()) throw new ArgumentException("Empty collection is not expected!");

            var root = Node.Root(iter.Current);
            var parity = root.Value.GetParity();

            var candidates = new List<Node>(1) { root };

            var lineNumber = 1;

            while (iter.MoveNext())
            {
                parity = parity.InvertParity();

                candidates = CollectNumbers(candidates, parity, iter, ++lineNumber);
            }

            return GetResult(candidates, lineNumber);
        }

        private List<Node> CollectNumbers(List<Node> parents, Parity validParity, IEnumerator<int> iter, int itemsInLine)
        {
            var expectedCandidatesCount = 2 * parents.Count;

            if (expectedCandidatesCount > itemsInLine) expectedCandidatesCount = itemsInLine;

            var candidates = new List<Node>(expectedCandidatesCount);

            var itemIndex = 0;

            do
            {
                var value = iter.Current;

                if (candidates.Count < expectedCandidatesCount && value.GetParity() == validParity)
                {
                    for (var i = 0; i < parents.Count; i++)
                    {
                        var parent = parents[i];

                        if (parent.Index == itemIndex - 1)
                        {
                            if (++i < parents.Count)
                            {
                                var parent2 = parents[i];

                                if (parent2.Index == itemIndex && parent2.SubTotal >= parent.SubTotal)
                                {
                                    parent = parent2;
                                }
                            }
                        }
                        else
                        {
                            if (parent.Index != itemIndex) continue;

                            if (i > 0)
                            {
                                var parent2 = parents[i - 1];

                                if (parent2.Index == itemIndex - 1 && parent2.SubTotal > parent.SubTotal)
                                {
                                    parent = parent2;
                                }
                            }
                        }


                        candidates.Add(new Node(itemIndex, value, parent));
                        break;
                    }
                }

                if (++itemIndex == itemsInLine) return candidates;
            }
            while (iter.MoveNext());

            throw new Exception("Unexpected truncation of list of numbers!");
        }

        private Result GetResult(List<Node> candidates, int pathLength)
        {
            var node = candidates.OrderByDescending(x => x.SubTotal).First();

            var sum = node.SubTotal;

            var path = new List<int>(pathLength);

            while(node != null)
            {
                path.Add(node.Value);

                node = node.Parent;
            }

            path.Reverse();

            return new Result(sum, path);
        }

        internal enum Parity { Even, Odd };

        public class Result
        {
            public int Sum { get; }

            public IReadOnlyCollection<int> Path { get; }

            public Result(int sum, List<int> path)
            {
                Sum = sum;
                Path = path;
            }
        }
    }
}
