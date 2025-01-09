public class Project
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime Deadline { get; set; }
    public string Status { get; set; } = "In Progress"; // Default to In Progress
    public int? ResponsibleEmployeeID { get; set; }
    public Employee? ResponsibleEmployee { get; set; }
    public int? AssociatedClientID { get; set; }
    public Client? AssociatedClient { get; set; }
}
