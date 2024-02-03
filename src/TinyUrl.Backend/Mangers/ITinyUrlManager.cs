using TinyUrl.Backend.Models;

namespace TinyUrl.Backend.Mangers
{
    public interface ITinyUrlManager
    {
        Task<TinyUrlResponse> Create(TinyUrlRequest request);
        Task<string> GetOriginUrl(string tinyUrlId);
    }
}