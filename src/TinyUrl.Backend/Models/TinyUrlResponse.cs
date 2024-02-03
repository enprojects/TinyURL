namespace TinyUrl.Backend.Models
{
    public class TinyUrlResponse
    {
        public string TinyUrl { get; set; }

        public TinyUrlResponse(string tinyUrl)
        {
            TinyUrl = tinyUrl;
        }
    }
}
