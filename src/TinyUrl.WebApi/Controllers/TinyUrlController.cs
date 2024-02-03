using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using TinyUrl.Backend.Infrastructure;
using TinyUrl.Backend.Mangers;
using TinyUrl.Backend.Models;
using TinyUrl.WebApi.ViewModel;

namespace TinyUrl.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TinyUrlController : ControllerBase
    {

        private readonly ILogger<TinyUrlController> _logger;
        private readonly IMapper _mapper;
        private readonly ITinyUrlManager _tinyUrlManager;

        public TinyUrlController(ILogger<TinyUrlController> logger,
                                 IMapper mapper, 
                                 ITinyUrlManager tinyUrlManager)
        {
            _logger = logger;
            _mapper = mapper;
            _tinyUrlManager = tinyUrlManager;
        }

        [HttpPost(Name = "create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TinyUrlResponseViewModel))]
        public async Task<IActionResult> Create(TinyUrlRequestViewModel viewModel)
        {
            var response = await _tinyUrlManager.Create(_mapper.Map<TinyUrlRequest>(viewModel));
            return Ok(_mapper.Map<TinyUrlResponseViewModel>(response));
        }


        [HttpGet("{tinyUrl}")]
        public async Task<IActionResult> GetOriginUrl([FromRoute]  string tinyUrl)
        { 
            var originalUrl = await _tinyUrlManager.GetOriginUrl(tinyUrl);

            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return NotFound(); 
            }

            return Redirect(originalUrl);
        }
    }
}
