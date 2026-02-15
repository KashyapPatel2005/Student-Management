using CRUDMVC.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVC.Data
{
    public class ApplicationDbContext
     : IdentityDbContext   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> students { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<UserLessonProgress> UserLessonProgresses { get; set; }


    }
}
