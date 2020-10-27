
using BMS.AppConfig;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public class clsMongoDBDataContext
    {
        private string _connectionStrings = string.Empty;
        private string _databaseName = string.Empty;
        private string _collectionName = string.Empty;


        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public clsMongoDBDataContext(string strCollectionName)
        {
            this._collectionName = strCollectionName;
            this._connectionStrings = AppConfiguration.GetConfiguration("ServerName");
            this._databaseName = AppConfiguration.GetConfiguration("DatabaseName");
            this._client = new MongoClient(_connectionStrings);
            this._database = _client.GetDatabase(_databaseName);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoDatabase Database
        {
            get { return _database; }
        }

        public IMongoCollection<Product> GetProducts
        {
            get { return _database.GetCollection<Product>(_collectionName); }
        }
        public IMongoCollection<Registration> Registration
        {
            get { return _database.GetCollection<Registration>(_collectionName); }
        }
        public IMongoCollection<Movies> Movies 
        {
            get { return _database.GetCollection<Movies>(_collectionName); }
        }
        public IMongoCollection<Theater> Theater
        {
            get { return _database.GetCollection<Theater>(_collectionName); }
        }
    }
}
