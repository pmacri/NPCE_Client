using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class Documenti: ComponentBase
    {
        public IEnumerable<Documento> DocumentiList { get; set; }

        [Inject]
        public IDocumentiDataService DocumentiDataService { get; set; }

        public async Task DeleteDocument(int idDocument)
        {
            await DocumentiDataService.DeleteDocumento(idDocument);
            DocumentiList = await DocumentiDataService.GetAllDocumenti();
        }

        protected override async  Task OnInitializedAsync()
        {
            DocumentiList = await DocumentiDataService.GetAllDocumenti();
        }
    }
}
