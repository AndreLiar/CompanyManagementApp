using System.Linq;

public class DataSeeder
{
    private readonly DataContext _context;

    public DataSeeder(DataContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        // Check if Users DbSet is not null before calling Any
        if (_context.Users != null && !_context.Users.Any())
        {
            var adminUser = new User { Username = "admin", Role = "Admin" };
            adminUser.SetPassword("adminpass");
            var standardUser = new User { Username = "user", Role = "Standard" };
            standardUser.SetPassword("userpass");

            _context.Users.AddRange(adminUser, standardUser);
            _context.SaveChanges();
        }

        // Check if Employees DbSet is not null before calling Any
        if (_context.Employees != null && !_context.Employees.Any())
        {
            _context.Employees.Add(new Employee { FirstName = "John", LastName = "Doe", Position = "Developer", Salary = 50000 });
            _context.SaveChanges();
        }
    }
}
