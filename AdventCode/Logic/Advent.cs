using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCode.Logic
{
    public static class Advent2018
    {
        #region Go_01

        public static void Go_01()
        {
            var input = InputParser.GetLines("18", "01");

            Console.WriteLine("");
        }

        #endregion

        #region Go_02

        public static void Go_02()
        {
            var input = InputParser.GetLines("18", "02");

            Console.WriteLine("");
        }

        #endregion

        #region Go_03

        public static void Go_03()
        {
            var input = InputParser.GetLines("18", "03");

            Console.WriteLine("");
        }

        #endregion

        #region Go_04

        public static void Go_04()
        {
            var input = InputParser.GetLines("18", "04");

            Console.WriteLine("");
        }

        #endregion

        #region Go_05

        public static void Go_05()
        {
            var input = InputParser.GetLines("18", "05");

            List<char> chars = input[0].ToCharArray().ToList();

            int React()
            {
                int count = 0;

                for (int i = chars.Count - 1; i > 0; i--)
                {
                    if (
                        chars[i] != chars[i-1]
                     && char.ToUpper(chars[i]) == char.ToUpper(chars[i-1])
                    )
                    {
                        count++;
                        chars.RemoveAt(i);
                        chars.RemoveAt(i-1);
                        i--;
                    }
                }

                return count;
            }

            int reactions = React();
            while (reactions > 0)
            {
                reactions = React();
            }

            int shortest = int.MaxValue;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                chars = input[0].ToCharArray().Where(x => char.ToUpper(x) != c).ToList();
                
                while (React() > 0) { }

                shortest = Math.Min(chars.Count, shortest);
            }
            
            Console.WriteLine(chars.Count);
            Console.WriteLine(shortest);
        }

        #endregion

        #region Go_06

        public static void Go_06()
        {
            var input = InputParser.GetLines("18", "06");

            var coords = new List<IntCoord>();

            int xMax = 0;
            int yMax = 0;

            foreach (var line in input)
            {
                var nums = line.Split(", ");
                int x = int.Parse(nums[0]);
                int y = int.Parse(nums[1]);

                xMax = Math.Max(xMax, x);
                yMax = Math.Max(yMax, y);

                coords.Add(new IntCoord(x, y));
            }

            int centerCount = 0;
            void CheckCoords(int x, int y)
            {
                IntCoord closest = null;
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

        #region Go_07

        public static void Go_07()
        {
            var input = InputParser.GetLines("18", "07");

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

        #region Go_08

        public static void Go_08()
        {
            var input = InputParser.GetLines("18", "08");

            int totalData = 0;
            var numbers = input[0].Split(' ').Select(x => int.Parse(x)).ToList();
            var index = 0;

            void Read(MetadataTree tree)
            {
                int childsLeft = numbers[index];
                index++;
                int dataLeft = numbers[index];
                index++;

                while (childsLeft > 0)
                {
                    MetadataTree child = new MetadataTree();
                    Read(child);
                    tree.Children.Add(child);
                    childsLeft--;
                }
                while (dataLeft > 0)
                {
                    tree.DataItems.Add(numbers[index]);
                    totalData += numbers[index];

                    index++;
                    dataLeft--;
                }
            }

            void Calc(MetadataTree tree)
            {
                if (tree.Children.Count == 0)
                {
                    tree.Val = tree.DataItems.Aggregate(0, (a, b) => a + b);
                }
                else
                {
                    foreach (var child in tree.Children)
                    {
                        Calc(child);
                    }

                    foreach (int i in tree.DataItems)
                    {
                        if (i <= tree.Children.Count)
                        {
                            tree.Val += tree.Children[i - 1].Val;
                        }
                    }
                }
            }

            MetadataTree root = new MetadataTree();
            Read(root);
            Calc(root);

            Console.WriteLine(totalData);
            Console.WriteLine(root.Val);
        }

        #endregion

        #region Go_09

        public static void Go_09()
        {
            var input = InputParser.GetLines("18", "09");

            Console.WriteLine("");
        }

        #endregion

        #region Go_10

        public static void Go_10()
        {
            var input = InputParser.GetLines("18", "10");

            Console.WriteLine("");
        }

        #endregion

        #region Go_11

        public static void Go_11()
        {
            var input = InputParser.GetLines("18", "11");

            Console.WriteLine("");
        }

        #endregion

        #region Go_12

        public static void Go_12()
        {
            var input = InputParser.GetLines("18", "12");

            Console.WriteLine("");
        }

        #endregion

        #region Go_13

        public static void Go_13()
        {
            var input = InputParser.GetLines("18", "13");

            Console.WriteLine("");
        }

        #endregion

        #region Go_14

        public static void Go_14()
        {
            var input = InputParser.GetLines("18", "14");

            Console.WriteLine("");
        }

        #endregion

        #region Go_15

        public static void Go_15()
        {
            var input = InputParser.GetLines("18", "15");

            Console.WriteLine("");
        }

        #endregion

        #region Go_16

        public static void Go_16()
        {
            var input = InputParser.GetLines("18", "16");

            Console.WriteLine("");
        }

        #endregion

        #region Go_17

        public static void Go_17()
        {
            var input = InputParser.GetLines("18", "17");

            Console.WriteLine("");
        }

        #endregion

        #region Go_18

        public static void Go_18()
        {
            var input = InputParser.GetLines("18", "18");

            Console.WriteLine("");
        }

        #endregion

        #region Go_19

        public static void Go_19()
        {
            var input = InputParser.GetLines("18", "19");

            Console.WriteLine("");
        }

        #endregion

        #region Go_20

        public static void Go_20()
        {
            var input = InputParser.GetLines("18", "20");

            Console.WriteLine("");
        }

        #endregion

        #region Go_21

        public static void Go_21()
        {
            var input = InputParser.GetLines("18", "21");

            Console.WriteLine("");
        }

        #endregion

        #region Go_22

        public static void Go_22()
        {
            var input = InputParser.GetLines("18", "22");

            Console.WriteLine("");
        }

        #endregion

        #region Go_23

        public static void Go_23()
        {
            var input = InputParser.GetLines("18", "23");

            Console.WriteLine("");
        }

        #endregion

        #region Go_24

        public static void Go_24()
        {
            var input = InputParser.GetLines("18", "24");

            Console.WriteLine("");
        }

        #endregion

        #region Go_25

        public static void Go_25()
        {
            var input = InputParser.GetLines("18", "25");

            Console.WriteLine("");
        }

        #endregion

    }
}
