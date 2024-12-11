using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs.AuthenticationDTO;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Author : ControllerBase
    {
        private readonly ILogger<Author> _logger;
        private readonly IConfiguration _configuration;
        private readonly IComicRepository _comicRepository;

        public Author(ILogger<Author> logger, IConfiguration configuration, IComicRepository comicRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _comicRepository = comicRepository;
        }

        [HttpGet("authors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                _logger.LogInformation("Getting all authors");
                var authors = await _comicRepository.GetAllAuthorsAsync();
                return Ok(authors);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("authors/{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var author = await _comicRepository.GetAuthorByIdAsync(id);
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

    }
}
