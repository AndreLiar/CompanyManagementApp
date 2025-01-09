using System;
using System.Security.Cryptography;
using System.Text;

public class User
{
    public int ID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; set; } = "Standard";

    public void SetPassword(string password)
    {
        PasswordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public bool ValidatePassword(string password)
    {
        return PasswordHash == HashPassword(password);
    }
}
