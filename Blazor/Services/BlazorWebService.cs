using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Blazor.Web.Data;

namespace Blazor.Web
{
    public partial class BlazorWebService
    {
        BlazorWebContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly BlazorWebContext context;
        private readonly NavigationManager navigationManager;

        public BlazorWebService(BlazorWebContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportClientsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazorweb/clients/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazorweb/clients/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClientsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazorweb/clients/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazorweb/clients/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClientsRead(ref IQueryable<Blazor.Web.Models.BlazorWeb.Client> items);

        public async Task<IQueryable<Blazor.Web.Models.BlazorWeb.Client>> GetClients(Query query = null)
        {
            var items = Context.Clients.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnClientsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClientGet(Blazor.Web.Models.BlazorWeb.Client item);
        partial void OnGetClientById(ref IQueryable<Blazor.Web.Models.BlazorWeb.Client> items);


        public async Task<Blazor.Web.Models.BlazorWeb.Client> GetClientById(Guid id)
        {
            var items = Context.Clients
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetClientById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnClientGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClientCreated(Blazor.Web.Models.BlazorWeb.Client item);
        partial void OnAfterClientCreated(Blazor.Web.Models.BlazorWeb.Client item);

        public async Task<Blazor.Web.Models.BlazorWeb.Client> CreateClient(Blazor.Web.Models.BlazorWeb.Client client)
        {
            OnClientCreated(client);

            var existingItem = Context.Clients
                              .Where(i => i.Id == client.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Clients.Add(client);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(client).State = EntityState.Detached;
                throw;
            }

            OnAfterClientCreated(client);

            return client;
        }

        public async Task<Blazor.Web.Models.BlazorWeb.Client> CancelClientChanges(Blazor.Web.Models.BlazorWeb.Client item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClientUpdated(Blazor.Web.Models.BlazorWeb.Client item);
        partial void OnAfterClientUpdated(Blazor.Web.Models.BlazorWeb.Client item);

        public async Task<Blazor.Web.Models.BlazorWeb.Client> UpdateClient(Guid id, Blazor.Web.Models.BlazorWeb.Client client)
        {
            OnClientUpdated(client);

            var itemToUpdate = Context.Clients
                              .Where(i => i.Id == client.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(client).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClientUpdated(client);

            return client;
        }

        partial void OnClientDeleted(Blazor.Web.Models.BlazorWeb.Client item);
        partial void OnAfterClientDeleted(Blazor.Web.Models.BlazorWeb.Client item);

        public async Task<Blazor.Web.Models.BlazorWeb.Client> DeleteClient(Guid id)
        {
            var itemToDelete = Context.Clients
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClientDeleted(itemToDelete);

            Reset();

            Context.Clients.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClientDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazorweb/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazorweb/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/blazorweb/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/blazorweb/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUsersRead(ref IQueryable<Blazor.Web.Models.BlazorWeb.AspNetUser> items);

        public async Task<IQueryable<Blazor.Web.Models.BlazorWeb.AspNetUser>> GetAspNetUsers(Query query = null)
        {
            var items = Context.AspNetUsers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserGet(Blazor.Web.Models.BlazorWeb.AspNetUser item);
        partial void OnGetAspNetUserById(ref IQueryable<Blazor.Web.Models.BlazorWeb.AspNetUser> items);


        public async Task<Blazor.Web.Models.BlazorWeb.AspNetUser> GetAspNetUserById(string id)
        {
            var items = Context.AspNetUsers
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAspNetUserById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserCreated(Blazor.Web.Models.BlazorWeb.AspNetUser item);
        partial void OnAfterAspNetUserCreated(Blazor.Web.Models.BlazorWeb.AspNetUser item);

        public async Task<Blazor.Web.Models.BlazorWeb.AspNetUser> CreateAspNetUser(Blazor.Web.Models.BlazorWeb.AspNetUser aspnetuser)
        {
            OnAspNetUserCreated(aspnetuser);

            var existingItem = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUsers.Add(aspnetuser);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuser).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserCreated(aspnetuser);

            return aspnetuser;
        }

        public async Task<Blazor.Web.Models.BlazorWeb.AspNetUser> CancelAspNetUserChanges(Blazor.Web.Models.BlazorWeb.AspNetUser item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserUpdated(Blazor.Web.Models.BlazorWeb.AspNetUser item);
        partial void OnAfterAspNetUserUpdated(Blazor.Web.Models.BlazorWeb.AspNetUser item);

        public async Task<Blazor.Web.Models.BlazorWeb.AspNetUser> UpdateAspNetUser(string id, Blazor.Web.Models.BlazorWeb.AspNetUser aspnetuser)
        {
            OnAspNetUserUpdated(aspnetuser);

            var itemToUpdate = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(aspnetuser).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserUpdated(aspnetuser);

            return aspnetuser;
        }

        partial void OnAspNetUserDeleted(Blazor.Web.Models.BlazorWeb.AspNetUser item);
        partial void OnAfterAspNetUserDeleted(Blazor.Web.Models.BlazorWeb.AspNetUser item);

        public async Task<Blazor.Web.Models.BlazorWeb.AspNetUser> DeleteAspNetUser(string id)
        {
            var itemToDelete = Context.AspNetUsers
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserDeleted(itemToDelete);

            Reset();

            Context.AspNetUsers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}