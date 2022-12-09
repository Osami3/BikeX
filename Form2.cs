using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml;
using System.IO;
using MySql.Data.MySqlClient;
using Google.Protobuf.WellKnownTypes;
using Excel = Microsoft.Office.Interop.Excel;


namespace BikeX
{
    public partial class Form2 : Form
    {
        DB db = new DB();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlDataReader reader;
        MySqlCommand command;

        public Form2()
        {
            InitializeComponent();
        }

        public void Update(string zapros, string name_table)
        {
            dataGridView_full.Rows.Clear();
            try
            {
                command = new MySqlCommand(zapros, db.getConnection());
                command.Connection.Open();
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    reader = command.ExecuteReader();
                    if (name_table == "Список велосипедов")
                    {
                        while (reader.Read())
                        {
                            string cache = Convert.ToString(reader["id_bike"]) + "!" +
                                           Convert.ToString(reader["number_bike"]) + "!" +
                                           Convert.ToString(reader["status"]) + "!" +
                                           Convert.ToString(reader["name_bike"]) + "!" +
                                           Convert.ToString(reader["size_rama"]) + "!" +
                                           Convert.ToString(reader["height_sm"]) + "!" +
                                           Convert.ToString(reader["name_size"]) + "!" +
                                           Convert.ToString(reader["price_min"]) + "!" +
                                           "удалить";
                            string[] full = cache.Split(new char[] { '!' });
                            dataGridView_full.Rows.Add(full);
                        }

                    }
                    else if (name_table == "Список пользователей")
                    {
                        while (reader.Read())
                        {
                            string cache = Convert.ToString(reader["id_client"]) + "!" +
                                           Convert.ToString(reader["name"]) + "!" +
                                           Convert.ToString(reader["phone"]) + "!" +
                                           Convert.ToString(reader["email"]) + "!" +
                                           " " + "!" +
                                           " " + "!" +
                                           " " + "!" +
                                           " " + "!" +
                                           "удалить";
                            string[] full = cache.Split(new char[] { '!' });
                            dataGridView_full.Rows.Add(full);
                        }
                    }
                    else if (name_table == "Продажи магазина")
                    {
                        while (reader.Read())
                        {
                            string cache = Convert.ToString(reader["id_zakaz"]) + "!" +
                                           Convert.ToString(reader["client"]) + "!" +
                                           Convert.ToString(reader["sotrudnik"]) + "!" +
                                           Convert.ToString(reader["number_bike"]) + "!" +
                                           Convert.ToString(reader["min"]) + "!" +
                                           Convert.ToString(reader["price"]) + "!" +
                                           Convert.ToString(reader["date_start"]) + "!" +
                                            Convert.ToString(reader["date_end"]) + "!" +
                                            "удалить";
                            string[] full = cache.Split(new char[] { '!' });
                            dataGridView_full.Rows.Add(full);
                        }
                    }
                    command.Connection.Close();
                    reader.Close();
                }

        }
            catch
            {
                command.Connection.Close();
                MessageBox.Show
                      ("Ожидайте отклика программы",
                      "Предупреждение",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Exclamation,
                      MessageBoxDefaultButton.Button1
                      );
            }

}
        public void Delete(string name_table)
        {
            string value = null;
            int index_num = 0;
            
            index = dataGridView_full.CurrentRow.Index; //получение индекса строки
            index_num = int.Parse(dataGridView_full.CurrentRow.Cells[0].Value.ToString()); //получение значения первого столбца строки
            var value_cell = dataGridView_full.CurrentCell.Value; //для определения значения в ячейке
            value = value_cell.ToString();
            if (value == "удалить")
            {
                DialogResult result = MessageBox.Show //Предупреждает об удалении строки
                         ("Вы действительно хотите удалить строку?",
                         "Подтверждение",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button1
                         );
                if (result == DialogResult.Yes)
                {
                    try
                    { 
                        dataGridView_full.Rows.RemoveAt(index); //удаление строки в таблице
                        switch(name_table) //определяет какая таблица активна 
                        {
                            //создание sql запроса для удаления
                            case "Список пользователей": command = new MySqlCommand("delete from client where id_client = @id LIMIT 1;", db.getConnection());
                                break;
                            case "Список велосипедов":
                                command = new MySqlCommand("delete from bike where id_bike = @id LIMIT 1;", db.getConnection());
                                break;
                            case "Продажи магазина":
                                command = new MySqlCommand("delete from zakaz where id_zakaz = @id LIMIT 1;", db.getConnection());
                                break;

                        }
                        //определяем параметр id
                        command.Parameters.Add("@id", MySqlDbType.VarChar).Value = index_num;

                        command.Connection.Open(); //подключение 
                        adapter.SelectCommand = command; 
                        adapter.Fill(table); 
                        command.Connection.Close(); //закрывает подключение
                    }
                    catch //вывод ошибки если удаление невозможно 
                    {   
                        MessageBox.Show
                        ("Данную запись временно невозможно удалить. \n Обратитесь в службу поддержки.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                        );
                    }
                }
                if (result == DialogResult.No) //убирает выделение ячейки
                {
                    dataGridView_full.ClearSelection();
                }
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView_full.MultiSelect = false; //запретить выделение нескольких строк
            dataGridView_full.SelectionMode = DataGridViewSelectionMode.CellSelect; // выделение только одной ячейки

            //this.Text = "Список пользователей";
            this.Text = Global_data.title;
            if (this.Text == "Список велосипедов")
            {
                dataGridView_full.Enabled = true;
                dataGridView_full.Columns[0].Visible = true;
                dataGridView_full.Columns[1].Visible = true;
                dataGridView_full.Columns[2].Visible = true;
                dataGridView_full.Columns[3].Visible = true;
                dataGridView_full.Columns[4].Visible = true;
                dataGridView_full.Columns[5].Visible = true;
                dataGridView_full.Columns[6].Visible = true;
                dataGridView_full.Columns[7].Visible = true;

                dataGridView_full.Columns[0].HeaderText = "Код велосипеда";
                dataGridView_full.Columns[1].HeaderText = "Номер велосипеда";
                dataGridView_full.Columns[2].HeaderText = "Статус";
                dataGridView_full.Columns[3].HeaderText = "Название";
                dataGridView_full.Columns[4].HeaderText = "Номер рамы в см.";
                dataGridView_full.Columns[5].HeaderText = "Рост в см.";
                dataGridView_full.Columns[6].HeaderText = "Наим. разм.";
                dataGridView_full.Columns[7].HeaderText = "Цена за мин.";

                dataGridView_full.Columns[0].Name = "id_bike";
                dataGridView_full.Columns[1].Name = "number_bike";
                dataGridView_full.Columns[2].Name = "status";
                dataGridView_full.Columns[3].Name = "name_bike";
                dataGridView_full.Columns[4].Name = "size_rama";
                dataGridView_full.Columns[5].Name = "height_sm";
                dataGridView_full.Columns[6].Name = "name_size";
                dataGridView_full.Columns[7].Name = "price_min";


                Update("select id_bike, number_bike, status, name_bike, size_rama, height_sm, name_size, price_min from bike,size_bike where bike.id_size = size_bike.id_size", Global_data.title);
            }
            else if (this.Text == "Список пользователей")
            {
                dataGridView_full.Enabled = true;
                dataGridView_full.Columns[0].Visible = true;
                dataGridView_full.Columns[1].Visible = true;
                dataGridView_full.Columns[2].Visible = true;
                dataGridView_full.Columns[3].Visible = true;
                dataGridView_full.Columns[4].Visible = false;
                dataGridView_full.Columns[5].Visible = false;
                dataGridView_full.Columns[6].Visible = false;
                dataGridView_full.Columns[7].Visible = false;

                dataGridView_full.Columns[0].HeaderText = "Код пользователя";
                dataGridView_full.Columns[1].HeaderText = "Имя";
                dataGridView_full.Columns[2].HeaderText = "Телефон";
                dataGridView_full.Columns[3].HeaderText = "Почта";
                dataGridView_full.Columns[4].HeaderText = " ";
                dataGridView_full.Columns[5].HeaderText = " ";
                dataGridView_full.Columns[6].HeaderText = " ";
                dataGridView_full.Columns[7].HeaderText = " ";

                dataGridView_full.Columns[0].Name = "id_client";
                dataGridView_full.Columns[1].Name = "name";
                dataGridView_full.Columns[2].Name = "phone";
                dataGridView_full.Columns[3].Name = "email";

                Update("select * from client", Global_data.title);
            }
            else if (this.Text == "Продажи магазина")
            {
                dataGridView_full.Columns[0].Visible = true;
                dataGridView_full.Columns[1].Visible = true;
                dataGridView_full.Columns[2].Visible = true;
                dataGridView_full.Columns[3].Visible = true;
                dataGridView_full.Columns[4].Visible = true;
                dataGridView_full.Columns[5].Visible = true;
                dataGridView_full.Columns[6].Visible = true;
                dataGridView_full.Columns[7].Visible = true;

                dataGridView_full.Columns[0].HeaderText = "Номер заказа";
                dataGridView_full.Columns[1].HeaderText = "Имя";
                dataGridView_full.Columns[2].HeaderText = "Сотрудник";
                dataGridView_full.Columns[3].HeaderText = "Номер вел.";
                dataGridView_full.Columns[4].HeaderText = "Время арен. (мин)";
                dataGridView_full.Columns[5].HeaderText = "Стоимость";
                dataGridView_full.Columns[6].HeaderText = "Дата аренды";
                dataGridView_full.Columns[7].HeaderText = "Дата окончания";


                dataGridView_full.Columns[0].Name = "id_zakaz";
                dataGridView_full.Columns[1].Name = "id_client_zakaz";
                dataGridView_full.Columns[2].Name = "id_bike_zakaz ";
                dataGridView_full.Columns[3].Name = "id_sotrudnik_zakaz";
                dataGridView_full.Columns[4].Name = "min";
                dataGridView_full.Columns[5].Name = "date_start";
                dataGridView_full.Columns[6].Name = "date_end";
                dataGridView_full.Columns[7].Name = "oplata";


                Update("select zakaz.id_zakaz,concat(client.id_client,'-', client.name) as client,concat(sotrudnik.name, ' ', sotrudnik.surname) as sotrudnik,bike.number_bike,min,price, date_start,date_end from zakaz,bike,sotrudnik,client where zakaz.id_client_zakaz = client.id_client and bike.id_bike = zakaz.id_bike_zakaz and sotrudnik.id_sotrudnik = zakaz.id_sotrudnik_zakaz", Global_data.title);
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            if (this.Text == "Список велосипедов")
            {
                Update("select id_bike, status, number_bike, name_bike, size_rama, height_sm, name_size, price_min from bike,size_bike where bike.id_size = size_bike.id_size", Global_data.title);
            }
            else if (this.Text == "Список пользователей")
            {
                Update("select * from client", Global_data.title);
            }
            else if (this.Text == "Продажи магазина")
            {
                Update("select zakaz.id_zakaz,concat(client.id_client,'-', client.name) as client,concat(sotrudnik.name, ' ', sotrudnik.surname) as sotrudnik,bike.number_bike,min,price, date_start,date_end from zakaz,bike,sotrudnik,client where zakaz.id_client_zakaz = client.id_client and bike.id_bike = zakaz.id_bike_zakaz and sotrudnik.id_sotrudnik = zakaz.id_sotrudnik_zakaz", Global_data.title);
            }
        }
        private void button_modify_Click(object sender, EventArgs e)
        {
            if (dataGridView_full.Visible)
            {
                //try
                //{
                    
                 
                index = dataGridView_full.CurrentRow.Index;
                    var name_column = dataGridView_full.CurrentCell.OwningColumn.Name; //для определения названия столбца
                    var value_cell = dataGridView_full.CurrentCell.Value; //для определения значения в ячейке

                    temp_name = name_column.ToString(); //заносим в переменную
                    string[] item_user = new string[dataGridView_full.ColumnCount];

                    for (int i = 0; i < item_user.Length; i++)
                    {
                        item_user[i] = (string)dataGridView_full.Rows[index].Cells[i].Value;
                    }
                    textBox3.Text = value_cell.ToString();
                    textBox1.Text = "Код: " + item_user[0] + ", Имя: " + item_user[1];
                    panel1.Visible = true;
                
                //}
                //catch
                //{
                    
                //        MessageBox.Show
                //            ("Строка не выделена.  \n Выделите строку и попробуйте снова.",
                //            "Ошибка",
                //            MessageBoxButtons.OK,
                //            MessageBoxIcon.Error,
                //            MessageBoxDefaultButton.Button1
                //            );
                    
                //}
            }

        }
        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            panel1.Visible = false;
        }

        string temp_name = null;
        int index = 0;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
            button_upload_panel.Enabled = true;

        }
        private void button_upload_panel_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("not text");
            }
            else
            {
                DialogResult result = MessageBox.Show
                        ("Вы действительно хотите изменить запись?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1
                        );
                if (result == DialogResult.Yes)
                {
                    string zapros = null;
                    try
                    {
                        int index_num = int.Parse(dataGridView_full.CurrentRow.Cells[0].Value.ToString());
                        switch (this.Text)
                        {
                            case "Список пользователей": zapros = "update client set " + temp_name + " = " + "'" + textBox2.Text + "'" + " where id_client=" + index_num.ToString();
                                break;
                            case "Список велосипедов":
                                zapros = "update bike set " + temp_name + " = " + "'" + textBox2.Text + "'" + " where id_bike=" + index_num.ToString();
                                break;
                            case "Продажи магазина":
                                zapros = "update zakaz set " + temp_name + " = " + "'" + textBox2.Text + "'" + " where id_zakaz=" + index_num.ToString();
                                break;

                        }
                        command = new MySqlCommand(zapros, db.getConnection());
                        command.Connection.Open();
                        adapter.SelectCommand = command;
                        adapter.Fill(table);

                        command.Connection.Close();

                    }
                    catch (Exception)
                    {
                        MessageBox.Show
                        ("Введены неверные данные",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1
                        );
                    }
                }
                if (result == DialogResult.No)
                {
                    textBox2.Text = "";
                    panel1.Visible = false;
                }

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            panel1.Visible = false;
        }

        private void dataGridView_full_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Delete(Global_data.title);
        }

        bool a = true; //для перетаскивания панелей
        int x, y;

        private void panel1_LocationChanged(object sender, EventArgs e)
        {
            Size size = new Size();
            size.Height = Screen.PrimaryScreen.WorkingArea.Height;
            size.Width = Screen.PrimaryScreen.WorkingArea.Width;
            if (panel1.Location.X < 0)
            {
                panel1.Location = new Point(0, panel1.Location.Y);
            }
            if (panel1.Location.Y < 0)
            {
                panel1.Location = new Point(panel1.Location.X, 0);
            }
            if (panel1.Location.X + panel1.Size.Width > size.Width)
            {
                panel1.Location = new Point(size.Width - panel1.Size.Width, panel1.Location.Y);
            }
            if (panel1.Location.Y + panel1.Size.Height > size.Height)
            {
                panel1.Location =
                   new Point(panel1.Location.X, size.Height - panel1.Size.Height);
            }

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!a)
            {
                Panel mPanel = (Panel)sender;
                mPanel.Left += e.X - x;
                mPanel.Top += e.Y - y;
            }

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        { a = true; }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application exApp = new Excel.Application();
            exApp.Workbooks.Add();
           // exApp.Name = " ";
            Excel.Worksheet wsh = (Excel.Worksheet)exApp.ActiveSheet;
            int i, j;
            for (i = 0; i <= dataGridView_full.RowCount - 2; i++)
            {
                for (j = 0; j <= dataGridView_full.ColumnCount - 2; j++)
                {
                    wsh.Cells[i + 1, j + 1] = dataGridView_full[j, i].Value.ToString();
                }

            }
            exApp.Visible = true; 
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Panel mPanel = (Panel)sender;
            x = e.X;
            y = e.Y;
            a = false;

        }
    }
}
