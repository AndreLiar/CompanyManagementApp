using System;
using System.Drawing;
using System.Windows.Forms;

public class ProjectForm : Form
{
    private readonly ProjectController _projectController;
    private readonly Project? _project;

    private TextBox nameTextBox = new TextBox();
    private TextBox descriptionTextBox = new TextBox();
    private TextBox budgetTextBox = new TextBox();
    private DateTimePicker startDatePicker = new DateTimePicker();
    private DateTimePicker deadlineDatePicker = new DateTimePicker();
    private ComboBox statusComboBox = new ComboBox();
    private ComboBox responsibleEmployeeComboBox = new ComboBox();
    private ComboBox clientComboBox = new ComboBox();
    private Button saveButton = new Button();

    public ProjectForm(ProjectController projectController, Project? project = null)
    {
        _projectController = projectController;
        _project = project;

        InitializeComponent();

        LoadDropdowns();

        if (_project != null)
        {
            PopulateFields();
        }
    }

    private void InitializeComponent()
    {
        this.Text = _project == null ? "Add Project" : "Edit Project";
        this.Size = new Size(500, 600);

        Controls.Add(new Label { Text = "Name:", Location = new Point(20, 20), AutoSize = true });
        nameTextBox.Location = new Point(120, 20);
        Controls.Add(nameTextBox);

        Controls.Add(new Label { Text = "Description:", Location = new Point(20, 60), AutoSize = true });
        descriptionTextBox.Location = new Point(120, 60);
        Controls.Add(descriptionTextBox);

        Controls.Add(new Label { Text = "Budget:", Location = new Point(20, 100), AutoSize = true });
        budgetTextBox.Location = new Point(120, 100);
        Controls.Add(budgetTextBox);

        Controls.Add(new Label { Text = "Start Date:", Location = new Point(20, 140), AutoSize = true });
        startDatePicker.Location = new Point(120, 140);
        Controls.Add(startDatePicker);

        Controls.Add(new Label { Text = "Deadline:", Location = new Point(20, 180), AutoSize = true });
        deadlineDatePicker.Location = new Point(120, 180);
        Controls.Add(deadlineDatePicker);

        Controls.Add(new Label { Text = "Status:", Location = new Point(20, 220), AutoSize = true });
        statusComboBox.Location = new Point(120, 220);
        statusComboBox.Items.AddRange(new string[] { "In Progress", "Completed", "Cancelled" });
        Controls.Add(statusComboBox);

        Controls.Add(new Label { Text = "Responsible Employee:", Location = new Point(20, 260), AutoSize = true });
        responsibleEmployeeComboBox.Location = new Point(120, 260);
        Controls.Add(responsibleEmployeeComboBox);

        Controls.Add(new Label { Text = "Client:", Location = new Point(20, 300), AutoSize = true });
        clientComboBox.Location = new Point(120, 300);
        Controls.Add(clientComboBox);

        saveButton.Text = "Save";
        saveButton.Location = new Point(120, 340);
        saveButton.Click += SaveButton_Click;
        Controls.Add(saveButton);
    }

 private void LoadDropdowns()
{
    // Instantiate DataContext without passing DbContextOptions
    using (var ctx = new DataContext())
    {
        var employeeService = new EmployeeService(ctx);
        responsibleEmployeeComboBox.DataSource = employeeService.GetAllEmployees();
        responsibleEmployeeComboBox.DisplayMember = "LastName";
        responsibleEmployeeComboBox.ValueMember = "ID";

        var clientService = new ClientService(ctx);
        clientComboBox.DataSource = clientService.GetAllClients();
        clientComboBox.DisplayMember = "Name";
        clientComboBox.ValueMember = "ID";
    }
}


    private void PopulateFields()
    {
        if (_project == null) return;
        nameTextBox.Text = _project.Name;
        descriptionTextBox.Text = _project.Description;
        budgetTextBox.Text = _project.Budget.ToString("F2");
        startDatePicker.Value = _project.StartDate;
        deadlineDatePicker.Value = _project.Deadline;
        statusComboBox.SelectedItem = _project.Status;

        if (_project.ResponsibleEmployeeID.HasValue)
            responsibleEmployeeComboBox.SelectedValue = _project.ResponsibleEmployeeID.Value;
        if (_project.AssociatedClientID.HasValue)
            clientComboBox.SelectedValue = _project.AssociatedClientID.Value;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        decimal parsedBudget;
        if (!decimal.TryParse(budgetTextBox.Text, out parsedBudget))
        {
            MessageBox.Show("Invalid budget value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        int? responsibleEmployeeId = responsibleEmployeeComboBox.SelectedValue as int?;
        int? clientId = clientComboBox.SelectedValue as int?;

        if (_project == null)
        {
            var newProject = new Project
            {
                Name = nameTextBox.Text,
                Description = descriptionTextBox.Text,
                Budget = parsedBudget,
                StartDate = startDatePicker.Value,
                Deadline = deadlineDatePicker.Value,
                Status = statusComboBox.Text,
                ResponsibleEmployeeID = responsibleEmployeeId,
                AssociatedClientID = clientId
            };

            _projectController.CreateProject(newProject);
        }
        else
        {
            _project.Name = nameTextBox.Text;
            _project.Description = descriptionTextBox.Text;
            _project.Budget = parsedBudget;
            _project.StartDate = startDatePicker.Value;
            _project.Deadline = deadlineDatePicker.Value;
            _project.Status = statusComboBox.Text;
            _project.ResponsibleEmployeeID = responsibleEmployeeId;
            _project.AssociatedClientID = clientId;

            _projectController.EditProject(_project);
        }

        MessageBox.Show("Project saved successfully.");
        Close();
    }
}
