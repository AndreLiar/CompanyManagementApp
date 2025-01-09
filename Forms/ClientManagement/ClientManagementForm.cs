using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;

public class ClientManagementForm : Form
{
    private readonly ClientController _clientController;
    private readonly string _role;

    private DataGridView clientsDataGridView = new DataGridView();
    private Button addButton = new Button();
    private Button editButton = new Button();
    private Button deleteButton = new Button();
    private Button exportPdfButton = new Button();
    private Button exportExcelButton = new Button();
    private Button backButton = new Button();

    public event Action? BackToDashboardRequested;

    public ClientManagementForm(ClientController clientController, string role)
    {
        _clientController = clientController;
        _role = role;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Client Management";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;

        clientsDataGridView.Dock = DockStyle.Top;
        clientsDataGridView.Height = 400;
        clientsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        clientsDataGridView.ReadOnly = true;
        clientsDataGridView.AllowUserToAddRows = false;
        clientsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        clientsDataGridView.MultiSelect = false;

        // Configure Buttons
        ConfigureButton(addButton, "Add Client", Color.Blue, true, async (s, e) => await ShowClientFormAsync(null));
        ConfigureButton(editButton, "Edit Client", Color.Blue, true, async (s, e) => await EditSelectedClientAsync());
        ConfigureButton(deleteButton, "Delete Client", Color.Red, true, async (s, e) => await DeleteSelectedClientAsync());
        ConfigureButton(exportPdfButton, "Export to PDF", Color.Green, _role.Equals("Admin", StringComparison.OrdinalIgnoreCase), (s, e) => ExportToPdf());
        ConfigureButton(exportExcelButton, "Export to Excel", Color.Green, _role.Equals("Admin", StringComparison.OrdinalIgnoreCase), (s, e) => ExportToExcel());
        ConfigureButton(backButton, "Back to Dashboard", Color.Gray, true, (s, e) => BackToDashboardRequested?.Invoke());

        // Add controls to the form
        Controls.Add(clientsDataGridView);
        Controls.Add(addButton);
        Controls.Add(editButton);
        Controls.Add(deleteButton);
        Controls.Add(exportPdfButton);
        Controls.Add(exportExcelButton);
        Controls.Add(backButton);

        this.Load += async (s, e) => await LoadClientsAsync();
    }

    private void ConfigureButton(Button button, string text, Color color, bool enabled, EventHandler clickEvent)
    {
        button.Text = text;
        button.Dock = DockStyle.Top;
        button.Height = 40;
        button.Font = new Font("Segoe UI", 10);
        button.BackColor = color;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Enabled = enabled;
        button.Click += clickEvent;
    }

    public async Task LoadClientsAsync()
    {
        try
        {
            var clients = await _clientController.GetClientsAsync();
            clientsDataGridView.DataSource = null;
            clientsDataGridView.DataSource = clients;

            if (clientsDataGridView.Columns["ID"] != null)
            {
                clientsDataGridView.Columns["ID"].Visible = false;
            }

            clientsDataGridView.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Task ShowClientFormAsync(Client? client)
    {
        var clientForm = new ClientForm(_clientController, client);
        clientForm.FormClosed += async (s, e) => await LoadClientsAsync();
        clientForm.ShowDialog();
        return Task.CompletedTask;
    }

    private async Task EditSelectedClientAsync()
    {
        if (clientsDataGridView.SelectedRows.Count > 0)
        {
            var client = clientsDataGridView.SelectedRows[0].DataBoundItem as Client;
            if (client != null)
            {
                await ShowClientFormAsync(client);
            }
            else
            {
                MessageBox.Show("Selected client is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select a client to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async Task DeleteSelectedClientAsync()
    {
        if (clientsDataGridView.SelectedRows.Count > 0)
        {
            var client = clientsDataGridView.SelectedRows[0].DataBoundItem as Client;
            if (client != null)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete '{client.Name}'?",
                                                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    bool isDeleted = false;
                    try
                    {
                        isDeleted = await _clientController.DeleteClientAsync(client.ID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (isDeleted)
                    {
                        MessageBox.Show("Client deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadClientsAsync();
                    }
                    else
                    {
                        MessageBox.Show("Client deletion failed. Client not found.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selected client is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select a client to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ExportToPdf()
    {
        using var saveFileDialog = new SaveFileDialog { Filter = "PDF files (*.pdf)|*.pdf", FileName = "Clients.pdf" };
        if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

        using var writer = new StreamWriter(saveFileDialog.FileName);
        writer.WriteLine("Client Data Export");
        writer.WriteLine(new string('-', 50));

        foreach (DataGridViewRow row in clientsDataGridView.Rows)
        {
            var line = string.Join(", ", row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString()));
            writer.WriteLine(line);
        }

        MessageBox.Show("PDF export complete.");
    }

    private void ExportToExcel()
    {
        using var saveFileDialog = new SaveFileDialog { Filter = "Excel files (*.csv)|*.csv", FileName = "Clients.csv" };
        if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

        using var writer = new StreamWriter(saveFileDialog.FileName);
        var headers = string.Join(",", clientsDataGridView.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText));
        writer.WriteLine(headers);

        foreach (DataGridViewRow row in clientsDataGridView.Rows)
        {
            var line = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString()));
            writer.WriteLine(line);
        }

        MessageBox.Show("Excel export complete.");
    }
}
