using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NPCE_Client.Api.Data;
using NPCE_Client.AppComponents.Shared;
using NPCE_Client.Model;

namespace NPCE_Client.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagraficheSelectorController : ControllerBase
    {
        private readonly IAnagraficaRepository anagraficheRepository;

        public AnagraficheSelectorController(IAnagraficaRepository anagraficheRepository)
        {
            this.anagraficheRepository = anagraficheRepository;
        }

       
        [HttpGet()]
        public IActionResult GetAllAnagraficheSelector()
        {
            return Ok(GetAllAnagrafiche()); ;
        }

        private List<AnagraficheSelectorViewModel> GetAllAnagrafiche()
        {
            List<AnagraficheSelectorViewModel> result = new List<AnagraficheSelectorViewModel>();
            var all = anagraficheRepository.GetAllAnagrafiche();

            foreach (var anagrafica in all)
            {
                result.Add(new AnagraficheSelectorViewModel { Anagrafica = anagrafica, IsMittente = false });
            }

            return result;
        }

        [HttpGet("{servizioId}")]
        public IActionResult GetByServizio(int servizioId)
        {
            var anagraficheInservizio = anagraficheRepository.GetByServizio(servizioId);
            var all = GetAllAnagrafiche();
            foreach (var anag in all)
            {
                if (anagraficheInservizio.Any(a => a.Anagrafica.Id == anag.Anagrafica.Id))
                {
                    anag.IsSelected = true;
                }
            }
            return Ok(all);
        }

        [HttpPut("{servizioId}")]
        public async  Task<IActionResult> UpdateAnagraficheServizio(int servizioId, IEnumerable<AnagraficheSelectorViewModel> anagrafiche)
        {
            await anagraficheRepository.UpdateAngraficheServizioAsync(servizioId, anagrafiche);
            return Ok();

        }

    }
}
