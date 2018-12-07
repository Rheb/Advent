using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode
{
    public static class InputParser
    {
        public static List<string> GetLines(int p_iDay)
        {
            return File.ReadAllLines(@"Input\" + p_iDay + ".txt").ToList();
        }

        public static string GetFirstLine(int p_iDay)
        {
            return GetLines(p_iDay)[0];
        }

        public static List<string> SplitLine(string p_sLine)
        {
            List<string> output = new List<string>();
            foreach (string word in p_sLine.Split(' '))
            {
                output.Add(word.Trim());
            }
            return output;
        }

        public static List<string> GetWords(int p_iDay)
        {
            List<string> lines = GetLines(p_iDay);

            List<string> output = new List<string>();
            foreach (string line in lines)
            {
                output.AddRange(SplitLine(line));
            }

            return output;
        }

        public static List<string> GetUniqueWords(int p_iDay)
        {
            List<string> input = GetWords(p_iDay);
            List<string> output = new List<string>();
            foreach (string word in input)
            {
                if (!output.Contains(word))
                {
                    output.Add(word);
                }
            }

            return output;
        }

        public static bool LettersMatches(char[] first, char[] second)
        {
            List<char> check = second.ToList();
            foreach (char c in first)
            {
                if (check.Contains(c))
                {
                    check.Remove(c);
                }
                else
                {
                    return false;
                }
            }

            return check.Count == 0;
        }
    }
}
