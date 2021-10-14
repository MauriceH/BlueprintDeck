using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Node.Default
{
    public class ActivateNodeTests
    {

        [Fact]
        public async Task TestActivation()
        {

            var logger = Substitute.For<ILogger<ActivateNode>>();
            
            var sut = new ActivateNode(logger);

            var isDone = false;
            
            var simpleOutput = new SimpleOutput();
            simpleOutput.Observable.Subscribe(obj =>
            {
                isDone = true;
            });
            sut.Event = simpleOutput;
            
            await sut.Activate();
            await sut.Deactivate();
            Assert.True(isDone);
            
        }
        
        [Fact]
        public async Task TestActivation_IfOutputNotSet()
        {

            var logger = Substitute.For<ILogger<ActivateNode>>();
            var sut = new ActivateNode(logger);
            await sut.Activate();
            await sut.Deactivate();
        }
        
        
    }
}