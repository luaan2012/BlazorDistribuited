using Blazor.Api.Client.InputModels;
using Blazor.Api.Client.Services;

namespace Blazor.Api.Client.Endpoints
{
    public static class ClientEndpoint
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("ListAll", ListAll).RequireAuthorization();
            app.MapPost("ListById", ListById).RequireAuthorization();
            app.MapPost("Edit", Edit).RequireAuthorization();
            app.MapPost("Delete", Delete).RequireAuthorization();
            app.MapPost("Inactive", Inactive).RequireAuthorization();
            app.MapPost("Active", Inactive).RequireAuthorization();
        }

        public static async Task<IResult> ListAll(IClientService clientService)
        {
            var result = await clientService.ListAll();

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
            );
        }

        public static async Task<IResult> ListById(Guid id, IClientService clientService)
        {
            var result = await clientService.ListById(id);

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
            );
        }

        public static async Task<IResult> Delete(Guid guid, IClientService clientService)
        {
            var result = await clientService.Delete(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Edit(Guid guid, RequestClient request, IClientService clientService)
        {
            var result = await clientService.Edit(guid, request);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Inactive(Guid guid, IClientService clientService)
        {
            var result = await clientService.Inactive(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }

        public static async Task<IResult> Active(Guid guid, IClientService clientService)
        {
            var result = await clientService.Active(guid);

            return result ? Results.Ok(result) : Results.BadRequest(result);
        }
    }
}
