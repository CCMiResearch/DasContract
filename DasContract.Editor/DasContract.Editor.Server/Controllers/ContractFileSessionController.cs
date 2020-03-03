using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DasContract.Editor.AppLogic.Facades;
using DasContract.Editor.AppLogic.Facades.Interfaces;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Entities.Serialization.XML;
using DasContract.Editor.Interfaces.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DasContract.Editor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractFileSessionController : ControllerBase
    {
        readonly IContractFileSessionFacade facade;

        public ContractFileSessionController(IContractFileSessionFacade facade)
        {
            this.facade = facade;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContractFileSession>>> GetAsync()
        {
            return Ok(await facade.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContractFileSession>> GetAsync(string id)
        {
            try
            {
                return Ok(await facade.GetAsync(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await facade.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertAsync(ContractFileSession item)
        {
            try
            {
                await facade.InsertAsync(item);
                return Ok();
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(ContractFileSession item)
        {
            try
            {
                await facade.UpdateAsync(item);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("InitiateWithFile/{id}")]
        public async Task<ActionResult> InitiateSessionAsync(string id, List<IFormFile> contractFile)
        {
            if (contractFile == null)
                throw new ArgumentNullException(nameof(contractFile));

            var fileContentBuilder = new StringBuilder();
            using (var reader = new StreamReader(contractFile.Single().OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    fileContentBuilder.AppendLine(await reader.ReadLineAsync());
            }

            var newItem = new ContractFileSession()
            {
                Id = id,
                SerializedContract = fileContentBuilder.ToString()
            };

            return await InsertAsync(newItem);
        }

        [HttpGet("{id}/Download")]
        public async Task<ActionResult> DownloadAsync(string id)
        {
            try
            {
                var session = await facade.GetAsync(id);
                var contract = EditorContractXML.From(session.SerializedContract);
                return File(Encoding.UTF8.GetBytes(session.SerializedContract), "application/xml", contract.Name + "_" + contract.Id + ".dascontract");
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
        }
    }

    public class ContractFileSessionStarterController : Controller
    {
        
    }

    
}
