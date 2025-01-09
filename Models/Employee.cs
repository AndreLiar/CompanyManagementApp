using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Employee
{
    public int ID { get; set; }
    
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; } = string.Empty;
    
    [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
    public string Position { get; set; } = string.Empty;
    
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
    public decimal Salary { get; set; }
    
    [Required]
    public DateTime HireDate { get; set; } = DateTime.Now;
    
    [Required]
    [MaxLength(20, ErrorMessage = "Contract type cannot exceed 20 characters.")]
    public string ContractType { get; set; } = "CDI"; // Options: CDI, CDD, FREELANCE, INTERIMAIRE, Consultant
    
    [Required]
    [MaxLength(20, ErrorMessage = "Social Security Number cannot exceed 20 characters.")]
    public string SocialSecurityNumber { get; set; } = string.Empty;
    
    [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "Invalid phone number.")]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Active"; // Options: Active, On Leave, Resigned
    
    // Navigation property to link Employee to the Projects they're assigned to (Many-to-Many)
    public List<Project> Projects { get; set; } = new List<Project>();
}
