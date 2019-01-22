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

    }
}
