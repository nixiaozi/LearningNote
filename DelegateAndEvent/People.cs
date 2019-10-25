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
        }

        public void BeginDinner()
        {
            if (toEat!=null)
            {
                toEat();
            }
        }

}
}
