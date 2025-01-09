using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog; // If you don't use Serilog, remove this

public class ClientService
{
    private readonly DataContext _context;

    public ClientService(DataContext context)
    {
        _context = context;
    }

    // Add a synchronous method to get all clients
    public List<Client> GetAllClients()
    {
        return _context.Clients.ToList();
    }


    // Asynchronous method to get all clients
    public async Task<List<Client>> GetAllClientsAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    // Add a new client (synchronous for simplicity, could be made async if needed)
    public void AddClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null.");

        _context.Clients.Add(client);
        try
        {
            _context.SaveChanges();
            Log.Information("Added new client with ID {ClientID}", client.ID);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding new client.");
            throw;
        }
    }

    // Update an existing client asynchronously
    public async Task<bool> UpdateClientAsync(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null.");

        var existingClient = await _context.Clients.FindAsync(client.ID);
        if (existingClient != null)
        {
            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.Address = client.Address;
            existingClient.Industry = client.Industry;
            existingClient.CollaborationHistory = client.CollaborationHistory;
            existingClient.ClientType = client.ClientType;
            existingClient.PrimaryContact = client.PrimaryContact;

            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Updated client with ID {ClientID}", client.ID);
                return true; // Update successful
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Database update error while updating client with ID {ClientID}.", client.ID);
                return false;
            }
        }
        else
        {
            Log.Warning("Client with ID {ClientID} not found. Update failed.", client.ID);
            return false;
        }
    }

    // Delete a client asynchronously
    public async Task<bool> DeleteClientAsync(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);
        if (client != null)
        {
            _context.Clients.Remove(client);
            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Deleted client with ID {ClientID}", clientId);
                return true; // Deletion successful
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting client with ID {ClientID}.", clientId);
                return false;
            }
        }
        else
        {
            Log.Warning("Attempted to delete non-existent client with ID {ClientID}.", clientId);
            return false; // Client not found
        }
    }
}
