using System.ComponentModel.DataAnnotations;
using BlueprintDeck.Design;
using BlueprintDeck.Design.Registry;
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
        private readonly BlueprintInstance _instance;

        public BlueprintDeckController(ILogger<BlueprintDeckController> logger, IRegistryFactory registryFactory, BlueprintInstance instance)
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
        public Blueprint GetBlueprint()
        {
            return _instance.DesignBlueprint;
        }

        [HttpPut("Design")]
        public IActionResult SetBluePrint([FromBody,Required] Blueprint design)
        {
            _instance.Start(design);
            return Ok();
        }
        
    }
}