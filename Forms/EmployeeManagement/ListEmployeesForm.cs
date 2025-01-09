using System;
using System.Drawing;
using System.Windows.Forms;

public class ListEmployeesForm : Form
{
    private readonly EmployeeController _employeeController;
    private readonly string _role;

    private DataGridView employeesDataGridView = new DataGridView();
    private Button addButton = new Button();
    private Button editButton = new Button();
    private Button deleteButton = new Button();
    private Button backButton = new Button();

    public event Action? BackToDashboardRequested;

    public ListEmployeesForm(EmployeeController employeeController, string role)
    {
        _employeeController = employeeController;
        _role = role;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // Form Styles
        this.Text = "Employee Management";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;

        // DataGridView Styles
        employeesDataGridView.Dock = DockStyle.Top;
        employeesDataGridView.Height = 400;
        employeesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        employeesDataGridView.ReadOnly = true;
        employeesDataGridView.AllowUserToAddRows = false;
        employeesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        employeesDataGridView.MultiSelect = false;
        employeesDataGridView.Font = new Font("Segoe UI", 10);
        employeesDataGridView.BorderStyle = BorderStyle.Fixed3D;
        employeesDataGridView.GridColor = Color.LightGray;
        employeesDataGridView.DefaultCellStyle.BackColor = Color.White;
        employeesDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        employeesDataGridView.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
        employeesDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

        // Button Layout Panel
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(20, 20, 20, 20),
            BackColor = Color.White
        };

        // Add Button
        StyleButton(addButton, "Add Employee", _role == "Admin");
        addButton.Click += (s, e) => ShowEmployeeForm(null);

        // Edit Button
        StyleButton(editButton, "Edit Employee", _role == "Admin");
        editButton.Click += (s, e) => EditSelectedEmployee();

        // Delete Button
        StyleButton(deleteButton, "Delete Employee", _role == "Admin");
        deleteButton.Click += (s, e) => DeleteSelectedEmployee();

        // Back Button
        StyleButton(backButton, "Back to Dashboard", true);
        backButton.Click += (s, e) => BackToDashboardRequested?.Invoke();

        // Add buttons to panel
        buttonPanel.Controls.Add(addButton);
        buttonPanel.Controls.Add(editButton);
        buttonPanel.Controls.Add(deleteButton);
        buttonPanel.Controls.Add(backButton);

        // Add controls to form
        Controls.Add(employeesDataGridView);
        Controls.Add(buttonPanel);

        // Load employees into the grid
        LoadEmployees();
    }

    private void StyleButton(Button button, string text, bool enabled)
    {
        button.Text = text;
        button.Width = 180;
        button.Height = 40;
        button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        button.BackColor = enabled ? Color.Blue : Color.LightGray;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Enabled = enabled;
    }

    public void LoadEmployees()
    {
        try
        {
            var employees = _employeeController.GetEmployees();
            employeesDataGridView.DataSource = null; // Clear previous data
            employeesDataGridView.DataSource = employees;

            // Hide any unnecessary columns
            if (employeesDataGridView.Columns["ID"] != null)
            {
                employeesDataGridView.Columns["ID"].Visible = false;
            }

            employeesDataGridView.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowEmployeeForm(Employee? employee)
    {
        var employeeForm = new EmployeeForm(_employeeController, employee);
        employeeForm.FormClosed += (s, e) => LoadEmployees();
        employeeForm.ShowDialog();
    }

    private void EditSelectedEmployee()
    {
        if (employeesDataGridView.SelectedRows.Count > 0)
        {
            var employee = employeesDataGridView.SelectedRows[0].DataBoundItem as Employee;
            if (employee != null)
            {
                ShowEmployeeForm(employee);
            }
            else
            {
                MessageBox.Show("The selected employee is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void DeleteSelectedEmployee()
    {
        if (employeesDataGridView.SelectedRows.Count > 0)
        {
            var employee = employeesDataGridView.SelectedRows[0].DataBoundItem as Employee;
            if (employee != null)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete {employee.FirstName} {employee.LastName}?",
                                                     "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    _employeeController.DeleteEmployee(employee.ID);
                    LoadEmployees();
                }
            }
            else
            {
                MessageBox.Show("The selected employee is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select an employee to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
