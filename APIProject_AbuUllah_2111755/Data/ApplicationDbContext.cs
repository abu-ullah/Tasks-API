using APIProject_AbuUllah_2111755.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProject_AbuUllah_2111755.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<Session> Sessions { get; set; }

    }
}
