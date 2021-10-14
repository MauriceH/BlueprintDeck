using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlueprintDeck
{
    public class TimeoutCall
    {
        public static async Task CallAsync(int timeoutInMilliseconds, Func<Task> call)
        {
            var cts = new CancellationTokenSource();
            var timeoutCall = Task.Delay(TimeSpan.FromMilliseconds(timeoutInMilliseconds), cts.Token);
            var directCall = Task.Run(async ()=> await call(), cts.Token);
            try
            {
                await Task.WhenAny(timeoutCall, directCall);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                cts.Cancel();
            }
            catch (Exception)
            {
                // ignored
            }

            if (directCall.IsCompleted || directCall.IsFaulted)
            {
                await directCall;
                return;
            }
            
            throw new Exception("Timeout reached");
        }
    }
}