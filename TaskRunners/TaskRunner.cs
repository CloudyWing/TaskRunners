using System;

namespace CloudyWing.TaskRunners {
    public class TaskRunner : TaskRunnerBase {
        private readonly TaskInfo taskInfo;
        private readonly Action taskAction;

        public TaskRunner(string taskName, Action taskAction) {
            taskInfo = new TaskInfo(taskName);
            this.taskAction = taskAction ?? throw new ArgumentNullException(nameof(taskAction));
        }

        public override TaskInfo TaskInfo => taskInfo;

        protected override void RunTask() => taskAction();
    }
}
