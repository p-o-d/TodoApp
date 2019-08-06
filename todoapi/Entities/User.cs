using System.ComponentModel.DataAnnotations;

namespace todoapi.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Range(3, 30)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Hash { get; set; }
    }
}