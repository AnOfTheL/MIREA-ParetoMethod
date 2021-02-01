using System;
using System.Windows.Forms;

namespace TPR_pr1
{
    public class Gym
    {
        private uint number; // Номер тренажерного зала в списке
        private ushort price; // Минимальная стоимость абонемента (руб) (min)
        private ushort area; // Площадь зала (м^2) (max)
        private byte distance; // Удаленность от дома (км) (min)
        private byte inventory; // Качество оборудования (max)
        private byte rating; // Рейтинг зала(max)
        //
        //  Конструктор для генерации данных таблицы
        //
        public Gym(uint number)
        {
            Random rnd = new Random();
            this.number = number;
            this.price = (ushort)rnd.Next(15000);
            this.area = (ushort)rnd.Next(1000);
            this.distance = (byte)rnd.Next(50);
            this.inventory = (byte)rnd.Next(1, 5);
            this.rating = (byte)rnd.Next(1, 5);
        }
        //
        //  Конструктор ручного заполнения таблицы
        //
        public Gym(uint number, ushort price, ushort area, byte distance, byte inventory, byte rating)
        {
            this.number = number;
            this.price = price;
            this.area = area;
            this.distance = distance;
            this.inventory = inventory;
            this.rating = rating;
        }

        public Gym(Gym element)
        {
            number = element.number;
            price = element.price;
            area = element.area;
            distance = element.distance;
            inventory = element.inventory;
            rating = element.rating;
        }
        //
        //  Получение функцией ссылки на определенный элемент DataGridView
        //  и добавление в него новой строки с данными
        //
        public void AddToDataGridView(ref DataGridView dataGridView)
        {
            dataGridView.Rows.Add();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[0].Value = number.ToString();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[1].Value = price.ToString();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[2].Value = area.ToString();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[3].Value = distance.ToString();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[4].Value = inventory.ToString();
            dataGridView.Rows[dataGridView.Rows.Count-1].Cells[5].Value = rating.ToString();
        }
    }
}
