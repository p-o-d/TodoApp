using System.ComponentModel.DataAnnotations;

namespace todoapi.Dtos
{
    public class AuthCredentialsDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [Range(8,30)]
        public string Password { get; set; }
    }
}