using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class ChainingContinuationTask
    {
        public static DateTime DoWork()
        {
            //Simulate work by suspending the current thread
            //for two seconds.
            Thread.Sleep(2000);

            //Return the current time.
            return DateTime.Now;
        }
        private static void Elapsed(object state)
        {
            CancellationTokenSource cts = state as CancellationTokenSource;
            if (cts == null) return;

            cts.Cancel();
            Console.WriteLine("\nCancellation request issued...\n");
        }

        public static void ContinuationSingleAntecedcent()
        {
            //Execute the antecedent
            Task<DayOfWeek> taskA = Task.Run(() => DateTime.Today.DayOfWeek);

            //Execute the continuation when the antecedent finishes.
            Task taskB = taskA.ContinueWith(antecedent => Console.WriteLine("Today is {0}.", antecedent.Result));

            //taskA.Wait(); 这个只会等待taskA 执行完成
            taskB.Wait();
        }

        public static void ContinuationForMultipleAntecedents()
        {
            //Note that calls to the Task.WhenAll and Task.WhenAny overloads do not block the calling thread. However, you
            //typically call all but the Task.WhenAll(IEnumerable<Task>) and Task.WhenAll(Task[]) methods to retrieve the
            //returned Task< TResult >.Result property, which does block the calling thread. 
            //Task.WhenAll 与Task.WhenAny 不会自动阻塞主线程，只有在方法中调用Task.Result时才会阻塞主线程。
            List<Task<int>> tasks = new List<Task<int>>();
            for(int ctr = 1; ctr <= 10; ctr++)
            {
                int baseValue = ctr;
                tasks.Add(Task.Factory.StartNew((b) =>
                {
                    int i = (int)b;
                    return i * i;
                }, baseValue));
            }
            //var continuation = Task.WhenAny(tasks);
            //如果使用WhenAny,会出现什么情况？
            //WhenAny 只会放回单一结果,并非int数组
            var continuation = Task.WhenAll(tasks);

            long sum = 0;
            for(int ctr = 0; ctr <= continuation.Result.Length - 1; ctr++)
            {
                Console.Write("{0} {1}", continuation.Result[ctr], ctr == continuation.Result.Length - 1 ? "=" : "+");
                sum += continuation.Result[ctr];
            }
            Console.WriteLine(sum);

        }

        /// <summary>
        ///此方法有async标识，表示调用该方法的外部显示不会被该方法阻塞
        /// </summary>
        public async static void PassingDataContinuation()
        {
            var t = Task.Run(() =>
            {
                DateTime dat = DateTime.Now;
                if (dat == DateTime.MinValue)
                    throw new ArgumentException("The clock is not working.");

                if (dat.Hour > 17)
                    return "evening";
                else if (dat.Hour > 12)
                    return "afternoon";
                else
                    return "morning";
            });

            await t.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Good {0}!", antecedent.Result);
                Console.WriteLine("And how are you this fine {0}?", antecedent.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            //如果任务t被取消或者执行失败，那么读取Result属性会导致报错，
            //你可以使用TaskContinuationOptions.OnlyOnRanToCompletion 指定只有正常执行后才会继续来避免问题
        }

        /// <summary>
        /// 该方法会阻止调用该方法的外部线程
        /// </summary>
        public static void PassingDataContinuationWithWait()
        {
            var t = Task.Run(() =>
            {
                DateTime dat = DateTime.Now;
                if (dat == DateTime.MinValue)
                    throw new ArgumentException("The clock is not working.");

                if (dat.Hour > 17)
                    return "evening";
                else if (dat.Hour > 12)
                    return "afternoon";
                else
                    return "morning";
            });

            t.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Good {0}!", antecedent.Result);
                Console.WriteLine("And how are you this fine {0}?", antecedent.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion).Wait();
            //Wait()方法会在方法内部阻塞线程，知道任务执行完毕
        }

        public static void PassingDataContinuationWithStatusCheck()
        {
            var t = Task.Run(() =>
            {
                DateTime dat = DateTime.Now;
                if (dat == DateTime.MinValue)
                    throw new ArgumentException("The clock is not working.");
                if (dat.Hour > 17)
                    return "evening";
                else if (dat.Hour > 12)
                    return "afternoon";
                else
                    return "morning";
            });

            var c = t.ContinueWith((antecedent) =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Good {0}!",
                    antecedent.Result);
                    Console.WriteLine("And how are you this fine {0}?",
                    antecedent.Result);
                }
                else if (t.Status == TaskStatus.Faulted)
                {
                    Console.WriteLine(t.Exception.GetBaseException().Message);
                }
            });
        }

        public static void CancelingContinuationExp()
        {
            Random rnd = new Random();
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Timer timer = new Timer(Elapsed, cts, 5000, Timeout.Infinite);

            var t = Task.Run(() =>
            {
                List<int> product33 = new List<int>();
                for (int ctr = 1; ctr < Int16.MaxValue; ctr++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("\nCancellation requested in antecedent...\n");
                        token.ThrowIfCancellationRequested();
                    }
                    if (ctr % 2000 == 0)
                    {
                        int delay = rnd.Next(16, 501);
                        Thread.Sleep(delay);
                    }
                    if (ctr % 33 == 0)
                    {
                        product33.Add(ctr);
                    }
                }
                return product33.ToArray();
            }, token);

            Task continuation = t.ContinueWith(antecedent =>
            {
                Console.WriteLine("Multiples of 33:\n");
                var arr = antecedent.Result;
                for (int ctr = 0; ctr < arr.Length; ctr++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("\nCancellation requested in continuation...\n");
                        token.ThrowIfCancellationRequested();
                    }

                    if (ctr % 100 == 0)
                    {
                        int delay = rnd.Next(16, 251);
                        Thread.Sleep(delay);
                    }
                    Console.Write("{0:N0}{1}", arr[ctr], ctr != arr.Length - 1 ? "," : "");
                    if (Console.CursorLeft >= 74)
                        Console.WriteLine();
                }
                Console.WriteLine();
            }, token);

            try
            {
                continuation.Wait();
            }
            catch(AggregateException e)
            {
                foreach(Exception ie in e.InnerExceptions)
                {
                    Console.WriteLine("{0}: {1}", ie.GetType().Name,
                        ie.Message);
                }
            }
            finally
            {
                cts.Dispose();
            }
            Console.WriteLine("\nAntecedent Status: {0}", t.Status);
            Console.WriteLine("Continuation Status: {0}", continuation.Status);

        }

        public static void CancelContinuationFromAntecedent()
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();

            var t = Task.FromCanceled(token);
            var continuation = t.ContinueWith((antecedent) =>
            {
                Console.WriteLine("The continuation is running.");
            },TaskContinuationOptions.NotOnCanceled);
            //TaskContinuationOptions.NotOnCanceled  表示在父任务取消之后不会再执行子任务
            //如果没有这个表示，则会无条件的执行该子任务
            //A continuation does not run until the antecedent and all of its attached child tasks have completed.
            //一个后续的任务会等待所有他所依赖的父集合中的子任务全部完成之后才会执行

            try
            {
                t.Wait();
            }
            catch(AggregateException ae)
            {
                foreach(var ie in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:{1}", ie.GetType().Name, ie.Message);
                }
                Console.WriteLine();
            }
            finally
            {
                cts.Dispose();
            }
            Console.WriteLine("Task {0}: {1:G}", t.Id, t.Status);
            Console.WriteLine("Task {0}: {1:G}", continuation.Id,continuation.Status);
        }

        public static void ContinuationWithAntecedentChild()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running antecedent task {0}...", Task.CurrentId);
                Console.WriteLine("Launching attached child task...");
                for (int ctr = 1; ctr <= 5; ctr++)
                {
                    int index = ctr;
                    Task.Factory.StartNew((value) =>
                    {
                        Console.WriteLine("Attached child task #{0} running", value);
                        Thread.Sleep(1000);
                    }, index, TaskCreationOptions.AttachedToParent);
                    //TaskCreationOptions.AttachedToParent 表示这几个子任务会附加到主任务
                }
                Console.WriteLine("Finished launching attached child tasks...");
            });

            //antecedent 自动获得t的引用
            var continuation = t.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Executing continuation of Task {0}", antecedent.Id);
            });

            continuation.Wait();

        }

        public static void ContinuationWithAntDetachedChild()
        {
            var t = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Running antecedent task {0}...", Task.CurrentId);
                Console.WriteLine("Launching attached child task...");
                for (int ctr = 1; ctr <= 5; ctr++)
                {
                    int index = ctr;
                    Task.Factory.StartNew((value) =>
                    {
                        Console.WriteLine("Attached child task #{0} running", value);
                        Thread.Sleep(1000);
                    }, index);
                    //TaskCreationOptions.AttachedToParent 去掉后，不会自动附加到主任务
                }
                Console.WriteLine("Finished launching attached child tasks...");
            });

            //antecedent 自动获得t的引用
            var continuation = t.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Executing continuation of Task {0}", antecedent.Id);
            });

            continuation.Wait();
        }

        public static void AssociateStateWithContinuations()
        {
            //Start a root task that performs work.
            Task<DateTime> t = Task<DateTime>.Run(
                delegate {
                    //Task.Run 默认就会执行
                    Console.WriteLine("这里会默认执行的！");
                    return DoWork();
                });

            //Create a chain of continuation tasks,where each task is
            //followed by another task that performs work.
            List<Task<DateTime>> continuations = new List<Task<DateTime>>();

            for(int i = 0; i < 5; i++)
            {
                //Provide the current time as the state of the continuation.
                t = t.ContinueWith(delegate { return DoWork(); }, DateTime.Now); //这里的Datetime.Now标识当前的AsyncState
                continuations.Add(t);
            }

            //Wait for the last task in the chain to complete.
            t.Wait();

            //Print the creation time of each continuation(the state object)
            //and the completion time(the result of that task) to the console.
            foreach(var continuation in continuations)
            {
                DateTime start = (DateTime)continuation.AsyncState;
                DateTime end = continuation.Result;

                Console.WriteLine("Task was created at {0} and finished at {1}", start.TimeOfDay, end.TimeOfDay);
            }

        }

        public static void HandingExceptionsFromContinuations()
        {
            var task1 = Task<int>.Run(() =>
            {
                Console.WriteLine("Executing task {0}", Task.CurrentId);
                return 54;
            });

            var continuation = task1.ContinueWith((antecedent) =>
            {
                Console.WriteLine("Executing continuation task {0}", Task.CurrentId);
                Console.WriteLine("Value from antecedent:{0}", antecedent.Result);
                throw new InvalidOperationException();
            });

            try
            {
                task1.Wait();
                continuation.Wait();
            }
            catch(AggregateException ae)
            {
                foreach(var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        public static void HandingAntecedentExceptionsFromContinuations()
        {
            var t = Task.Run(() =>
            {
                string s = File.ReadAllText(@"C:\NonexistentFile.txt");
                return s;
            });

            var c = t.ContinueWith((antecedent) =>
            {
                //Get the antecedent's exception information.
                foreach (var ex in antecedent.Exception.InnerExceptions)
                {
                    if (ex is FileNotFoundException)
                        Console.WriteLine(ex.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            c.Wait();
        }
    }
}
