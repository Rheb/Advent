using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode.Logic
{
    #region MovingPoint

    public class MovingPoint
    {
        public bool FoundTouhing { get; set; } = false;

        public CoordI Pos { get; set; }
        public CoordI Vel { get; set; }

        public MovingPoint(CoordI p_cPos, CoordI p_cVel)
        {
            Pos = p_cPos;
            Vel = p_cVel;
        }
    }

    #endregion

    #region FabricClaim

    public class FabricClaim
    {
        public int id { get; set; } = 0;
        public bool FoundOverlap { get; set; } = false;

        public CoordI TopL { get; set; }
        public CoordI BotR { get; set; }

        public FabricClaim(int p_iID, CoordI p_cTopL, CoordI p_cBotR)
        {
            id = p_iID;
            TopL = p_cTopL;
            BotR = p_cBotR;
        }
    }

    #endregion

    #region SleepyGurad

    public class SleepyGurad
    {
        public int id { get; set; } = 0;

        public int TotalSleepTime { get; set; } = 0;

        public List<DateTime> Start { get; set; } = new List<DateTime>();
        public List<DateTime> Sleep { get; set; } = new List<DateTime>();
        public List<DateTime> Wake { get; set; } = new List<DateTime>();

        public Dictionary<int, int> SleepyMinutes { get; set; } = new Dictionary<int, int>();

        public SleepyGurad(int p_iID)
        {
            id = p_iID;

            for (int i = 0; i < 60; i++)
            {
                SleepyMinutes.Add(i, 0);
            }
        }
    }

    #endregion

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

    #region AreaBoundCoord

    public class AreaBoundCoord : CoordI
    {
        public bool IsBounded { get; set; } = true;
        public int ClosestCount { get; set; } = 0;

        public AreaBoundCoord(int p_iX, int p_iY)
            : base(p_iX, p_iY)
        {
        }
    }

    #endregion
}
