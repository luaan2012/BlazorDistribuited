using Blazor.Api.Client.Data;
using Blazor.Api.Client.InputModels;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Api.Client.Repository
{
    public interface IClientRepository
    {
        Task<bool> Add(Models.Client client);
        Task<bool> Delete(Guid id);
        Task<bool> Update(Guid id, RequestClient client);
        Task<IEnumerable<Models.Client>> ListAll();
        Task<Models.Client> ListId(Guid id);
        Task<bool> Inactive(Guid id);
        Task<bool> Active(Guid id);
    }

    public class ClientRepository (ApplicationContext context) : IClientRepository
    {
        private readonly ApplicationContext _context = context;
        
        public async Task<bool> Add(Models.Client client)
        {
            client.DateCreated = DateTime.Now;

            await _context.Client.AddAsync(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if(client is null)
            {
                return false;
            }

            _context.Client.Remove(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Inactive(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if (client is null)
            {
                return false;
            }

            client.DateDelete = DateTime.Now;
            client.Active = false;

            _context.Client.Update(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<bool> Active(Guid id)
        {
            var client = _context.Client.FirstOrDefault(x => x.Id == id);

            if (client is null)
            {
                return false;
            }

            client.DateDelete = null;
            client.Active = true;

            _context.Client.Update(client);

            return (await _context.SaveChangesAsync()) == 1;
        }

        public async Task<IEnumerable<Models.Client>> ListAll()
        {
            return await _context.Client.ToListAsync();
        }

        public async Task<Models.Client> ListId(Guid id)
        {
            return _context.Client.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> Update(Guid id, RequestClient client)
        {
            var findClient = _context.Client.FirstOrDefault(x => x.Id == id);

            if (findClient is null)
            {
                return false;
            }

            findClient.Number = client.Number;
            findClient.Name = client.Name;
            findClient.LastName = client.LastName;
            findClient.Address = client.Address;
            findClient.DateModificated = DateTime.Now;

            _context.Client.Update(findClient);

            return (await _context.SaveChangesAsync()) == 1;
        }
    }
}
