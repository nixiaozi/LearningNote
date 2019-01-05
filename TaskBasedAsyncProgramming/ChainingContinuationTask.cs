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


        }

    }
}
