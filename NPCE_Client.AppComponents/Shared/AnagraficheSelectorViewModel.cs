using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.AppComponents.Shared
{
    public class AnagraficheSelectorViewModel
    {
        public Anagrafica Anagrafica { get; set; }
        public bool IsMittente { get; set; }
        public bool IsSelected { get; set; }
    }
}
