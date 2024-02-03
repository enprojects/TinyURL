using TinyUrl.Backend.Models;

namespace TinyUrl.Backend.Accessors
{
    public interface ITinyUrlAccessor
    {
        Task<TinyUrlDb?> GetTinyUrlByAddress(string address);
        Task<TinyUrlDb> GetTinyUrlById(string address);
        Task InsertTinyUrl(TinyUrlModel model);
    }
}