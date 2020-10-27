using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BMS.Areas.Users.Controllers
{
    [Area("Users")]
    public class ShowTheaterController : Controller
    {
        public static string connstring = "DefaultEndpointsProtocol=https;AccountName=demoqueue;AccountKey=vfxaR5nsyt9HXgNnOutXwfM1HV3gix4YKtFH05Kesc3oJGAidX/PBH40MjWCAylKlkCcXAIEpMeRXu5eNOJM5Q==;EndpointSuffix=core.windows.net";
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("Theater");
        public async Task<IActionResult> Index(string id)
        {

             TempData["data"] = id;
          
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://theaterapi20201025193301.azurewebsites.net/Home/TheaterSeats/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var a = JsonConvert.DeserializeObject<JObject>(apiResponse);
                    var s = a.First.First.ToString();
                    var d = a.Last.Last.ToString();
                    ViewBag.booked = Int16.Parse(s);
                    ViewBag.unbooked = Int16.Parse(d);


                }
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> TheaterSeats(string[] values)
        {

            if (values != null)
            {
                var value = _dbContext.Theater.Find(e => e.MovieId == TempData["data"].ToString()).FirstOrDefault();

                if (value != null)
                {

                    string Seats = (Int16.Parse(value.ReservedSeats) + values.Length).ToString();
                   
                    
                    FilterDefinition<Theater> filter = Builders<Theater>.Filter.Eq("MovieId", TempData["data"].ToString());
                    var updateDef = Builders<Theater>.Update.Set("ReservedSeats", Seats);
                     this._dbContext.Theater.UpdateOne(filter, updateDef);

                    try
                    {
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("thisistestuser1244@gmail.com");
                        message.To.Add(new MailAddress(HttpContext.Session.GetString("Email")));
                        message.Subject = "Booking succesfull";
                        message.IsBodyHtml = true; //to make message body as html  
                        message.Body = "Your Booking is done now you will get a ticket soon ";

                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com"; //for gmail host  
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("thisistestuser1244@gmail.com", "nskxyfngqxzsmxsi");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e);

                    }

                }
                else
                {
                    Theater t = new Theater()
                    {
                        MovieId = TempData["data"].ToString(),
                        ReservedSeats = values.Length.ToString()
                    };
                    await this._dbContext.Theater.InsertOneAsync(t);

                    try
                    {
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("thisistestuser1244@gmail.com");
                        message.To.Add(new MailAddress(HttpContext.Session.GetString("Email")));
                        message.Subject = "Booking succesfull";
                        message.IsBodyHtml = true; //to make message body as html  
                        message.Body = "Your Booking is done now you will get a ticket soon ";

                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com"; //for gmail host  
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("thisistestuser1244@gmail.com", "nskxyfngqxzsmxsi");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e);

                    }
                }
            }
          
            return RedirectToAction("Index", new { id = TempData["data"].ToString() });

        }
    }
}
