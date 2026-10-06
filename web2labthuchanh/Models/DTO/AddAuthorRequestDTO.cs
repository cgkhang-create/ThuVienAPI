using System.ComponentModel.DataAnnotations;

namespace web2labthuchanh.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required]
        [MinLength(3)]
        public string FullName { set; get; }
    }
}