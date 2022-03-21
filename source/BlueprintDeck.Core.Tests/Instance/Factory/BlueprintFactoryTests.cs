using System;
using System.Collections.Generic;
using BlueprintDeck.Misc;
using BlueprintDeck.ValueSerializer.Registration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Instance.Factory
{
    public class BlueprintFactoryTests
    {

        [Fact]
        public void TestFactory_WhenInvalidDesign_ThrowsInvalidBlueprintException()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var scope = Substitute.For<IServiceScope>();
            var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
            var nodeFactory = Substitute.For<INodeFactory>();
            var portConnectionManager = Substitute.For<PortConnectionManager>();

            serviceScopeFactory.CreateScope().Returns(scope);
            serviceProvider.GetService(Arg.Any<Type>()).Returns(serviceScopeFactory);

            var serializerRepo = Substitute.For<IValueSerializerRepository>();
            
            var sut = new BlueprintFactory(serviceProvider, nodeFactory, portConnectionManager, serializerRepo);

            Assert.Throws<ArgumentNullException>(() =>
            {
                sut.CreateBlueprint(null);
            });
            
            var design = new Design.Blueprint
            {
                Nodes = new List<Design.Node>
                {
                    null
                }
            };

            Assert.Throws<InvalidBlueprintException>(() =>
            {
                sut.CreateBlueprint(design);
            });
            
            design = new Design.Blueprint
            {
                Nodes = new List<Design.Node>
                {
                    new()
                    {
                        Id = ""
                    }
                }
            };

            Assert.Throws<InvalidBlueprintException>(() =>
            {
                sut.CreateBlueprint(design);
            });
            
            design = new Design.Blueprint
            {
                Nodes = new List<Design.Node>
                {
                    new()
                    {
                        Id = "asd",
                        NodeTypeKey = ""
                    }
                }
            };

            Assert.Throws<InvalidBlueprintException>(() =>
            {
                sut.CreateBlueprint(design);
            });
            
        }
        
        
    }
}