using System.ComponentModel.DataAnnotations;

namespace TinyUrl.WebApi.ViewModel
{
    public class TinyUrlRequestViewModel
    {
        [Required]
        public string OriginUrl { get; set; }
    }
}
