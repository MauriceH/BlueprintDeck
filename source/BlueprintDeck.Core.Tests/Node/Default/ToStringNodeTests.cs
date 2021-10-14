using System;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Xunit;

namespace BlueprintDeck.Node.Default
{
    public class ToStringNodeTests
    {
        [Fact]
        public async Task TestToStringNode()
        {
            var sb = new StringBuilder();
            sb.Append("test");
            var sut = new ToStringNode<StringBuilder>();
            
            var trigger = new Subject<StringBuilder>();
            var triggerInput = new DataInput<StringBuilder>(trigger);
            
            sut.Input = triggerInput;

            var tcs = new TaskCompletionSource<string>();

            var triggerOutput = new DataOutput<string>();
            triggerOutput.Observable.Subscribe(value => { tcs.SetResult(value); });

            sut.Output = triggerOutput;

            await sut.Activate();
            
            trigger.OnNext(sb);


            await TimeoutCall.CallAsync(5000, async () =>
            {
                var result = await tcs.Task;
                Assert.Equal("test",result);
            });
            
            await sut.Deactivate();
        }
    }
}