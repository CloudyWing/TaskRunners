using System;

namespace CloudyWing.TaskRunners {
    public class ExceptionEventArgs : EventArgs {
        public ExceptionEventArgs(Exception exception) {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}