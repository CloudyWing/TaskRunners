using System;

namespace CloudyWing.TaskRunners {
    public class TaskRunningEventArgs : EventArgs {
        public TaskRunningEventArgs(TaskInfo taskInfo) {
            TaskInfo = taskInfo;
        }

        public TaskInfo TaskInfo { get; }
    }
}