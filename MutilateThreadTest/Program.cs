using System;

namespace MutilateThreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestWithWaitHold.HoldWait();

            Console.ReadKey();
        }
    }
}
