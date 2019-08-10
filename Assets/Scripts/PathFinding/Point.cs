
namespace PathFind
{
    /**
    * A 2d point on the grid
    */
    public class Point
    {
        public readonly int X;
        public readonly int Y;

        public Point()
        {
            X = 0;
            Y = 0;
        }
        public Point(int iX, int iY)
        {
            this.X = iX;
            this.Y = iY;
        }

        public Point(Point b)
        {
            X = b.X;
            Y = b.Y;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override bool Equals(System.Object obj)
        {
            // Unlikely to compare incorrect type so removed for performance
            // if (!(obj.GetType() == typeof(PathFind.Point)))
            //     return false;
            Point p = (Point)obj;

            if (ReferenceEquals(null, p))
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public bool Equals(Point p)
        {
            if (ReferenceEquals(null, p))
            {
                return false;
            }
            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(null, a))
            {
                return false;
            }
            if (ReferenceEquals(null, b))
            {
                return false;
            }
            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}
