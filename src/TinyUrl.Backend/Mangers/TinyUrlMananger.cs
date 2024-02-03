using System.Text;
using System.Text.Json;
using System.Web;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TinyUrl.Backend.Accessors;
using TinyUrl.Backend.Configurations;
using TinyUrl.Backend.Engines;
using TinyUrl.Backend.Infrastructure;
using TinyUrl.Backend.Models;

namespace TinyUrl.Backend.Mangers
{
    public class TinyUrlManager : ITinyUrlManager
    {
        private readonly ILogger<TinyUrlManager> _logger;
        private readonly ITinyUrlAccessor _accessor;
        private readonly ITinyUrlEngine _tinyUrlEngine;
        private readonly ICacheRepos _cache;
        private readonly IMapper _mapper;
        private readonly TinyUrlConfiguration _configuration;

        public TinyUrlManager(ILogger<TinyUrlManager> logger,
                              TinyUrlConfiguration configuration,
                              ITinyUrlAccessor accessor,
                              ITinyUrlEngine tinyUrlEngine,
                              ICacheRepos cache,
                              IMapper mapper)
        {
            _logger = logger;
            _accessor = accessor;
            _tinyUrlEngine = tinyUrlEngine;
            _cache = cache;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<TinyUrlResponse> Create(TinyUrlRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginUrl))
            {
                _logger.LogError("Url is missing, for the create request");
                throw new ArgumentNullException("Url is missing");
            }

            // I used the and not the cache because 
            // Iam not sure if cache is loaded,
            // My lookup for searching is the the tiny url id and not the original id
            // This is is kind of tread of tha create will be more slower the the search

            var result = await _accessor.GetTinyUrlByAddress(request.OriginUrl);

            if (result != null)
            {
                var tinyUrlObj = _mapper.Map<TinyUrlModel>(result);
                await _cache.SetCacheItem(result.Id, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tinyUrlObj)));

                return new TinyUrlResponse(result.Id);
            }

            // if OriginUrl do not exist , generate tiny url 
            var model = await CreateTinyUrlModel(request.OriginUrl);

            var saveToDb = _accessor.InsertTinyUrl(model);
            var saveToCache = _cache.SetCacheItem(model.Id, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));

            await Task.WhenAll(saveToDb, saveToCache);

            return new TinyUrlResponse(model.Id);
        }

        /// <summary>
        /// Creating a model for the iny url
        /// </summary>
        /// <param name="originUrl"></param>
        /// <returns></returns>
        private async Task<TinyUrlModel> CreateTinyUrlModel(string originUrl)
        {

            
            var tinyUrlNewId = await _tinyUrlEngine.GenerateRandomId();

            var uri = new Uri(originUrl);
            var newUrl = $"{uri.Scheme}://{_configuration.TinyNewDomain}/{tinyUrlNewId}";

            var tinyUrlObj = new TinyUrlModel()
            {
                Id = newUrl,
                OriginalUrl = originUrl,
            };

            return tinyUrlObj;
        }

        public async Task<string> GetOriginUrl(string url)
        {
            var decodedUrl = HttpUtility.UrlDecode(url);
            ValidateUrl(decodedUrl);


            // Try to fetch tiny url from the cache
            var tinyUrl = await _cache.GetCacheItem(decodedUrl);
            if (tinyUrl != null)
            {
                var json = Encoding.UTF8.GetString(tinyUrl.Value);
                var model = JsonSerializer.Deserialize<TinyUrlModel>(json);

                return model.OriginalUrl;
            }

            // try to fetch the tiny url from the Db
            var tinyUrlDb = await _accessor.GetTinyUrlById(decodedUrl);
            return tinyUrlDb?.OriginalUrl;
        }

        private void ValidateUrl(string decodedUrl)
        {
            if (!Uri.IsWellFormedUriString(decodedUrl, UriKind.Absolute))
            {
                _logger.LogError("Url Is invalid");
                throw new Exception("Invalid url");
            }

        }
    }
}
