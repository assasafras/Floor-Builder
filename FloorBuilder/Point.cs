using FloorBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    public struct Point
    {
        private static Random r = new Random();

        public int X { get { return x; } set { x = value; } }
        private int x;
        public int Y { get { return y; } set { y = value; } }
        private int y;

        public Point(int initX, int initY) : this()
        {
            X = initX;
            Y = initY;
        }
        public static Point Left(int magnitude = 1)
        {
            return new Point(-Math.Abs(magnitude), 0);
        }

        public static Point Right(int magnitude = 1)
        {
            return new Point(Math.Abs(magnitude), 0);
        }

        public static Point Up(int magnitude = 1)
        {
            return new Point(0, -Math.Abs(magnitude));
        }

        public static Point Down(int magnitude = 1)
        {
            return new Point(0, Math.Abs(magnitude));
        }

        public static Point operator *(Point p, int m)
        {
            return new Point(p.X * m, p.Y * m);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", X, Y);
        }

        public static Point RandomDirection(int min, int max, DirectionFlag flag)
        {
            Point pt = new Point();
            var x = r.Next(min, max);
            var y = r.Next(min, max);

            var z = r.Next(min, max);

            switch (flag)
            {
                case DirectionFlag.AllowDiagonals:
                    pt = new Point(x, y);
                    break;
                case DirectionFlag.NoDiagonals:
                    var dir = r.Next(100);
                    if      (dir < 25) pt = Left();
                    else if (dir < 50) pt = Right();
                    else if (dir < 75) pt = Up();
                    else               pt = Down();
                    break;
            }
            return pt;
        }

        internal static int Difference(Point a, Point b)
        {
            var cSquared = (Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2));
            return (int) Math.Round(Math.Sqrt(cSquared));
        }

        public static Point RandomPoint(int Width, int Height)
        {
            return new Point(r.Next(Width), r.Next(Height));
        }
    }
}
