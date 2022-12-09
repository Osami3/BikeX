using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeX
{
    class DB
    {
      // MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=swag;password=swag;database=19099_bikex");
        MySqlConnection connection = new MySqlConnection("server=pma.web.edu;port=3306;username=19099;password=razjol;database=19099_BikeX");


        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
