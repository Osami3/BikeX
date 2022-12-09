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
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
//using Microsoft.Data.SqlClient;
namespace BikeX
{

    public partial class Form1 : Form
    {
        //размеры панелей логина рега 1351; 701
        //static string pyt_Users = @"users.txt";  
        //static string pyt_History = @"history.txt";
        //string[] text_Users = File.ReadAllLines(pyt_Users); //заносит весь текст в массив по строкам
        //string[] text_History = File.ReadAllLines(pyt_History); //заносит весь текст в массив по строкам
        Form2 form_table = new Form2();
        DB db = new DB();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlDataReader reader;
        MySqlCommand command;
        Random rnd = new Random();
        int number_tovar = 0; //для счета товара на левой панели
        int number_bike = 0; //Для записи номера велосипеда
        int id_client = 0; //Для определения кода вошедшего пользователя
        int id_sotrudnik = 0; //Для определения кода вошедшего сотрудника
        bool sotrudnik = false; //Для определения зашел сотрудник или нет в систему
        public Form1()
        {
            InitializeComponent(); 
            
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //panelLogin.Visible = false;
            //panelReg.Visible = true;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) //список велосипедов
        {
            if (form_table == null || form_table.IsDisposed)
            {
                Global_data.title = "Список велосипедов";
                form_table = new Form2();
                form_table.StartPosition = FormStartPosition.CenterScreen;
                form_table.Show();

            }
            else
            {
                Global_data.title = "Список велосипедов";
                form_table.Show();

            }
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //список пользователей
        {
            if (form_table == null || form_table.IsDisposed)
            {
                Global_data.title = "Список пользователей";
                form_table = new Form2();
                form_table.StartPosition = FormStartPosition.CenterScreen;
                form_table.Show();

            }
            else
            {
                Global_data.title = "Список пользователей";
                form_table.Show();

            }
        }






        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView_shop.ColumnCount == 6) 
            {
                dataGridView_shop.Columns.Remove("delete_act_zakaz");
                dataGridView_shop.Columns.Remove("date");
                dataGridView_shop.Columns[0].Visible = true;
                dataGridView_shop.Columns[1].Visible = true;
                dataGridView_shop.Columns[2].Visible = true;
                dataGridView_shop.Columns[3].Visible = true;

                dataGridView_shop.Columns[1].HeaderText = "Велосипед";
                dataGridView_shop.Columns[2].HeaderText = "Время аренды";
                dataGridView_shop.Columns[3].HeaderText = "Ценa/мин.";


            }
            if (id_client != 0)
            {
                label17.Text = "Отчет: ";
                button_input_tovar.Enabled = true;
                textBox2.Enabled = true;
                button_add_zakaz.Enabled = true;
                button_zakaz_start.Enabled = false;
                //button_cancel_zakaz.Enabled = true;

            }
            else
            {
                button1.BackColor = Color.Silver;
                timer_color_but.Enabled = true;
                label17.Text = "Отчет: необходимо авторизировать клиента";

            }
        }

        private void Form1_Load(object sender, EventArgs e)

        {
        }

        private void linkLabel_mag_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sotrudnik == false)
            {
                panel_log.Visible = true;
            }
            else if (sotrudnik == true)
            {
                panelTable.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel_log.Visible = false;
            PhoneBox.Text = "";
            PasswordBox.Text = "";
        }


        private void button_input_tovar_Click(object sender, EventArgs e)
        {
            if (textBox2.Modified)
            {
                number_tovar++;
                command = new MySqlCommand("select id_bike, name_bike, price_min, number_bike, status from bike where number_bike = @number_bike", db.getConnection());
                command.Parameters.Add("@number_bike", MySqlDbType.VarChar).Value = textBox2.Text;
                command.Connection.Open();
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if(Convert.ToInt32(reader["status"]) == 1) //Проверка есть ли велосипед в наличии
                        {
                            number_bike = Convert.ToInt32(reader["id_bike"]);
                            //Заносим данные заказа в таблицу
                            string cache = Convert.ToString(number_tovar) + "!" +
                                            Convert.ToString(reader["name_bike"]) + "!" +
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "!" +//системное время и + 5 мин.
                                            Convert.ToString(reader["price_min"]);
                            string[] full = cache.Split(new char[] { '!' });

                            dataGridView_shop.Rows.Add(full);

                        }
                        else
                            {
                            label17.Text = "Отчет: Данный велосипед уже арендован";
                            }

                    }
                    command.Connection.Close();
                    reader.Close();
                    textBox2.Text = "";
                }
                else
                {
                    MessageBox.Show
                            ("Данного товара не существует.  \n Попробуйте ввести корректные данные.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1
                            );
                }
            }

        }

        private void button_log_Click(object sender, EventArgs e)
        {
            if (PhoneBox.Modified && PasswordBox.Modified) //проверка введены ли данные в поля
            {


                string loginUser = PhoneBox.Text;
                string pasUser = PasswordBox.Text;
                command = new MySqlCommand("select id_sotrudnik,surname,name,partronymic from sotrudnik where phone = @ul and password = @up", db.getConnection()); //sql команда
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = loginUser; //передаем значение из переменной в sql команду
                command.Parameters.Add("@up", MySqlDbType.VarChar).Value = pasUser;

                adapter.SelectCommand = command;
                adapter.Fill(table); //заполняем виртуальную таблицу

                if (table.Rows.Count > 0) //проверка на наличие данных
                {
                    command.Connection.Open(); //подключение команды
                    reader = command.ExecuteReader();
                    reader.Read(); //считывает данные
                    id_sotrudnik = Convert.ToInt32(reader["id_sotrudnik"]);
                    label13.Text = "Сотрудник: " + Convert.ToString(reader["surname"]) + " " + Convert.ToString(reader["name"]);
                    command.Connection.Close();
                    reader.Close();
                    panel_log.Visible = false;
                    panelTable.Visible = true;
                    linkLabel_exit.Text = "• выход из профиля";
                    sotrudnik = true;
                    PhoneBox.Text = "";
                    PasswordBox.Text = "";

                }
                else MessageBox.Show("Данного пользователя не существует");
            }
        }

        private void linkLabel_exit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(sotrudnik == false)
            {
                panel_log.Visible = true;
            }
            else if(sotrudnik == true)
            {
                panelTable.Visible = false;
                linkLabel_exit.Text = "• вход";
                sotrudnik = false;
                id_sotrudnik = 0;
                label13.Text = "Соткудник:";
            }
        }

        private void linkLabel_newUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel_reg.Visible = true;
        }

        private void button_reg_Click(object sender, EventArgs e)
        {
            if((Phone_userBox.Modified) && (Phone_userBox.Text.Length == 11) && (Phone_userBox.Text.StartsWith("8")))
            {
                try
                {
                    string PhoneUser = Phone_userBox.Text;
                    command = new MySqlCommand("select * from client where phone = @ul", db.getConnection());
                    command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = PhoneUser;

                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    if (table.Rows.Count > 0) 
                    {
                        command.Connection.Open();
                        reader = command.ExecuteReader();
                        reader.Read();
                        id_client = Convert.ToInt32(reader["id_client"]);
                        label16.Text = "Клиент: " + Convert.ToString(reader["name"]);
                        Name_userBox.Text = Convert.ToString(reader["name"]);
                        Email_userBox.Text = Convert.ToString(reader["email"]);

                        command.Connection.Close();
                        reader.Close();

                        button2.Visible = true;
                        button_reg.Visible = false;
                        //MessageBox.Show("Данный пользователь уже существует");
                    }


                    else
                    {
                        Name_userBox.Enabled = true;
                        Email_userBox.Enabled = true;
                    }

                }
                catch
                {
                    Name_userBox.Enabled = true;
                    Email_userBox.Enabled = true;
                }
            }
            else
            {
                label18.Text = "Введены некоректные данные";
                timer_color_but.Enabled = true;
            }

            if (Phone_userBox.Modified && Name_userBox.Modified && Email_userBox.Modified && Email_userBox.Text.EndsWith("@mail.ru"))
                {
                    command = new MySqlCommand("insert into client(phone,name,email) values(@phone,@name,@email)", db.getConnection());
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = Phone_userBox.Text;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name_userBox.Text;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email_userBox.Text;
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                command.Connection.Close();


                string PhoneUser = Phone_userBox.Text;
                command = new MySqlCommand("select * from client where phone = @ul", db.getConnection());
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = PhoneUser;
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    command.Connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    id_client = Convert.ToInt32(reader["id_client"]);
                    label16.Text = "Клиент: " + Convert.ToString(reader["name"]);
                    command.Connection.Close();
                    reader.Close();
                    button2.Visible = true;
                    button_reg.Visible = false;

                }


            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button_reg.Visible = true;
            button2.Visible = false;
            panel_reg.Visible = false;
            Phone_userBox.Text = "";
            Name_userBox.Text = "";
            Email_userBox.Text = "";
            Name_userBox.Enabled = false;
            Email_userBox.Enabled = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            button_reg.Visible = true;
            button2.Visible = false;
            panel_reg.Visible = false;
            Phone_userBox.Text = "";
            Name_userBox.Text = "";
            Email_userBox.Text = "";
            Name_userBox.Enabled = false;
            Email_userBox.Enabled = false;

        }

        private void linkLabel_prodaji_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (form_table == null || form_table.IsDisposed)
            {
                Global_data.title = "Продажи магазина";
                form_table = new Form2();
                form_table.StartPosition = FormStartPosition.CenterScreen;
                form_table.Show();

            }
            else
            {
                Global_data.title = "Продажи магазина";
                form_table.Show();

            }
        }

        private void linkLabel_history_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBox_size.SelectedIndex = 0;
            panelAddBike.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Box_name.Text = "";
            Box_number.Text = "";
            Box_price.Text = "";
            panelAddBike.Visible = false;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Box_name.Text = "";
            Box_number.Text = "";
            Box_price.Text = "";
            panelAddBike.Visible = false;

        }

        private void button10_Click(object sender, EventArgs e)
        {

            int size = 0;
            if(Box_name.Modified && Box_number.Modified && Box_price.Modified)
            {
                switch (listBox_size.SelectedIndex)
                {
                    case 0: size = 1; break;
                    case 1: size = 2; break;
                    case 2: size = 3; break;
                    case 3: size = 4; break;
                    case 4: size = 5; break;

                }
                try
                {
                    MySqlCommand command1 = new MySqlCommand("insert into bike(status,number_bike,id_size,price_min,name_bike) values(@status,@number_bike,@id_size,@price_min,@name_bike)", db.getConnection());
                    command1.Parameters.Add("@status", MySqlDbType.Int32).Value = 0;
                    command1.Parameters.Add("@number_bike", MySqlDbType.VarChar).Value = Box_number.Text;
                    command1.Parameters.Add("@id_size", MySqlDbType.Int32).Value = size;
                    command1.Parameters.Add("@price_min", MySqlDbType.Int32).Value = Convert.ToInt32( Box_price.Text);
                    command1.Parameters.Add("@name_bike", MySqlDbType.VarChar).Value = Box_name.Text;

                    adapter.SelectCommand = command1;
                    adapter.Fill(table);
                    MessageBox.Show("успешно");

                }
                    catch (System.FormatException)
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id_client == 0)
            {
                panel_reg.Visible = true;
            }
        }

        private void button_add_zakaz_Click(object sender, EventArgs e)
        {
            button_zakaz_end.Enabled = true;
            //button_cancel_zakaz.Enabled = false;

            for (int i = 0; i < dataGridView_shop.Rows.Count; i++)
            {
                string date_start = (string)dataGridView_shop.Rows[i].Cells[2].Value;

                command = new MySqlCommand("insert into active_zakaz(id_client,id_bike,id_sotrudnik,date_start) values(@id_client,@id_bike,@id_sotrudnik,@date_start)", db.getConnection());
                command.Parameters.Add("@id_client", MySqlDbType.Int32).Value = id_client;
                command.Parameters.Add("@id_bike", MySqlDbType.Int32).Value = number_bike;
                command.Parameters.Add("@id_sotrudnik", MySqlDbType.Int32).Value = id_sotrudnik;
                command.Parameters.Add("@date_start", MySqlDbType.DateTime).Value = date_start;
                adapter.SelectCommand = command;
                adapter.Fill(table);

                string zapros = "update bike set status = " + 0 + " where id_bike = " + number_bike;
                           
                    command = new MySqlCommand(zapros, db.getConnection());
                    command.Connection.Open();
                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    command.Connection.Close();

                    id_client = 0;
                label16.Text = "Клиент: ";
                label17.Text = "Отчет: заказ успешно добавлен";
                button_zakaz_start.Enabled = true;
                dataGridView_shop.Rows.Clear();
                button_input_tovar.Enabled = false;
                textBox2.Enabled = false;
                button_add_zakaz.Enabled = true;

            }
        }
        private void button_cancel_zakaz_Click(object sender, EventArgs e)
        {

            button_zakaz_start.Enabled = true;
            dataGridView_shop.Rows.Clear();
            button_input_tovar.Enabled = false;
            textBox2.Enabled = false;
            button_zakaz_end.Enabled = true;
            button_add_zakaz.Enabled = false;
            id_client = 0;
            label16.Text = "Клиент: ";
            label17.Text = "Отчет: заказ отменен";
            timer_color_but.Enabled = true;

        }

        private void timer_color_but_Tick(object sender, EventArgs e)
        {
            button1.BackColor = Color.DimGray;
            label17.Text = "Отчет: ";
            label18.Text = "";
        }

        

        private void button_zakaz_end_Click(object sender, EventArgs e)
        {
            if (dataGridView_shop.ColumnCount != 6)
            {
                dataGridView_shop.Enabled = true;
                dataGridView_shop.Columns.Add("delete_act_zakaz", "Закрыть");
                dataGridView_shop.Columns.Add("date", "Время аренды");
                dataGridView_shop.Columns[1].Visible = true;
                dataGridView_shop.Columns[2].Visible = true;
                dataGridView_shop.Columns[3].Visible = true;
                dataGridView_shop.Columns[4].Visible = true;
                dataGridView_shop.Columns[5].Visible = true;

                dataGridView_shop.Columns[1].HeaderText = "Имя клиента";
                dataGridView_shop.Columns[2].HeaderText = "Велосипед";
                dataGridView_shop.Columns[3].HeaderText = "Имя сотрудника";
                dataGridView_shop.Columns[4].HeaderText = "Время аренды";
                dataGridView_shop.Columns[5].HeaderText = " ";
            }

            command = new MySqlCommand("select id_active_zakaz, client.name, bike.name_bike, sotrudnik.name as name_sotr, date_start from active_zakaz,client,bike,sotrudnik where active_zakaz.id_client = client.id_client and active_zakaz.id_bike = bike.id_bike and active_zakaz.id_sotrudnik = sotrudnik.id_sotrudnik", db.getConnection());
            command.Connection.Open();
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                reader = command.ExecuteReader();
                
                    while (reader.Read())
                    {
                        string cache = Convert.ToString(reader["id_active_zakaz"]) + "!" +
                                       Convert.ToString(reader["name"]) + "!" +
                                       Convert.ToString(reader["name_bike"]) + "!" +
                                       Convert.ToString(reader["name_sotr"]) + "!" +
                                       Convert.ToString(reader["date_start"]) + "!" +
                                       "закрыть";
                        string[] full = cache.Split(new char[] { '!' });
                        dataGridView_shop.Rows.Add(full);
                    }

                
                command.Connection.Close();
                reader.Close();
            }


        }

        private void dataGridView_shop_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            string value = null;
            int index_num = 0;
            int index = 0;
            try
            {
                index = dataGridView_shop.CurrentRow.Index; //получение индекса строки
                index_num = int.Parse(dataGridView_shop.CurrentRow.Cells[0].Value.ToString()); //получение значения первого столбца строки
                var value_cell = dataGridView_shop.CurrentCell.Value; //для определения значения в ячейке
                value = value_cell.ToString();

            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show
                ("Заказ не добавлен. \nДобавьте заказ",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1
                );
                value = null;
                index_num = 0;
                index = 0;

            }
            if (value == "закрыть")
                {
                    DialogResult result = MessageBox.Show
                             ("Вы действительно хотите закрыть заказ?",
                             "Подтверждение",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question,
                             MessageBoxDefaultButton.Button1
                             );
                    if (result == DialogResult.Yes)
                    {
                        try
                        {

                            //достать данные из строки активных заказов
                            MySqlCommand com_conclusion = new MySqlCommand("select id_active_zakaz, id_client, active_zakaz.id_bike, id_sotrudnik, price_min,date_start from active_zakaz,bike where id_active_zakaz = @id and bike.id_bike = active_zakaz.id_bike", db.getConnection());
                            com_conclusion.Parameters.Add("@id", MySqlDbType.Int32).Value = index_num;
                            com_conclusion.Connection.Open();
                            adapter.SelectCommand = com_conclusion;
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                reader = com_conclusion.ExecuteReader();
                                reader.Read();

                                //меняет статус велосипеда
                                MySqlCommand com_bike = new MySqlCommand("update bike set status = 1 where id_bike = @id_bike", db.getConnection());
                                com_bike.Parameters.Add("@id_bike", MySqlDbType.Int32).Value = Convert.ToInt32(reader["id_bike"]);


                                string date1 = Convert.ToString(reader["date_start"]);
                                string[] temp_date1 = date1.Split(new char[] { ' ' });
                                date1 = temp_date1[1] + ":" + DateTime.Now.ToString("HH:mm:ss");
                                string[] minut = date1.Split(new char[] { ':' });
                                date1 = Convert.ToString(((Convert.ToInt32(minut[3]) - Convert.ToInt32(minut[0])) * 60) + (Convert.ToInt32(minut[4]) - Convert.ToInt32(minut[1])));
                                int price = Convert.ToInt32(date1) * Convert.ToInt32(reader["price_min"]);

                                //добавить заказ из активных заказов в историю заказов
                                command = new MySqlCommand("insert into zakaz(id_client_zakaz,id_bike_zakaz,id_sotrudnik_zakaz,min,price,date_start,date_end) values(@id_client_zakaz,@id_bike_zakaz,@id_sotrudnik_zakaz,@min,@price,@date_start,@date_end)", db.getConnection());
                                command.Parameters.Add("@id_client_zakaz", MySqlDbType.Int32).Value = Convert.ToInt32(reader["id_client"]);
                                command.Parameters.Add("@id_bike_zakaz", MySqlDbType.Int32).Value = Convert.ToInt32(reader["id_bike"]);
                                command.Parameters.Add("@id_sotrudnik_zakaz", MySqlDbType.Int32).Value = Convert.ToInt32(reader["id_sotrudnik"]);
                                command.Parameters.Add("@min", MySqlDbType.Int32).Value = date1;
                                command.Parameters.Add("@price", MySqlDbType.Int32).Value = price;
                                command.Parameters.Add("@date_start", MySqlDbType.DateTime).Value = Convert.ToDateTime(reader["date_start"]);
                                command.Parameters.Add("@date_end", MySqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                com_conclusion.Connection.Close();
                                reader.Close();

                                com_bike.Connection.Open();
                                adapter.SelectCommand = com_bike;
                                adapter.Fill(table);
                                com_bike.Connection.Close();

                                command.Connection.Open();
                                adapter.SelectCommand = command;
                                adapter.Fill(table);
                                command.Connection.Close();
                            }

                            dataGridView_shop.Rows.RemoveAt(index);
                            //удалить строку из таблицы активных заказов
                            command = new MySqlCommand("delete from active_zakaz where id_active_zakaz = @id LIMIT 1;", db.getConnection());
                            command.Parameters.Add("@id", MySqlDbType.Int32).Value = index_num;
                            command.Connection.Open();
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                            command.Connection.Close();





                            dataGridView_shop.Enabled = true;

                        }
                        catch
                        {
                            MessageBox.Show
                            ("Данный заказ временно невозможно закрыть. \n Обратитесь в службу поддержки \n Тел. 89996262067",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1
                            );
                        }
                    }
                    if (result == DialogResult.No)
                    {
                        dataGridView_shop.ClearSelection();
                    }
                }

            
        }

        bool a = true; //для перетаскивания панелей
        int x, y;

        private void panel_reg_MouseDown(object sender, MouseEventArgs e)
        {
            Panel mPanel = (Panel)sender;
            x = e.X;
            y = e.Y;
            a = false;
        }

        private void panel_reg_MouseMove(object sender, MouseEventArgs e)
        {
            if (!a)
            {
                Panel mPanel = (Panel)sender;
                mPanel.Left += e.X - x;
                mPanel.Top += e.Y - y;
            }

        }

        private void panel_reg_MouseUp(object sender, MouseEventArgs e)
        { a = true; }

        private void panelTable_MouseDown(object sender, MouseEventArgs e)
        {
            Panel mPanel = (Panel)sender;
            x = e.X;
            y = e.Y;
            a = false;
        }

        private void panelTable_MouseMove(object sender, MouseEventArgs e)
        {
            if (!a)
            {
                Panel mPanel = (Panel)sender;
                mPanel.Left += e.X - x;
                mPanel.Top += e.Y - y;
            }
        }

        private void panelAddBike_MouseDown(object sender, MouseEventArgs e)
        {
            Panel mPanel = (Panel)sender;
            x = e.X;
            y = e.Y;
            a = false;
        }

        private void panelAddBike_MouseMove(object sender, MouseEventArgs e)
        {
            if (!a)
            {
                Panel mPanel = (Panel)sender;
                mPanel.Left += e.X - x;
                mPanel.Top += e.Y - y;
            }

        }

        private void panelAddBike_MouseUp(object sender, MouseEventArgs e)
        { a = true; }

        private void panelAddBike_LocationChanged(object sender, EventArgs e)
        {
            Size size = new Size();
            size.Height = Screen.PrimaryScreen.WorkingArea.Height;
            size.Width = Screen.PrimaryScreen.WorkingArea.Width;
            if (panelAddBike.Location.X < 0)
            {
                panelAddBike.Location = new Point(0, panelAddBike.Location.Y);
            }
            if (panelAddBike.Location.Y < 0)
            {
                panelAddBike.Location = new Point(panelAddBike.Location.X, 0);
            }
            if (panelAddBike.Location.X + panelAddBike.Size.Width > size.Width)
            {
                panelAddBike.Location = new Point(size.Width - panelAddBike.Size.Width, panelAddBike.Location.Y);
            }
            if (panelAddBike.Location.Y + panelAddBike.Size.Height > size.Height)
            {
                panelAddBike.Location =
                   new Point(panelAddBike.Location.X, size.Height - panelAddBike.Size.Height);
            }

        }

        private void linkLabel_otchet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void panel_reg_LocationChanged(object sender, EventArgs e)
        {
            Size size = new Size();
            size.Height = Screen.PrimaryScreen.WorkingArea.Height;
            size.Width = Screen.PrimaryScreen.WorkingArea.Width;
            if (panel_reg.Location.X < 0)
            {
                panel_reg.Location = new Point(0, panel_reg.Location.Y);
            }
            if (panel_reg.Location.Y < 0)
            {
                panel_reg.Location = new Point(panel_reg.Location.X, 0);
            }
            if (panel_reg.Location.X + panel_reg.Size.Width > size.Width) 
            {
                panel_reg.Location = new Point(size.Width - panel_reg.Size.Width, panel_reg.Location.Y);
            }
            if (panel_reg.Location.Y + panel_reg.Size.Height > size.Height)
            {
                panel_reg.Location =
                   new Point(panel_reg.Location.X, size.Height - panel_reg.Size.Height);
            }
        }
    }
}
