using System;
using System.Threading.Tasks;

namespace SqliteWasmTests.TestHelpers
{
    public class DelayedTrigger<T>
    {
        private Action? triggeredAction;

        private TaskCompletionSource<T>? tcs = null;
        
        public DelayedTrigger()
        {
        }

        public DelayedTrigger(Action triggeredAction) =>
            this.triggeredAction = triggeredAction;

        public Task<T> DelayUntilTriggeredAsync()
        {
            tcs = new TaskCompletionSource<T>();
            return tcs.Task;
        }

        public void Trigger(T val)
        {
            if (tcs == null)
            {
                throw new InvalidOperationException("Safety on.");
            }
            triggeredAction?.Invoke();
            tcs.TrySetResult(val);
        }
    }
}
