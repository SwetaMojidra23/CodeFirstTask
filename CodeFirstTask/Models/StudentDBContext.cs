using Microsoft.EntityFrameworkCore;

namespace CodeFirstTask.Models
{
    public class StudentDBContext : DbContext
    {
        public StudentDBContext(DbContextOptions option) : base(option){ 
        }   

        public DbSet<Student> Students { get; set; }
    }
}
