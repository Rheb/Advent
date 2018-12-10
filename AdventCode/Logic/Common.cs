using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode.Logic
{
    #region Common

    public static class Common
    {
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

        public void Add(CoordI other)
        {
            x += other.x;
            y += other.y;
        }

        public bool IsTouching(CoordI other)
        {
            if (x == other.x)
            {
                return y - 1 <= other.y && other.y <= y + 1;
            }

            if (y == other.y)
            {
                return x - 1 <= other.x && other.x <= x + 1;
            }

            return false;
        }

        public static bool operator ==(CoordI a, CoordI b)
        {
            if (a is null)
            {
                return b is null;
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
