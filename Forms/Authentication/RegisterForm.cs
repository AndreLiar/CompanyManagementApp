using System;
using System.Drawing;
using System.Windows.Forms;

public class RegisterForm : Form
{
    private readonly AuthController _authController;
    private readonly Action _showLoginView;

    // Initialize fields to prevent CS8618 warnings
    private TextBox usernameTextBox = null!;
    private TextBox passwordTextBox = null!;
    private TextBox confirmPasswordTextBox = null!;
    private Button registerButton = null!;
    private Button backButton = null!;

    public RegisterForm(AuthController authController, Action showLoginView)
    {
        _authController = authController;
        _showLoginView = showLoginView;
        InitializeComponent();
    }

    private void InitializeComponent()
{
    this.Size = new Size(450, 350);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.BackColor = Color.White;

    usernameTextBox = new TextBox
    {
        Location = new Point(180, 40),
        Width = 220,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.White,
        ForeColor = Color.Black
    };

    passwordTextBox = new TextBox
    {
        Location = new Point(180, 90),
        Width = 220,
        Font = new Font("Segoe UI", 10),
        PasswordChar = '*',
        BackColor = Color.White,
        ForeColor = Color.Black
    };

    confirmPasswordTextBox = new TextBox
    {
        Location = new Point(180, 140),
        Width = 220,
        Font = new Font("Segoe UI", 10),
        PasswordChar = '*',
        BackColor = Color.White,
        ForeColor = Color.Black
    };

    registerButton = new Button
    {
        Text = "Register",
        Location = new Point(180, 200),
        Width = 220,
        Height = 35,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.Blue,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
    };
    registerButton.FlatAppearance.BorderSize = 0;
    registerButton.Click += RegisterButton_Click;

    backButton = new Button
    {
        Text = "Back to Login",
        Location = new Point(180, 250),
        Width = 220,
        Height = 35,
        Font = new Font("Segoe UI", 10),
        BackColor = Color.White,
        ForeColor = Color.Blue,
        FlatStyle = FlatStyle.Flat
    };
    backButton.FlatAppearance.BorderSize = 1;
    backButton.Click += (s, e) => _showLoginView();

    Controls.Add(new Label
    {
        Text = "Username:",
        Location = new Point(60, 40),
        AutoSize = true,
        Font = new Font("Segoe UI", 10),
        ForeColor = Color.Black
    });
    Controls.Add(usernameTextBox);

    Controls.Add(new Label
    {
        Text = "Password:",
        Location = new Point(60, 90),
        AutoSize = true,
        Font = new Font("Segoe UI", 10),
        ForeColor = Color.Black
    });
    Controls.Add(passwordTextBox);

    Controls.Add(new Label
    {
        Text = "Confirm Password:",
        Location = new Point(60, 140),
        AutoSize = true,
        Font = new Font("Segoe UI", 10),
        ForeColor = Color.Black
    });
    Controls.Add(confirmPasswordTextBox);

    Controls.Add(registerButton);
    Controls.Add(backButton);

    Text = "Register";
}

    private void RegisterButton_Click(object? sender, EventArgs e)
    {
        if (usernameTextBox.Text == string.Empty || passwordTextBox.Text == string.Empty)
        {
            MessageBox.Show("Please fill in all fields.");
            return;
        }

        if (passwordTextBox.Text != confirmPasswordTextBox.Text)
        {
            MessageBox.Show("Passwords do not match.");
            return;
        }

        _authController.Register(usernameTextBox.Text, passwordTextBox.Text, "Standard");
        MessageBox.Show("Registration successful!");
        _showLoginView();
    }
}
