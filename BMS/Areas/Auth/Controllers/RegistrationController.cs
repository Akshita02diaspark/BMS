using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMS.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BMS.Areas.Auth.Controllers
{

    [Area("Auth")]
    public class RegistrationController : Controller
    {
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("Registration");
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Registration register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(register);
                }
             
                await this._dbContext.Registration.InsertOneAsync(register);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
