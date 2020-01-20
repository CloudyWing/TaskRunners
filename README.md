# TaskRunners

## 範例

isStoppedIfFail = false時
```
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

// Test2 throw的Exeption會被忽略，然後觸發OnExceptionThrown()事件
// 所以taskNames 包含Test1、Test2和Test3
runners.Run();
```

isStoppedIfFail = true時
```
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

// 因為Test2 throw Exception，所以taskNames 包含Test1和Test2，不包含Test3
runners.Run();
```

## License
This project is MIT [licensed](https://github.com/CloudyWing/TaskRunners/blob/master/LICENSE.md).