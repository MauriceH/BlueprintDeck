using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BlueprintDeck.Node.Default
{
    public class DelayNodeTests
    {
        [Fact]
        public async Task TestDelayNode()
        {
            var logger = Substitute.For<ILogger<DelayNode>>();

            var sut = new DelayNode(logger)
            {
                DefaultDelay = TimeSpan.FromSeconds(2)
            };

            await TestDelayNodeInternal(sut, 1.0);
        }

       
        private static async Task TestDelayNodeInternal(DelayNode sut, double failIfReadyBeforeTotalSeconds = 2.0)
        {
            var trigger = new Subject<object>();
            var triggerInput = new SimpleInput(trigger);
            sut.Input = triggerInput;

            var tcs = new TaskCompletionSource();

            var triggerOutput = new SimpleOutput();
            triggerOutput.Observable.Subscribe(_ => { tcs.SetResult(); });

            sut.Output = triggerOutput;

            await sut.Activate();
            var sw = Stopwatch.StartNew();
            trigger.OnNext(new object());
            var timeout = Task.Delay(TimeSpan.FromSeconds(20));
            await Task.WhenAny(timeout, tcs.Task);
            sw.Stop();

            if (sw.Elapsed.TotalSeconds < failIfReadyBeforeTotalSeconds)
            {
                throw new Exception("Delay output reached before delay duration elapsed");
            }

            if (timeout.IsCompleted || sw.Elapsed.TotalSeconds < failIfReadyBeforeTotalSeconds)
            {
                throw new Exception("Delay timout reached");
            }

            await sut.Deactivate();
        }
    }
}