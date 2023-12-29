using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Blazor.Web.Components.Pages
{
    public partial class AddAspNetUser
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public BlazorWebService BlazorWebService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            aspNetUser = new Blazor.Web.Models.BlazorWeb.AspNetUser();
        }
        protected bool errorVisible;
        protected Blazor.Web.Models.BlazorWeb.AspNetUser aspNetUser;

        protected async Task FormSubmit()
        {
            try
            {
                await BlazorWebService.CreateAspNetUser(aspNetUser);
                DialogService.Close(aspNetUser);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}