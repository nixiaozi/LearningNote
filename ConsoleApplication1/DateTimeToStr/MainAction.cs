using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.DateTimeToStr
{
    public class MainAction
    {
        public static void Do()
        {
            var dateStr = "20141030133525";
            CultureInfo provider = CultureInfo.CurrentCulture;
            var theDate = DateTime.ParseExact(dateStr, "yyyyMMddHHmmss", provider);

            BeLoop:
            Console.WriteLine("当前时间为：" + theDate);
            Console.WriteLine("请输入要比较的时间：");
            string DateString = Console.ReadLine();
            long id = DateTime.Parse(DateString).Ticks - DateTime.Parse("2017/01/01").Ticks;
            Console.WriteLine(id.ToString("D10"));

            Console.WriteLine("继续比较请按l：");
            string Inputstr = Console.ReadLine();
            if (Inputstr == "y")
            {
                goto BeLoop;
            }

        }
    }
}
