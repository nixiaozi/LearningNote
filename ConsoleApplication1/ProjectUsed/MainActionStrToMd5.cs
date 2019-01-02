using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.ProjectUsed
{
    public class MainActionStrToMd5
    {
        public static void Do()
        {
            var number = "15678100829";
            var nStr = number.Trim().Substring(5).ToMd5();
            Console.WriteLine("MD5为：" + nStr);
        }

    }
}
