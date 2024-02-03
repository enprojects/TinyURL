
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TinyUrl.Backend.Infrastructure;
using TinyUrl.Backend.Models;
using AutoMapper;

namespace TinyUrl.Backend.Accessors
{
    public class TinyUrlAccessor : ITinyUrlAccessor
    {
        private readonly ILogger<TinyUrlAccessor> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContext _context;

        public TinyUrlAccessor(ILogger<TinyUrlAccessor> logger,IMapper mapper, IDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<TinyUrlDb?> GetTinyUrlByAddress(string address)
        {
            var collection = _context.TinyUrl;
            var result = await collection.FindAsync(q => q.OriginalUrl == address);
           
            return result.FirstOrDefault();
        }

        public async Task<TinyUrlDb> GetTinyUrlById(string urlId)
        {
            var collection = _context.TinyUrl;
            var filter = Builders<TinyUrlDb>.Filter.Eq(doc => doc.Id, urlId);
          
            return await collection.Find(filter).FirstOrDefaultAsync();
        } 
        
        public async Task InsertTinyUrl(TinyUrlModel model)
        {
            var collection = _context.TinyUrl;
            var tinyUrlDb   = _mapper.Map<TinyUrlDb>(model);
           
            await collection.InsertOneAsync(tinyUrlDb);
        }
    }
}
