using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Registration;

namespace BlueprintDeck.Design.Registry
{
    internal class RegistryFactory : IRegistryFactory
    {
        private readonly List<NodeRegistration> _nodeRegistrations;
        private readonly List<DataTypeRegistration> _dataTypeRegistrations;
        private readonly List<ConstantValueRegistration> _constantValueRegistrations;
        

        public RegistryFactory(IEnumerable<NodeRegistration> nodeRegistrations, IEnumerable<DataTypeRegistration> dataTypeRegistrations, IEnumerable<ConstantValueRegistration> constantValueRegistrations)
        {
            if (nodeRegistrations == null) throw new ArgumentNullException(nameof(nodeRegistrations));
            if (dataTypeRegistrations == null) throw new ArgumentNullException(nameof(dataTypeRegistrations));
            if (constantValueRegistrations == null) throw new ArgumentNullException(nameof(constantValueRegistrations));
            _nodeRegistrations = nodeRegistrations.ToList();
            _dataTypeRegistrations = dataTypeRegistrations.ToList();
            _constantValueRegistrations = constantValueRegistrations.ToList();
        }

        public BlueprintRegistry CreateNodeRegistry()
        {
            return new BlueprintRegistry
            {
                NodeTypes = CreateNodeTypes(),
                DataTypes = CreateDataTypes(),
                ConstantValueNodeTypes = CreateConstantValueNodeTypes()
            };
        }

        private List<NodeType> CreateNodeTypes()
        {
            return _nodeRegistrations.Select(node =>
            {
                var ports = new List<NodePort>();
                foreach (var port in node.Ports)
                {
                    var nodePort = new NodePort
                    {
                        Key = port.Key,
                        Title = port.Title,
                        Mandatory = port.Mandatory,
                        Direction = port.Direction,
                    };
                    if (port.DataType != null || port.GenericTypeParameterName != null)
                    {
                        if (!string.IsNullOrWhiteSpace(port.GenericTypeParameterName))
                        {
                            nodePort.GenericTypeParameter = port.GenericTypeParameterName;
                        }
                        else
                        {
                            var portDataType = port.DataType!;
                            var dataTypeRegistration = _dataTypeRegistrations.FirstOrDefault(t => t.DataType == portDataType);
                            if (dataTypeRegistration == null) throw new Exception("Node without registered type");

                            nodePort.TypeId = dataTypeRegistration.Id;
                        }
                    }
                    ports.Add(nodePort);
                }
                
                List<NodeProperty>? properties = null;
                if (node.Properties.Any())
                {
                    properties = new List<NodeProperty>();
                    foreach (var property in node.Properties)
                    {
                        var nodeProperty = new NodeProperty
                        {
                            Name = property.Name,
                            Title = property.Title
                        };
                        var dataTypeRegistration = _dataTypeRegistrations.FirstOrDefault(t =>
                            t.DataType == property.Type || Nullable.GetUnderlyingType(property.Type) == t.DataType);
                        if (dataTypeRegistration == null)
                            throw new Exception($"Node property {property.Name} type {property.Type.Name} not registered");
                        nodeProperty.TypeId = dataTypeRegistration.Id;

                    }
                }
                

                return new NodeType
                {
                    Id = node.Id,
                    Title = node.Title,
                    Ports = ports,
                    Properties = properties,
                    GenericTypes = node.GenericTypes.Count <= 0 ? null : node.GenericTypes.ToList()
                };
            }).ToList();
        }

        private List<DataType> CreateDataTypes()
        {
            return _dataTypeRegistrations.Select(x => new DataType
            {
                Id = x.Id,
                Title = x.Title,
                TypeName = x.DataType.FullName
            }).ToList();
        }

        private List<ConstantValueNodeType> CreateConstantValueNodeTypes()
        {
            return _constantValueRegistrations.Select(x =>
            {
                var dataType = _dataTypeRegistrations.FirstOrDefault(dt => dt.DataType == x.DataType);
                if (dataType == null) throw new Exception("Node without registered type");
                return new ConstantValueNodeType
                {
                    Id = x.Key,
                    Title = x.Title,
                    Port = new ConstantValueNodePortType
                    {
                        Key = "value",
                        Title = "Value",
                        TypeId = dataType.Id
                    }
                };
            }).ToList();
        }
        
        
    }
}