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
    public class AmbientiController : ControllerBase
    {

        private readonly IAmbientiRepository AmbientiRepository;

        public AmbientiController(IAmbientiRepository AmbientiRepository)
        {
            this.AmbientiRepository = AmbientiRepository;
        }

        [HttpGet]
        public IActionResult GetAllAmbienti()
        {
            return Ok(AmbientiRepository.GetAllAmbienti());
        }

        [HttpGet("{AmbienteId}")]
        public IActionResult GetAmbienteById(int AmbienteId)
        {
            return Ok(AmbientiRepository.GetAmbienteById(AmbienteId));
        }

        [HttpPost]
        public IActionResult CreateAmbiente([FromBody] Ambiente Ambiente)
        {
            if (Ambiente == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdAmbiente = AmbientiRepository.AddAmbiente(Ambiente);

            return Created("Ambiente", createdAmbiente);
        }

        [HttpPut]
        public IActionResult UpdateAmbiente([FromBody] Ambiente Ambiente)
        {
            if (Ambiente == null)
                return BadRequest();



            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var AmbienteToUpdate = AmbientiRepository.GetAmbienteById(Ambiente.Id);

            if (AmbienteToUpdate == null)
                return NotFound();

            AmbientiRepository.UpdateAmbiente(Ambiente);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAmbiente(int id)
        {
            if (id == 0)
                return BadRequest();

            var AmbienteToDelete = AmbientiRepository.GetAmbienteById(id);
            if (AmbienteToDelete == null)
                return NotFound();

            AmbientiRepository.DeleteAmbiente(id);

            return NoContent();//success
        }

    }
}
