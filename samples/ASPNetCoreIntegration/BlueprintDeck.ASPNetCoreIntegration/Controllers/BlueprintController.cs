using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueprintDeck.DependencyInjection;
using BlueprintDeck.Design;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Instance.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.ASPNetCoreIntegration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlueprintControllerController : ControllerBase
    {

        private readonly IBlueprintFactory _blueprintFactory;
        
        private Blueprint _latestBlueprint;

        public BlueprintControllerController(IBlueprintFactory blueprintFactory)
        {
            _blueprintFactory = blueprintFactory ?? throw new ArgumentNullException(nameof(blueprintFactory));
        }

        [HttpGet("registry")]
        public ActionResult<BlueprintRegistry> GetRegistry([FromServices] IBlueprintDeckRegistryFactory registryFactory)
        {
            return Ok(registryFactory);
        }
        
        [HttpGet]
        public ActionResult<Blueprint> GetBlueprint()
        {
            return Ok(_latestBlueprint);
        }
        
        [HttpPost]
        public ActionResult<Blueprint> SetBlueprint([FromBody] Blueprint blueprint)
        {
            _latestBlueprint = blueprint;
            return Ok();
        }
        
        
        
        //Start Blueprint from any location
        public void Start()
        {
            if (_latestBlueprint == null) throw new Exception("Blueprint not initialized");
            using var blueprintInstance = _blueprintFactory.CreateBlueprint(_latestBlueprint);
            blueprintInstance.Activate();
        }
        
        
    }
}