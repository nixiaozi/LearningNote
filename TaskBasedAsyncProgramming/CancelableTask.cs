using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public class CancelableTask<T>
    {
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken canceltoken;
        private Action<T, CancellationToken> action;

        private Action<CancellationToken> cancelAction = (ct) =>
        {
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }
        };

        public T TaskDetail;

        public Task TheTask;

        public CancelableTask(T t,Action<T, CancellationToken> _action)
        {
            cancelTokenSource = new CancellationTokenSource();
            canceltoken = cancelTokenSource.Token; // 初始化token
            action = _action;

            TaskDetail = t;

            // 定义一个发起取消时的回调
            canceltoken.Register(() =>
            {
                Console.WriteLine("Print To tell cancelled！");
            });
        }

        public void DoTask()
        {
            Console.WriteLine("开始生成并执行任务。。。");
            TheTask = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "ThisName";
                action.Invoke(default(T), canceltoken);
            },canceltoken);

            //try
            //{
            //    task.Wait();
            //}
            //catch(AggregateException e)
            //{
            //    foreach (var v in e.InnerExceptions)
            //    {
            //        Console.WriteLine(e.Message + " " + v.Message);
            //    }
            //}
            //finally
            //{
            //    cancelTokenSource.Dispose();
            //}

        }

        public void CancelTask()
        {
            Console.WriteLine("准备取消任务。。。");
            cancelTokenSource.CancelAfter(3000);
        }

    }
}
