using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace CloudyWing.TaskRunners.Tests {
    [TestFixture]
    public class AsyncBulkTaskRunnerTests {
        [Test]
        public void Run_NotIgnoreException_HasException() {
            List<string> taskNames = new List<string>();
            TaskRunner runner1 = new TaskRunner("Test1", () => {
                taskNames.Add("Test1");
            });
            TaskRunner runner2 = new TaskRunner("Test2", () => {
                taskNames.Add("Test2");
                throw new Exception("Test2");
            });
            TaskRunner runner3 = new TaskRunner("Test3", () => {
                taskNames.Add("Test3");
            });

            AsyncBulkTaskRunner runners = new AsyncBulkTaskRunner("TestBulk", false, runner1, runner2, runner3);

            Action actual = () => runners.Run();

            actual.Should().Throw<AggregateException>();

            taskNames.Should()
                .Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.Contain(runner3.TaskInfo.Name)
                .And.HaveCount(3);
        }

        [Test]
        public void Run_NotIgnoreException_WithoutException() {
            List<string> taskNames = new List<string>();
            TaskRunner runner1 = new TaskRunner("Test1", () => {
                taskNames.Add("Test1");
            });
            TaskRunner runner2 = new TaskRunner("Test2", () => {
                taskNames.Add("Test2");
                throw new Exception("Test2");
            });
            TaskRunner runner3 = new TaskRunner("Test3", () => {
                taskNames.Add("Test3");
            });

            AsyncBulkTaskRunner runners = new AsyncBulkTaskRunner("TestBulk", true, runner1, runner2, runner3);

            Action actual = () => runners.Run();

            actual.Should().NotThrow();

            taskNames.Should()
                .Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.Contain(runner3.TaskInfo.Name)
                .And.HaveCount(3);
        }
    }
}