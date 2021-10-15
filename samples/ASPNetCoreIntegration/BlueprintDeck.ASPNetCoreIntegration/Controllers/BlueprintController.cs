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

        private Blueprint _latestBlueprint;
       
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
        
        
        public void Start(IBlueprintFactory blueprintFactory)
        {
            if (_latestBlueprint == null) throw new Exception("Blueprint not initialized");
            using var blueprintInstance = blueprintFactory.CreateBlueprint(_latestBlueprint);
            blueprintInstance.Activate();
        }
        
        
    }
}