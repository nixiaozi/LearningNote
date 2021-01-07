using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseTaskExample.MainAction();

            try
            {

                AttachDetachedChildTask.TaskCancellation();
            }
            catch(Exception ex)
            {

            }

            var obj = new CancelableTask<string>("The demo T is string!",(str, ct) =>
              {
                  while (true)
                  {
                      Thread.Sleep(1000);
                      Console.WriteLine("我想你了,当前线程为名为:"+ Thread.CurrentThread.Name);
                      if (ct.IsCancellationRequested)
                      {
                          ct.ThrowIfCancellationRequested();
                      }
                  }
              });

            obj.DoTask();
            obj.CancelTask();

            try
            {
                obj.TheTask.Wait();
                var taskException = obj.TheTask.Exception; // 或取当前任务执行生成的异常，所以可以不用等待而获得异常
            }
            catch (AggregateException e)
            {
                foreach (var v in e.InnerExceptions)
                {
                    Console.WriteLine(e.Message + " " + v.Message);
                }
            }

            Console.WriteLine("按任意键退出……");
            Console.ReadKey();
        }
    }
}
