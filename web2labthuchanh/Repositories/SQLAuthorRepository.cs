using web2labthuchanh.Data;
using web2labthuchanh.Models.Domain;
using web2labthuchanh.Models.DTO;

namespace web2labthuchanh.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET ALL AUTHORS
        public List<AuthorDTO> GetAllAuthors()
        {
            var allAuthorsDomain = _dbContext.Authors.ToList();

            var allAuthorDTO = new List<AuthorDTO>();

            foreach (var authorDomain in allAuthorsDomain)
            {
                allAuthorDTO.Add(new AuthorDTO()
                {
                    Id = authorDomain.Id,
                    FullName = authorDomain.FullName
                });
            }

            return allAuthorDTO;
        }

        // GET AUTHOR BY ID
        public AuthorNoIdDTO? GetAuthorById(int id)
        {
            var authorWithIdDomain = _dbContext.Authors
                .FirstOrDefault(x => x.Id == id);

            if (authorWithIdDomain == null)
            {
                return null;
            }

            var authorNoIdDTO = new AuthorNoIdDTO()
            {
                FullName = authorWithIdDomain.FullName
            };

            return authorNoIdDTO;
        }

        // ADD AUTHOR
        public AddAuthorRequestDTO AddAuthor(
            AddAuthorRequestDTO addAuthorRequestDTO)
        {
            // Map DTO to Domain Model
            var authorDomainModel = new Author()
            {
                FullName = addAuthorRequestDTO.FullName
            };

            // Add Author
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();

            return addAuthorRequestDTO;
        }

        // UPDATE AUTHOR
        public AuthorNoIdDTO? UpdateAuthorById(
            int id,
            AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors
                .FirstOrDefault(x => x.Id == id);

            if (authorDomain == null)
            {
                return null;
            }

            authorDomain.FullName = authorNoIdDTO.FullName;

            _dbContext.SaveChanges();

            return authorNoIdDTO;
        }

        // DELETE AUTHOR
        public Author? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors
                .FirstOrDefault(x => x.Id == id);

            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();

                return authorDomain;
            }

            return null;
        }
    }
}