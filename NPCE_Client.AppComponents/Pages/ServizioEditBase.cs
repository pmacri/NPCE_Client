using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public class ServizioEditBase: ComponentBase
    {

        public Servizio Servizio { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IServiziDataService ServiziDataService { get; set; }

        [Parameter]
        public string ServizioId { get; set; }

        protected  Task HandleValidSubmit()
        {

            throw new NotImplementedException();
        }

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            int servizioId;

            int.TryParse(ServizioId, out servizioId);

            if (servizioId == 0) //new servizio is being created
            {
                //add some defaults
                Servizio = new Servizio();
            }
            else
            {
                // TODO: Get servizio by id
                Servizio = await ServiziDataService.GetServizioDetail(servizioId);
            }

        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/servizi");
        }
        protected  Task DeleteServizio()
        {
            throw new NotImplementedException();
        }
    }
}
