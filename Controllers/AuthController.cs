//Controllers/AuthController.cs
public class AuthController
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    public bool Login(string username, string password, out User? user)
    {
        user = _authService.Authenticate(username, password);
        return user != null;
    }

    public bool Register(string username, string password, string role)
    {
        return _authService.RegisterUser(username, password, role);
    }
}
