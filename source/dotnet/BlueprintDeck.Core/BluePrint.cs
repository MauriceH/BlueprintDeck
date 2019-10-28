using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using BlueprintDeck.Instance;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck
{
    public class BluePrint : IDisposable
    {
        private readonly ILogger<BluePrint> _logger;

        private readonly List<ConstantValueInstance> _values;
        private readonly List<NodeInstance> _nodes;
        private readonly ILifetimeScope _scope;

        public BluePrint(ILifetimeScope scope, List<NodeInstance> nodes, List<ConstantValueInstance> values, ILogger<BluePrint> logger)
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
                var validation = nodeInstance.CheckIfValid();
                if (validation)
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
                sb.Append(nodeInstance.Descriptor.Id);
                sb.Append(") ");
                sb.AppendLine(nodeInstance.Descriptor.Title);
            }

            _logger.LogTrace(sb.ToString());
        }
    }
}