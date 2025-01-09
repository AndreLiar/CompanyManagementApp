using System.Collections.Generic;
using System.Threading.Tasks;

public class ClientController
{
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }

    // Get all clients asynchronously
    public async Task<List<Client>> GetClientsAsync()
    {
        return await _clientService.GetAllClientsAsync();
    }

    // Add a new client (synchronous call)
    public void AddClient(Client client)
    {
        _clientService.AddClient(client);
    }

    // Update an existing client asynchronously
    public async Task<bool> UpdateClientAsync(Client client)
    {
        return await _clientService.UpdateClientAsync(client);
    }

    // Delete a client asynchronously
    public async Task<bool> DeleteClientAsync(int clientId)
    {
        return await _clientService.DeleteClientAsync(clientId);
    }
}
