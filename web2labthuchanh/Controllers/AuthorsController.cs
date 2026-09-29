using Microsoft.AspNetCore.Mvc;
using web2labthuchanh.Data;
using web2labthuchanh.Models.DTO;
using web2labthuchanh.Repositories;

namespace web2labthuchanh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(
            AppDbContext dbContext,
            IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        // GET: /api/Authors/get-all-author
        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor()
        {
            // Sử dụng Repository Pattern
            var allAuthors = _authorRepository.GetAllAuthors();

            return Ok(allAuthors);
        }

        // GET: /api/Authors/get-author-by-id/1
        [HttpGet]
        [Route("get-author-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var authorWithId =
                _authorRepository.GetAuthorById(id);

            return Ok(authorWithId);
        }

        // POST: /api/Authors/add-author
        [HttpPost("add-author")]
        public IActionResult AddAuthors(
            [FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd =
                _authorRepository.AddAuthor(addAuthorRequestDTO);

            return Ok(authorAdd);
        }

        // PUT: /api/Authors/update-author-by-id/1
        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(
            int id,
            [FromBody] AuthorNoIdDTO authorDTO)
        {
            var authorUpdate =
                _authorRepository.UpdateAuthorById(id, authorDTO);

            return Ok(authorUpdate);
        }

        // DELETE: /api/Authors/delete-author-by-id/1
        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var authorDelete =
                _authorRepository.DeleteAuthorById(id);

            return Ok(authorDelete);
        }
    }
}