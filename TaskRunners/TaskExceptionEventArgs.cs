using System;

namespace CloudyWing.TaskRunners {
    public class TaskExceptionEventArgs : TaskRunningEventArgs {
        public TaskExceptionEventArgs(TaskInfo taskInfo, Exception exception, TimeSpan elapsed) : base(taskInfo) {
            Exception = exception;
            Elapsed = elapsed;
        }

        public Exception Exception { get; }

        public TimeSpan Elapsed { get; }
    }
}