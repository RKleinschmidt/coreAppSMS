using System;
using System.Data;  
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using coreAppSMS.Models;
using coreAppSMS.Api;
using coreAppSMS.Database;

namespace coreAppSMS.Controllers
{
    public class HomeController : Controller
    {
        private string conString = "User ID=z94bvdHI4u;Password=CRtkqc0m6i;Server=remotemysql.com;Database=z94bvdHI4u;port=3306";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<smsMessage> smsLIST = new List<smsMessage>();  
            using (var connection = new MySqlConnection(conString))
            {
                connection.Open();
                 MySqlCommand command = new MySqlCommand("SELECT * FROM smsLOG", connection);
                using(MySqlDataReader reader = command.ExecuteReader()) 
                {
                    while(reader.Read())
                    {
                        smsMessage smsITEM = new smsMessage();
                        smsITEM.id = reader.GetString(reader.GetOrdinal("sms_id"));
                        smsITEM.message = reader.GetString(reader.GetOrdinal("sms_msg"));
                        smsITEM.date = reader.GetString(reader.GetOrdinal("sms_date"));
                        smsITEM.number = reader.GetString(reader.GetOrdinal("sms_number"));
                        smsLIST.Add(smsITEM);  
                    }
                }
                Console.WriteLine(smsLIST[0].message);
                return View(smsLIST);
            }
        }

        public IActionResult SendSMS()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SendNewSMS([FromBody] SeedMessage data){
            _logger.LogInformation("Starting message send");
            // In a production environment this input needs to be sanatised, but I am not going to do it for this exercise
                        
           IWorker worker = new ClickatellWrapperClient("ahtBtY72SJOsbrTZJu-SOg==");
            if (!worker.IsInitialized)
            {
                worker.Init();
            }

           SendMessageToAPI(worker, "27813457008", data.msg);

            using (var connection = new MySqlConnection(conString))
            {
                connection.Open();
                 MySqlCommand command = new MySqlCommand("Insert into smsLOG (sms_date, sms_msg, sms_number) values ('" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + "','" + data.msg + "','27813457008')", connection);
                 command.ExecuteReader();               
            }

            return Content("Your message was added to the send queue.");
        }

        private async void SendMessageToAPI(IWorker c, string to, string msg){   
            var res = await c.SendSmsAsync(to, msg);    

            Console.WriteLine(res.Messages[0].ApiMessageId);
            Console.WriteLine("-----------");
            Console.WriteLine(res.Messages[0].Accepted);
        }
    }
}
