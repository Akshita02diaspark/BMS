using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BMS.Areas.Users.Controllers
{
    [Area("Users")]
    public class ShowMoviesController : Controller
    {
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("Movies");
        private readonly IDistributedCache _cache;

        public ShowMoviesController(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Movies> movies = null;
            using (IAsyncCursor<Movies> cursor = await this._dbContext.Movies.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    movies = cursor.Current;
                }
            }
          
            string jsonEmployees = _cache.GetString("Movies");

            if (jsonEmployees == null)
            {
                IEnumerable<Movies> employees = movies;
                jsonEmployees = JsonSerializer.
                Serialize<IEnumerable<Movies>>(employees);
                var options = new DistributedCacheEntryOptions();
                options.SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(1));
                _cache.SetString("Movies", jsonEmployees, options);
            }

            JsonSerializerOptions opt = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            List<Movies> data = JsonSerializer.Deserialize<List<Movies>>(jsonEmployees, opt);

            return View(movies);

        }
    }
}
