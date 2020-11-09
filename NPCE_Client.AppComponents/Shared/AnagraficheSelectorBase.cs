using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Shared
{
    public class AnagraficheSelectorBase : ComponentBase
    {
        [Parameter]
        public Servizio Servizio { get; set; }

        [Inject]
        public IAnagraficheDataService AnagraficheDataService { get; set; }
        public IEnumerable<AnagraficheSelectorViewModel> Anagrafiche { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Anagrafiche = await AnagraficheDataService.AnagraficheSelectorGetByServizio(Servizio.Id);
        }

        public bool SaveButtonDisabled { get; set; }

        public void SaveClick()
        {
            AnagraficheDataService.UpdateAnagraficheServizioAsync(Servizio.Id, Anagrafiche);
        }
    }

}
