using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TPR_pr1
{
    public partial class Records : Form
    {
        public Button buttonShowTheFirstList, buttonClearTheTable; 
        public List<Gym> gyms = new List<Gym>();
        public Records()
        {
            InitializeComponent();
        }
        //
        // Кнопка "Отмена", закрывает форму ручного ввода данных
        //
        private void button2_Click(object sender, EventArgs e)
        {
            buttonShowTheFirstList.Enabled = true;
            Close();
        }
        //
        // Функция проверки ввода данных (позволяет ввести только цифры)
        //
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
        //
        // Кнопка "Добавить", заполняет новую строку таблицы введенными данными
        //
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 mainform = Owner as Form1;
            if (!CheckForInput())
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                if (!mainform.flag)
                {
                    mainform.gyms.Clear();
                    mainform.dataGridView1.Rows.Clear();
                }
                Gym element = new Gym(uint.Parse((mainform.dataGridView1.Rows.Count + 1).ToString()), ushort.Parse(textBox1.Text), 
                    ushort.Parse(textBox2.Text), byte.Parse(textBox3.Text), byte.Parse(textBox4.Text), byte.Parse(textBox5.Text));
                element.AddToDataGridView(ref mainform.dataGridView1); 
                gyms.Add(new Gym(element));
                mainform.gyms.Add(new Gym(element));
                mainform.flag = true; 
                buttonClearTheTable.Enabled = true;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            catch
            {
                MessageBox.Show("Введены неверные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        //
        // Проверяет факт ввода всех данных
        //
        private bool CheckForInput()
        {
            if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox3.TextLength == 0 ||
                textBox4.TextLength == 0 || textBox5.TextLength == 0) { return false; }
            else { return true; }
        }
    }
}
