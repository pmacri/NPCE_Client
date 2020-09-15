using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPCE_Client.Api.Data;

namespace NPCE_Client.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagraficheController : ControllerBase
    {

        private readonly IAnagraficheRepository anagraficheRepository;

        public AnagraficheController(IAnagraficheRepository anagraficheRepository)
        {
            this.anagraficheRepository = anagraficheRepository;
        }

        [HttpGet]
        public IActionResult GetAllAnagrafiche()
        {
            return Ok(anagraficheRepository.GetAllAnagrafiche());
        }
    }
}
