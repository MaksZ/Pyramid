using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    internal static class Helper
    {
        public static PyramidSolver.Parity InvertParity(this PyramidSolver.Parity value)
            =>
                value == PyramidSolver.Parity.Even
                ? PyramidSolver.Parity.Odd
                : PyramidSolver.Parity.Even;

        public static PyramidSolver.Parity GetParity(this int value)
            =>
                (PyramidSolver.Parity)(value & 0x01);
         

    }
}
