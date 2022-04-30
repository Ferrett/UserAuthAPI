using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        [HttpGet]
        public ObjectResult Register(string email, string login, string password, int confirmationCode)
        {
            if (DBrequests.IsUserExists(login, password) == true)
                return StatusCode(203, "THIS USER ALREADY EXISTS");
            if (GetConfirmationCodeController.ConfirmationCode != confirmationCode)
                return StatusCode(203, "WRONG CONFIRMATION CODE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection($@"Data Source=database-2.cu19far0udcw.us-east-1.rds.amazonaws.com,3306;User ID=admin;Password=12345qwert"))
                {
                    connection.Open();

                    string stringCommand = @$"INSERT INTO AUTH.USER(EMAIL,LOGIN,PASS_HASH) VALUES" +
                    @$"('{email}','{login}','{BCrypt.Net.BCrypt.HashPassword(password)}');";

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
