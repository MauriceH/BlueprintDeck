using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Instance
{
    internal class Blueprint : IDisposable, IBlueprintInstance
    {
        private readonly ILogger<Blueprint> _logger;

        private readonly List<NodeInstance> _nodes;
        private readonly IServiceScope _scope;

        public Blueprint(ILogger<Blueprint> logger, IServiceScope scope, List<NodeInstance> nodes)
        {
            _nodes = nodes;
            _logger = logger;
            _scope = scope;
        }

        public void Dispose()
        {
            foreach (var nodeInstance in _nodes)
            {
                nodeInstance.Node.Deactivate();
            }

            _scope.Dispose();
        }

        public void Activate()
        {
            var reverseNodes = _nodes.Reverse<NodeInstance>().ToList();

            LogActivationOrder(reverseNodes);
            foreach (var nodeInstance in reverseNodes)
            {
                if (nodeInstance.AllRequiredInputsConnected)
                {
                    nodeInstance.Activate();
                }
            }
        }

        private void LogActivationOrder(IEnumerable<NodeInstance> nodes)
        {
            if (!_logger.IsEnabled(LogLevel.Debug)) return;
            var sb = new StringBuilder();
            sb.AppendLine("Activating BlueprintDeck nodes in order: ");
            foreach (var nodeInstance in nodes)
            {
                sb.Append(" - (");
                sb.Append(nodeInstance.Registration.Id);
                sb.Append(") ");
                sb.AppendLine(nodeInstance.Registration.Title);
            }

            _logger.LogTrace(sb.ToString());
        }
    }
}