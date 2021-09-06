using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("递归算法求第30位数!");

            var cache = 1;
            var countTime = 30; // 递归的次数

           var result = DoAdd(1,0, countTime);// 初始值为一递归30次

            Console.WriteLine("递归算法求第30位数数值为："+result);
        }

        

        public static int DoAdd(int perData,int nowData,int Time)
        {
            if(Time == 1)
            {
                return perData;
            }

            return DoAdd((nowData + perData),perData, Time - 1);

        }

    }
}
