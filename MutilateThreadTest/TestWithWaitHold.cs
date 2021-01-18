using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MutilateThreadTest
{
    public static class TestWithWaitHold
    {
        public static void HoldWait()
        {
            Console.WriteLine("HoldWait==>1");
            Thread.Sleep(5000);
            var str =  MustHoldWait();
            Console.WriteLine("HoldWait==>2");

        }


        public static string MustHoldWait()
        {
            Console.WriteLine("Begin MustHold Wait");
            Thread.Sleep(7000);
            Console.WriteLine("MustHoldWait===>1");

            return "";
        }


    }
}
