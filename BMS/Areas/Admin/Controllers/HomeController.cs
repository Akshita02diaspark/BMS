using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;
        clsMongoDBDataContext _dbContext = new clsMongoDBDataContext("Movies");
        clsMongoDBDataContext _dbContextforregister = new clsMongoDBDataContext("Registration");



        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;


        }
        

        public async Task<IActionResult> Index()
        {
            IEnumerable<Registration> users = null;
            using (IAsyncCursor<Registration> cursor = await this._dbContextforregister.Registration.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    users = cursor.Current;
                }
            }
            return View(users);
        }


        // delete user
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
                {
                if (string.IsNullOrEmpty(id))
                {
                   
                    FilterDefinition<Registration> filter = Builders<Registration>.Filter.Eq("_id", ObjectId.Parse(id));
                    await this._dbContextforregister.Registration.DeleteOneAsync(filter);
                    return RedirectToAction("Index");
                }
                }
                catch
                {
                    return View();
                }
             return View();
        }

        // add movies
        public async Task<IActionResult> Addmovies()
        {
             return View();
        }
           
        
        [HttpPost]
        public async Task<IActionResult> Addmovies(Movies files)
             {
                        string blobstorageconnection = _configuration.GetValue<string>("blobstorage");

                        byte[] dataFiles;
                        // Retrieve storage account from connection string.
                        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                        // Create the blob client.
                        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                        // Retrieve a reference to a container.
                        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("blobcontainer");

                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };
                        string systemFileName = files.ImageFile.FileName;
                        await cloudBlobContainer.SetPermissionsAsync(permissions);
                        await using (var target = new MemoryStream())
                        {
                            files.ImageFile.CopyTo(target);
                            dataFiles = target.ToArray();
                        }
                        // This also does not make a service call; it only creates a local object.
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                        await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                        BlobResultSegment resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty,
                            true, BlobListingDetails.Metadata, 100, null, null, null);
                         Movies m = new Movies()
                        {
                            MovieName = files.MovieName,
                            InitialRelease = files.InitialRelease,
                            Director = files.Director,
                            Genre = files.Genre,
                            ImageURL = resultSegment.Results.FirstOrDefault().Uri.ToString()
                        };
                        await this._dbContext.Movies.InsertOneAsync(m);
            return RedirectToAction("Index");
        }

        //  delete movies
        public async Task<IActionResult> Delete(string id)
            {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    //  model.ObjectId = ObjectId.Parse(id);
                    FilterDefinition<Movies> filter = Builders<Movies>.Filter.Eq("_id", ObjectId.Parse(id));
                    await this._dbContext.Movies.DeleteOneAsync(filter);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();

        }

        //edit movies
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                FilterDefinition<Movies> filter = Builders<Movies>.Filter.Eq("_id", ObjectId.Parse(id));
                IEnumerable<Movies> entity = null;
                using (IAsyncCursor<Movies> cursor = await this._dbContext.Movies.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        entity = cursor.Current;
                    }
                }
                return View(entity.FirstOrDefault());
            }
            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Movies model)
        {
            try
            {

                string blobstorageconnection = _configuration.GetValue<string>("blobstorage");

                byte[] dataFiles;
                // Retrieve storage account from connection string.
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                // Create the blob client.
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("blobcontainer");

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                string systemFileName = model.ImageFile.FileName;
                await cloudBlobContainer.SetPermissionsAsync(permissions);
                await using (var target = new MemoryStream())
                {
                    model.ImageFile.CopyTo(target);
                    dataFiles = target.ToArray();
                }
                // This also does not make a service call; it only creates a local object.
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                BlobResultSegment resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty,
                    true, BlobListingDetails.Metadata, 100, null, null, null);
                List<FileData> fileList = new List<FileData>();


                Movies m = new Movies()
                {
                     ObjectId = ObjectId.Parse(id),
                    MovieName = model.MovieName,
                    InitialRelease = model.InitialRelease,
                    Director = model.Director,
                    Genre = model.Genre,
                    ImageURL = resultSegment.Results.LastOrDefault().Uri.ToString()
                };
                
                FilterDefinition<Movies> filter = Builders<Movies>.Filter.Eq("_id", ObjectId.Parse(id));
                await this._dbContext.Movies.ReplaceOneAsync(filter, m, new UpdateOptions() { IsUpsert = true });
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        
        
        public async Task<IActionResult> ShowAllBlobs()
        {
            IEnumerable<Movies> movies = null;
            using (IAsyncCursor<Movies> cursor = await this._dbContext.Movies.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    movies = cursor.Current;
                }
            }
            return View(movies);
           

        }

    }
}
