using System;
using System;
using System.Drawing;


namespace paint
{
    [Serializable()]
    class Rectangle : Figure
    {
        public Color solidColor;
        public Rectangle(Point pointOne, Point pointTwo, Point offset, int lineSize, Color lineColor, Color fillColor) :
        base(pointOne, pointTwo, offset, lineSize, lineColor)
        {
            solidColor = fillColor;
        }
        public override void BaseDraw(Graphics g, Point offset, Pen pen, SolidBrush solidBrush)
        {
            Point normPointOne = new Point(pointOne.X + offset.X, pointOne.Y + offset.Y);
            Point normPointTwo = new Point(pointTwo.X + offset.X, pointTwo.Y + offset.Y);
            Normalization(ref normPointOne, ref normPointTwo);
            System.Drawing.Rectangle rectangle = System.Drawing.Rectangle.FromLTRB(normPointOne.X, normPointOne.Y, normPointTwo.X, normPointTwo.Y);

            if (solidBrush != null)
            {
                g.FillRectangle(solidBrush, rectangle);
            }

            g.DrawRectangle(pen, rectangle);
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