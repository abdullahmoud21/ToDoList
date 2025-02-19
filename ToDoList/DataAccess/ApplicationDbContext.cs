using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoTask> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=ABDULLAH; Initial Catalog=ToDolist; Integrated Security=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
