using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Registration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlueprintDeck.ASPNetCore
{
    [Route("blueprintdeck")]
    public class BlueprintDeckController : Controller
    {
        private readonly IEnumerable<NodeRegistration> _nodes;

        public BlueprintDeckController(IEnumerable<NodeRegistration> nodes)
        {
            _nodes = nodes;
        }

        [HttpGet("nodes")]
        public IActionResult Get()
        {
            return Json(new {Nodes = _nodes.ToList()},new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}