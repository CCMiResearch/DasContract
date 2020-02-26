using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DasContract.Editor.AppLogic.Facades;
using DasContract.Editor.AppLogic.Facades.Interfaces;
using DasContract.Editor.DataPersistence.Entities;
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

        [HttpPost("InitiateSession/{id}")]
        public async Task<ActionResult> InitiateSession(string id, List<IFormFile> files)
        {
            var temp = Request.Form.Files;
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            var contractFile = files.Single();
            var fileContentBuilder = new StringBuilder();
            using (var reader = new StreamReader(contractFile.OpenReadStream()))
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
    }

    public class ContractFileSessionStarterController : Controller
    {
        
    }

    
}
