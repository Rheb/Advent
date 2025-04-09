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
    }
}
