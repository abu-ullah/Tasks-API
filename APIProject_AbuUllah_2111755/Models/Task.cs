using System.ComponentModel.DataAnnotations;

namespace APIProject_AbuUllah_2111755.Models
{
    public class Task
    {
        [Key]
        public string TaskUid { get; set; } 

        [Required]
        public string CreatedByUid { get; set; }

        [Required]
        public string CreatedByName { get; set; }   

        [Required]
        public string AssignedToUid { get; set; }

        [Required]
        public string AssignedToName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool Done { get; set; }  

        public Task()
        {
            this.Done = false;
            this.TaskUid = Guid.NewGuid().ToString();
        }

    }
}
