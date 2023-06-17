using System;
using System.Drawing;

namespace paint
{
    public enum FigureID
    {
        Line = 0,
        Curve = 1,
        Rectangle = 2,
        Ellipse = 3
    }

    [Serializable()]
    abstract class Figure
    {
        protected Point pointOne;
        protected Point pointTwo;
        protected Color lineColor;
        protected int lineSize;

        public Figure(Point pointOne, Point pointTwo, Point offset, int lineSize, Color lineColor)
        {
            this.pointOne.X = pointOne.X - offset.X;
            this.pointOne.Y = pointOne.Y - offset.Y;
            this.pointTwo.X = pointTwo.X - offset.X;
            this.pointTwo.Y = pointTwo.Y - offset.Y;

            this.lineSize = lineSize;
            this.lineColor = lineColor;
        }
        public abstract void BaseDraw(Graphics g, Point offset, Pen pen, SolidBrush solidBrush);

        public abstract void Draw(Graphics g, Point offset);

        public abstract void DrawHash(Graphics g, Point offset);

        public abstract void Hide(Graphics g, Point offset);

        public void Normalization(ref Point pointOne, ref Point pointTwo)
        {
            int tmp;
            if ((pointOne.X <= pointTwo.X) && (pointOne.Y >= pointTwo.Y))
            {
                tmp = pointOne.Y;
                pointOne.Y = pointTwo.Y;
                pointTwo.Y = tmp;
            }
            else if ((pointOne.X >= pointTwo.X) && (pointOne.Y <= pointTwo.Y))
            {
                tmp = pointOne.X;
                pointOne.X = pointTwo.X;
                pointTwo.X = tmp;
            }
            else if ((pointOne.X >= pointTwo.X) && (pointOne.Y >= pointTwo.Y))
            {
                tmp = pointOne.Y;
                pointOne.Y = pointTwo.Y;
                pointTwo.Y = tmp;

                tmp = pointOne.X;
                pointOne.X = pointTwo.X;
                pointTwo.X = tmp;

            }
        }

        public virtual void MouseMove(Graphics g, Point mousePosition, Point offset)
        {
            //Hide(g, offset);

            pointTwo.X = mousePosition.X - offset.X;
            pointTwo.Y = mousePosition.Y - offset.Y;

            DrawHash(g, offset);
        }

    }
}
