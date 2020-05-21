using System;
using System.Collections.Generic;
using System.Text;

namespace ClassConvertOverLoadConsole
{
    public class Animal
    {
        public Animal()
        {
            Console.WriteLine("Create Animal!@");
        }

        public string LeiName = "";

        public virtual void GetName()
        {
            Console.WriteLine("Animal");
        }

        public virtual void Todo()
        {
            Console.WriteLine("Animal Todo");
        }

    }


    public class Dog : Animal
    {
        //会报错指定了多个入口点
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello Animal!");
        //}


        public Dog():base()
        {
            this.LeiName = "Dog";
        }


        public Dog(int color):this()
        {
            Color = color;
        }

        public int Color;


        public override void GetName()
        {
            Console.WriteLine("Dog With Color"+Color);
        }
        public override void Todo()
        {
            Console.WriteLine("Dog Todo");
        }

    }

}
