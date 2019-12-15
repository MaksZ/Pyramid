using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Solver
{
    /// <summary>
    /// Describes a parity of a number
    /// </summary>
    internal enum Parity { Even, Odd };

    internal static class ParityHelper
    {
        /// <summary>
        /// Changes parity to opposite to the given value
        /// </summary>
        public static Parity InvertParity(this Parity value)
            =>
                value == Parity.Even ? Parity.Odd : Parity.Even;

        /// <summary>
        /// Calculates parity for a number
        /// </summary>
        public static Parity GetParity(this int number) => (Parity)(number & 0x01);
    }
}
