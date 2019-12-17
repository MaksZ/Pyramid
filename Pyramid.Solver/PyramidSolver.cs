using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    /// <summary>
    /// Solves pyramid task in case when all input is not known at once
    /// (like they would scanned one by one from each line (from left to right) moving from top line to bottom one)
    /// </summary>
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

        /// <summary>
        /// Implements search algorithm;
        /// </summary>
        /// <remarks>
        /// From the given input we collect only numbers that can form a valid path;
        /// each such number is stored together with calculated subtotal and 
        /// is linked to a valid (parent) number from previous line;
        /// so when the last number of the input is traversed (i.e. no more input data), 
        /// we will just need to choose such number selected in the last line number, 
        /// that has a maximum subtotal and follow links to reach the top element, 
        /// to restore all the path.
        /// </remarks>
        private Result Solve()
        {
            var root = LineNumber.AsRoot(_numbers.Current);
            var parity = root.Value.GetParity();

            var lineNumber = 1;
            var pathEndings = new List<LineNumber>(1) { root };

            while (_numbers.MoveNext())
            {
                parity = parity.InvertParity();

                pathEndings = FindNumbersInLine(++lineNumber, parity, pathEndings);

                if (pathEndings.Count == 0)
                    throw new ArgumentException("The given sequence of numbers doesn't contain any valid path!");
            }

            return GetResult(pathEndings, lineNumber);
        }

        /// <summary>
        /// Looks for valid (according given parity) numbers and tries to link them with already found numbers in previous line
        /// </summary>
        /// <param name="lineNumber">Denotes current line number (also servses as a count of numbers in the line)</param>
        /// <param name="validParity">Parity that numbers have to match</param>
        /// <param name="previousLineNumbers">valid numbers from previous line</param>
        /// <returns></returns>
        private List<LineNumber> FindNumbersInLine(int lineNumber, Parity validParity, List<LineNumber> previousLineNumbers)
        {
            var iterator = new LineIterator(previousLineNumbers);

            var expectedCount = 2 * previousLineNumbers.Count;

            var pathEndings = new List<LineNumber>(expectedCount > lineNumber ? lineNumber : expectedCount);

            var itemIndex = 0;
            LineNumber related;

            do
            {
                // read a number from the input
                var number = _numbers.Current;

                // validate the number and try to link to one from previous line
                if (number.GetParity() == validParity && TryFindRelatedNumber(itemIndex, ref iterator, out related))
                {
                    pathEndings.Add(new LineNumber(itemIndex, number, related));
                }

                // when all numbers of a line are traversed, we return sub-result
                if (++itemIndex == lineNumber) return pathEndings;
            }
            while (_numbers.MoveNext());

            // error: the line does not contain a proper count of numbers
            throw new ArgumentException("Input doesn't contain all the numbers to complete the task!");
        }

        /// <summary>
        /// Searches for related number in previous line by given index of a number in current line
        /// </summary>
        /// <remarks>
        /// Line j-1:    N(i-1) N(i)
        /// Line  j :        N(i)
        /// 
        /// here N(k) denotes some number with index k,
        /// 
        /// a number N(i) from line j IS in relation only with a number N(i-1) or N(i) from line j-1,
        /// (a number N(0) as well as N(j) has only one relation, all other have two relations)
        /// </remarks>
        /// <returns>True, if was able to locate such number</returns>
        private bool TryFindRelatedNumber(int itemIndex, ref LineIterator iterator,  out LineNumber related)
        {
            related = null;

            if (!iterator.CanIterate) return false;

            do
            {
                // take a number from previous line, that is not yet linked
                var current = iterator.Current;
                var currentIndex = current.Index;

                if (itemIndex < currentIndex) return false;

                if (itemIndex - 1 == currentIndex)
                {
                    // case when for a number N(i) in line j (i == itemIndex)
                    // found number N(i-1) in line j-1
                    related = iterator.Next;

                    // before return we need to check, if N(i) in line j-1 is also selected as valid;
                    // if both exists, then we choose for relation one, having bigger subtotal
                    var isNextRelated =
                        related != null &&
                        related.Index == itemIndex && 
                        related.SubTotal >= current.SubTotal;

                    if (!isNextRelated) related = current;

                    return true;
                }

                if (itemIndex == currentIndex)
                {
                    // case when for a number N(i) in line j (i == itemIndex)
                    // found number N(i) in line j-1, 
                    // and if we are here, than N(i-1) is not selected as valid
                    related = current;
                    return true;
                }

            } while (iterator.MoveNext());

            return false;
        }

        /// <summary>
        /// Prepares the output
        /// </summary>
        private Result GetResult(List<LineNumber> pathEndings, int pathLength)
        {
            var item = pathEndings.OrderByDescending(x => x.SubTotal).First();

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
