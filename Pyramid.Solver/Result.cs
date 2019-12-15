using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    /// <summary>
    /// Holds a result of calculation
    /// </summary>
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
