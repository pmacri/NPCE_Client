using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class AmbienteEdit : ComponentBase
    {

        [Inject]
        public IAmbientiDataService AmbientiDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string AmbienteId { get; set; }

        public Ambiente Ambiente { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            int ambienteId;

            int.TryParse(AmbienteId, out ambienteId);

            if (ambienteId == 0) //new ambiente is being created
            {
                //add some defaults
                Ambiente = new Ambiente();
            }
            else
            {
                Ambiente = await AmbientiDataService.GetAmbienteDetail(ambienteId);
            }

        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/ambienti");
        }
        protected async Task DeleteAmbiente()
        {
            await AmbientiDataService.DeleteAmbiente(Ambiente.Id);

            StatusClass = "alert-success";
            Message = "Ambiente eliminato";

            Saved = true;
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                if (Ambiente.Id == 0)
                // Add logic
                {
                    var addedAmbiente = await AmbientiDataService.AddAmbiente(Ambiente);

                    if (addedAmbiente != null)
                    {
                        StatusClass = "alert-success";
                        Message = "Nuovo ambiente creata con successo.";
                        Saved = true;
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = "Si è verificato un errore nella creazione delnuovo ambiente. Riprovare.";
                        Saved = false;
                    }
                }
                else
                // Edit logic
                {
                    await AmbientiDataService.EditAmbiente(Ambiente);
                    NavigateToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }


    }
}
