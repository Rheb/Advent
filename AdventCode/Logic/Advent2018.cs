using System;
using System.Collections.Generic;
using System.IO;
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

            int freq = 0;
            int? twice = null;
            int? freqFirst = null;

            List<int> tracked = new List<int>();

            while (twice == null)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    string line = input[i];

                    int num = int.Parse(line.Substring(1));

                    if (line.Contains('+'))
                    {
                        freq += num;
                    }
                    else
                    {
                        freq -= num;
                    }

                    if (tracked.Contains(freq) && twice == null)
                    {
                        twice = freq;
                    }

                    tracked.Add(freq);
                }

                if (freqFirst == null)
                {
                    freqFirst = freq;
                }
            }

            Console.WriteLine(freqFirst);
            Console.WriteLine(twice);
        }

        #endregion

        #region Go_02

        public static void Go_02()
        {
            var input = InputParser.GetLines("18", "02");

            int count2 = 0;
            int count3 = 0;

            string shared = "";

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
                var chars = new Dictionary<char, int>();

                foreach (var c in line)
                {
                    if (chars.ContainsKey(c))
                    {
                        chars[c]++;
                    }
                    else
                    {
                        chars.Add(c, 1);
                    }
                }

                if (chars.Any(x => x.Value == 2))
                {
                    count2++;
                }

                if (chars.Any(x => x.Value == 3))
                {
                    count3++;
                }

                if (i < input.Count-1)
                {
                    for (int j = i + 1; j < input.Count; j++)
                    {
                        string other = input[j];
                        int errors = 0;
                        string tmp = "";

                        for (int ci = 0; ci < line.Length; ci++)
                        {
                            if (line[ci] == other[ci])
                            {
                                tmp += line[ci];
                            }
                            else
                            {
                                errors++;
                                if (errors > 1)
                                {
                                    ci = line.Length;
                                }
                            }
                        }

                        if (errors == 1)
                        {
                            shared = tmp;
                        }
                    }
                }
            }

            int check = count2 * count3;

            Console.WriteLine(check);
            Console.WriteLine(shared);
        }

        #endregion

        #region Go_03

        public static void Go_03()
        {
            var input = InputParser.GetLines("18", "03");

            var claims = new Dictionary<int, FabricClaim>();
            int id = 0;

            int xMax = 0;
            int yMax = 0;

            foreach (var line in input)
            {
                int indexAt = line.IndexOf('@');
                int indexComma = line.IndexOf(',');
                int indexColon = line.IndexOf(':');
                int indexX = line.IndexOf('x');

                id = int.Parse(line.Substring(1, indexAt - 2));

                int topX = int.Parse(line.Substring(indexAt + 2, indexComma - indexAt - 2));
                int topY = int.Parse(line.Substring(indexComma + 1, indexColon - indexComma - 1));
                int botX = topX + int.Parse(line.Substring(indexColon + 2, indexX - indexColon - 2)) - 1;
                int botY = topY + int.Parse(line.Substring(indexX + 1)) - 1;

                if (!claims.ContainsKey(id))
                {
                    claims.Add(id, new FabricClaim(
                        id,
                        new CoordI(topX, topY),
                        new CoordI(botX, botY)
                    ));

                    xMax = Math.Max(xMax, botX);
                    yMax = Math.Max(yMax, botY);
                }
            }

            int overlapCount = 0;
            var InsideCountByCords = new Dictionary<CoordI, List<int>>();

            for (int y = 0; y <= yMax; y++)
            {
                for (int x = 0; x <= xMax; x++)
                {
                    InsideCountByCords.Add(new CoordI(x, y), new List<int>());
                }
            }

            foreach (var claim in claims.Values)
            {
                CoordI it = new CoordI(claim.TopL.x, claim.TopL.y);

                while (it.y <= claim.BotR.y)
                {
                    while (it.x <= claim.BotR.x)
                    {
                        InsideCountByCords[it].Add(claim.id);

                        if (InsideCountByCords[it].Count > 1)
                        {
                            claim.FoundOverlap = true;
                        }
                        if (InsideCountByCords[it].Count == 2)
                        {
                            claims[InsideCountByCords[it][0]].FoundOverlap = true;
                            overlapCount++;
                        }

                        it.x++;
                    }

                    it.x = claim.TopL.x;
                    it.y++;
                }
            }

            var clean = claims.Where(claim => !claim.Value.FoundOverlap).Select(claim => claim.Key).ToList();

            int cleanId = clean.Last();

            Console.WriteLine(overlapCount);
            Console.WriteLine(cleanId);
        }

        #endregion

        #region Go_04

        public static void Go_04()
        {
            var input = InputParser.GetLines("18", "04");

            input.Sort();
            var guards = new Dictionary<int, SleepyGurad>();
            int id = 0;

            foreach (var line in input)
            {
                int dtStart = line.IndexOf('[') + 1;
                int dtLen = line.IndexOf(']') - dtStart;

                int guardIndex = line.IndexOf('#');

                DateTime timestamp = DateTime.Parse(line.Substring(dtStart, dtLen));

                if (guardIndex > 0)
                {
                    guardIndex++;
                    int guardLen = line.IndexOf(" begins") - guardIndex;

                    id = int.Parse(line.Substring(guardIndex, guardLen));

                    if (!guards.ContainsKey(id))
                    {
                        guards.Add(id, new SleepyGurad(id));
                    }

                    guards[id].Start.Add(timestamp);
                }
                else if (line.Contains("asleep"))
                {
                    guards[id].Sleep.Add(timestamp);
                }
                else
                {
                    DateTime fellAsleep = guards[id].Sleep.Last();
                    for (int i = fellAsleep.Minute; i < timestamp.Minute; i++)
                    {
                        guards[id].SleepyMinutes[i]++;
                    }

                    guards[id].TotalSleepTime += (int)(timestamp - fellAsleep).TotalMinutes;
                    guards[id].Wake.Add(timestamp);
                }
            }

            SleepyGurad sleepy = guards.Values.Aggregate((a, b) => a.TotalSleepTime > b.TotalSleepTime ? a : b);

            int minute = sleepy.SleepyMinutes.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;

            SleepyGurad topGuard = new SleepyGurad(-1);
            int topMinute = 0;
            foreach (var guard in guards.Values)
            {
                int top = guard.SleepyMinutes.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;

                if (guard.SleepyMinutes[top] > topGuard.SleepyMinutes[topMinute])
                {
                    topGuard = guard;
                    topMinute = top;
                }
            }

            Console.WriteLine(minute * sleepy.id);
            Console.WriteLine(topMinute * topGuard.id);
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

            var coords = new List<AreaBoundCoord>();

            int xMax = 0;
            int yMax = 0;

            foreach (var line in input)
            {
                var nums = line.Split(", ");
                int x = int.Parse(nums[0]);
                int y = int.Parse(nums[1]);

                xMax = Math.Max(xMax, x);
                yMax = Math.Max(yMax, y);

                coords.Add(new AreaBoundCoord(x, y));
            }

            int centerCount = 0;
            void CheckCoords(int x, int y)
            {
                AreaBoundCoord closest = null;
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

            int iLast = input[0].IndexOf("worth") + 6;
            int playerCount = int.Parse(input[0].Substring(0, input[0].IndexOf(" play")));
            int lastScore = int.Parse(input[0].Substring(iLast, input[0].IndexOf(" points") - iLast)); ;

            long maxScoreLow = 0;

            List<long> playerScores = new List<long>();
            playerScores.AddRange(Enumerable.Repeat(0L, playerCount));

            LinkedList<int> placedMarbles = new LinkedList<int>();
            var currentNode = placedMarbles.AddFirst(0);
            currentNode = placedMarbles.AddAfter(currentNode, 1);
            currentNode = placedMarbles.AddBefore(currentNode, 2);
            var removeNode = currentNode;

            int marble = placedMarbles.Count;
            while (marble <= lastScore * 100)
            {
                if (marble % 23 == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        currentNode = currentNode.Previous;
                        if (currentNode == null)
                        {
                            currentNode = placedMarbles.Last;
                        }
                    }

                    removeNode = currentNode;

                    currentNode = currentNode.Next;
                    if (currentNode == null)
                    {
                        currentNode = placedMarbles.Last;
                    }

                    playerScores[marble % playerCount] += marble + removeNode.Value;
                    placedMarbles.Remove(removeNode);
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        currentNode = currentNode.Next;
                        if (currentNode == null)
                        {
                            currentNode = placedMarbles.First;
                        }
                    }

                    currentNode = placedMarbles.AddAfter(currentNode, marble);
                }

                if (marble == lastScore)
                {
                    maxScoreLow = playerScores.Aggregate((a, b) => Math.Max(a, b));
                }

                marble++;
            }

            long maxScore = playerScores.Aggregate((a, b) => Math.Max(a, b));

            Console.WriteLine(maxScoreLow);
            Console.WriteLine(maxScore);
        }

        #endregion

        #region Go_10

        public static void Go_10()
        {
            var input = InputParser.GetLines("18", "10");

            List<MovingPoint> points = new List<MovingPoint>();
            int touchCount = 0;

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];

                var split = line.Split("=<");

                var sP = split[1].Split(">")[0].Split(", ");
                var sV = split[2].Split(">")[0].Split(", ");

                points.Add(new MovingPoint(
                    new CoordI(int.Parse(sP[0]), int.Parse(sP[1])),
                    new CoordI(int.Parse(sV[0]), int.Parse(sV[1]))
                ));
            }

            int iteration = Math.Abs(points[0].Pos.x / points[0].Vel.x) - 150;

            foreach (var point in points)
            {
                point.Pos.x += point.Vel.x * iteration;
                point.Pos.y += point.Vel.y * iteration;
            }

            while (touchCount < points.Count - 10)
            {
                iteration++;

                foreach (var point in points)
                {
                    point.Pos.Add(point.Vel);
                    point.FoundTouhing = false;
                }

                for (int i = 0; i < points.Count - 1; i++)
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        if (points[i].Pos.IsTouching(points[j].Pos))
                        {
                            points[i].FoundTouhing = true;
                            points[j].FoundTouhing = true;
                        }
                    }
                }

                touchCount = points.Aggregate(0, (c, pt) => c + (pt.FoundTouhing ? 1 : 0));
            }

            points = points.OrderBy(pt => pt.Pos.y).ThenBy(pt => pt.Pos.x).ToList();

            int yFrom = points.Aggregate((a, b) => a.Pos.y < b.Pos.y ? a : b).Pos.y;
            int yTo = points.Aggregate((a, b) => a.Pos.y > b.Pos.y ? a : b).Pos.y;

            int xFrom = points.Aggregate((a, b) => a.Pos.x < b.Pos.x ? a : b).Pos.x;
            int xTo = points.Aggregate((a, b) => a.Pos.x > b.Pos.x ? a : b).Pos.x;

            for (int y = yFrom; y <= yTo; y++)
            {
                for (int x = xFrom; x <= xTo; x++)
                {
                    bool match = false;
                    while (
                        points.Count > 0
                        && points[0].Pos.x == x
                        && points[0].Pos.y == y)
                    {
                        points.RemoveAt(0);

                        match = true;
                    }

                    if (match)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }

                }

                Console.Write('\n');
            }

            Console.WriteLine();
            Console.WriteLine(iteration);
        }

        #endregion

        #region Go_11

        public static void Go_11()
        {
            var input = InputParser.GetLines("18", "11");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_12

        public static void Go_12()
        {
            var input = InputParser.GetLines("18", "12");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_13

        public static void Go_13()
        {
            var input = InputParser.GetLines("18", "13");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_14

        public static void Go_14()
        {
            var input = InputParser.GetLines("18", "14");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_15

        public static void Go_15()
        {
            var input = InputParser.GetLines("18", "15");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_16

        public static void Go_16()
        {
            var input = InputParser.GetLines("18", "16");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_17

        public static void Go_17()
        {
            var input = InputParser.GetLines("18", "17");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_18

        public static void Go_18()
        {
            var input = InputParser.GetLines("18", "18");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_19

        public static void Go_19()
        {
            var input = InputParser.GetLines("18", "19");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_20

        public static void Go_20()
        {
            var input = InputParser.GetLines("18", "20");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_21

        public static void Go_21()
        {
            var input = InputParser.GetLines("18", "21");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_22

        public static void Go_22()
        {
            var input = InputParser.GetLines("18", "22");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_23

        public static void Go_23()
        {
            var input = InputParser.GetLines("18", "23");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_24

        public static void Go_24()
        {
            var input = InputParser.GetLines("18", "24");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

        #region Go_25

        public static void Go_25()
        {
            var input = InputParser.GetLines("18", "25");

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
            }

            Console.WriteLine("");
        }

        #endregion

    }
}
