using System;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Helpers
{
    public class Repeater
    {
        public static void RepeatTask(Action action, int seconds, CancellationToken token)
        {
            if (action == null)
                return;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                }
            }, token);
        }
    }
}