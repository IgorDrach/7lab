using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Drawing.Text;
using paint;

namespace WindowsFormsApp5
{
    [Serializable()]
    public partial class Form2 : Form
    {
        


  public int X=0; // Form2.X - обращаться так
  public int Y=0; // Form2.Y - обращаться так
    
    public System.Drawing.Region Clip { get; set; }
        public Size workPlaceSize;
        
            
        
        Point p1, p2, p3, end;
        int x1, y1,x2,y2;
        bool Flag = false; //флаг нажатия мыши
        Figure Rc1, Rc2;
        Form1 f1;
        Form4 f4;
        public bool flagIzmen = false; //флаг изменения дочерней формы
        public string fileName = null; //имя файла
        //public bool flagName = false;
        internal List<Figure> array; //динамический массив сделан полем класса Form2  
        public bool fromFile = false; //флаг: существовал ли данный файл
        bool paintAction = false; // удержание кнопки мыши (флаг)
        bool changedCanvas = false; //изменение рисунка (флаг)
        Point start, finish; //точки (для рисования) начала фигуры и конца
        List<Figure> fstorage = new List<Figure>(); //список базовых фигур
        Figure toPaint; //базовая фигура
        //параметры рисования
        //класс Bitmap инкапсулирует точечный рисунок, состоящий из данных точек графического изображения и атрибутов рисунка.
        Bitmap canvas = new Bitmap(10, 10);
        public Color backColor = Color.White;//цвет фона
        public Color frameColor = Color.Black;//цвет фигуры непосредственно при её рисовании
        public Color primaryColor = Color.Black;//цвет линий
        public Color secondaryColor = Color.Black;//цвет заливки
        public int lineWidth = 1;//толщина пера
        public bool solidFill = false;//по умолчанию заливки нет
        public int figureID = 0;//фигура по умолчанию - линия
        public int pictWidth = 1, pictHeight = 1;
        public bool _isDrawing = false;
        private bool isMousePresed = false;
        private bool isMouseMoved = false;
        private bool isModificated = false;
        public BufferedGraphics buffer;
        public BufferedGraphicsContext contex;
        public string FilePathSave = System.String.Empty;
        public void drawCanvas() //рисование точечного рисунка
        {
            canvas.Dispose();// Dispose - явное освобождение ресурсов, удаляем старый точечный рисунок
            canvas = new Bitmap(pictWidth, pictHeight); //создаём новый
            Graphics g = Graphics.FromImage(canvas);//Graphics.FromImage cоздает новый объект Graphics из указанного объекта canvas
            g.Clear(backColor);//Очищаем всю поверхность рисования и выполняем заливку поверхности указанным цветом фона
            foreach (Figure go in fstorage) go.Draw( g, p3);
            if (paintAction) toPaint.DrawHash( g, p3); //если зажата кнопка мыши, то рисуем временный рисунок
            g.Dispose();
        }

        

       
    public Form2(Size size)
        {

            
        InitializeComponent();
        array = new List<Figure>();
        workPlaceSize = size;
        Size = size;
        AutoScrollMinSize = size;
        array = new List<Figure>();

        }
        internal List<Figure> Array { get => array; set => array = value; }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePresed)
            {
                array.Last().MouseMove(buffer.Graphics, e.Location, AutoScrollPosition);
                Invalidate();
                isMouseMoved = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)// обработчика для события Load
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            contex = BufferedGraphicsManager.Current;
            contex.MaximumBuffer = new Size(workPlaceSize.Width, workPlaceSize.Height);

            buffer = contex.Allocate(CreateGraphics(), new System.Drawing.Rectangle(0, 0, workPlaceSize.Width, workPlaceSize.Height));

            System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(startPoint, workPlaceSize);
            System.Drawing.SolidBrush solidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            buffer.Graphics.FillRectangle(solidBrush, rectangle);
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {

            if ((e.X > workPlaceSize.Width) || (e.Y > workPlaceSize.Height))
            {
                return;
            }
            isMousePresed = true;
            Form1 m = (Form1)ParentForm;
            switch (m.FigureID)
            {
                case FigureID.Line:
                    {
                        array.Add(new Line(e.Location, e.Location, AutoScrollPosition, m.brushSize, m.colorLine));
                        break;
                    }

                case FigureID.Curve:
                    {
                        array.Add(new Curve(e.Location, e.Location, AutoScrollPosition, m.brushSize, m.colorLine));
                        break;
                    }

                case FigureID.Rectangle:
                    {
                        array.Add(new paint.Rectangle(e.Location, e.Location, AutoScrollPosition, m.brushSize, m.colorLine, m.colorFon));
                        break;
                    }

                case FigureID.Ellipse:
                    {
                        array.Add(new Ellipse(e.Location, e.Location, AutoScrollPosition, m.brushSize, m.colorLine, m.colorFon));
                        break;
                    }
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)//обработчик события отпускание левой кнопки мыши
        {

            if (isMousePresed && !isMouseMoved)
            {
                array.RemoveAt(array.Count - 1);
            }
            else if (isMousePresed && isMouseMoved)
            {
                if (!IsFigureInCanvas(array.Last(), e.Location))
                {
                    //array.Last().Hide(buffer.Graphics, AutoScrollPosition);
                    Invalidate();
                    array.RemoveAt(array.Count - 1);
                }
                else
                {
                    array.Last().Draw(buffer.Graphics, AutoScrollPosition);
                    Invalidate();
                    isModificated = true;
                }
            }
            isMousePresed = false;
            isMouseMoved = false;
        }

            private void Form2_RegionChanged(object sender, EventArgs e)
        {
            
        }

        private void Form2_BackColorChanged(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagIzmen) //если текущий файл изменен
            {
                DialogResult result;
                result = MessageBox.Show("Сохранить изменения в \"" + this.Text + "\"", "Граф.ред", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            f1 = new Form1();
                            f1.SaveFile(this); //сохранение
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true; //возвращение к файлу
                            return;
                        }
                }
            }
        }
        

        private void Form2_Paint(object sender, PaintEventArgs e)
        {

            System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(startPoint, this.workPlaceSize);
            System.Drawing.SolidBrush solidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            buffer.Graphics.FillRectangle(solidBrush, rectangle);

            foreach (Figure i in array)
            {
                i.Draw(buffer.Graphics, AutoScrollPosition);
            }
            buffer.Render(e.Graphics);


        }
        private bool IsFigureInCanvas(Figure f, Point p)
        {
            Point pointWithOffset;

            if (f is Curve)
            {
                Curve curve = (Curve)f;
                foreach (Point i in curve.points)
                {
                    if (!IsPointInWorkplace(i))
                    {
                        return false;
                    }
                }
            }
            else
            {
                pointWithOffset = new Point(p.X - AutoScrollPosition.X, p.Y - AutoScrollPosition.Y);

                if (!IsPointInWorkplace(pointWithOffset))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPointInWorkplace(Point point)
        {
            return ((point.X <= workPlaceSize.Width) && (point.Y <= workPlaceSize.Height) &&
                   (point.X >= 0) && (point.Y >= 0));
        }

    }
}
