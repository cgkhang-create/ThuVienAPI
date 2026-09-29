using web2labthuchanh.Models.Domain;
using web2labthuchanh.Models.DTO;

namespace web2labthuchanh.Repositories
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GetAllAuthors();

        AuthorNoIdDTO? GetAuthorById(int id);

        AddAuthorRequestDTO AddAuthor(
            AddAuthorRequestDTO addAuthorRequestDTO);

        AuthorNoIdDTO? UpdateAuthorById(
            int id,
            AuthorNoIdDTO authorNoIdDTO);

        Author? DeleteAuthorById(int id);
    }
}