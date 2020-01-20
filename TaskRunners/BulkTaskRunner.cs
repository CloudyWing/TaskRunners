using System;
using System.Collections.Generic;

namespace CloudyWing.TaskRunners {
    public class BulkTaskRunner : TaskRunnerBase {
        private readonly List<ITaskRunnable> runners = new List<ITaskRunnable>();
        private readonly TaskInfo taskInfo;

        public event EventHandler<TaskExceptionEventArgs> ExceptionThrownEvent;

        public BulkTaskRunner(string taskName, bool isStoppedIfFail = false) {
            if (taskName is null) {
                throw new ArgumentNullException(nameof(taskName));
            }

            taskInfo = new TaskInfo(taskName);
            IsStoppedIfFail = isStoppedIfFail;
        }

        public BulkTaskRunner(string taskName, bool isStoppedIfFail, IEnumerable<ITaskRunnable> items) : this(taskName, isStoppedIfFail) {
            AddRange(items);
        }

        public BulkTaskRunner(string taskName, bool isStoppedIfFail, params ITaskRunnable[] items) : this(taskName, isStoppedIfFail) {
            AddRange(items);
        }

        public override TaskInfo TaskInfo => taskInfo;

        public bool IsStoppedIfFail { get; }

        public int Count => runners.Count;

        protected IList<ITaskRunnable> Runners => runners;

        public ITaskRunnable this[int index] { get => runners[index]; set => runners[index] = value; }

        protected override void RunTask() {
            foreach (ITaskRunnable runner in runners) {
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

        public bool Contains(ITaskRunnable item) => runners.Contains(item);

        public int IndexOf(ITaskRunnable item) => runners.IndexOf(item);

        public void Add(ITaskRunnable item) => runners.Add(item);

        public void Insert(int index, ITaskRunnable item) => runners.Insert(index, item);

        public void AddRange(params ITaskRunnable[] items) => AddRange(items as IEnumerable<ITaskRunnable>);

        public void AddRange(IEnumerable<ITaskRunnable> items) => runners.AddRange(items);

        public void CopyTo(ITaskRunnable[] array, int arrayIndex) => runners.CopyTo(array, arrayIndex);

        public bool Remove(ITaskRunnable item) => runners.Remove(item);

        public void RemoveAt(int index) => runners.RemoveAt(index);

        public void Clear() => runners.Clear();

        public void AddBulk(string taskName, bool isStoppedIfFail, params ITaskRunnable[] items)
            => runners.Add(new BulkTaskRunner(taskName, isStoppedIfFail, items));

        public void AddBulk(string taskName, bool isStoppedIfFail, IEnumerable<ITaskRunnable> items)
            => runners.Add(new BulkTaskRunner(taskName, isStoppedIfFail, items));

        public void AddRunningEventToRunners(Action<TaskRunningEventArgs> action) {
            foreach (ITaskRunnable item in runners) {
                item.TaskRunningEvent += (o, e) => action(e);
            }
        }

        public void AddRunnedEventToRunners(Action<TaskRunnedEventArgs> action) {
            foreach (ITaskRunnable item in runners) {
                item.TaskRunnedEvent += (o, e) => action(e);
            }
        }
    }
}