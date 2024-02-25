namespace EntityFramework_DotNet7_SQLServer.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Weapon> Weapons => Set<Weapon>();
    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>().HasData(
            new Skill
            {
                Id = 1,
                Name = "Hammer of Justice",
                Damage = 30
            },
            new Skill
            {
                Id = 2,
                Name = "Consecration",
                Damage = 20
            },
            new Skill
            {
                Id = 3,
                Name = "Divine Storm",
                Damage = 50
            }
        );
    }
}