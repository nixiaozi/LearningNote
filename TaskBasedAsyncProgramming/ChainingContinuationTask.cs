using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class ChainingContinuationTask
    {
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



    }
}
