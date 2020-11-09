using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.AppComponents.Shared;
using NPCE_Client.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public class ServizioEditBase : ComponentBase
    {

        public Servizio Servizio { get; set; }

        public IEnumerable<Ambiente> Ambienti { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IServiziDataService ServiziDataService { get; set; }

        [Inject]
        public IAmbientiDataService AmbientiDataService { get; set; }

        [Inject]
        public IAnagraficheDataService AnagraficheDataService { get; set; }



        [Parameter]
        public string ServizioId { get; set; }

        protected AnagraficheSelector anagraficheSelectorDialog;


        private string _ambienteIdString;

        public string AmbienteIdString
        {
            get { return _ambienteIdString; }
            set
            {
                _ambienteIdString = value;
                Servizio.AmbienteId = int.Parse(_ambienteIdString);
            }
        }


        protected async Task HandleValidSubmit()
        {

            if (string.IsNullOrEmpty(ServizioId))
            {
                AddAnagrafiche();
                Servizio.TipoServizioId = (int)Servizio.TipoServizioEnum;
                await ServiziDataService.AddServizioAsync(Servizio);
            }
            else
            {
                await ServiziDataService.EditServizioAsync(Servizio);
            }
        }

        private void AddAnagrafiche()
        {
            var anagrafiche = anagraficheSelectorDialog.Anagrafiche;
            Servizio.ServizioAnagrafiche = new System.Collections.ObjectModel.Collection<ServizioAnagrafica>();
            foreach (var anagrafica in anagrafiche)
            {
                Servizio.ServizioAnagrafiche.Add(
                    new ServizioAnagrafica
                    {
                        AnagraficaId = anagrafica.Anagrafica.Id,
                        ServizioId = Servizio.Id,
                        IsMittente = anagrafica.IsMittente
                    });
            }
        }

        public bool Loaded { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Loaded = false;

            Ambienti = await AmbientiDataService.GetAllAmbienti();

            Saved = false;

            if (string.IsNullOrEmpty(ServizioId)) //new servizio is being created
            {
                // add some defaults
                Servizio = new Servizio { DataCreazione = System.DateTime.Now };
            }
            else
            {
                // TODO: Get servizio by id
                Servizio = await ServiziDataService.GetServizioDetailAsync(int.Parse(ServizioId));
            }

            Loaded = true;

        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/servizi");
        }
        protected Task DeleteServizio()
        {
            throw new NotImplementedException();
        }

        public bool ShowAnagraficheSelector { get; set; }


    }
}
