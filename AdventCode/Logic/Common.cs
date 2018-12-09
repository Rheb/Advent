using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode.Logic
{
    #region Common

    public static class Common
    {
        /// <summary>
        /// Probably buggy: see 2018 Go_03: id 469 
        /// </summary>
        public static bool AreaOverlap(CoordI aTopL, CoordI aBotR, CoordI bTopL, CoordI bBotR)
        {
            if (
                (
                    (IntInBounds(aTopL.x, bTopL.x, bBotR.x) || IntInBounds(aBotR.x, bTopL.x, bBotR.x))
                 && (IntInBounds(aTopL.y, bTopL.y, bBotR.y) || IntInBounds(aBotR.y, bTopL.y, bBotR.y))
                )
             || (
                    (IntInBounds(bTopL.x, aTopL.x, aBotR.x) || IntInBounds(bBotR.x, aTopL.x, aBotR.x))
                 && (IntInBounds(bTopL.y, aTopL.y, aBotR.y) || IntInBounds(bBotR.y, aTopL.y, aBotR.y))
                )
            )
            {
                return true;
            }

            return false;
        }

        public static bool IntInBounds(int x, int min, int max)
        {
            if (x >= min && x <= max)
            {
                return true;
            }

            return false;
        }
    }

    #endregion

    #region IntCoord

    public class CoordI : IEquatable<CoordI>
    {
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;

        public CoordI(int p_iX, int p_iY)
        {
            x = p_iX;
            y = p_iY;
        }

        public static bool operator ==(CoordI a, CoordI b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(CoordI a, CoordI b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            var i = obj as CoordI;
            return i != null &&
                   x == i.x &&
                   y == i.y;
        }

        public bool Equals(CoordI other)
        {
            return other != null &&
                   x == other.x &&
                   y == other.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return x + "," + y;
        }
    }

    #endregion
}
