using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace paint
{
    [Serializable()]
    class Curve : Figure
    {
        public List<Point> points;

        public Curve(Point pointOne, Point pointTwo, Point offset, int lineSize, Color lineColor) :
        base(pointOne, pointTwo, offset, lineSize, lineColor)
        {
            points = new List<Point>();
            points.Add(new Point(pointOne.X - offset.X, pointOne.Y - offset.Y));
            points.Add(new Point(pointTwo.X - offset.X, pointTwo.Y - offset.Y));
        }

        public override void BaseDraw(Graphics g, Point offset, Pen pen, SolidBrush solidBrush = null)
        {
            Point[] p = points.ToArray();

            for (int i = 0; i < p.Count(); ++i)
            {
                p[i].X += offset.X;
                p[i].Y += offset.Y;
            }

            g.DrawCurve(pen, p);
        }
        public override void Draw(Graphics g, Point offset)
        {
            Pen pen = new Pen(lineColor, lineSize);

            BaseDraw(g, offset, pen);

            pen.Dispose();
        }

        public override void DrawHash(Graphics g, Point offset)
        {
            Pen pen = new Pen(lineColor, lineSize)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };

            BaseDraw(g, offset, pen);

            pen.Dispose();
        }

        public override void Hide(Graphics g, Point offset)
        {
            Pen pen = new Pen(Color.White, lineSize);

            BaseDraw(g, offset, pen);

            pen.Dispose();
        }

        public override void MouseMove(Graphics g, Point mousePosition, Point offset)
        {
            pointTwo.X = mousePosition.X - offset.X;
            pointTwo.Y = mousePosition.Y - offset.Y;

            points.Add(pointTwo);
        }
    }
}