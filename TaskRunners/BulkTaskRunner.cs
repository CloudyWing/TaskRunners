using System;
using System.Collections.Generic;

namespace CloudyWing.TaskRunners {
    public class BulkTaskRunner : TaskRunnerBase {
        private readonly List<ITaskRunner> runners = new List<ITaskRunner>();
        private readonly TaskInfo taskInfo;

        public event EventHandler<TaskExceptionEventArgs> ExceptionThrownEvent;

        public BulkTaskRunner(string taskName, bool isStoppedIfFail = false) {
            if (taskName is null) {
                throw new ArgumentNullException(nameof(taskName));
            }

            taskInfo = new TaskInfo(taskName);
            IsStoppedIfFail = isStoppedIfFail;
        }

        public BulkTaskRunner(string taskName, bool isStoppedIfFail, IEnumerable<ITaskRunner> items) : this(taskName, isStoppedIfFail) {
            AddRange(items);
        }

        public BulkTaskRunner(string taskName, bool isStoppedIfFail, params ITaskRunner[] items) : this(taskName, isStoppedIfFail) {
            AddRange(items);
        }

        public override TaskInfo TaskInfo => taskInfo;

        public bool IsStoppedIfFail { get; }

        public int Count => runners.Count;

        protected IList<ITaskRunner> Runners => runners;

        public ITaskRunner this[int index] { get => runners[index]; set => runners[index] = value; }

        protected override void RunTask() {
            foreach (ITaskRunner runner in runners) {
                try {
                    runner.Run();
                } catch (Exception ex) {
                    OnExceptionThrown(ex);

                    if (IsStoppedIfFail) {
                        throw;
                    }
                }
            }
        }

        protected virtual void OnExceptionThrown(Exception exception) {
            ExceptionThrownEvent?.Invoke(this, new TaskExceptionEventArgs(TaskInfo, exception, Elapsed));
        }

        public bool Contains(ITaskRunner item) => runners.Contains(item);

        public int IndexOf(ITaskRunner item) => runners.IndexOf(item);

        public void Add(ITaskRunner item) => runners.Add(item);

        public void Insert(int index, ITaskRunner item) => runners.Insert(index, item);

        public void AddRange(params ITaskRunner[] items) => AddRange(items as IEnumerable<ITaskRunner>);

        public void AddRange(IEnumerable<ITaskRunner> items) => runners.AddRange(items);

        public void CopyTo(ITaskRunner[] array, int arrayIndex) => runners.CopyTo(array, arrayIndex);

        public bool Remove(ITaskRunner item) => runners.Remove(item);

        public void RemoveAt(int index) => runners.RemoveAt(index);

        public void Clear() => runners.Clear();

        public void AddBulk(string taskName, bool isStoppedIfFail, params ITaskRunner[] items)
            => runners.Add(new BulkTaskRunner(taskName, isStoppedIfFail, items));

        public void AddBulk(string taskName, bool isStoppedIfFail, IEnumerable<ITaskRunner> items)
            => runners.Add(new BulkTaskRunner(taskName, isStoppedIfFail, items));

        public void AddRunningEventToRunners(Action<TaskRunningEventArgs> action) {
            foreach (ITaskRunner item in runners) {
                item.TaskRunningEvent += (o, e) => action(e);
            }
        }

        public void AddRunnedEventToRunners(Action<TaskRunnedEventArgs> action) {
            foreach (ITaskRunner item in runners) {
                item.TaskRunnedEvent += (o, e) => action(e);
            }
        }
    }
}