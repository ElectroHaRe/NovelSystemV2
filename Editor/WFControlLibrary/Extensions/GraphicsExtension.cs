using System.Drawing;

namespace WFControlLibrary
{
    public static class GraphicsExtension
    {
        public static void DrawArrowCap(this Graphics g, Pen pen, Point destination, Point guideVector, int length, int width, bool closedLine = false)
        {
            var normal_guide_vector = guideVector.Normalize();
            var start_arrow_point = destination.Sub(normal_guide_vector.Multiply(length).ToPoint());
            var first_arrow_point = start_arrow_point.Add(normal_guide_vector.Perpendicular().Multiply(width / 2).ToPoint());
            var second_arrow_point = start_arrow_point.Add(normal_guide_vector.Perpendicular().Multiply(-width / 2).ToPoint());
            g.DrawLine(pen, first_arrow_point, destination);
            g.DrawLine(pen, second_arrow_point, destination);
            if (closedLine)
                g.DrawLine(pen, first_arrow_point, second_arrow_point);
        }
    }
}
