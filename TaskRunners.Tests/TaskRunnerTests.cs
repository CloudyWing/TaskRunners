using CloudyWing.TaskRunners;
using FluentAssertions;
using NUnit.Framework;

namespace TaskRunners.Tests {
    public class TaskRunnerTests {
        [Test]
        public void Execute_Timing() {
            int expected = 3;
            int actual = 0;
            TaskRunner runner = new TaskRunner("", () => actual = expected);
            actual.Should().Equals(0);

            runner.Run();

            actual.Should().Equals(expected);
        }
    }
}