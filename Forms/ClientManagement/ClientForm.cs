using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ClientForm : Form
{
    private readonly ClientController _clientController;
    private readonly Client? _client;

    private TextBox nameTextBox = new TextBox();
    private TextBox emailTextBox = new TextBox();
    private TextBox phoneNumberTextBox = new TextBox();
    private TextBox addressTextBox = new TextBox();
    private TextBox industryTextBox = new TextBox();
    private TextBox collaborationHistoryTextBox = new TextBox();
    private ComboBox clientTypeComboBox = new ComboBox { Items = { "prospect", "actif", "inactif" } };
    private TextBox primaryContactTextBox = new TextBox();
    private Button saveButton = new Button { Text = "Save" };
//constructor
    public ClientForm(ClientController clientController, Client? client = null)
    {
        _clientController = clientController;
        _client = client;

        InitializeComponent();
        if (client != null) PopulateFields();
    }
//initialize the components of the form
   private void InitializeComponent()
{
    this.Size = new Size(500, 600);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.BackColor = Color.White;
    this.Text = _client == null ? "Add Client" : "Edit Client";

    var labelFont = new Font("Segoe UI", 10);
    var inputFont = new Font("Segoe UI", 10);

    var nameLabel = new Label
    {
        Text = "Client Name*",
        Location = new Point(30, 30),
        AutoSize = true,
        Font = labelFont
    };
    nameTextBox.Location = new Point(200, 30);
    nameTextBox.Width = 250;
    nameTextBox.Font = inputFont;

    var emailLabel = new Label
    {
        Text = "Email*",
        Location = new Point(30, 80),
        AutoSize = true,
        Font = labelFont
    };
    emailTextBox.Location = new Point(200, 80);
    emailTextBox.Width = 250;
    emailTextBox.Font = inputFont;

    var phoneLabel = new Label
    {
        Text = "Phone Number",
        Location = new Point(30, 130),
        AutoSize = true,
        Font = labelFont
    };
    phoneNumberTextBox.Location = new Point(200, 130);
    phoneNumberTextBox.Width = 250;
    phoneNumberTextBox.Font = inputFont;

    var addressLabel = new Label
    {
        Text = "Address",
        Location = new Point(30, 180),
        AutoSize = true,
        Font = labelFont
    };
    addressTextBox.Location = new Point(200, 180);
    addressTextBox.Width = 250;
    addressTextBox.Font = inputFont;

    var industryLabel = new Label
    {
        Text = "Industry",
        Location = new Point(30, 230),
        AutoSize = true,
        Font = labelFont
    };
    industryTextBox.Location = new Point(200, 230);
    industryTextBox.Width = 250;
    industryTextBox.Font = inputFont;

    var historyLabel = new Label
    {
        Text = "Collaboration History",
        Location = new Point(30, 280),
        AutoSize = true,
        Font = labelFont
    };
    collaborationHistoryTextBox.Location = new Point(200, 280);
    collaborationHistoryTextBox.Width = 250;
    collaborationHistoryTextBox.Height = 50;
    collaborationHistoryTextBox.Multiline = true;
    collaborationHistoryTextBox.Font = inputFont;

    var clientTypeLabel = new Label
    {
        Text = "Client Type",
        Location = new Point(30, 350),
        AutoSize = true,
        Font = labelFont
    };
    clientTypeComboBox.Location = new Point(200, 350);
    clientTypeComboBox.Width = 250;
    clientTypeComboBox.Font = inputFont;
    clientTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    clientTypeComboBox.SelectedIndex = 0;

    var primaryContactLabel = new Label
    {
        Text = "Primary Contact",
        Location = new Point(30, 400),
        AutoSize = true,
        Font = labelFont
    };
    primaryContactTextBox.Location = new Point(200, 400);
    primaryContactTextBox.Width = 250;
    primaryContactTextBox.Font = inputFont;

    saveButton.Location = new Point(200, 470);
    saveButton.Width = 250;
    saveButton.Height = 40;
    saveButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
    saveButton.BackColor = Color.Blue;
    saveButton.ForeColor = Color.White;
    saveButton.FlatStyle = FlatStyle.Flat;
    saveButton.FlatAppearance.BorderSize = 0;
    saveButton.Click += async (s, e) => await SaveButton_ClickAsync(s, e);

    Controls.AddRange(new Control[]
    {
        nameLabel, nameTextBox,
        emailLabel, emailTextBox,
        phoneLabel, phoneNumberTextBox,
        addressLabel, addressTextBox,
        industryLabel, industryTextBox,
        historyLabel, collaborationHistoryTextBox,
        clientTypeLabel, clientTypeComboBox,
        primaryContactLabel, primaryContactTextBox,
        saveButton
    });
}

//populate the fields of the form with the client data
    private void PopulateFields()
    {
        if (_client != null)
        {
            nameTextBox.Text = _client.Name;
            emailTextBox.Text = _client.Email;
            phoneNumberTextBox.Text = _client.PhoneNumber;
            addressTextBox.Text = _client.Address;
            industryTextBox.Text = _client.Industry;
            collaborationHistoryTextBox.Text = _client.CollaborationHistory;
            clientTypeComboBox.SelectedItem = _client.ClientType;
            primaryContactTextBox.Text = _client.PrimaryContact;
        }
    }
//save the client data to the database
    private async Task SaveButton_ClickAsync(object? sender, EventArgs e)
    {
        // Validate mandatory fields
        if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
            string.IsNullOrWhiteSpace(emailTextBox.Text))
        {
            MessageBox.Show("Please fill in all mandatory fields (Name and Email).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!IsValidEmail(emailTextBox.Text))
        {
            MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_client == null)
        {
            var newClient = new Client
            {
                Name = nameTextBox.Text.Trim(),
                Email = emailTextBox.Text.Trim(),
                PhoneNumber = phoneNumberTextBox.Text.Trim(),
                Address = addressTextBox.Text.Trim(),
                Industry = industryTextBox.Text.Trim(),
                CollaborationHistory = collaborationHistoryTextBox.Text.Trim(),
                ClientType = clientTypeComboBox.SelectedItem?.ToString() ?? "prospect",
                PrimaryContact = primaryContactTextBox.Text.Trim()
            };

            try
            {
                _clientController.AddClient(newClient);
                MessageBox.Show("Client added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            _client.Name = nameTextBox.Text.Trim();
            _client.Email = emailTextBox.Text.Trim();
            _client.PhoneNumber = phoneNumberTextBox.Text.Trim();
            _client.Address = addressTextBox.Text.Trim();
            _client.Industry = industryTextBox.Text.Trim();
            _client.CollaborationHistory = collaborationHistoryTextBox.Text.Trim();
            _client.ClientType = clientTypeComboBox.SelectedItem?.ToString() ?? "prospect";
            _client.PrimaryContact = primaryContactTextBox.Text.Trim();

            bool isUpdated = false;
            try
            {
                isUpdated = await _clientController.UpdateClientAsync(_client);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isUpdated)
            {
                MessageBox.Show("Client updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Client update failed. Client not found.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Helper method to validate email format
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
