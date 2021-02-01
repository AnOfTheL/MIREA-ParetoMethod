using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace TPR_pr1
{
    public partial class Form1 : Form
    {
        public List<Gym> gyms = new List<Gym>();
        public bool flag;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        //
        // Определение способа заполнения таблицы
        //
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                Records recordsForm = new Records();
                recordsForm.Owner = this;
                recordsForm.buttonShowTheFirstList = button3; 
                recordsForm.buttonClearTheTable = button2;
                recordsForm.ShowDialog();
            } 
            else
            {
                button1.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        //
        // Ограничение на ввод символов
        //
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        //
        // Кнопка "Сгенерировать"
        //
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            gyms.Clear();
            Gym element;
            for (uint i = 0; i < uint.Parse(textBox1.Text); i++)
            {
                element = new Gym(i+1);
                element.AddToDataGridView(ref dataGridView1); 
                gyms.Add(new Gym(element)); // Создание копии списка исходных значений
                System.Threading.Thread.Sleep(1);
            }

            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            flag = false;
        }

        //
        // Кнопка "Оптимизировать", запускает оптимизацию выбранным методом
        //
        private void button2_Click(object sender, EventArgs e)
        {
            // Выбор метода оптимизации
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    Pareto(); // Метода Парето
                    dataGridView1.Refresh();
                    break;
                case 1:
                    Suboptimization suboptimizationForm = new Suboptimization(); 
                    suboptimizationForm.Owner = this;
                    suboptimizationForm.ShowDialog(); // Субоптимизация
                    break;
                case 2:
                    LexicographicOptimization lexicographicOptimizationForm = new LexicographicOptimization();
                    lexicographicOptimizationForm.Owner = this;
                    lexicographicOptimizationForm.ShowDialog(); // Лексикографическая оптимизация
                    break;
                default:
                    break;
            }
        }

        //
        // Метод Парето
        //
        private void Pareto()
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) // Цикл для перебора доминантов
            {
                for (int j = i + 1; j < dataGridView1.Rows.Count - 1; j++) // Цикл для перебора альтернатив
                {
                    if (ToCompareAlternatives(i, j)) // Сравнение альтернатив и удаление одной (первой) из них,
                                                     // если вторая является доминирующей или эквивалентной
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        i--; 
                        break;
                    }
                }
            }
        }

        //
        // Сравнение альтернатив
        //
        private bool ToCompareAlternatives(int i, int j)
        {
            return ushort.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) >= ushort.Parse(dataGridView1.Rows[j].Cells[1].Value.ToString()) &&
                   ushort.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()) <= ushort.Parse(dataGridView1.Rows[j].Cells[2].Value.ToString()) &&
                   byte.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) >= byte.Parse(dataGridView1.Rows[j].Cells[3].Value.ToString()) &&
                   byte.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) <= byte.Parse(dataGridView1.Rows[j].Cells[4].Value.ToString()) &&
                   byte.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()) <= byte.Parse(dataGridView1.Rows[j].Cells[5].Value.ToString());
        }

        //
        // Кнопка "Показать исходные значения", выводит исходный список альтернатив
        //
        public void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < gyms.Count; i++)
            {
                gyms[i].AddToDataGridView(ref dataGridView1);
            }
        }

        //
        // Кнопка "Очистить таблицу"
        //
        private void button4_Click(object sender, EventArgs e)
        {
            flag = false;
            dataGridView1.Rows.Clear();
            gyms.Clear();
        }
    }
}
