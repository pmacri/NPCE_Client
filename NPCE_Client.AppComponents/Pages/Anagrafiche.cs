using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class Anagrafiche
    {
        public IEnumerable<Anagrafica> AnagraficheList { get; set; }

        [Inject]
        public IAnagraficheDataService AnagraficheDataService { get; set; }

        protected AddAnagraficaDialog AddAnagraficaDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AnagraficheList = (await AnagraficheDataService.GetAllAnagrafiche()).ToList();
        }

        protected void QuickAddAnagrafica()
        {
            AddAnagraficaDialog.Show();
        }

        public async Task AddAnagraficaDialog_OnDialogClose()
        {
            AnagraficheList = (await AnagraficheDataService.GetAllAnagrafiche()).ToList();
            StateHasChanged();
        }
    }
}