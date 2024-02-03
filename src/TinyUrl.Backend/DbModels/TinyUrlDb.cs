using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TinyUrl.Backend.Models
{
    public class TinyUrlDb
    {
        [BsonId]  
        public string Id { get; set; }
      //  public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
      //  public DateTime CreatedAt { get; set; } = DateTime.Now;
     
    }
}
