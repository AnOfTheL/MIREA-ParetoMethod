using System;
using System.Windows.Forms;


namespace TPR_pr1
{
    public partial class Suboptimization : Form
    {
        static string[] _aspirations = { "min", "max", "min", "max", "max" };

        public Suboptimization()
        {
            InitializeComponent();
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
        // Проверка на выбор главного критерия
        //
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !radioButton1.Checked;
            textBox2.Enabled = !radioButton2.Checked;
            textBox3.Enabled = !radioButton3.Checked;
            textBox4.Enabled = !radioButton4.Checked;
            textBox5.Enabled = !radioButton5.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 mainForm = Owner as Form1;
            
            // Проверка на факт ввода всех значений
            if (!CheckForInput())
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Проверка на правильность ввода значений
            if (!CheckForRightEnter())
            {
                MessageBox.Show("Введены неверные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Отсеивание альтернатив, не подходящих под заданные параметры 
            ToSortAltenatives(mainForm);

            // Вывод сообщения в случае, если не осталось строк для продолжения субоптимизации
            if (mainForm.dataGridView1.Rows.Count == 0)
            {
                var result = MessageBox.Show("Все строки были удалены в результате сужения множества альтернатив.\nВернуться к исходному варианту?",
                    "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    Close();
                }
                if (result == DialogResult.Yes)
                {
                    mainForm.button3_Click(this, new EventArgs());
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                }
                return;
            }

            mainForm.dataGridView1.Refresh();
            Close();
        }

        //
        // Кнопка "Отмена"
        //
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        //
        // Проверяет факт ввода всех данных
        //
        private bool CheckForInput()
        {
            if ((textBox1.TextLength == 0 && textBox1.Enabled) || 
                (textBox2.TextLength == 0 && textBox2.Enabled) || 
                (textBox3.TextLength == 0 && textBox3.Enabled) ||
                (textBox4.TextLength == 0 && textBox4.Enabled) || 
                (textBox5.TextLength == 0 && textBox5.Enabled)) 
            { return false; }
            else { return true; }
        }

        //
        // Проверка правильности ввода данных
        //
        private bool CheckForRightEnter()
        {
            try
            {
                if (textBox1.Enabled) { ushort maxPrice = ushort.Parse(textBox1.Text); }
                if (textBox2.Enabled) { ushort minArea = ushort.Parse(textBox2.Text); }
                if (textBox3.Enabled) { byte maxDistance = byte.Parse(textBox3.Text); }
                if (textBox4.Enabled) { byte minInventory = byte.Parse(textBox4.Text); }
                if (textBox5.Enabled) { byte maxRating = byte.Parse(textBox5.Text); }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //
        // Отсеивание альтернатив, не подходящих под указанные параметры
        // 
        private void ToSortAltenatives(Form1 mainForm)
        {
            int mainCriterionIndex = 0;

            RadioButton[] radioButtons = new RadioButton[5]
            {
                radioButton1,
                radioButton2,
                radioButton3,
                radioButton4,
                radioButton5
            };

            TextBox[] textBoxes = new TextBox[5]
            {
                textBox1,
                textBox2,
                textBox3,
                textBox4,
                textBox5
            };

            // Отбрасываем варианты, не удовлетворяющие заданным ограничениям
            for (int i = 0; i < radioButtons.Length; i++)
            {
                if (!radioButtons[i].Checked)
                {
                    for (int j = 0; j < mainForm.dataGridView1.Rows.Count; j++)
                    {
                        switch (_aspirations[i])
                        {
                            case ("min"):
                                if (ushort.Parse(mainForm.dataGridView1.Rows[j].Cells[i + 1].Value.ToString()) > ushort.Parse(textBoxes[i].Text))
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(j);
                                    j--;
                                }
                                break;
                            case ("max"):
                                if (ushort.Parse(mainForm.dataGridView1.Rows[j].Cells[i + 1].Value.ToString()) < ushort.Parse(textBoxes[i].Text))
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(j);
                                    j--;
                                }
                                break;
                            default:
                                throw new InvalidOperationException("Invalid operation exception");
                        }
                    }
                }
                else { mainCriterionIndex = i;  }
            }

            // Отбрасываем варианты, проигрывающие по значению главного критерия
            if (mainForm.dataGridView1.Rows.Count > 1)
            {
                for (int i = 0; i < mainForm.dataGridView1.Rows.Count-1; i++)
                {
                    for (int j = i+1; j < mainForm.dataGridView1.Rows.Count; j++)
                    {
                        switch (_aspirations[mainCriterionIndex])
                        {
                            case ("min"):
                                if (ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[mainCriterionIndex + 1].Value.ToString()) <
                                    ushort.Parse(mainForm.dataGridView1.Rows[j].Cells[mainCriterionIndex + 1].Value.ToString()))
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(j);
                                    j--;
                                }
                                else
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(i);
                                    i--;
                                }
                                break;
                            case ("max"):
                                if (ushort.Parse(mainForm.dataGridView1.Rows[i].Cells[mainCriterionIndex + 1].Value.ToString()) >
                                    ushort.Parse(mainForm.dataGridView1.Rows[j].Cells[mainCriterionIndex + 1].Value.ToString()))
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(j);
                                    j--;
                                }
                                else
                                {
                                    mainForm.dataGridView1.Rows.RemoveAt(i);
                                    i--;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
