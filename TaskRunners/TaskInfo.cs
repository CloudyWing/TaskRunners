using System;
using System.Collections.Generic;

namespace CloudyWing.TaskRunners {
    public struct TaskInfo {
        public TaskInfo(string name) : this() => (Id, Name) = (Guid.NewGuid(), name);

        public static bool operator ==(TaskInfo left, TaskInfo right) {
            return left.Equals(right);
        }

        public static bool operator !=(TaskInfo left, TaskInfo right) {
            return !(left == right);
        }

        public Guid Id { get; }

        public string Name { get; }

        public override bool Equals(object obj) => obj is TaskInfo info && Id.Equals(info.Id) && Name == info.Name;

        public override int GetHashCode() {
            var hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
