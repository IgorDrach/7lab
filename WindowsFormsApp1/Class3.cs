using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace paint
{
    [Serializable()]
    class Line : Figure
    {
        public Line(Point pointOne, Point pointTwo, Point offset, int lineSize, Color lineColor) :
        base(pointOne, pointTwo, offset, lineSize, lineColor)
        { }

        public override void BaseDraw(Graphics g, Point offset, Pen pen, SolidBrush solidBrush = null)
        {
            Point pointOneOffset = new Point(pointOne.X + offset.X, pointOne.Y + offset.Y);
            Point pointTwoOffset = new Point(pointTwo.X + offset.X, pointTwo.Y + offset.Y);

            g.DrawLine(pen, pointOneOffset, pointTwoOffset);
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
    }
}