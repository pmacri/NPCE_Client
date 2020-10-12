using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{

    //This page can be in two modes : Edit, Add
    public partial class AnagraficaEdit :ComponentBase
    {

        [Inject]
        public IAnagraficheDataService AnagraficheDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        // Blazor inject the parameter anagraficaId when this component is invoked (in edit mode; in add mode will be 0 )
        [Parameter]
        public string AnagraficaId { get; set; }

        public Anagrafica Anagrafica { get; set; } 
        
        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;


        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            int anagraficaId;

            int.TryParse(AnagraficaId, out anagraficaId);

            if (anagraficaId == 0) //new employee is being created
            {
                //add some defaults
                Anagrafica = new Anagrafica();
            }
            else
            {
                Anagrafica = await AnagraficheDataService.GetAnagraficaDetail(anagraficaId);
            }

        }

        protected async Task HandleValidSubmit()
        {
            if (Anagrafica.Id==0)
            // Add logic
            {
                var addedEmployee = await AnagraficheDataService.AddAnagrafica(Anagrafica);

                if (addedEmployee != null)
                {
                    StatusClass = "alert-success";
                    Message = "Nuova anagrafica creata con successo.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Si è verificato un errore nella creazione della nuova anagrafica. Riprovare.";
                    Saved = false;
                }
            }
            else
            // Edit logic
            {
                await AnagraficheDataService.EditAnagrafica(Anagrafica);
            }
        }

        protected async Task DeleteAnagrafica()
        {
            await AnagraficheDataService.DeleteAnagrafica(Anagrafica.Id);

            StatusClass = "alert-success";
            Message = "Anagrafica eliminata";

            Saved = true;
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/anagrafiche");
        }
    }
}
