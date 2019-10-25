using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateAndEvent
{
    class Program
    {
        delegate void Procedure();
        static void Main(string[] args)
        {
            Procedure someProcs = null;

            someProcs += new Procedure(DelegateTest.Method1); //添加委托
            someProcs += new Procedure(DelegateTest.Method1); //可以添加重复的委托
            someProcs += new Procedure(DelegateTest.Method2);

            DelegateTest test = new DelegateTest();
            someProcs += new Procedure(test.Method3);

            someProcs.Invoke();
            someProcs-= new Procedure(DelegateTest.Method2); //去掉委托方法
            someProcs -= new Procedure(DelegateTest.Method2); //可以重复去掉委托方法
            someProcs += new Procedure(delegate { Console.WriteLine("这是一个匿名委托方法"); });
            someProcs += new Procedure(() => Console.WriteLine("这是一个Lambda表达式匿名委托方法"));
            someProcs();  //等同于someProcs.Invoke();

            //Event 调用实例
            Waiter waiter = new Waiter();//有一个服务生
            People people = new People(); //进来了一个客人
            waiter.hasReady(people); //这个服务生招呼客人

            people.BeginDinner(); //客人开始吃饭了

            Console.ReadKey();
        }
    }
}
