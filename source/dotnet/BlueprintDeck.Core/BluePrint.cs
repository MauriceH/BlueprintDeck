using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BlueprintDeck.Instance;

namespace BlueprintDeck
{
    public class BluePrint : IDisposable
    {
        private readonly List<NodeInstance> _nodes;
        private readonly ILifetimeScope _scope;

        public BluePrint(ILifetimeScope scope, List<NodeInstance> nodes)
        {
            _nodes = nodes;
            _scope = scope;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        public void Activate()
        {
            var reverseNodes = _nodes.Reverse<NodeInstance>();
            Console.WriteLine("- - activating node order - - ");
            foreach (var nodeInstance in reverseNodes)
            {
                Console.WriteLine(nodeInstance);
            }
            Console.WriteLine("- - end order - - ");
            foreach (var nodeInstance in reverseNodes)
            {
                var validation = nodeInstance.CheckIfValid();
                if (validation)
                {
                    nodeInstance.Activate();
                }
            }
        }

     
    }
}