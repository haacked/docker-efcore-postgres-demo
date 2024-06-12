using Microsoft.EntityFrameworkCore;

namespace DockerDemo.Web;

public class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }
}