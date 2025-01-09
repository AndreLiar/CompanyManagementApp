using System;
using System.Drawing;
using System.Windows.Forms;

public class ProjectManagementForm : Form
{
    private readonly ProjectController _projectController;
    private readonly string _role; // "Admin" or "Standard"

    private DataGridView projectsDataGridView = new DataGridView();
    private Button addButton = new Button();
    private Button editButton = new Button();
    private Button deleteButton = new Button();
    private Button backButton = new Button();

    public event Action? BackToDashboardRequested;

    public ProjectManagementForm(ProjectController projectController, string role)
    {
        _projectController = projectController;
        _role = role;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // Form Styles
        this.Text = "Gestion des Projets";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;

        // DataGridView Styles
        projectsDataGridView.Dock = DockStyle.Top;
        projectsDataGridView.Height = 400;
        projectsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        projectsDataGridView.ReadOnly = true;
        projectsDataGridView.AllowUserToAddRows = false;
        projectsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        projectsDataGridView.MultiSelect = false;
        projectsDataGridView.Font = new Font("Segoe UI", 10);
        projectsDataGridView.BorderStyle = BorderStyle.Fixed3D;
        projectsDataGridView.GridColor = Color.LightGray;
        projectsDataGridView.DefaultCellStyle.BackColor = Color.White;
        projectsDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        projectsDataGridView.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
        projectsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

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
        StyleButton(addButton, "Ajouter Projet", _role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
        addButton.Click += (s, e) => ShowProjectForm(null);

        // Edit Button
        StyleButton(editButton, "Modifier Projet", _role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
        editButton.Click += (s, e) => EditSelectedProject();

        // Delete Button
        StyleButton(deleteButton, "Supprimer Projet", _role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
        deleteButton.Click += (s, e) => DeleteSelectedProject();

        // Back Button
        StyleButton(backButton, "Retour au Tableau de Bord", true);
        backButton.Click += (s, e) => BackToDashboardRequested?.Invoke();

        // Add buttons to panel
        buttonPanel.Controls.Add(addButton);
        buttonPanel.Controls.Add(editButton);
        buttonPanel.Controls.Add(deleteButton);
        buttonPanel.Controls.Add(backButton);

        // Add controls to form
        Controls.Add(projectsDataGridView);
        Controls.Add(buttonPanel);

        // Load projects into the grid
        this.Load += (s, e) => LoadProjects();
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

    public void LoadProjects()
    {
        try
        {
            var projects = _projectController.GetAllProjects();
            projectsDataGridView.DataSource = null; // Clear previous data
            projectsDataGridView.DataSource = projects;

            // Hide unnecessary columns
            if (projectsDataGridView.Columns["ID"] != null)
            {
                projectsDataGridView.Columns["ID"].Visible = false;
            }

            // Adjust column headers
            if (projectsDataGridView.Columns["ResponsibleEmployee"] != null)
                projectsDataGridView.Columns["ResponsibleEmployee"].HeaderText = "Responsable";

            if (projectsDataGridView.Columns["AssociatedClient"] != null)
                projectsDataGridView.Columns["AssociatedClient"].HeaderText = "Client";

            projectsDataGridView.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du chargement des projets: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowProjectForm(Project? project)
    {
        var projectForm = new ProjectForm(_projectController, project);
        projectForm.FormClosed += (s, e) => LoadProjects(); // Refresh after closing
        projectForm.ShowDialog();
    }

    private void EditSelectedProject()
    {
        if (projectsDataGridView.SelectedRows.Count > 0)
        {
            var project = projectsDataGridView.SelectedRows[0].DataBoundItem as Project;
            if (project != null)
            {
                var fullProject = _projectController.GetProjectById(project.ID);
                ShowProjectForm(fullProject);
            }
            else
            {
                MessageBox.Show("Projet sélectionné invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner un projet à modifier.", "Aucune Sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void DeleteSelectedProject()
    {
        if (projectsDataGridView.SelectedRows.Count > 0)
        {
            var project = projectsDataGridView.SelectedRows[0].DataBoundItem as Project;
            if (project != null)
            {
                var confirmResult = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le projet '{project.Name}'?",
                                                    "Confirmer la Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    bool isDeleted = false;
                    try
                    {
                        isDeleted = _projectController.DeleteProject(project.ID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression du projet: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (isDeleted)
                    {
                        MessageBox.Show("Projet supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProjects();
                    }
                    else
                    {
                        MessageBox.Show("Échec de la suppression du projet. Projet introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Projet sélectionné invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner un projet à supprimer.", "Aucune Sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
