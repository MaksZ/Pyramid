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
            if (source == null) throw new ArgumentNullException(nameof(source));

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
            var root = LineNumber.Root(_numbers.Current);
            var parity = root.Value.GetParity();

            var lineNumber = 1;
            var lineNumbers = new List<LineNumber>(1) { root };

            while (_numbers.MoveNext() && lineNumbers.Count > 0)
            {
                parity = parity.InvertParity();

                lineNumbers = FindNumbersInLine(++lineNumber, parity, lineNumbers);
            }

            if (lineNumbers.Count == 0)
                throw new ArgumentException("The given sequence of numbers doesn't contain any valid path!");

            return GetResult(lineNumbers, lineNumber);
        }

        private List<LineNumber> FindNumbersInLine(int lineNumber, Parity validParity, List<LineNumber> previousLineNumbers)
        {
            var iterator = new LineIterator(previousLineNumbers);

            var expectedCount = 2 * previousLineNumbers.Count;

            var lineNumbers = new List<LineNumber>(expectedCount > lineNumber ? lineNumber : expectedCount);

            var itemIndex = 0;
            LineNumber related;

            do
            {
                var number = _numbers.Current;

                if (number.GetParity() == validParity && TryFindRelatedNumber(itemIndex, ref iterator, out related))
                {
                    lineNumbers.Add(new LineNumber(itemIndex, number, related));
                }

                if (++itemIndex == lineNumber) return lineNumbers;
            }
            while (_numbers.MoveNext());

            throw new Exception("Unexpected truncation of list of numbers!");
        }

        /// <summary>
        /// Searches for related number in previous line by given index of current line number 
        /// </summary>
        /// <returns>True if was able to locate such number</returns>
        private bool TryFindRelatedNumber(int itemIndex, ref LineIterator iterator,  out LineNumber related)
        {
            related = null;

            if (!iterator.CanIterate) return false;

            do
            {
                var current = iterator.Current;
                var currentIndex = current.Index;

                if (itemIndex < currentIndex) return false;

                // Line j-1:    N(i-1) N(i)
                // Line  j :        N(i)
                // here N(k) - a number with k-th index
                if (itemIndex - 1 == currentIndex)
                {
                    related = iterator.Next;

                    var isNextRelated =
                        related != null &&
                        related.Index == itemIndex && 
                        related.SubTotal >= current.SubTotal;

                    if (!isNextRelated) related = current;

                    return true;
                }

                if (itemIndex == currentIndex)
                {
                    related = iterator.Previous;

                    var isPreviousRelated =
                        related != null &&
                        related.Index == itemIndex -1 &&
                        related.SubTotal > current.SubTotal;

                    if (!isPreviousRelated) related = current;

                    return true;
                }

            } while (iterator.MoveNext());

            return false;
        }

        private Result GetResult(List<LineNumber> lineNumbers, int pathLength)
        {
            var item = lineNumbers.OrderByDescending(x => x.SubTotal).First();

            var sum = item.SubTotal;

            var path = new List<int>(pathLength);

            while(item != null)
            {
                path.Add(item.Value);

                item = item.Parent;
            }

            path.Reverse();

            return new Result(sum, path);
        }

    }
}
