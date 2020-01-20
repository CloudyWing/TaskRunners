using System;
using System.Diagnostics;

namespace CloudyWing.TaskRunners {
    public abstract class TaskRunnerBase : ITaskRunner {
        private readonly Stopwatch stopwatch = new Stopwatch();

        public event EventHandler<TaskRunningEventArgs> TaskRunningEvent;
        public event EventHandler<TaskRunnedEventArgs> TaskRunnedEvent;

        protected TaskRunnerBase() { }

        public void Run() {
            stopwatch.Start();
            TaskStatus = TaskStatus.Running;
            OnTaskRunning();

            try {
                RunTask();
            } catch {
                // 為了讓OnTaskRunned判斷是否為Faulted，所以放前面
                TaskStatus = TaskStatus.Faulted;
                OnTaskRunned();
                stopwatch.Stop();
                throw;
            }

            // 執行完TaskRunned才算結束
            OnTaskRunned();
            TaskStatus = TaskStatus.RanToCompletion;
            stopwatch.Stop();
        }

        public abstract TaskInfo TaskInfo { get; }

        public TaskStatus TaskStatus { get; private set; } = TaskStatus.Created;

        public TimeSpan Elapsed => stopwatch.Elapsed;

        protected abstract void RunTask();

        protected virtual void OnTaskRunning() {
            TaskRunningEvent?.Invoke(this, new TaskRunningEventArgs(TaskInfo));
        }

        protected virtual void OnTaskRunned() {
            TaskRunnedEvent?.Invoke(this, new TaskRunnedEventArgs(TaskInfo, TaskStatus, Elapsed));
        }
    }
}