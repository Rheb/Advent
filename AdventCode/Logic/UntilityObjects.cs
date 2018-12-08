using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode.Logic
{
    #region MetadataTree

    public class MetadataTree
    {
        public List<MetadataTree> Children { get; set; } = new List<MetadataTree>();
        public List<int> DataItems { get; set; } = new List<int>();

        public int Val { get; set; } = 0;
    }

    #endregion

    #region OrderedStep

    public class OrderedStep
    {
        public char Key { get; set; } = '0';
        public bool FastComplete { get; set; } = false;

        public bool SlowComplete { get; set; } = false;
        public int SecondsLeft { get; set; } = 0;

        public List<OrderedStep> DependsOn { get; set; } = new List<OrderedStep>();

        public OrderedStep(char p_sKey)
        {
            Key = p_sKey;
            SecondsLeft = 60 + Key - 64;
        }
    }

    #endregion

    #region IntCoord

    public class IntCoord
    {
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;

        public bool IsBounded { get; set; } = true;
        public int ClosestCount { get; set; } = 0;

        public IntCoord(int p_iX, int p_iY)
        {
            x = p_iX;
            y = p_iY;
        }
    }

    #endregion
}
