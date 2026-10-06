using System.ComponentModel.DataAnnotations;

namespace web2labthuchanh.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}