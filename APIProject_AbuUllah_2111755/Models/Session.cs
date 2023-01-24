using System.ComponentModel.DataAnnotations;

namespace APIProject_AbuUllah_2111755.Models
{
    public class Session
    {
        [Key]
        public string Token { get; set; } 
        
        [Required]
        public string Email { get; set; }

        public Session()
        {
            this.Token = Guid.NewGuid().ToString();
        }

        public Session(string email)
        {
            this.Email = email;
            this.Token = Guid.NewGuid().ToString();
        }

    }
}
