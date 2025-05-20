using Microsoft.EntityFrameworkCore;
using Tutorial9.Model;

namespace Tutorial9.DAL;

public class DatabaseDbContext: DbContext
{
    public DbSet<Student> Student { get; set; }
    protected DatabaseDbContext()
    {
    }

    public DatabaseDbContext(DbContextOptions options) : base(options)
    {
    }
}