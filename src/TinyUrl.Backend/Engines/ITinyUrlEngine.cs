namespace TinyUrl.Backend.Engines
{
    public interface ITinyUrlEngine
    {
        Task<string> GenerateRandomId();
    }
}
