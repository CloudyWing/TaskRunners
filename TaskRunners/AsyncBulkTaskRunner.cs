using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudyWing.TaskRunners {
    public sealed class AsyncBulkTaskRunner : BulkTaskRunner {
        private readonly bool ignoreException;

        public AsyncBulkTaskRunner(string taskName, bool ignoreException = false)
            : base(taskName, false) {
            this.ignoreException = ignoreException;
        }

        public AsyncBulkTaskRunner(string taskName, bool ignoreException, params ITaskRunner[] items)
            : base(taskName, false, items) {
            this.ignoreException = ignoreException;
        }

        public AsyncBulkTaskRunner(string taskName, bool ignoreException, IEnumerable<ITaskRunner> items)
            : base(taskName, false, items) {
            this.ignoreException = ignoreException;
        }

        protected override void RunTask() {
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(Runners, runner => {
                try {
                    runner.Run();
                } catch (Exception e) {
                    OnExceptionThrown(e);
                    exceptions.Enqueue(e);
                }
            });

            if (!ignoreException && exceptions.Count > 0) {
                throw new AggregateException(exceptions);
            }
        }
    }
}