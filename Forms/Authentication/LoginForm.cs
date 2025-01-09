using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

public class LoginForm : Form
{
    private readonly AuthController _authController;
    private readonly IServiceProvider _serviceProvider;

    private TextBox? usernameTextBox;
    private TextBox? passwordTextBox;
    private Button? loginButton;
    private LinkLabel? registerLink;

    public LoginForm(AuthController authController, IServiceProvider serviceProvider)
    {
        _authController = authController;
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private void InitializeComponent()
{
    this.Size = new System.Drawing.Size(400, 250);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.BackColor = Color.White;

    usernameTextBox = new TextBox
    {
        Location = new System.Drawing.Point(150, 40),
        Width = 200,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.White,
        ForeColor = Color.Black
    };

    passwordTextBox = new TextBox
    {
        Location = new System.Drawing.Point(150, 80),
        Width = 200,
        Font = new Font("Segoe UI", 10),
        PasswordChar = '*',
        BackColor = Color.White,
        ForeColor = Color.Black
    };

    loginButton = new Button
    {
        Text = "Login",
        Location = new System.Drawing.Point(150, 130),
        Width = 200,
        Height = 35,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.Blue,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
    };
    loginButton.FlatAppearance.BorderSize = 0;
    loginButton.Click += LoginButton_Click;

    registerLink = new LinkLabel
    {
        Text = "New user? Register here",
        Location = new System.Drawing.Point(150, 180),
        Width = 200,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 9),
        LinkColor = Color.Blue
    };
    registerLink.Click += RegisterLink_Click;

    var usernameLabel = new Label
    {
        Text = "Username:",
        Location = new System.Drawing.Point(50, 40),
        AutoSize = true,
        Font = new Font("Segoe UI", 10),
        ForeColor = Color.Black
    };

    var passwordLabel = new Label
    {
        Text = "Password:",
        Location = new System.Drawing.Point(50, 80),
        AutoSize = true,
        Font = new Font("Segoe UI", 10),
        ForeColor = Color.Black
    };

    Controls.Add(usernameLabel);
    Controls.Add(usernameTextBox);
    Controls.Add(passwordLabel);
    Controls.Add(passwordTextBox);
    Controls.Add(loginButton);
    Controls.Add(registerLink);

    Text = "Login";
}

    //newly added test

    private void RegisterLink_Click(object? sender, EventArgs e)
    {
        // Hide login form
        this.Hide();
        
        // Create and show register form
        var registerForm = new RegisterForm(_authController, () => {
            // Show login form again when returning from register
            this.Show();
        });
        
        registerForm.ShowDialog();
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        if (usernameTextBox is null || passwordTextBox is null) return;

        bool success = _authController.Login(usernameTextBox.Text, passwordTextBox.Text, out User? user);
        if (success && user != null)
        {
            // Successful login: navigate to dashboard
            MessageBox.Show($"Welcome, {user.Role}");

            // Always get a new instance of DashboardForm
            var dashboardForm = new DashboardForm(_serviceProvider);
            // dashboardForm = _serviceProvider.GetRequiredService<DashboardForm>();
            dashboardForm.SetUserRole(user.Role);

            // Show the DashboardForm and hide the login form
            dashboardForm.Show();
            this.Hide();
        }
        else
        {
            // Show error message if login fails
            MessageBox.Show("Invalid login credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
