using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace CloudyWing.TaskRunners.Tests {
    [TestFixture]
    public class BulkTaskRunnerTests {
        private BulkTaskRunner runners;

        [SetUp]
        public void Setup() {
            runners = new BulkTaskRunner("Test");
        }

        [Test]
        public void GuidIndexer_IsExisting_取得TaskRunner() {
            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });
            runners.AddRange(runner1, runner2);

            ITaskRunner runner3 = runners[runner1.TaskInfo.Id];
            ITaskRunner runner4 = runners[runner2.TaskInfo.Id];

            runner3.Should().Be(runner1);
            runner4.Should().Be(runner2);
        }

        [Test]
        public void GuidIndexer_IsNotExisting_ReturnNull() {
            runners.AddRange(new TaskRunner("Test1", () => { }));
            ITaskRunner taskRunner = runners[Guid.Empty];

            taskRunner.Should().BeNull();
        }

        [Test]
        public void Contains_IsTrue() {
            TaskRunner runner = new TaskRunner("Test", () => { });
            runners.Add(runner);
            runners.Contains(runner);

            runners.Contains(runner).Should().BeTrue();
        }

        [Test]
        public void Add_AddDuplicates_ThrowArgumentException() {
            TaskRunner runner = new TaskRunner("Test", () => { });
            runners.Add(runner);
            Action act = () => runners.Add(runner);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void AddRangeTest_ArgsIsArray() {
            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });

            runners.AddRange(runner1, runner2);

            runners.Count.Should().Be(2);
            runners.Contains(runner1).Should().BeTrue();
            runners.Contains(runner2).Should().BeTrue();
        }

        [Test]
        public void AddRangeTest_ArgsIsEnumerable() {
            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });
            List<TaskRunner> list = new List<TaskRunner> {
                runner1,
                runner2
            };

            runners.AddRange(list);

            runners.Count.Should().Be(2);
            runners.Contains(runner1).Should().BeTrue();
            runners.Contains(runner2).Should().BeTrue();
        }

        [Test]
        public void AddBulk_ArgsIsEnumerable() {
            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });
            List<TaskRunner> list = new List<TaskRunner> {
                runner1,
                runner2
            };

            runners.AddBulk("SubBulk", true, list);

            BulkTaskRunner subBulk = runners[0] as BulkTaskRunner;
            subBulk.Count.Should().Be(2);
            subBulk.Contains(runner1).Should().BeTrue();
            subBulk.Contains(runner2).Should().BeTrue();
        }

        [Test]
        public void AddRunningEventToRunners() {
            List<string> taskNames = new List<string>();

            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });

            runners.AddRange(runner1, runner2);
            runners.AddRunningEventToRunners(x => taskNames.Add(x.TaskInfo.Name));
            runners.Run();

            taskNames.Should().Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.HaveCount(2);
        }

        [Test]
        public void AddRunnedEventToRunners() {
            List<string> taskNames = new List<string>();

            TaskRunner runner1 = new TaskRunner("Test1", () => { });
            TaskRunner runner2 = new TaskRunner("Test2", () => { });

            runners.AddRange(runner1, runner2);
            runners.AddRunnedEventToRunners(x => taskNames.Add(x.TaskInfo.Name));
            runners.Run();

            taskNames.Should()
                .Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.HaveCount(2);
        }

        [Test]
        public void Run_IsStoppedIfFaiIsTrue_CantExecuteAll() {
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

            runners.AddRange(runner1, runner2, runner3);

            Action actual = () => runners.Run();

            actual.Should().NotThrow();

            taskNames.Should()
                .Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.Contain(runner3.TaskInfo.Name)
                .And.HaveCount(3);
        }

        [Test]
        public void Run_IsStoppedIfFailIsFalse_CantExecuteAll() {
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

            BulkTaskRunner runners = new BulkTaskRunner("BulkTest", true);
            runners.AddRange(runner1, runner2, runner3);

            Action actual = () => runners.Run();

            actual.Should().Throw<Exception>()
                .WithMessage("Test2");

            taskNames.Should()
                .Contain(runner1.TaskInfo.Name)
                .And.Contain(runner2.TaskInfo.Name)
                .And.NotContain(runner3.TaskInfo.Name)
                .And.HaveCount(2);
        }
    }
}