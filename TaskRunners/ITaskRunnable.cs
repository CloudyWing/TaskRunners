using System;

namespace CloudyWing.TaskRunners {
    public interface ITaskRunnable {
        TaskStatus TaskStatus { get; }

        TimeSpan Elapsed { get; }

        event EventHandler<TaskRunningEventArgs> TaskRunningEvent;

        event EventHandler<TaskRunnedEventArgs> TaskRunnedEvent;

        void Run();
    }
}