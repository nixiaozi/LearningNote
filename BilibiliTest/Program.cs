using System;

namespace BilibiliTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var url = "https://www.bilibili.com/video/av671737528/";

            var start = url.LastIndexOf("av")+2;

            var end = url.Length-1;

            var length = end - start;

            var result = url.Substring(start, length);


        }
    }
}
