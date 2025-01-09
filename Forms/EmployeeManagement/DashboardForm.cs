using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

public class DashboardForm : Form
{
    private string _role = "Standard"; // Default to Standard user
    private readonly IServiceProvider _serviceProvider;
    private Panel mainPanel;

    private ListEmployeesForm? employeeManagementForm;
    private ClientManagementForm? clientManagementForm;
    private bool _isDisposed = false; // Track if form is disposed

    public DashboardForm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        mainPanel = new Panel { Dock = DockStyle.Fill };
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Size = new System.Drawing.Size(800, 600);
        Controls.Add(mainPanel);
        ShowDashboardView();
    }

    public void SetUserRole(string role)
    {
        _role = role;
        ShowDashboardView(); // Refresh dashboard based on the role
    }

   private void ShowDashboardView()
{
    if (_isDisposed) return;

    mainPanel.Controls.Clear();

    var welcomeLabel = new Label
    {
        Text = $"Welcome, {_role}",
        Location = new Point(50, 20),
        AutoSize = true,
        Font = new Font("Segoe UI", 14, FontStyle.Bold),
        ForeColor = Color.Black
    };

    var employeeManagementButton = CreateStyledButton("Employee Management", new Point(50, 80));
    employeeManagementButton.Click += (s, e) => ShowEmployeeManagementView();

    mainPanel.Controls.Add(welcomeLabel);
    mainPanel.Controls.Add(employeeManagementButton);

    if (_role == "Admin")
    {
        var manageUsersButton = CreateStyledButton("Manage Users", new Point(50, 140));
        manageUsersButton.Click += (s, e) => ShowUserManagementView();

        var manageProjectsButton = CreateStyledButton("Manage Projects", new Point(50, 200));
        manageProjectsButton.Click += (s, e) => ShowProjectManagementView();

        var manageClientsButton = CreateStyledButton("Manage Clients", new Point(50, 260));
        manageClientsButton.Click += (s, e) => ShowClientManagementView();

        mainPanel.Controls.Add(manageUsersButton);
        mainPanel.Controls.Add(manageProjectsButton);
        mainPanel.Controls.Add(manageClientsButton);
    }
    else
    {
        var viewProjectsButton = CreateStyledButton("View Projects", new Point(50, 140));
        viewProjectsButton.Click += (s, e) => ShowProjectManagementView();

        var viewClientsButton = CreateStyledButton("View Clients", new Point(50, 200));
        viewClientsButton.Click += (s, e) => ShowClientManagementView();

        mainPanel.Controls.Add(viewProjectsButton);
        mainPanel.Controls.Add(viewClientsButton);
    }

    var logoutButton = CreateStyledButton("Logout", new Point(50, 320));
    logoutButton.Click += LogoutButton_Click;

    mainPanel.Controls.Add(logoutButton);

    Text = "Dashboard";
}

private Button CreateStyledButton(string text, Point location)
{
    return new Button
    {
        Text = text,
        Location = location,
        Width = 200,
        Height = 40,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.Blue,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat,
        FlatAppearance = { BorderSize = 0 }
    };
}

    private void ShowEmployeeManagementView()
    {
        if (_isDisposed) return;

        mainPanel.Controls.Clear();

        var employeeController = _serviceProvider.GetRequiredService<EmployeeController>();
        employeeManagementForm = new ListEmployeesForm(employeeController, _role);
        employeeManagementForm.BackToDashboardRequested += ShowDashboardView;

        employeeManagementForm.TopLevel = false;
        employeeManagementForm.FormBorderStyle = FormBorderStyle.None;
        employeeManagementForm.Dock = DockStyle.Fill;

        mainPanel.Controls.Add(employeeManagementForm);
        employeeManagementForm.Show();
    }

    private void ShowProjectManagementView()
    {
        if (_isDisposed) return;

        mainPanel.Controls.Clear();

        var projectController = _serviceProvider.GetRequiredService<ProjectController>();
        var projectManagementForm = new ProjectManagementForm(projectController, _role);
        projectManagementForm.BackToDashboardRequested += ShowDashboardView;

        projectManagementForm.TopLevel = false;
        projectManagementForm.FormBorderStyle = FormBorderStyle.None;
        projectManagementForm.Dock = DockStyle.Fill;

        mainPanel.Controls.Add(projectManagementForm);
        projectManagementForm.Show();
    }

    private void ShowClientManagementView()
    {
        if (_isDisposed) return;

        mainPanel.Controls.Clear();

        var clientController = _serviceProvider.GetRequiredService<ClientController>();
        clientManagementForm = new ClientManagementForm(clientController, _role);
        clientManagementForm.BackToDashboardRequested += ShowDashboardView;

        clientManagementForm.TopLevel = false;
        clientManagementForm.FormBorderStyle = FormBorderStyle.None;
        clientManagementForm.Dock = DockStyle.Fill;

        mainPanel.Controls.Add(clientManagementForm);
        clientManagementForm.Show();
    }

    private void ShowUserManagementView()
    {
        if (_isDisposed) return;

        mainPanel.Controls.Clear();

        var adminLabel = new Label
        {
            Text = "Admin User Management",
            Location = new System.Drawing.Point(100, 20),
            AutoSize = true,
            Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold)
        };

        var backButton = new Button
        {
            Text = "Back to Dashboard",
            Location = new System.Drawing.Point(100, 80),
            Width = 200,
            Height = 40
        };
        backButton.Click += (s, e) => ShowDashboardView();

        mainPanel.Controls.Add(adminLabel);
        mainPanel.Controls.Add(backButton);
    }

    private void LogoutButton_Click(object? sender, EventArgs e)
    {
        _isDisposed = false;

        // Hide and dispose the DashboardForm before showing the LoginForm
        this.Hide();
        this.Dispose();
        this.Close();

        // Show the login form again
        var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
        loginForm.Show();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        foreach (Control control in mainPanel.Controls)
        {
            control.Dispose();
        }
        base.OnFormClosing(e);
    }
}
