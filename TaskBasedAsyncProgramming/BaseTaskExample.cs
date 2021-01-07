using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class BaseTaskExample
    {
        private static Double DoComputation(Double start)
        {
            Double sum = 0;
            for (var value = start; value <= start + 10; value += .1)
                sum += value;
            return sum;
        }
        public static void MainAction()
        {
            Thread.CurrentThread.Name = "Main";
            // Create a task and supply a user delegate by using a lambda expression.
            Task taskA = new Task(() => {
                    Thread.CurrentThread.Name = "TaskA";
                    Console.WriteLine("Hello from {0}.线程ID为：{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
                });
            // Start the task.
            taskA.Start();
            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread '{0}'.with ThreadID is:{1}",
            Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);

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

        public static void ComputeMutliTaskResultAction()
        {
            Task<Double>[] taskArray =
            {
                Task<Double>.Factory.StartNew(()=>DoComputation(1.0)),
                Task<Double>.Factory.StartNew(()=>DoComputation(100.0)),
                Task<Double>.Factory.StartNew(()=>DoComputation(1000.0)),
            };

            var result = new Double[taskArray.Length];
            Double sum = 0;

            //测试Console是否会被Task获取Result阻止
            Console.WriteLine("第3个任务的执行结果为：" + taskArray[2].Result);
            Console.WriteLine("直接执行到这里，不等待任务3结果"); //taskArray[2].Result 需要获取异步任务结果，会始终阻塞，直到获取结果


            for (int i = 0; i < taskArray.Length; i++)
            {
                result[i] = taskArray[i].Result;
                Console.Write("{0:N1} {1}", result[i], i == taskArray.Length - 1 ? "=" : "+");
                sum += result[i];
            }

            Console.WriteLine("{0:N1}",sum);
            Console.WriteLine("测试显示顺序");
        }

        public static void LambdaCaptureOutExpectedExp()
        {
            Task[] taskArray = new Task[10];
            for(int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                  {
                      var data = new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks };
                      data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                      Console.WriteLine("Task #{0} created at {1} on thread #{2}.", data.Name, data.CreationTime, data.ThreadNum);
                  }, i);
            }
            //由于下面没有阻塞性代码，方法外的代码也可以被执行到
            Console.WriteLine("由于下面没有阻塞性代码，方法外的代码也可以异步执行");
            taskArray[2].Wait(); //阻塞直到第三个Task执行完成
            //如果阻塞其中的一个Task，
            Console.WriteLine("获取第四个Task的返回值：" );
            taskArray[3].Wait();
        }

        public static void LabdaCaptureInExpectedExp()
        {
            Task[] taskArray = new Task[10];
            for(int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    CustomData data = obj as CustomData;
                    if (data == null)
                        return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                        data.Name, data.CreationTime, data.ThreadNum);
                }, new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }

        }

        public static void UseAsyncStateForTask()
        {
            Task[] taskArray = new Task[10];
            for(int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    CustomData data = obj as CustomData;
                    if (data == null)
                        return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                }, new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            // Task.WaitAll 这段代码可以不加因为在下面的for循环中 会自动阻塞线程
            //Task.WaitAll(taskArray);
            foreach(var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if(data!=null)
                    Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                        data.Name, data.CreationTime, data.ThreadNum);
            }
        }

        public static void TaskCultureCustomization()
        {
            decimal[] values = { 163025412.32m, 18905365.59m };
            string formatString = "C2";
            Func<string> formatDelegate = () =>
            {
                string output = string.Format("Formatting using the {0} culture on thread {1}. \n",
                    CultureInfo.CurrentCulture.Name, Thread.CurrentThread.ManagedThreadId);
                foreach (var value in values)
                    output += String.Format("{0}  ", value.ToString(formatString));

                output += Environment.NewLine;
                return output;
            };

            Console.WriteLine("The example is running on thread {0}", Thread.CurrentThread.ManagedThreadId);

            //Make the current cluture different from the system culture.
            Console.WriteLine("The current culture is {0}", CultureInfo.CurrentCulture.Name);

            if (CultureInfo.CurrentCulture.Name == "fr-FR")
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            else
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            Console.WriteLine("Changed the current cluture to {0}.\n", CultureInfo.CurrentCulture.Name);

            //Execute the delegate synchronously.
            Console.WriteLine("Executing the delegate synchronously:");
            Console.WriteLine(formatDelegate());

            //Call an async delegate to format the values using one format string.
            Console.WriteLine("Executing a task asynchronously:");
            var t1 = Task.Run(formatDelegate);
            Console.WriteLine(t1.Result);


            Console.WriteLine("Executing a task synchronously:");
            var t2 = new Task<string>(formatDelegate);
            t2.RunSynchronously();
            Console.WriteLine(t2.Result);

            // 这个实例说明：task所使用的culture只与所在的线程相关；新的线程会重新使用app默认的culture

        }

        public static void ContinuationsTaskExp()
        {
            var getData = Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                int[] values = new int[100];
                for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
                    values[ctr] = rnd.Next();

                return values;
            });
            var processData = getData.ContinueWith((x) =>
            {
                int n = x.Result.Length;
                long sum = 0;
                double mean;

                for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
                    sum += x.Result[ctr];

                mean = sum / (double)n;
                return Tuple.Create(n, sum, mean);
            });
            var displayData = processData.ContinueWith((x) =>
            {
                return String.Format("N={0:N0},Total={1:N0},mean={2:N2}", x.Result.Item1, x.Result.Item2, x.Result.Item3);
            });
            Console.WriteLine(displayData.Result);
        }

        public static void ChainTypeContinuationsTask()
        {
            //The ContinueWhenAll and ContinueWhenAny methods enable you to continue from multiple tasks.
            var displayData = Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                int[] values = new int[100];
                for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
                    values[ctr] = rnd.Next();
                return values;
            })
            .ContinueWith((x) =>
            {
                int n = x.Result.Length;
                long sum = 0;
                double mean;
                for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
                    sum += x.Result[ctr];
                mean = sum / (double)n;
                return Tuple.Create(n, sum, mean);
            })
            .ContinueWith((x) =>
            {
                return String.Format("N={0:N0}, Total = {1:N0}, Mean = {2:N2}",
                    x.Result.Item1, x.Result.Item2,
                    x.Result.Item3);
            });
            Console.WriteLine(displayData.Result);
        }

        public static void DetachedChildTasks()
        {
            var outer = Task.Factory.StartNew(() => {
                Console.WriteLine("Outer task beginning.");
                //父任务只会掉起该子任务，并继续向下执行，不会等到子任务完成
                var child = Task.Factory.StartNew(() =>
                {
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Detached task completed.");
                });
            });
            // outer.RunSynchronously(); 不能对已开始的任务调用 RunSynchronously
            outer.Wait();
            Console.WriteLine("Outer task completed.");
        }

        public static void AttachedToParentTasks()
        {
            //A parent task can use the TaskCreationOptions.DenyChildAttach option to prevent other tasks from attaching to
            //the parent task.For more information, see Attached and Detached Child Tasks.
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task beginning.");
                for (int ctr = 0; ctr < 10; ctr++)
                {
                    int taskNo = ctr;
                    Task.Factory.StartNew((x) => {
                        Thread.SpinWait(5000000);
                        Console.WriteLine("Attached child #{0} completed.",
                        x);
                    },
                    taskNo, TaskCreationOptions.AttachedToParent);
                }
            });
            parent.Wait();
            Console.WriteLine("Parent task completed.");
        }

    }
    class CustomData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
    }
}
