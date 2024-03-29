using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Node.Registration;

namespace BlueprintDeck.Design.Registry
{
    internal class BlueprintDeckRegistryFactory : IBlueprintDeckRegistryFactory
    {
        private readonly List<NodeRegistration> _nodeRegistrations;
        private readonly List<DataTypeRegistration> _dataTypeRegistrations;
        

        public BlueprintDeckRegistryFactory(List<NodeRegistration> nodeRegistrations, List<DataTypeRegistration> dataTypeRegistrations)
        {
            if (nodeRegistrations == null) throw new ArgumentNullException(nameof(nodeRegistrations));
            if (dataTypeRegistrations == null) throw new ArgumentNullException(nameof(dataTypeRegistrations));
            _nodeRegistrations = nodeRegistrations.ToList();
            _dataTypeRegistrations = dataTypeRegistrations.ToList();
        }

        public BlueprintRegistry CreateNodeRegistry()
        {
            var nodeTypes = CreateNodeTypes();
            var dataTypes = CreateDataTypes();
            return new BlueprintRegistry(nodeTypes, dataTypes);
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
                    if (port.DataType != null || port.GenericTypeParameter != null)
                    {
                        if (!string.IsNullOrWhiteSpace(port.GenericTypeParameter))
                        {
                            nodePort.GenericTypeParameter = port.GenericTypeParameter;
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
                        properties.Add(nodeProperty);
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
        
    }
}