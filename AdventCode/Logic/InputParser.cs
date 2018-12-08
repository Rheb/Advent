using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode.Logic
{
    public static class InputParser
    {
        public static List<string> GetLines(string p_sYear, string p_sDay)
        {
            return File.ReadAllLines(@"Input\" + p_sYear + @"\" + p_sDay + ".txt").ToList();
        }
    }
}
