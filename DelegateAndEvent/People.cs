using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateAndEvent
{
    public delegate void eatHandler();
    public class People
    {
        public event eatHandler toEat;
        public People()
        {
            toEat += () => { Console.WriteLine("Want To Eat !"); };
            toEat += new eatHandler(delegate { Console.WriteLine("顾客要开始吃饭了！"); });
        }

        public void BeginDinner()
        {
            toEat?.Invoke(); //简化委托调用前为空判断
        }

}
}
