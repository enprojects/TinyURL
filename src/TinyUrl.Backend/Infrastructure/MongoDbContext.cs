using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TinyUrl.Backend.Configurations;
using TinyUrl.Backend.Models;

namespace TinyUrl.Backend.Infrastructure
{
    public class MongoDbContext : IDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(ILogger<MongoDbContext> logger, DbConfiguration configuration)
        {
            var client = new MongoClient(configuration.Connection);
            _database = client.GetDatabase(configuration.DbName);
        }

        public IMongoCollection<TinyUrlDb> TinyUrl => _database.GetCollection<TinyUrlDb>("TinyUrl");

    }
}
