using Blazor.Api.Client.InputModels;
using Blazor.Api.Client.OutputModels;
using Blazor.Api.Client.Repository;
using Blazor.Api.Core.OutputModels;
using MassTransit;

namespace Blazor.Api.Client.Services
{
    public interface IClientService
    {
        Task<Result<IEnumerable<Models.Client>, string>> ListAll();
        Task<Result<Models.Client, string>> ListById(Guid id);
        Task<bool> Add(RequestClient requestClient);
        Task<bool> Delete(Guid guid);
        Task<bool> Edit(Guid guid, RequestClient requestClient);
        Task<bool> Inactive(Guid guid);
        Task<bool> Active(Guid guid);
    }

    public class ClientService(IClientRepository clientRepository) : IConsumer<ClientCommander>, IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task Consume(ConsumeContext<ClientCommander> context)
        {
            await _clientRepository.Add(ClientCommanderToClient(context.Message));
        }

        public async Task<Result<IEnumerable<Models.Client>, string>> ListAll()
        {
            var clients = await _clientRepository.ListAll();

            if(clients is not null && clients.Count() > 0)
            {
                return clients.ToArray();
            }

            return "Nenhum cliente encontrado";
        }

        public async Task<Result<Models.Client, string>> ListById(Guid id)
        {
            var clients = await _clientRepository.ListId(id);

            if (clients is not null)
            {
                return clients;
            }

            return "Nenhum cliente encontrado";
        }

        public async Task<bool> Add(RequestClient requestClient)
        {
            return await _clientRepository.Add(RequestClientToClient(requestClient));
        }

        public async Task<bool> Delete(Guid guid)
        {
            return await _clientRepository.Delete(guid);
        }

        public async Task<bool> Edit(Guid guid, RequestClient client)
        {
            return await _clientRepository.Update(guid, client);
        }

        public async Task<bool> Inactive(Guid guid)
        {
            return await _clientRepository.Inactive(guid);
        }

        public async Task<bool> Active(Guid guid)
        {
            return await _clientRepository.Active(guid);
        }

        private Models.Client ClientCommanderToClient(ClientCommander clientCommander)
        {
            return new Models.Client
            {
                Name = clientCommander.Name,
                LastName = clientCommander.LastName,
                Address = clientCommander.Address,
                Number = clientCommander.Number
            };
        }

        private Models.Client RequestClientToClient(RequestClient requestClient)
        {
            return new Models.Client
            {
                Name = requestClient.Name,
                LastName = requestClient.LastName,
                Address = requestClient.Address,
                Number = requestClient.Number
            };
        }
    }
}
