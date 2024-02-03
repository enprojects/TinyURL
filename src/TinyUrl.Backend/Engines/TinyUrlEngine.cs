using Microsoft.Extensions.Logging;
using TinyUrl.Backend.Accessors;
using TinyUrl.Backend.Configurations;
using TinyUrl.Backend.Infrastructure;

namespace TinyUrl.Backend.Engines
{
    public class TinyUrlEngine : ITinyUrlEngine
    {
        private readonly ITinyUrlAccessor _accessor;
        private readonly ILogger<TinyUrlEngine> _logger;
        private readonly TinyUrlConfiguration _configuration;

        public TinyUrlEngine(ITinyUrlAccessor accessor, ILogger<TinyUrlEngine> logger, TinyUrlConfiguration configuration)
        {
            _accessor = accessor;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> GenerateRandomId()
        {
            var retrial = 0;
            string tinyUrlNewId;
            do
            {
                tinyUrlNewId = StringHelper.GenerateRandomBase62String(_configuration.TinyUrlLength);

                // check if record already exist in db to avoid collisions
                var tinyUrlDb = await _accessor.GetTinyUrlById(tinyUrlNewId);

                // If the ID does not exist in the database, break out of the loop. our tiny url Id is ok
                if (tinyUrlDb == null) break;

                retrial++;
            }
            // Continue retrying until a unique ID is found or the maximum number of retrials is reached.
            while (retrial < _configuration.MaxRetrials);

            if (retrial == _configuration.MaxRetrials)
            {
                _logger.LogError("Failed to create tiny ur");
                throw new Exception("Failed to create tiny url");
            }

            return tinyUrlNewId;
        }
    }
}
