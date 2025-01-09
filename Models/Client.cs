using System.ComponentModel.DataAnnotations;

public class Client
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Client name is required.")]
    [MaxLength(100, ErrorMessage = "Client name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number.")]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Industry cannot exceed 100 characters.")]
    public string Industry { get; set; } = string.Empty;

    public string CollaborationHistory { get; set; } = string.Empty;

    [Required]
    public string ClientType { get; set; } = "prospect"; // Options: prospect, actif, inactif

    [MaxLength(100, ErrorMessage = "Primary contact cannot exceed 100 characters.")]
    public string PrimaryContact { get; set; } = string.Empty; // Name of the primary contact person
}
