using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class BaseTaskExample
    {
        public static void MainAction()
        {
            Thread.CurrentThread.Name = "Main";
            // Create a task and supply a user delegate by using a lambda expression.
            Task taskA = new Task(() => Console.WriteLine("Hello from taskA."));
            // Start the task.
            taskA.Start();
            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread '{0}'.",
            Thread.CurrentThread.Name);

            //强制等待taskA执行完成之后，才执行下面的操作
            taskA.Wait();
        }

        public static void TaskRunAction()
        {
            Thread.CurrentThread.Name = "Main";
            // Define and run the task.
            Task taskA = Task.Run(() => Console.WriteLine("Hello from taskA."));
            // run会立刻开始执行任务，不能重复启动Start.
            // taskA.Start();

            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread '{0}'.",
            Thread.CurrentThread.Name);
            taskA.Wait();
        }

        public static void TaskFactoryStartAction()
        {
            Task[] taskArray = new Task[10];
            for(int i=0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(
                    (Object obj) =>
                    {
                        CustomData data = obj as CustomData;
                        if (data == null)
                            return;

                        data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    },
                    new CustomData() { Name=i,CreationTime=DateTime.Now.Ticks}
                );
            }
            Task.WaitAll(taskArray);
            foreach(var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if (data != null)
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                        data.Name, data.CreationTime, data.ThreadNum);
            }
        }

    }
    class CustomData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
    }
}
