using web2labthuchanh.Models.Domain;
using System.ComponentModel.DataAnnotations;
namespace web2labthuchanh.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
    public class AuthorNoIdDTO
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; }
    }
}