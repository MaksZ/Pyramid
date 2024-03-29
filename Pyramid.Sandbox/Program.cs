﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var result = Solver.PyramidSolver.Solve(TestData2);

                Console.WriteLine("Output:");
                Console.WriteLine($"Max sum: {result.Sum}");
                Console.WriteLine($"Path: {string.Join(", ", result.Path)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed: {e}");
            }

            Console.ReadKey();
        }

        private static int[] TestData = new int[] 
        {
            1,
            8, 9,
            1, 5, 9,
            4, 5, 2, 3
        };

        private static int[] TestData2 = new int[]
        {
             1,
             2, 10,
            11,  5, 4,
             8, 12, 6, 6,
            13,  9, 5, 6,  7,
             3, 10, 5, 6,  2, 1,
             3, 10, 5, 6,  2, 1, 1,
             3, 10, 5, 6,  2, 1, 1,  2,
             3, 10, 5, 6, 23, 1, 1, 23, 3,
             3, 10, 5, 6,  2, 1, 2,  3, 5, 6
        };

        private static int[] Data = new int[]
        {
            215,
            192, 124,
            117, 269, 442,
            218, 836, 347, 235,
            320, 805, 522, 417, 345,
            229, 601, 728, 835, 133, 124,
            248, 202, 277, 433, 207, 263, 257,
            359, 464, 504, 528, 516, 716, 871, 182,
            461, 441, 426, 656, 863, 560, 380, 171, 923,
            381, 348, 573, 533, 448, 632, 387, 176, 975, 449,
            223, 711, 445, 645, 245, 543, 931, 532, 937, 541, 444,
            330, 131, 333, 928, 376, 733, 017, 778, 839, 168, 197, 197,
            131, 171, 522, 137, 217, 224, 291, 413, 528, 520, 227, 229, 928,
            223, 626, 034, 683, 839, 052, 627, 310, 713, 999, 629, 817, 410, 121,
            924, 622, 911, 233, 325, 139, 721, 218, 253, 223, 107, 233, 230, 124, 233
        };
    }
}
