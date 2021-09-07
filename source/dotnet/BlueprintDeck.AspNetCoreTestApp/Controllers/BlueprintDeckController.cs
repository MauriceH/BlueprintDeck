using System;
using System.ComponentModel.DataAnnotations;
using BlueprintDeck.Design;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Registration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.AspNetCoreTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlueprintDeckController : ControllerBase
    {

        private readonly ILogger<BlueprintDeckController> _logger;
        private readonly IRegistryFactory _registryFactory;
        private readonly BluePrintInstance _instance;

        public BlueprintDeckController(ILogger<BlueprintDeckController> logger, IRegistryFactory registryFactory, BluePrintInstance instance)
        {
            _logger = logger;
            _registryFactory = registryFactory;
            _instance = instance;
        }

        [HttpGet("Registry")]
        public BlueprintRegistry GetRegistry()
        {
            return _registryFactory.CreateNodeRegistry();
        }

        [HttpGet("Design")]
        public BluePrint GetBluePrint()
        {
            return _instance.DesignBluePrint;
        }

        [HttpPut("Design")]
        public IActionResult SetBluePrint([FromBody,Required] BluePrint design)
        {
            _instance.Start(design);
            return Ok();
        }
        
    }
}