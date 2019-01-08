using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class ChainingContinuationTask
    {
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

    }
}
