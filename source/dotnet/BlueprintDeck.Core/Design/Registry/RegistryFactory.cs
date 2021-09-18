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
            return _nodeRegistrations.Select(x =>
            {
                var listPortDefinition = new List<NodePort>();
                foreach (var d in x.PortDefinitions)
                {
                    var nodePort = new NodePort
                    {
                        Key = d.Key,
                        Title = d.Title,
                        Mandatory = d.Mandatory,
                        Direction = d.Direction,
                    };
                    if (d.PortDataType != null || d.GenericTypeParameterName != null)
                    {
                        if (!string.IsNullOrWhiteSpace(d.GenericTypeParameterName))
                        {
                            nodePort.GenericTypeParameter = d.GenericTypeParameterName;
                        }
                        else
                        {
                            var portDataType = d.PortDataType!;
                            var dataTypeRegistration = _dataTypeRegistrations.FirstOrDefault(t => t.DataType == portDataType);
                            if (dataTypeRegistration == null) throw new Exception("Node without registered type");

                            nodePort.TypeId = dataTypeRegistration.Key;
                        }
                    }
                    listPortDefinition.Add(nodePort);
                }

                return new NodeType
                {
                    Id = x.Id,
                    Title = x.Title,
                    Ports = listPortDefinition,
                    GenericTypes = x.GenericTypes.Count <= 0 ? null : x.GenericTypes.ToList()
                };
            }).ToList();
        }

        private List<DataType> CreateDataTypes()
        {
            return _dataTypeRegistrations.Select(x => new DataType
            {
                Id = x.Key,
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
                        TypeId = dataType.Key
                    }
                };
            }).ToList();
        }
        
        
    }
}