using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TPR_pr1
{
    public partial class LexicographicOptimization : Form
    {
        static string[] _aspirations = { "min", "max", "min", "max", "max" };

        public LexicographicOptimization()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CheckForInput()) { return; } // Проверка правильности расставления приоритетов критериев
            Form1 mainForm = Owner as Form1;
            byte priority = 0;
            while (!CheckForLast(mainForm))
            {
                priority++;
                Optimization(priority, mainForm);
            }
            mainForm.dataGridView1.Refresh();
            Close(); 
        }

        //
        // Проверка правильности расставления приоритетов критериев
        //
        private bool CheckForInput()
        {
            List<byte> check = new List<byte>() { 1, 2, 3, 4, 5 };
            for (byte i = 0; i < check.Count; i++)
            {
                if (numericUpDown1.Value == check[i] ||
                   numericUpDown2.Value == check[i] ||
                   numericUpDown3.Value == check[i] ||
                   numericUpDown4.Value == check[i] ||
                   numericUpDown5.Value == check[i])
                {
                    check.RemoveAt(i);
                    i--;
                }
            }
            if (check.Count != 0)
            {
                MessageBox.Show("Критерии не могут иметь одинаковый приоритет.", "Ошибка!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else { return true; }
        }

        //
        // Лексикографическая оптимизация
        //
        private void Optimization(byte priority, Form1 mainForm)
        {
            NumericUpDown[] numericUpDowns = new NumericUpDown[5]
            {
                numericUpDown1,
                numericUpDown2,
                numericUpDown3,
                numericUpDown4,
                numericUpDown5
            };

            int index = 0;
            while (numericUpDowns[index].Value == priority) { index++; }

            ushort min, max;
            min = max = ushort.Parse(mainForm.dataGridView1.Rows[0].Cells[index + 1].Value.ToString());
            DataGridViewRow row = mainForm.dataGridView1.Rows[0];

            switch (_aspirations[index])
            {
                case "min":
                    for (int i = 1; i < mainForm.dataGridView1.Rows.Count; i++)
                    {
                        if (ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[index + 1].Value.ToString()) < min)
                        {
                            row = mainForm.dataGridView1.Rows[i];
                            min = ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[index + 1].Value.ToString());
                        }
                    }
                    mainForm.dataGridView1.Rows.Clear();
                    mainForm.dataGridView1.Rows.Add(row);
                    break;
                case "max":
                    for (int i = 1; i < mainForm.dataGridView1.Rows.Count; i++)
                    {
                        if (ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[index + 1].Value.ToString()) > max)
                        {
                            row = mainForm.dataGridView1.Rows[i];
                            max = ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[index + 1].Value.ToString());
                        }
                    }
                    mainForm.dataGridView1.Rows.Clear();
                    mainForm.dataGridView1.Rows.Add(row);
                    break;
                default:
                    throw new InvalidOperationException("Invalid operation exception");
            }
        }
        
        //
        // Проверка на то, единственный ли исход
        // Если да, то этот исход является оптимальным
        // Если нет, то производим отбор по следующему из важнейших критериев
        //
        private bool CheckForLast(Form1 mainForm)
        {
            if (mainForm.dataGridView1.Rows.Count > 1) { return false; }
            else { return true; }
        }

        //
        // Кнопка "Отмена"
        //
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
