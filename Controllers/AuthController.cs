//Controllers/AuthController.cs
public class AuthController
{
    // Private field to hold the AuthService instance
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
//login method
    public bool Login(string username, string password, out User? user)
    {
        user = _authService.Authenticate(username, password);
        return user != null;
    }
//register method
    public bool Register(string username, string password, string role)
    {
        return _authService.RegisterUser(username, password, role);
    }
}
