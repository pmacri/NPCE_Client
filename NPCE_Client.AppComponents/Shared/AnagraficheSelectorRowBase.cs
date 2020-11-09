using Microsoft.AspNetCore.Components;

namespace NPCE_Client.AppComponents.Shared
{
    public class AnagraficheSelectorRowBase : ComponentBase
    {
        [Parameter]
        public AnagraficheSelectorViewModel AnagraficaSelector { get; set; }
    }
}
