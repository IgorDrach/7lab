using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class BrushSize : Form
    {
        string b0;
        public int b1;
        public BrushSize()

        {
            InitializeComponent();
            int[] mass = new int[] { 1, 2, 5, 8, 10, 12, 15 };
            int i = 0;
            while (i < mass.Length)
            { comboBox2.Items.Add(mass[i]); i++; }//Добавляем элемент в список позиций      
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b0 = comboBox2.Text;
            b1 = Convert.ToInt32(b0);//Преобразовываем текстовое  значение этого поля в целочисленно
            button4.DialogResult = DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BrushSize_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.DialogResult = DialogResult.Cancel;
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
