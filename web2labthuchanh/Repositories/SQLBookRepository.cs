using Microsoft.EntityFrameworkCore;
using web2labthuchanh.Data;
using web2labthuchanh.Models.Domain;
using web2labthuchanh.Models.DTO;
using Wweb2labthuchanh.Repositories;

namespace web2labthuchanh.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET ALL BOOKS
        public List<BookWithAuthorAndPublisherDTO> GetAllBooks()
        {
            var allBooks = _dbContext.Books
                .Select(book => new BookWithAuthorAndPublisherDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead : null,
                    Rate = book.IsRead ? book.Rate : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    DateAdded = book.DateAdded,

                    PublisherName = book.Publisher != null
                        ? book.Publisher.Name
                        : "Unknown",

                    AuthorNames = book.Book_Authors
                        .Where(ba => ba.Author != null)
                        .Select(ba => ba.Author.FullName)
                        .ToList()
                })
                .ToList();

            return allBooks;
        }

        // GET BOOK BY ID
        public BookWithAuthorAndPublisherDTO? GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books
                .Where(book => book.Id == id);

            var bookWithIdDTO = bookWithDomain
                .Select(book => new BookWithAuthorAndPublisherDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    DateAdded = book.DateAdded,

                    PublisherName = book.Publisher != null
                        ? book.Publisher.Name
                        : "Unknown",

                    AuthorNames = book.Book_Authors
                        .Where(ba => ba.Author != null)
                        .Select(ba => ba.Author.FullName)
                        .ToList()
                })
                .FirstOrDefault();

            return bookWithIdDTO;
        }

        // ADD BOOK
        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            // Map DTO to Domain Model
            var bookDomainModel = new Book
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,

                PublisherID = addBookRequestDTO.PublisherID
            };

            // Add Book
            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            // Add Authors
            foreach (var authorId in addBookRequestDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = authorId
                };

                _dbContext.Books_Authors.Add(bookAuthor);
            }

            _dbContext.SaveChanges();

            return addBookRequestDTO;
        }

        // UPDATE BOOK
        public AddBookRequestDTO? UpdateBookById(
            int id,
            AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books
                .FirstOrDefault(book => book.Id == id);

            if (bookDomain == null)
            {
                return null;
            }

            bookDomain.Title = bookDTO.Title;
            bookDomain.Description = bookDTO.Description;
            bookDomain.IsRead = bookDTO.IsRead;
            bookDomain.DateRead = bookDTO.DateRead;
            bookDomain.Rate = bookDTO.Rate;
            bookDomain.Genre = bookDTO.Genre;
            bookDomain.CoverUrl = bookDTO.CoverUrl;
            bookDomain.DateAdded = bookDTO.DateAdded;
            bookDomain.PublisherID = bookDTO.PublisherID;

            _dbContext.SaveChanges();

            // Remove old authors
            var authorDomain = _dbContext.Books_Authors
                .Where(author => author.BookId == id)
                .ToList();

            if (authorDomain.Count > 0)
            {
                _dbContext.Books_Authors.RemoveRange(authorDomain);
                _dbContext.SaveChanges();
            }

            // Add new authors
            foreach (var authorId in bookDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorId
                };

                _dbContext.Books_Authors.Add(bookAuthor);
            }

            _dbContext.SaveChanges();

            return bookDTO;
        }

        // DELETE BOOK
        public Book? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books
                .FirstOrDefault(book => book.Id == id);

            if (bookDomain == null)
            {
                return null;
            }

            // Xóa các tác giả liên kết với sách
            var authorDomain = _dbContext.Books_Authors
                .Where(author => author.BookId == id)
                .ToList();

            if (authorDomain.Count > 0)
            {
                _dbContext.Books_Authors.RemoveRange(authorDomain);
            }

            // Xóa sách
            _dbContext.Books.Remove(bookDomain);

            _dbContext.SaveChanges();

            return bookDomain;
        }
    }
}