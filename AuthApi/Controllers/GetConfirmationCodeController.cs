using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetConfirmationCodeController : Controller
    {
        public static int ConfirmationCode;
        public Random rnd = new Random();

        [HttpGet]
        public ObjectResult SendMail(string email)
        {
            try
            {
                ConfirmationCode =int.Parse($"{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}");

                var fromAddress = new MailAddress("vovkaprikhod@gmail.com", "Automatic Email");
                var toAddress = new MailAddress($"{email}", "To Name");
                const string fromPassword = "Prikhod322";
                const string subject = "Ваш код подтверждения";
                string body = ConfirmationCode.ToString();

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
                return StatusCode(200,"OK");
            }
            catch (Exception ex)
            {
                return StatusCode(200, ex.Message);
            }
        }
    }
}