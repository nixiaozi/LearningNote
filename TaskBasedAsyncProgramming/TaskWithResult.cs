using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBasedAsyncProgramming
{
    public  class TaskWithResult
    {
        public static string ReturnStrAction(CancellationToken ct)
        {
            bool moreToDo = true;
            int whileTime = 0;
            while (moreToDo&&whileTime<10)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Do ReturnStrAction");

                if (ct.IsCancellationRequested)
                {
                    return "Get The Error";
                    // ct.ThrowIfCancellationRequested();
                }

                whileTime++;
            }

            return "Hi, you got it!";
        }


    }
}
