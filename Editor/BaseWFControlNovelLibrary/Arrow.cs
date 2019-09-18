using System;
using System.Drawing;
using static BaseWFControlNovelLibrary.PointWorker;

namespace BaseWFControlNovelLibrary
{
    internal class Arrow : IDisposable
    {
        internal Arrow()
        {
            CapPen = new Pen(Color.Black, 1);
            LinePen = new Pen(Color.Black, 1);
        }
        internal Arrow(Pen capPen, Pen linePen, int capLength = 30, int capWidth = 20)
        {
            CapLength = capLength;
            CapWidth = capWidth;
            CapPen = capPen;
            LinePen = linePen;
        }

        internal int CapLength = 30;
        internal int CapWidth = 20;

        internal Pen CapPen { get; private set; }
        internal Pen LinePen { get; private set; }

        private void GetLineParamsOf(Rectangle rect, Side side, out Point linePoint, out Point guideVector)
        {
            switch (side)
            {
                case Side.Down:
                    linePoint = rect.Location.Add((Point)rect.Size);
                    guideVector = new Point(1, 0);
                    break;
                case Side.Up:
                    linePoint = rect.Location.Sub(new Point(0, 1));
                    guideVector = new Point(1, 0);
                    break;
                case Side.Left:
                    linePoint = rect.Location.Sub(new Point(1, 0));
                    guideVector = new Point(0, 1);
                    break;
                case Side.Right:
                    linePoint = rect.Location.Add((Point)rect.Size);
                    guideVector = new Point(0, 1);
                    break;
                default:
                    throw new ArgumentException("Side nowhere error...");
            }
        }

        internal void Draw(Graphics g, FieldElement from, FieldElement to, float capScaleFactor = 1)
        {
            var first_point = from.Location.Add(((Point)to.Size).Multiply(0.5f));
            var second_point = to.Location.Add(((Point)to.Size).Multiply(0.5f));
            var second_rect = new Rectangle(to.Left, to.Top, to.Width, to.Height);
            var first_to_second_vector = second_point.Sub(first_point);

            Point line_point, guideVector;

            GetLineParamsOf(second_rect, first_point.WhereIAm(second_rect), out line_point, out guideVector);

            var intersection_point = GetIntersectionPoint(first_point, first_to_second_vector, line_point, guideVector);

            g.DrawLine(LinePen, first_point, intersection_point);
            g.DrawArrowCap(CapPen, intersection_point, intersection_point.Sub(first_point), (int)(CapLength * capScaleFactor), (int)(CapWidth * capScaleFactor));
        }
        internal void Draw(Graphics g, Point from, Point to, float capScaleFactor = 1)
        {
            g.DrawLine(LinePen, from, to);
            g.DrawArrowCap(CapPen, to, to.Sub(from), (int)(CapLength * capScaleFactor), (int)(CapWidth * capScaleFactor));
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                LinePen?.Dispose();
                CapPen?.Dispose();

                disposedValue = true;
            }
        }

        ~Arrow()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
