using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BMS.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class LoginController : Controller
    {
 

        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("Registration");
        public async Task<IActionResult> IndexAsync()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Registration register)
        {
            try
            {
              
                var email=  _dbContext.Registration.Find(e => e.EmailId == register.EmailId).FirstOrDefault();
                const string sessionEmail = "Email";
                const string sessionobjId = "ObjID";


                if ( email != null)
                {
                    if (email.EmailId != null)
                    {
                        HttpContext.Session.SetString(sessionEmail, email.EmailId);
                        HttpContext.Session.SetString(sessionobjId, email.ObjectId.ToString());
                   /*  var e =    HttpContext.Session.GetString("Email").FirstOrDefault();*/
                        return RedirectToAction("Index", "ShowMovies", new { area = "Users" });
                    }


                }else
                {
                    ModelState.AddModelError(string.Empty, "Email & Password is invalid..");
                    return View();
                }
               

                return RedirectToAction("Index");
            }
            catch(Exception e )
            {
                return View(e);
            }
        }

        // logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "Home", new { area = "Users" });
        }
    }
}
