using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization; //сериализация
using System.Runtime.Serialization.Formatters.Binary; //сериализация
using System.IO; //сериализация
using WindowsFormsApp1;
using System.Reflection;
using paint;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        int keyopen;
        public int curX;
        public int curY;
        
        Form2 f2;
        public int brushSize; //толщина линии
        public Color colorLine, colorFon; //цвет линии и фона
        public int wCount = 0; //количество созданных дочерних окон
        public int lineWidth = 1;   //ширина пера по умолчанию
        public int pictHeight = 600, pictWidth = 800; //параметры нового окна
        public bool solidFill = false; //заливка фигуры (флаг)
        public FigureID FigureID; //тип фигуры
        private Size canvasSize;

        public Form1()
        {
            FigureID = FigureID.Line;
            InitializeComponent();
            colorLine = Color.Black;//по умолчанию цвет линии - чёрный    
            colorFon = Color.White;//по умолчанию цвет фона - белый    
            brushSize = 1;//по умолчанию толщина линии - 1 
        }
        public void allDown()
        { //все "флажки" сняты
            линияToolStripMenuItem.Checked = false; //свойства - линия
            прямоугольникToolStripMenuItem.Checked = false; //свойства - прямоугольник 
            эллипсToolStripMenuItem.Checked = false; //свойства - эллипс 
            криваяToolStripMenuItem.Checked = false; //свойства - кривая

        }
       


        public void ProverkaOpen()
        {
            keyopen = MdiChildren.Length; //проверка: открыта ли в настоящее время дочерняя форма в роительской форме?
            if (keyopen == 0)
            {
                сохранитьToolStripMenuItem.Enabled = false; //блокировка меню сохранить
                сохранитьКакToolStripMenuItem.Enabled = false; //блокировка меню сохранить как
            }

            if (keyopen > 0)
            {
                сохранитьToolStripMenuItem.Enabled = true; //разблокировка меню сохранить
                сохранитьКакToolStripMenuItem.Enabled = true; //разблокировка меню сохранить как
                f2 = (Form2)this.ActiveMdiChild; // возвращает объект Form, который представляет дочернее окно текущего активного интерфейса
                if (f2.flagIzmen == false) //если дочерняя форма не изменялась с момента открытия
                {
                    сохранитьToolStripMenuItem.Enabled = false; //блокировка меню сохранить          
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void setLine()//линия
        {
            allDown();
            линияToolStripMenuItem.Checked = true;
            FigureID=FigureID.Line;
            
        }

        public void setCurve()//кривая
        {
            allDown();
            криваяToolStripMenuItem.Checked = true;
            FigureID = FigureID.Curve;
            
        }

        public void setRectangle()//прямоугольник
        {
            allDown();
            прямоугольникToolStripMenuItem.Checked = true;
            FigureID = FigureID.Rectangle;
            
        }

        public void setEllipse()//эллипс
        {
            allDown();
            эллипсToolStripMenuItem.Checked = true;
            FigureID = FigureID.Ellipse;
            
        }

        public void SaveFile(Form2 f) // функция сохранения файла
        {
            string fileName;
            List<Figure> array; //объект array класса Figure
            f2 = f;
            BinaryFormatter formatter1 = new BinaryFormatter(); //Сохранение объекта obj некоторого класса X в файле с именем fileName 
            if (f2.fileName == null) //Имя файла, выбранное в диалоговом окне файла - если раньше этот файл не был сохранен
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory; //При инициализации файловых диалогов указываем в качестве стартового каталога текущий каталог программы
                saveFileDialog1.Filter = "Граф.ред(*.grm)|*.grm|All files (*.*)|*.*"; //Задаем текущую строку фильтра имен файлов, которая определяет варианты, которые появляются в окне "сохранить как тип файла" или "файлы типа" в диалоговом окне
                saveFileDialog1.FilterIndex = 1; //Задаем индекс фильтра, выбранного в настоящий момент в диалоговом окне файла.
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) //если нажали OK
                {
                    fileName = saveFileDialog1.FileName;
                    array = f2.array;
                    Stream myStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter1.Serialize(myStream, array);
                    myStream.Close();
                    f2.flagIzmen = false;
                    f2.fileName = fileName;
                    f2.Text = Path.GetFileName(saveFileDialog1.FileName);
                }
            }
            else //Имя файла, выбранное в диалоговом окне файла -если раньше этот файл был сохранен
            {
                fileName = f2.fileName;
                array = f2.array;
                Stream myStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter1.Serialize(myStream, array);
                myStream.Close();
                f2.flagIzmen = false;
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)// обработчика для события создать
        {

            Form f = new Form2(canvasSize)
            {
                MdiParent = this,
                Text = "Рисунок " + MdiChildren.Length.ToString(),
            };
            f.Show();

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)// обработчика для события открыть
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory
            };

            DialogResult dialogResult = openFileDialog.ShowDialog();


            for (int i = 0; i < MdiChildren.Length; ++i)
            {
                Form2 canvas = (Form2)MdiChildren[i];
                if (canvas.FilePathSave == openFileDialog.FileName)
                {
                    MessageBox.Show("Файл с данным именем уже открыт!");
                    return;
                }
            }


            if (dialogResult == DialogResult.OK)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                List<Figure> array = (List<Figure>)formatter.Deserialize(stream);
                Size size = (Size)formatter.Deserialize(stream);
                stream.Close();

                Form2 canvas = new Form2(size)
                {
                    Array = array,
                    Text = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('\\') + 1),
                    FilePathSave = openFileDialog.FileName
                };

                Form f = canvas;
                f.MdiParent = this;

                сохранитьToolStripMenuItem.Enabled = true;
                сохранитьКакToolStripMenuItem.Enabled = true;

                f.Show();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)// обработчика для события сохранить 
        {
            SaveFile((Form2)this.ActiveMdiChild);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)// обработчика для события сохранить как
        {
            f2 = (Form2)this.ActiveMdiChild;
            f2.fileName = null;
            SaveFile((Form2)this.ActiveMdiChild);
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)// обработчика для события файл
        {
            ProverkaOpen();
        }

        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog();  //открытие диалогового окна                              
            DialogResult result = myDialog.ShowDialog();
            if (result == DialogResult.OK) colorLine = myDialog.Color; //проверка и установление выбранного цвета с помощью свойства Color класса ColorDialog.
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog(); //открытие диалогового окна 
            DialogResult result = myDialog.ShowDialog();
            if (result == DialogResult.OK) colorFon = myDialog.Color;// проверка и установление выбранного цвета с помощью свойства Color класса ColorDialog.
        }

        private void толщинаЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrushSize myDialog = new BrushSize(); //открытие диалогового окна                           
            DialogResult result = myDialog.ShowDialog(this);
            if (result == DialogResult.OK) brushSize = myDialog.b1;//проверка и установление выбранной толщины линии
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setLine();
        }

        private void криваяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCurve();
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setRectangle();
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setEllipse();
        }

        

        public void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form4 canvasSize = new Form4();

            if (canvasSize.ShowDialog() == DialogResult.OK)
            {
                this.canvasSize = canvasSize.size;
            }
            
                
            
        }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
