using System.ComponentModel.DataAnnotations;

namespace UserManagement.Dtos
{
    public class loginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
