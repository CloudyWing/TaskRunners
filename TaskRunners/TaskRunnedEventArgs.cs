using System;

namespace CloudyWing.TaskRunners {
    public class TaskRunnedEventArgs : TaskRunningEventArgs {
        public TaskRunnedEventArgs(TaskInfo taskInfo, TaskStatus taskStatus, TimeSpan elapsed)
            : base(taskInfo) {
            TaskStatus = taskStatus;
            Elapsed = elapsed;
        }

        public TaskStatus TaskStatus { get; }

        public TimeSpan Elapsed { get; }
    }
}