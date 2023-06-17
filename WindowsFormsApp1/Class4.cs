using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace paint
{
    [Serializable()]
    class Ellipse : Figure
    {
        public Color solidColor;

        public Ellipse(Point pointOne, Point pointTwo, Point offset, int lineSize, Color lineColor, Color fillColor) :
        base(pointOne, pointTwo, offset, lineSize, lineColor)
        {
            solidColor = fillColor;
        }

        public override void BaseDraw(Graphics g, Point offset, Pen pen, SolidBrush solidBrush)
        {
            Point normalPointOne = new Point(pointOne.X + offset.X, pointOne.Y + offset.Y);
            Point normalPointTwo = new Point(pointTwo.X + offset.X, pointTwo.Y + offset.Y);

            Normalization(ref normalPointOne, ref normalPointTwo);

            System.Drawing.Rectangle rectangle =
            System.Drawing.Rectangle.FromLTRB(normalPointOne.X, normalPointOne.Y, normalPointTwo.X, normalPointTwo.Y);

            if (solidBrush != null)
            {
                g.FillEllipse(solidBrush, rectangle);
            }
            g.DrawEllipse(pen, rectangle);
        }

        public override void Draw(Graphics g, Point offset)
        {
            Pen pen = new Pen(lineColor, lineSize);

            SolidBrush solidBrush = new SolidBrush(solidColor);

            BaseDraw(g, offset, pen, solidBrush);

            pen.Dispose();
            solidBrush.Dispose();
        }

        public override void DrawHash(Graphics g, Point offset)
        {
            Pen pen = new Pen(lineColor, lineSize)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };

            BaseDraw(g, offset, pen, null);

            pen.Dispose();
        }

        public override void Hide(Graphics g, Point offset)
        {
            Pen pen = new Pen(Color.White, lineSize);

            SolidBrush solidBrush = new SolidBrush(Color.White);

            BaseDraw(g, offset, pen, solidBrush);

            pen.Dispose();
            solidBrush.Dispose();
        }
    }
}