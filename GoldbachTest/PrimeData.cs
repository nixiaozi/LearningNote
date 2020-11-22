using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldbachTest
{
    public class PrimeData
    {
        public static void PrintAllPrimes(Int64 prime=2)
        {
            for (; ; )
            {
                var flag = false;
                for (var i = 2; i <= prime / 2; i++)
                {
                    if (prime % i == 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    Console.WriteLine(prime);

                prime++;

            }

        }

        public static void PrintRangePrimes(int Start=2,int TheEnd=10000)
        {
            if(Start<2 || Start >= TheEnd)
            {
                throw new Exception("参数配置错误：开始值需要大于等于2，结束值需要大于开始值");
            }

            Console.WriteLine(String.Format("开始输出从{0}到{1}范围内的质数：", Start, TheEnd));
            for (; ; )
            {
                var flag = false;
                for (var i = 2; i <= Start / 2; i++)
                {
                    if (Start % i == 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    Console.WriteLine(Start);

                Start++;

                if (Start > TheEnd) // 超过范围就退出
                    return;
            }


        }

        public static List<int> GetNumRangePrimes(int Number)
        {
            if (Number <= 2)
            {
                throw new Exception("参数配置错误：开始值需要大于等于2，结束值需要大于开始值");
            }
            var Start = 2;
            List<int> result = new List<int>();

            for (; ; )
            {
                var flag = false;
                for (var i = 2; i <= Start / 2; i++)
                {
                    if (Start % i == 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    result.Add(Start);

                Start++;

                if (Start > Number) // 超过范围就退出
                    return result;
            }


        }



        /// <summary>
        /// 测试给你1-10 这10个数你能通过相乘后得到结果，最后得到多个100 及100以内的数
        /// </summary>
        public static void TenCreateHundred()
        {
            var MaxNumber = 100;
            HashSet<int> OrigionData = new HashSet<int>() { 2, 3, 4, 5, 6, 7, 8, 9 ,10};

            var count = OrigionData.Count; // 获取初始的条目数
            do
            {
                count = OrigionData.Count;
                var CacheSet = OrigionData.ToHashSet();
                // List<int> CacheList = new List<int>();
                foreach (var i in CacheSet)
                {
                    foreach(var j in CacheSet)
                    {
                        if (i * j <= MaxNumber)
                            // CacheList.Add(i * j);
                            OrigionData.Add(i*j);
                    }
                }
                // OrigionData.Add(CacheList);
                OrigionData = OrigionData.ToHashSet();

            }
            while (count<OrigionData.Count);

            OrigionData.Add(1); // 添加默认1

            Console.WriteLine("1-9所有的相乘可以得到的100以内的数字为："+String.Join(",",OrigionData.ToList().OrderBy(s => s)));
            // OrigionData.ToList().OrderByDescending(s=>s)


        }


        /// <summary>
        /// 通过计算机求出一个大偶数有多少个可分解为两个质数之和的形式的解
        /// </summary>
        /// <param name="EvenData">一个偶数</param>
        public static void GetEvenToTwoPrimesSum(int EvenData)
        {
            if (EvenData % 2 != 0)
                throw new Exception("EvenData需要是大于6的偶数！");

            // 首先获得小于它的质数数组
            List<int> PrimesList = GetNumRangePrimes(EvenData);
            var Count = PrimesList.Count;

            foreach(var a in PrimesList.Where(s=>s<= EvenData/2))
            {
                foreach(var b in PrimesList.Where(s=>s>=EvenData/2))
                {
                    if(a+b== EvenData)
                    {
                        Console.WriteLine("偶数" + EvenData + "可以表示成以下两个质数的和：" + a + "+" + b);
                    }
                }
            }

        }

        // 获取以某奇数为对称轴的质数对
        public static void GetOddSymmetryPrimesPair(int OddData)
        {
            if (OddData % 2 != 1)
                throw new Exception("OddData需要是大于3的奇数！");

            var TheEvenData = OddData * 2;
            // 首先获得小于它的质数数组
            List<int> PrimesList = GetNumRangePrimes(TheEvenData);
            var Count = PrimesList.Count;

            var time = 0;
            foreach (var a in PrimesList.Where(s=>s<=OddData))
            {
                foreach (var b in PrimesList.Where(s=>s>=OddData))
                {
                    if (a + b == TheEvenData)
                    {
                        Console.WriteLine("奇数" + OddData + "是以下两个质数的数轴对称点：" + a + "，" + b);
                        time++;
                    }
                }
            }

            Console.WriteLine("对称的质数对共有" + time + "对");

            // 打印出小于等于该奇数的质数
            var SmallPrimes = GetNumRangePrimes(OddData);
            Console.WriteLine("小于等于" + OddData + "的质数为：" + String.Join(",", SmallPrimes)+"共有"+SmallPrimes.Count+"个数字");
            

        }

    }
}
