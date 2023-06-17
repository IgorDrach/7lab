using System;
using System.Drawing;
using System.Windows.Forms;


namespace WindowsFormsApp5
{
    public partial class Form4 : Form
    {
        public Size size;
        public int height, width;
        public Form1 Mainform;
        
        public Form4()

            
        {
           
            
            InitializeComponent();
            checkBox1.Checked = false;//по умолчанию не установлен флажок выбора ввода вручную           
            TextEnFalse();//по умолчанию  выбираем из предложенного
        }
        private void TextEnFalse() //если выбираем из предложенного
        {
            label1.Enabled = false; label2.Enabled = false; //"ширина" и "высота" не доступны для пользователя
            textBox1.Enabled = false; textBox2.Enabled = false; //поля для ручного ввода ширины и высоты не доступны для пользователя
            radioButton1.Enabled = true; radioButton2.Enabled = true; radioButton3.Enabled = true; //флажки выбора размера доступны для пользователя
            button1.Enabled = true; //кнопка "ОК" доступна для нажатия

        }
        private void TextEnTrue() //если вводим вручную
        {
            label1.Enabled = true; label2.Enabled = true;
            textBox1.Enabled = true; textBox2.Enabled = true; //поля для ручного ввода ширины и высоты доступны для пользователя
            radioButton1.Enabled = false; radioButton2.Enabled = false; radioButton3.Enabled = false; //флажки выбора размера не доступны для пользователя
            if (textBox1.Text != "" && textBox2.Text != "") button1.Enabled = true; //если что-то введено, то доступна кнопка "ОК"
            else button1.Enabled = false; //если ничего не введено, то не доступна кнопка "ОК"
        }
        
    public void button1_Click(object sender, EventArgs e)//обработчик события нажатия кнопки OK
        {

            if (checkBox1.Checked == true)
            {
                size = new Size(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    size = new Size(320, 240);
                }
                else if (radioButton2.Checked == true)
                {
                    size = new Size(640, 480);
                }
                else if (radioButton3.Checked == true)
                {
                    size = new Size(800, 600);
                }
            }
            button1.DialogResult = DialogResult.OK;

        }

        private void button2_Click(object sender, EventArgs e)//обработчик события нажатия кнопки Cancel
        {
            this.Close();
        }
        private static void ProverkaText(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false && e.KeyChar != '\b')//проверяем, является ли введённое значение цифрой или нет
            {
                e.Handled = true;// обход обработки элемента по умолчанию
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//обработчик события "выбрать вручную"
        {
            if (checkBox1.Checked) TextEnTrue();//если установлен флажок выбора ввода вручную   
            else TextEnFalse();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)//обработчик события "высота"
        {
            ProverkaText(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//обработчик события "ширина"
        {
            ProverkaText(e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextEnTrue();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextEnTrue();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)//обработчик события Load
        {

        }
    }
}
