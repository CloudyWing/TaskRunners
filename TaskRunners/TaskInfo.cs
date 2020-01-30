using System;

namespace CloudyWing.TaskRunners {
    /// <summary>
    /// The task info. Use only Id to determine if they are equal.
    /// </summary>
    /// <seealso cref="IEquatable{TaskInfo}" />
    public struct TaskInfo : IEquatable<TaskInfo> {
        public TaskInfo(string name) : this() {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static bool operator ==(TaskInfo left, TaskInfo right) {
            return left.Equals(right);
        }

        public static bool operator !=(TaskInfo left, TaskInfo right) {
            return !(left == right);
        }

        public Guid Id { get; }

        public string Name { get; }

        public override bool Equals(object obj) => obj is TaskInfo info && Equals(info);

        public bool Equals(TaskInfo other) => Id.Equals(other.Id);

        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
