using System;

namespace CloudyWing.TaskRunners {
    public interface ITaskRunner {
        TaskStatus TaskStatus { get; }

        TimeSpan Elapsed { get; }

        event EventHandler<TaskRunningEventArgs> TaskRunningEvent;

        event EventHandler<TaskRunnedEventArgs> TaskRunnedEvent;

        void Run();
    }
}