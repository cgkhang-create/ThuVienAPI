using web2labthuchanh.Models.Domain;
using web2labthuchanh.Models.DTO;

namespace web2labthuchanh.Repositories
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
            bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO);
        PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO);
        Publisher? DeletePublisherById(int id);
        List<BookWithAuthorAndPublisherDTO> GetBooksByPublisherId(int id);
        bool ExistsPublisherName(string name, int? excludeId = null);

    }
}