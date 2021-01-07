using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class AttachDetachedChildTask
    {
        //默认情况下子任务是不会自动附加到主任务的
        public static void DefaultDetachedTask()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Outer task executing.");

                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Nested task starting.");
                    Thread.SpinWait(500000);
                    Console.WriteLine("Nested task completing.");
                });
            });

            parent.Wait();
            Console.WriteLine("Outer has completed.");
        }

        public static void ChildAttchToParentByResult()
        {
            var outer = Task<int>.Factory.StartNew(() => {
                Console.WriteLine("Outer task executing.");
                var nested = Task<int>.Factory.StartNew(() => {
                    Console.WriteLine("Nested task starting.");
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Nested task completing.");
                    return 42;
                });
                // Parent will wait for this detached child.
                return nested.Result; //由于主任务需要子任务的结果，所以必须先等待主任务执行完毕
            });
            Console.WriteLine("Outer has returned {0}.", outer.Result);
        }

        public static void ChildAttchToParentByOption()
        {
            //parent可以通过TaskCreationOptions.DenyChildAttach 选项
            //阻止子任务的附加，具有更高的优先级
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task executing.");
                var child = Task.Factory.StartNew(() => {
                    Console.WriteLine("Attached child starting.");
                    Thread.SpinWait(5000000);
                    Console.WriteLine("Attached child completing.");
                }, TaskCreationOptions.AttachedToParent);
            });
            parent.Wait();
            Console.WriteLine("Parent has completed.");
        }

        public static void TaskCancellation()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Factory.StartNew(() =>
            {
                //Wre we alread canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    Console.WriteLine("循环内还没有发起取消任务");
                    //Poll on this property if you have to do 
                    //other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        //Clean up hrer,then...
                        ct.ThrowIfCancellationRequested(); //抛出 RequestCancelled 异常，强行中断循环
                    }
                }
            }, tokenSource2.Token);

            //可以使用一个新的任务控制CancellationToken
            var cancelTask = Task.Run(() =>
            {
                Thread.SpinWait(20000);
                tokenSource2.Cancel();
            });
            //tokenSource2.Cancel(); 这样会立即执行

            //Just continue on this thread, or Wait/WaitAll with try-catch;
            try
            {
                task.Wait();
                //tokenSource2.Cancel(); 由于前面一直在task.Wait,此代码不会执行
            }
            catch(AggregateException e)
            {
                foreach(var v in e.InnerExceptions)
                {
                    Console.WriteLine(e.Message + " " + v.Message);
                }
            }
            finally
            {
                tokenSource2.Dispose();
            }


        }

    }
}
