using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode.Logic
{
    public static class InputParser
    {
        public static List<string> GetLines(int p_iYear, int p_iDay)
        {
            return File.ReadAllLines(@"Input\" + p_iYear.ToString("D2") + @"\" + p_iDay.ToString("D2") + ".txt").ToList();
        }
    }
}
