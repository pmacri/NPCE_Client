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
    public class DocumentiController : ControllerBase
    {
        private readonly IDocumentiRepository documentiRepository;

        public DocumentiController(IDocumentiRepository documentiRepository)
        {
            this.documentiRepository = documentiRepository;
        }

        [HttpGet]
        public IActionResult GetAllDocumenti()
        {
            return Ok(documentiRepository.GetAllDocumenti());
        }

        [HttpGet("{documentoId}")]
        public IActionResult GetDocumentoById(int anagraficaId)
        {
            return Ok(documentiRepository.GetDocumentoById(anagraficaId));
        }

        [HttpPost]
        public IActionResult CreateDocumento([FromBody] Documento documento)
        {
            if (documento == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDocumento = documentiRepository.AddDocumento(documento);

            return Created("documento", createdDocumento);
        }

        [HttpPut]
        public IActionResult UpdateDocumento([FromBody] Documento documento)
        {
            if (documento == null)
                return BadRequest();



            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var documentoToUpdate = documentiRepository.GetDocumentoById(documento.Id);

            if (documentoToUpdate == null)
                return NotFound();

            documentiRepository.UpdateDocumento(documento);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocumento(int id)
        {
            if (id == 0)
                return BadRequest();

            var documentoToDelete = documentiRepository.GetDocumentoById(id);
            if (documentoToDelete == null)
                return NotFound();

            documentiRepository.DeleteDocumento(id);

            return NoContent();//success
        }
    }
}
