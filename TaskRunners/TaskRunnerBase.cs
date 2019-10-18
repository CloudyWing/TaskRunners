using System;

namespace CloudyWing.TaskRunners {
    public abstract class TaskRunnerBase : ITaskRunnable {
        public event EventHandler TaskRunningEvent;
        public event EventHandler TaskRunnedEvent;

        protected TaskRunnerBase() { }

        public void Run() {
            OnTaskRunning();
            try {
                RunTask();
            } finally {
                OnTaskRunned();
            }
        }

        protected abstract void RunTask();

        protected virtual void OnTaskRunning() {
            TaskRunningEvent?.Invoke(this, new EventArgs());
        }

        protected virtual void OnTaskRunned() {
            TaskRunnedEvent?.Invoke(this, new EventArgs());
        }
    }
}