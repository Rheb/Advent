using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCode.Logic
{
    public static class Advent
    {
        #region 2018-12-06

        public static void Go_18_06()
        {
            var input = InputParser.GetLines(18, 6);

            var coords = new List<ICoord>();

            int xMax = 0;
            int yMax = 0;

            foreach (var line in input)
            {
                var nums = line.Split(", ");
                int x = int.Parse(nums[0]);
                int y = int.Parse(nums[1]);

                xMax = Math.Max(xMax, x);
                yMax = Math.Max(yMax, y);

                coords.Add(new ICoord(x, y));
            }

            int centerCount = 0;
            void CheckCoords(int x, int y)
            {
                ICoord closest = null;
                int currentDist = int.MaxValue;
                int dist = 0;

                int totalDists = 0;

                foreach(var coord in coords)
                {
                    dist = Math.Abs(x - coord.x) + Math.Abs(y - coord.y);
                    totalDists += dist;

                    if (dist < currentDist)
                    {
                        currentDist = dist;
                        closest = coord;
                    }
                    else if (dist == currentDist)
                    {
                        closest = null;
                    }
                }

                // Out of bounds?
                if (
                    x < 0
                    || y < 0
                    || x > xMax
                    || y > yMax
                )
                {
                    if (closest != null)
                    {
                        closest.IsBounded = false;
                    }
                }
                else
                {
                    if (closest != null)
                    {
                        closest.ClosestCount++;
                    }

                    if (totalDists < 10000)
                    {
                        centerCount++;
                    }
                }
            }


            for (int y = -1; y <= yMax + 1; y++)
            {
                for (int x = -1; x <= xMax + 1; x++)
                {
                    CheckCoords(x, y);
                }
            }

            int maxCount = coords.Where(c => c.IsBounded).Aggregate(0, (a, b) => Math.Max(a, b.ClosestCount));

            Console.WriteLine(maxCount);
            Console.WriteLine(centerCount);
        }

        #endregion

        #region 2018-12-07

        public static void Go_18_07()
        {
            var input = InputParser.GetLines(18, 7);
            string order = "";

            Dictionary<char, OrderedStep> Steps = new Dictionary<char, OrderedStep>();

            foreach(var line in input)
            {
                var pre = line[5];
                var after = line[36];

                if (!Steps.ContainsKey(pre))
                {
                    Steps.Add(pre, new OrderedStep(pre));
                }
                if (!Steps.ContainsKey(after))
                {
                    Steps.Add(after, new OrderedStep(after));
                }

                Steps[after].DependsOn.Add(Steps[pre]);
            }

            char GetNextStep()
            {
                var keys = new List<char>(Steps.Keys);

                keys = keys.Where(key =>
                    !Steps[key].FastComplete
                    && Steps[key].DependsOn.All(dep => dep.FastComplete)
                ).ToList();

                if (keys.Count > 0)
                {
                    keys.Sort();

                    Steps[keys[0]].FastComplete = true;
                    return keys[0];
                }

                return '0';
            }

            char next = GetNextStep();
            while (next != '0')
            {
                order += next;
                next = GetNextStep();
            }

            bool Tick5ActiveSteps()
            {
                var keys = new List<char>(Steps.Keys);

                keys = keys.Where(key =>
                    !Steps[key].SlowComplete
                    && Steps[key].DependsOn.All(dep => dep.SlowComplete)
                ).ToList();

                if (keys.Count > 0)
                {
                    keys.Sort();
                    keys = keys.Take(5).ToList();

                    keys.ForEach(key =>
                    {
                        Steps[key].SecondsLeft--;
                        if (Steps[key].SecondsLeft == 0)
                        {
                            Steps[key].SlowComplete = true;
                        }
                    });

                    return Steps.Any(pair => pair.Value.SlowComplete == false);
                }

                return false;
            }

            int ticks = 0;
            bool keepTickin = true;
            while (keepTickin)
            {
                ticks++;
                keepTickin = Tick5ActiveSteps();
            }

            Console.WriteLine(order);
            Console.WriteLine(ticks);
        }

        #endregion
    }
}
