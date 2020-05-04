using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public class NodeRegistryFactory : INodeRegistryFactory
    {
        private readonly object _lockObject = new object();
        
        private readonly IConstantValueSerializerRepository _constantValueSerializerRepository;
        private readonly Func<IEnumerable<NodeRegistration>> _nodeRegistrationProvider;
        private readonly Dictionary<Type, DataType> _dataTypes = new Dictionary<Type, DataType>();
        
        private List<NodeType>? _nodeTypes;
        

        public NodeRegistryFactory(Func<IEnumerable<NodeRegistration>> nodeRegistrationProvider, IConstantValueSerializerRepository constantValueSerializerRepository)
        {
            _nodeRegistrationProvider = nodeRegistrationProvider;
            _constantValueSerializerRepository = constantValueSerializerRepository;
        }

        private void InitializeNodeTypes()
        {
            lock (_lockObject)
            {
                if (_nodeTypes != null) return;
                var nodeRegistrations = _nodeRegistrationProvider.Invoke();
                _nodeTypes = nodeRegistrations.Select(x =>
                {
                    var listPortDefinition = new List<NodePort>();
                    foreach (var d in x.PortDefinitions)
                    {
                        

                        
                            
                        var nodePort = new NodePort
                        {
                            Key = d.Key,
                            Title = d.Title,
                            Mandatory = d.Mandatory,
                            DataMode = d.DataMode,
                            InputOutputType = d.InputOutputType,
                            
                        };
                        if (d.DataMode == DataMode.WithData)
                        {
                            var portDataType = d.PortDataType!;
                            var serializer = _constantValueSerializerRepository.LoadSerializer(portDataType);
                            if (!_dataTypes.TryGetValue(portDataType, out var dataType))
                            {
                                dataType = new DataType
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Assembly = portDataType.Assembly.GetName().Name,
                                    TypeName = $"{portDataType.Namespace}.{portDataType.Name}",
                                    FullTypeName = portDataType.FullName
                                };
                                _dataTypes[portDataType] = dataType;
                            }

                            nodePort.DefaultValue = serializer.Serialize(d.DefaultValue);
                            nodePort.TypeId = dataType.Id;
                        }
                        listPortDefinition.Add(nodePort);
                    }

                    return new NodeType
                    {
                        Key = x.Key,
                        Title = x.Title,
                        Ports = listPortDefinition
                    };
                }).ToList();
            }
        }

        public NodeRegistry LoadNodeRegistry()
        {
            InitializeNodeTypes();
            List<NodeType> nodeTypes;
            List<DataType> dataTypes;
            lock (_nodeTypes!)
            {
                nodeTypes = _nodeTypes.ToList();
                dataTypes = _dataTypes.Values.ToList();
            }

            return new NodeRegistry
            {
                NodeTypes = nodeTypes,
                DataTypes = dataTypes
            };
        }
    }
}