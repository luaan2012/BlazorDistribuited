using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Blazor.Web.Data;

namespace Blazor.Web.Controllers
{
    public partial class ExportBlazorWebController : ExportController
    {
        private readonly BlazorWebContext context;
        private readonly BlazorWebService service;

        public ExportBlazorWebController(BlazorWebContext context, BlazorWebService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/BlazorWeb/clients/csv")]
        [HttpGet("/export/BlazorWeb/clients/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClients(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BlazorWeb/clients/excel")]
        [HttpGet("/export/BlazorWeb/clients/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClientsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClients(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BlazorWeb/aspnetusers/csv")]
        [HttpGet("/export/BlazorWeb/aspnetusers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUsers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BlazorWeb/aspnetusers/excel")]
        [HttpGet("/export/BlazorWeb/aspnetusers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUsers(), Request.Query, false), fileName);
        }
    }
}
