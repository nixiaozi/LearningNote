using System;
using System.Threading;

namespace GoldbachTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // PrimeData.PrintAllPrimes(1000000000);

            // PrimeData.TenCreateHundred();

            //PrimeData.PrintRangePrimes();

            //PrimeData.GetEvenToTwoPrimesSum(14);
            var i = 3;
            for(; ; )
            {
                Console.WriteLine("现在测试数字"+i+":");
                
                Thread.Sleep(2000);
                PrimeData.GetOddSymmetryPrimesPair(i);
                Console.WriteLine();
                i = i + 2;
            }

            // PrimeData.GetOddSymmetryPrimesPair(19);

        }
    }
}
