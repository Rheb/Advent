using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCode.Logic
{
    internal static class Advent2024
    {
        #region Go_01

        public static void Go_01()
        {
            var input = InputParser.GetLines("24", "01");

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            foreach (string line in input)
            {
                string[] parts = line.Split("   ");
                left.Add(int.Parse(parts[0]));
                right.Add(int.Parse(parts[1]));
            }

            left = left.OrderBy(i => i).ToList();
            right = right.OrderBy(i => i).ToList();

            int diff = 0;

            for (int i = 0; i < left.Count; i++)
            {
                diff += Math.Abs(left[i] - right[i]);
            }

            // Part one
            Console.WriteLine(diff);

            Dictionary<int, int> rightCountByID =
                right.GroupBy(i => i)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                )
            ;

            int diffScore = 0;

            foreach (int i in left)
            {
                if (rightCountByID.ContainsKey(i))
                {
                    diffScore += i * rightCountByID[i];
                }
            }

            Console.WriteLine(diffScore);
        }

        #endregion

        #region Go_02

        public static void Go_02()
        {
            // TODO: Make is safe check func
            // Run with first item removed
            // Run with seccond item removed

            int minDiff = 1;
            int maxDiff = 3;

            var input = InputParser.GetLines("24", "02");

            List<List<int>> lines = 
                input
                .Select(l => 
                    l.Split(' ').Select(str => int.Parse(str))
                    .ToList()
                ).ToList();

            int safeCount = 0;
            int safeWithSingleIgnoreCount = 0;

            foreach (var line in lines)
            {
                bool shouldAscend = true;
                if (line[0] > line[1])
                {
                    shouldAscend = false;
                }

                int diff = 0;
                int absDiff = 0;
                bool isSafe = true;
                bool isSafeWithSingleIgnore = true;
                int ignorePreviousCount = 0;

                for (int i = 1; i < line.Count; i++)
                {
                    if (ignorePreviousCount == 0)
                    {
                        diff = line[i] - line[i - 1];
                    }
                    else if (ignorePreviousCount == 1)
                    {
                        diff = line[i] - line[i - 2];
                    }

                    absDiff = Math.Abs(diff);

                    // Check if not safe
                    if (
                        (shouldAscend && diff < 0)
                        || (!shouldAscend && diff > 0)
                        || (absDiff < 1)
                        || (absDiff > 3)
                    )
                    {
                        isSafe = false;
                        ignorePreviousCount++;

                        if (ignorePreviousCount > 1)
                        {
                            isSafeWithSingleIgnore = false;
                            i = line.Count;
                        }
                    }
                }

                if (isSafe)
                {
                    safeCount++;
                }

                if (isSafeWithSingleIgnore)
                {
                    safeWithSingleIgnoreCount++;
                }
            }

            Console.WriteLine(safeCount);
            Console.WriteLine(safeWithSingleIgnoreCount);
        }

        #endregion
    }
}
