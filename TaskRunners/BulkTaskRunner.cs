using System;
using System.Collections.Generic;

namespace CloudyWing.TaskRunners {
    public class BulkTaskRunner : TaskRunnerBase, ITaskRunnable {
        private readonly ICollection<ITaskRunnable> runners = new List<ITaskRunnable>();

        public event EventHandler<ExceptionEventArgs> ExceptionThrownEvent;

        public BulkTaskRunner(bool isStoppedIfFail = false) {
            IsStoppedIfFail = isStoppedIfFail;
        }

        public virtual bool IsStoppedIfFail { get; }

        protected override void RunTask() {
            foreach (ITaskRunnable runner in runners) {
                try {
                    runner.Run();
                } catch (Exception ex) {
                    OnExceptionThrown(ex);

                    if (IsStoppedIfFail) {
                        throw;
                    }
                }
            }
        }

        protected virtual void OnExceptionThrown(Exception exception) {
            ExceptionThrownEvent?.Invoke(this, new ExceptionEventArgs(exception));
        }

        public void AddRunner(ITaskRunnable runner) {
            runners.Add(runner);
        }

        public void RemoveRunner(ITaskRunnable runner) {
            runners.Remove(runner);
        }

        public void ClearRunners() {
            runners.Clear();
        }
    }
}