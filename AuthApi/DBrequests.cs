using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi
{
    public static class DBrequests
    {
        public static bool IsUserExists(string login, string password)
        {
            List<User> users = new List<User>();
            using (MySqlConnection connection = new MySqlConnection($@"Data Source=database-2.cu19far0udcw.us-east-1.rds.amazonaws.com,3306;User ID=admin;Password=12345qwert"))
            {
                connection.Open();

                string stringCommand = "SELECT * FROM AUTH.USER;";

                MySqlCommand command = new MySqlCommand(stringCommand, connection);
                using (MySqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        users.Add(new User
                        {
                            ID = int.Parse(oReader["ID"].ToString()),
                            EMAIL = oReader["EMAIL"].ToString(),
                            LOGIN = oReader["LOGIN"].ToString(),
                            PASS_HASH = oReader["PASS_HASH"].ToString(),
                        });
                    };
                }
                connection.Close();
            }

            return users.Any(x => x.LOGIN == login && BCrypt.Net.BCrypt.Verify(password,x.PASS_HASH));
        }
    }
}
