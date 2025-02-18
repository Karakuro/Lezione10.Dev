using Microsoft.EntityFrameworkCore;

namespace Lezione10.Dev.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext() : base() { }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) 
            : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Exam> Exams { get; set; }
    }
}
