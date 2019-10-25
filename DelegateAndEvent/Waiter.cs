using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateAndEvent
{
    public class Waiter
    {
        public List<People> ServicePeople;

        /// <summary>
        /// 为服务生绑定需要服务的人
        /// </summary>
        /// <param name="p"></param>
        public void hasReady(People p)
        {
            p.toEat += Serv;
        }

        public void Serv()
        {
            Console.WriteLine("服务生开始上菜了！");
        }
    }
}
