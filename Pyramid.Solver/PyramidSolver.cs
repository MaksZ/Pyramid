using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    public class PyramidSolver
    {
        private readonly IEnumerator<int> _numbers;

        /// <summary>
        /// Find the path that provides the maximum possible sum of the numbers
        /// </summary>
        /// <param name="source">A triangle input given as a non-empty sequence of numbers</param>
        /// <returns>The maximum possible sum and a path of numbers that provides that sum</returns>
        public static Result Solve(IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException();

            var numbers = source.GetEnumerator();

            if (!numbers.MoveNext()) throw new ArgumentException("Empty collection is not expected!");

            return new PyramidSolver(numbers).Solve();
        }

        private PyramidSolver(IEnumerator<int> numbers)
        {
            _numbers = numbers;
        }

        private Result Solve()
        {
            var root = Node.Root(_numbers.Current);
            var parity = root.Value.GetParity();

            var lineNumber = 1;
            var pathEndings = new List<Node>(1) { root };

            while (_numbers.MoveNext() && pathEndings.Count > 0)
            {
                parity = parity.InvertParity();

                pathEndings = FindPathEndings(pathEndings, parity, ++lineNumber);
            }

            if (pathEndings.Count == 0)
                throw new ArgumentException("The given sequence of numbers doesn't contain any valid path!");

            return GetResult(pathEndings, lineNumber);
        }

        private List<Node> FindPathEndings(List<Node> currentEndings, Parity validParity, int lineNumber)
        {
            var expectedEndingsCount = 2 * currentEndings.Count;

            if (expectedEndingsCount > lineNumber) expectedEndingsCount = lineNumber;

            var pathEndings = new List<Node>(expectedEndingsCount);

            var pathLinker = new PathLinker(currentEndings);

            var itemIndex = 0;
            Node path;

            do
            {
                var number = _numbers.Current;

                if (number.GetParity() == validParity && pathLinker.TryLocatePath(itemIndex, out path))
                {
                    pathEndings.Add(new Node(itemIndex, number, path));
                }

                if (++itemIndex == lineNumber) return pathEndings;
            }
            while (_numbers.MoveNext());

            throw new Exception("Unexpected truncation of list of numbers!");
        }

        private Result GetResult(List<Node> pathEndings, int pathLength)
        {
            var node = pathEndings.OrderByDescending(x => x.SubTotal).First();

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
    }
}
