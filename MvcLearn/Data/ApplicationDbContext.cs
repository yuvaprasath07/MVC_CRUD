using Microsoft.EntityFrameworkCore;
using MvcLearn.Models;

namespace MvcLearn.Data
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }

        public DbSet<Brand> table { get; set; }  
    }
}
