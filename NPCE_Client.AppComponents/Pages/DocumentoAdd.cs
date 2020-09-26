using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class DocumentoAdd : ComponentBase
    {
        [Inject]
        public IDocumentiDataService DocumentiDataService { get; set; }

        public Documento Documento { get; set; }

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        private string status;

        IFileListEntry file;

        public async Task HandleSelection(IFileListEntry[] files)
        {
             file = files.FirstOrDefault();
            if (file != null)
            {
                // Just load into .NET memory to show it can be done
                // Alternatively it could be saved to disk, or parsed in memory, or similar
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                Documento.Content = ms.ToArray();
                Documento.FileName = file.Name;
                Documento.Extension = new FileInfo(file.Name).Extension;
                Documento.Size = file.Size;

                status = $"Finished loading {file.Size} bytes from {file.Name}";

            }
        }

        protected override void OnInitialized()
        {
            Saved = false;
            Documento = new Documento();
        }

        public ElementReference InputFileDialog { get; set; }

        protected async Task HandleValidSubmit()
        {

            var added = await DocumentiDataService.AddDocumento(Documento);

            if (added != null)
            {
                StatusClass = "alert-success";
                Message = "Nuovo documento creato con successo.";
                Saved = true;
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "Si è verificato un errore nella creazione del nuovo documento. Riprovare.";
                Saved = false;
            }

        }

    }
}
