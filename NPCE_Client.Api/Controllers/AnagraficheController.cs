using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPCE_Client.Api.Data;
using NPCE_Client.Model;

namespace NPCE_Client.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagraficheController : ControllerBase
    {

        private readonly IAnagraficaRepository anagraficheRepository;

        public AnagraficheController(IAnagraficaRepository anagraficheRepository)
        {
            this.anagraficheRepository = anagraficheRepository;
        }

        [HttpGet]
        public IActionResult GetAllAnagrafiche()
        {
            return Ok(anagraficheRepository.GetAllAnagrafiche());
        }

        [HttpGet("{anagraficaId}")]
        public IActionResult GetAnagraficaById(int anagraficaId)
        {
            return Ok(anagraficheRepository.GetAnagraficaById(anagraficaId));
        }

        [HttpPost]
        public IActionResult CreateAnagrafica([FromBody] Anagrafica anagrafica)
        {
            if (anagrafica == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdAnagrafica = anagraficheRepository.AddAnagrafica(anagrafica);

            return Created("anagrafica", createdAnagrafica);
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] Anagrafica anagrafica)
        {
            if (anagrafica == null)
                return BadRequest();

           

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var anagraficaToUpdate = anagraficheRepository.GetAnagraficaById(anagrafica.Id);

            if (anagraficaToUpdate == null)
                return NotFound();

            anagraficheRepository.UpdateAnagrafica(anagrafica);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnagrafica(int id)
        {
            if (id == 0)
                return BadRequest();

            var anagraficaToDelete = anagraficheRepository.GetAnagraficaById(id);
            if (anagraficaToDelete == null)
                return NotFound();

            anagraficheRepository.DeleteAnagrafica(id);

            return NoContent();//success
        }
    }
}
