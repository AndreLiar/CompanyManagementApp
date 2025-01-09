public class AuthService
{
    private readonly DataContext _context;

    public AuthService(DataContext context)
    {
        _context = context;
    }

    public User? Authenticate(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        return user != null && user.ValidatePassword(password) ? user : null;
    }

    public bool RegisterUser(string username, string password, string role = "Standard")
{
    if (_context.Users.Any(u => u.Username == username))
    {
        return false; // Prevent duplicate usernames
    }

    var user = new User { Username = username, Role = role };
    user.SetPassword(password);
    _context.Users.Add(user);
    _context.SaveChanges();
    return true;
}

}
