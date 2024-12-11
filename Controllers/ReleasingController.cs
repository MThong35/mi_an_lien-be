using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs;
using thesis_comicverse_webservice_api.DTOs.AuthenticationDTO;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Releasing : ControllerBase
    {
        private readonly ILogger<Releasing> _logger;
        private readonly IConfiguration _configuration;
        private readonly IComicRepository _comicRepository;

        public Releasing(ILogger<Releasing> logger, IConfiguration configuration, IComicRepository comicRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _comicRepository = comicRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllReleasings()
        {
            try
            {
                _logger.LogInformation("Getting all releasing");
                var releasing = await _comicRepository.GetAllComicReleasingAsync();
                return Ok(releasing);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReleasingById(int id)
        {
            try
            {
                var author = await _comicRepository.GetReleasingByIdAsync(id);
                if (author == null)
                {
                    return NotFound();
                }
                return Ok(author);
            }
            catch
            {
                throw;
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateReleasing([FromBody] ComicReleasingDTO releasing)
        {
            try
            {
                _logger.LogInformation("Updating releasing");
                var repsonse = await _comicRepository.UpdateReleasingAsync(releasing);
                return Ok(repsonse);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateReleasing([FromBody] ComicReleasingDTO releasing)
        {
            try
            {
                _logger.LogInformation("Creating releasing");
                var repsonse = await _comicRepository.CreateReleasingAsync(releasing);
                return Ok(repsonse);
            }
            catch
            {
                throw;
            }
        }
    }
}
