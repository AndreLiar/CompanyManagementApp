using System;
using System.Drawing;
using System.Windows.Forms;

public class EmployeeForm : Form
{
    private readonly EmployeeController _employeeController;
    private readonly Employee? _employee;

    private TextBox? firstNameTextBox;
    private TextBox? lastNameTextBox;
    private TextBox? positionTextBox;
    private TextBox? salaryTextBox;
    private DateTimePicker? hireDatePicker;
    private ComboBox? contractTypeComboBox;
    private TextBox? socialSecurityTextBox;
    private TextBox? addressTextBox;
    private TextBox? emailTextBox;
    private TextBox? phoneNumberTextBox;
    private DateTimePicker? birthDatePicker;
    private ComboBox? statusComboBox;
    private Button? saveButton;

    public EmployeeForm(EmployeeController employeeController, Employee? employee = null)
    {
        _employeeController = employeeController;
        _employee = employee;

        InitializeComponent();

        if (_employee != null)
        {
            // Populate fields if editing an existing employee
            firstNameTextBox!.Text = _employee.FirstName;
            lastNameTextBox!.Text = _employee.LastName;
            positionTextBox!.Text = _employee.Position;
            salaryTextBox!.Text = _employee.Salary.ToString();
            hireDatePicker!.Value = _employee.HireDate;
            contractTypeComboBox!.SelectedItem = _employee.ContractType;
            socialSecurityTextBox!.Text = _employee.SocialSecurityNumber;
            addressTextBox!.Text = _employee.Address;
            emailTextBox!.Text = _employee.Email;
            phoneNumberTextBox!.Text = _employee.PhoneNumber;
            birthDatePicker!.Value = _employee.BirthDate;
            statusComboBox!.SelectedItem = _employee.Status;
        }
    }

    private void InitializeComponent()
    {
        this.Size = new Size(450, 600);

        // Define controls with consistent styling
        firstNameTextBox = new TextBox { Location = new Point(200, 20), Width = 200 };
        lastNameTextBox = new TextBox { Location = new Point(200, 60), Width = 200 };
        positionTextBox = new TextBox { Location = new Point(200, 100), Width = 200 };
        salaryTextBox = new TextBox { Location = new Point(200, 140), Width = 200 };
        hireDatePicker = new DateTimePicker { Location = new Point(200, 180), Width = 200 };
        
        contractTypeComboBox = new ComboBox { Location = new Point(200, 220), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        contractTypeComboBox.Items.AddRange(new string[] { "CDI", "CDD", "FREELANCE", "INTERIMAIRE", "Consultant" });

        socialSecurityTextBox = new TextBox { Location = new Point(200, 260), Width = 200 };
        addressTextBox = new TextBox { Location = new Point(200, 300), Width = 200 };
        emailTextBox = new TextBox { Location = new Point(200, 340), Width = 200 };
        phoneNumberTextBox = new TextBox { Location = new Point(200, 380), Width = 200 };
        birthDatePicker = new DateTimePicker { Location = new Point(200, 420), Width = 200 };

        statusComboBox = new ComboBox { Location = new Point(200, 460), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        statusComboBox.Items.AddRange(new string[] { "Active", "On Leave", "Resigned" });

        saveButton = new Button { Text = "Save", Location = new Point(200, 500), Width = 200 };
        saveButton.Click += SaveButton_Click;

        // Labels
        Controls.Add(new Label { Text = "First Name:", Location = new Point(20, 25), AutoSize = true });
        Controls.Add(firstNameTextBox);
        Controls.Add(new Label { Text = "Last Name:", Location = new Point(20, 65), AutoSize = true });
        Controls.Add(lastNameTextBox);
        Controls.Add(new Label { Text = "Position:", Location = new Point(20, 105), AutoSize = true });
        Controls.Add(positionTextBox);
        Controls.Add(new Label { Text = "Salary:", Location = new Point(20, 145), AutoSize = true });
        Controls.Add(salaryTextBox);
        Controls.Add(new Label { Text = "Hire Date:", Location = new Point(20, 185), AutoSize = true });
        Controls.Add(hireDatePicker);
        Controls.Add(new Label { Text = "Contract Type:", Location = new Point(20, 225), AutoSize = true });
        Controls.Add(contractTypeComboBox);
        Controls.Add(new Label { Text = "Social Security:", Location = new Point(20, 265), AutoSize = true });
        Controls.Add(socialSecurityTextBox);
        Controls.Add(new Label { Text = "Address:", Location = new Point(20, 305), AutoSize = true });
        Controls.Add(addressTextBox);
        Controls.Add(new Label { Text = "Email:", Location = new Point(20, 345), AutoSize = true });
        Controls.Add(emailTextBox);
        Controls.Add(new Label { Text = "Phone Number:", Location = new Point(20, 385), AutoSize = true });
        Controls.Add(phoneNumberTextBox);
        Controls.Add(new Label { Text = "Birth Date:", Location = new Point(20, 425), AutoSize = true });
        Controls.Add(birthDatePicker);
        Controls.Add(new Label { Text = "Status:", Location = new Point(20, 465), AutoSize = true });
        Controls.Add(statusComboBox);
        Controls.Add(saveButton);

        Text = _employee == null ? "Add Employee" : "Edit Employee";
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        if (decimal.TryParse(salaryTextBox?.Text, out var salary))
        {
            var employee = _employee ?? new Employee();
            employee.FirstName = firstNameTextBox?.Text ?? string.Empty;
            employee.LastName = lastNameTextBox?.Text ?? string.Empty;
            employee.Position = positionTextBox?.Text ?? string.Empty;
            employee.Salary = salary;
            employee.HireDate = hireDatePicker?.Value ?? DateTime.Now;
            employee.ContractType = contractTypeComboBox?.SelectedItem?.ToString() ?? "CDI";
            employee.SocialSecurityNumber = socialSecurityTextBox?.Text ?? string.Empty;
            employee.Address = addressTextBox?.Text ?? string.Empty;
            employee.Email = emailTextBox?.Text ?? string.Empty;
            employee.PhoneNumber = phoneNumberTextBox?.Text ?? string.Empty;
            employee.BirthDate = birthDatePicker?.Value ?? DateTime.Now;
            employee.Status = statusComboBox?.SelectedItem?.ToString() ?? "Active";

            if (_employee == null)
                _employeeController.AddEmployee(employee);
            else
                _employeeController.UpdateEmployee(employee);

            MessageBox.Show("Employee saved successfully.");
            Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid salary.");
        }
    }
}
