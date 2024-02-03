using MongoDB.Driver;
using TinyUrl.Backend.Models;

namespace TinyUrl.Backend.Infrastructure
{
    public interface IDbContext
    {
        IMongoCollection<TinyUrlDb> TinyUrl { get; }
    }
}