using System;
using System.Drawing;

namespace WFControlLibrary
{
    public static class PointWorker
    {
        public enum Side { Up, Down, Left, Right, Nowhere }

        public static Point Add(this Point point, Point p)
        {
            return new Point(point.X + p.X, point.Y + p.Y);
        }
        public static Point Sub(this Point point, Point p)
        {
            return new Point(point.X - p.X, point.Y - p.Y);
        }
        public static Point Multiply(this Point point, float factor)
        {
            return new Point((int)(point.X * factor), (int)(point.Y * factor));
        }
        public static Point Multiply(this Point point, float xFactor,float yFactor)
        {
            return new Point((int)(point.X * xFactor), (int)(point.Y * yFactor));
        }
        public static Point Perpendicular(this Point point)
        {
            return new Point(point.Y, -point.X);
        }
        public static Side WhereIAm(this Point point, Rectangle rect)
        {
            if (rect.Size.Width == 0 || rect.Size.Height == 0)
                return Side.Nowhere;
            var rect_centr = rect.Location.Add(((Point)rect.Size).Multiply(0.5f));
            var to_leftUp = rect.Location.Sub(rect_centr);
            var to_leftDown = new Point(to_leftUp.X, to_leftUp.Y + rect.Size.Height);
            var normalize_to_leftUp = to_leftUp.Normalize();
            var normalize_to_leftDown = to_leftDown.Normalize();
            var normalize_to_point = point.Sub(rect_centr).Normalize();
            if (normalize_to_point.Y < normalize_to_leftUp.Y)
                return Side.Up;
            else if (normalize_to_point.Y > normalize_to_leftDown.Y)
                return Side.Down;
            else if (normalize_to_point.X > 0)
                return Side.Right;
            else if (normalize_to_point.X < 0)
                return Side.Left;
            else return Side.Nowhere;
        }

        public static PointF Add(this PointF point, PointF p)
        {
            return new PointF(point.X + p.X, point.Y + p.Y);
        }
        public static PointF Sub(this PointF point, PointF p)
        {
            return new PointF(point.X - p.X, point.Y - p.Y);
        }
        public static PointF Multiply(this PointF point, float factor)
        {
            return new PointF(point.X * factor, point.Y * factor);
        }
        public static PointF Multiply(this PointF point, float xFactor, float yFactor)
        {
            return new PointF(point.X * xFactor, point.Y * yFactor);
        }
        public static PointF Perpendicular(this PointF point)
        {
            return new PointF(point.Y, -point.X);
        }
        public static Point ToPoint(this PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        public static PointF Normalize(this Point point)
        {
            return Normalize(new PointF(point.X,point.Y));
        }
        public static PointF Normalize(this PointF point)
        {
            if (point.X == 0 && point.Y == 0)
                throw new ArgumentException("Zero vector");
            float c = (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
            float x = point.X / c; float y = point.Y / c;
            return new PointF(x, y);
        }

        public static Point GetIntersectionPoint(Point p1, Point guideVector1, Point p2, Point guideVector2)
        {
            int A1 = guideVector1.Y; int B1 = -guideVector1.X; int C1 = p1.Y * guideVector1.X - p1.X * guideVector1.Y;
            int A2 = guideVector2.Y; int B2 = -guideVector2.X; int C2 = p2.Y * guideVector2.X - p2.X * guideVector2.Y;
            int x, y;
            if (new PointF(A1,B1).Normalize() == new PointF(A2,B2).Normalize())
                throw new ArgumentException("Lines are parallel");//сонаправленные векторы
            else if ((A1 == 0 && B1 == 0) || (A2 == 0 && B2 == 0))
                throw new ArgumentException("Zero guide vector");//нулевой вектор
            else if (A1 == 0)
            {
                y = -C1 / B1;
                x = -(B2 * y + C2) / A2;
            }
            else if (A2 == 0)
            {
                y = -C2 / B2;
                x = -(B1 * y + C1) / A1;
            }
            else if (B1 == 0)
            {
                x = -C1 / A1;
                y = -(C2 + A2 * x) / B2;
            }
            else if (B2 == 0)
            {
                x = -C2 / A2;
                y = -(C1 + A1 * x) / B1;
            }
            else
            {
                y = (-(C2 / B2) + (A2 * C1) / (B2 * A1)) / (1 - (A2 * B1) / (A1 * B2));
                x = -(C1 + B1 * y) / A1;
            }
            return new Point(x, y);
        }
    }
}
