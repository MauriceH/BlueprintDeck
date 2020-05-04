using BlueprintDeck.Registration;
using Microsoft.AspNetCore.Mvc;

namespace BlueprintDeck.ASPNetCore
{
    [Route("blueprintdeck")]
    public class BlueprintDeckController : Controller
    {
        private readonly INodeRegistryFactory _nodeRegistryFactory;

        public BlueprintDeckController(INodeRegistryFactory nodeRegistryFactory)
        {
            _nodeRegistryFactory = nodeRegistryFactory;
        }

        [HttpGet("types")]
        public IActionResult Get()
        {
            var registry = _nodeRegistryFactory.LoadNodeRegistry();
            return Json(registry);
        }
    }
}