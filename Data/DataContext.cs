using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // Parameterless constructor to allow manual instantiation without options
    public DataContext() : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            // Configure SQLite provider with the given database file
            optionsBuilder.UseSqlite("Data Source=companyinfo.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between Project and ResponsibleEmployee
        modelBuilder.Entity<Project>()
            .HasOne(p => p.ResponsibleEmployee)
            .WithMany(e => e.Projects) // Ensure the navigation property is correctly referenced
            .HasForeignKey(p => p.ResponsibleEmployeeID)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure the relationship between Project and Client
        modelBuilder.Entity<Project>()
            .HasOne(p => p.AssociatedClient)
            .WithMany()
            .HasForeignKey(p => p.AssociatedClientID)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override async System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
