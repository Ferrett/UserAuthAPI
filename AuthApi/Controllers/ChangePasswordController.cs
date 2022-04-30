using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangePasswordController : Controller
    {
        [HttpGet]
        public ObjectResult ChangePass(string login, string oldpass, string newpass)
        {
            if (DBrequests.IsUserExists(login, oldpass)==false)
                return StatusCode(203, "USER NOT FOUND");

            try
            {
                using (MySqlConnection connection = new MySqlConnection($@"Data Source=database-2.cu19far0udcw.us-east-1.rds.amazonaws.com,3306;User ID=admin;Password=12345qwert"))
                {
                    connection.Open();

                    string stringCommand = @$"UPDATE AUTH.USER SET PASS_HASH = '{BCrypt.Net.BCrypt.HashPassword(newpass)}'" +
                    @$"WHERE LOGIN = '{login}' and PASS_HASH = '{BCrypt.Net.BCrypt.HashPassword(oldpass)}'";

                    MySqlCommand command = new MySqlCommand(stringCommand, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                    return StatusCode(200, "OK");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(203, ex.Message);
            } 
        }
    }
}
