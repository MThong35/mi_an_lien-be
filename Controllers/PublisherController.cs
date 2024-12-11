using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs.AuthenticationDTO;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Publisher : ControllerBase
    {
        private readonly ILogger<Publisher> _logger;
        private readonly IConfiguration _configuration;
        private readonly IComicRepository _comicRepository;

        public Publisher(ILogger<Publisher> logger, IConfiguration configuration, IComicRepository comicRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _comicRepository = comicRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllPublishers()
        {
            try
            {
                _logger.LogInformation("Getting all publisher");
                var publisher = await _comicRepository.GetAllPublishersAsync();
                return Ok(publisher);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            try
            {
                var publisher = await _comicRepository.GetPublisherByIdAsync(id);
                if (publisher == null)
                {
                    return NotFound();
                }
                return Ok(publisher);
            }
            catch
            {
                throw;
            }
        }

    }
}
