using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.AppLogic.Facades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DasContract.Editor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        readonly ContractFileSessionFacade facade;

        public ContractController(ContractFileSessionFacade facade)
        {
            this.facade = facade;
        }

        
    }
}
