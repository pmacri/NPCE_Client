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
    public class ServiziController : ControllerBase
    {
        private readonly IServiziRepository serviziRepository;

        public ServiziController(IServiziRepository serviziRepository)
        {
            this.serviziRepository = serviziRepository;
        }

        [HttpGet("{ServizioId}")]
        public async Task<IActionResult> GetServizioById(int ServizioId)
        {
            var servizio = await serviziRepository.GetServizioByIdAsync(ServizioId);
            return Ok(servizio);
        }

        [HttpPost]
        public IActionResult AddServizio([FromBody] Servizio servizio)
        {
            if (servizio == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdServizio = serviziRepository.AddServizioAsync(servizio);

            return Created("Servizio", createdServizio);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(serviziRepository.GetAll());
        }
    }
}
