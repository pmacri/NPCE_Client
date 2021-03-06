﻿using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class Ambienti: ComponentBase
    {
        public IEnumerable<Ambiente> AmbientiList { get; set; }

        [Inject]
        public IAmbientiDataService AmbientiDataService { get; set; }

        //protected AddAmbienteDialog AddAmbienteDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AmbientiList = (await AmbientiDataService.GetAllAmbienti()).ToList();
        }

      

      
    }
}
