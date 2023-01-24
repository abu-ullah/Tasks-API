using System.ComponentModel.DataAnnotations;

namespace APIProject_AbuUllah_2111755.Models
{
    public class User
    {
        [Key]
        public string Uid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }

        public User()
        {
            this.Uid = Guid.NewGuid().ToString();
        }



    }
}
