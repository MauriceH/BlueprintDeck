using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Instance
{
    public class BluePrint : IDisposable
    {
        private readonly ILogger<BluePrint> _logger;

        private readonly List<ConstantValueInstance> _values;
        private readonly List<NodeInstance> _nodes;
        private readonly IServiceScope _scope;

        public BluePrint(ILogger<BluePrint> logger, IServiceScope scope, List<NodeInstance> nodes, List<ConstantValueInstance> values)
        {
            _nodes = nodes;
            _logger = logger;
            _values = values;
            _scope = scope;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        public void Activate()
        {
            
            
            var reverseNodes = _nodes.Reverse<NodeInstance>().ToList();

            foreach (var valueInstance in _values)
            {
                valueInstance.Activate();
            }
            
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
                sb.Append(nodeInstance.Registration.Key);
                sb.Append(") ");
                sb.AppendLine(nodeInstance.Registration.Title);
            }

            _logger.LogTrace(sb.ToString());
        }
    }
}