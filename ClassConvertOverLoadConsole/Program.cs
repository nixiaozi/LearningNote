using System;

namespace ClassConvertOverLoadConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Animal animal = new Dog(4);
            animal.GetName(); // 这里虽然时Animal类型，但是访问的时Dog的GetName方法，并且方法里面调用了Dog独有的属性也是没有问题的

            Console.WriteLine(animal is Dog);

            animal.Todo();

            Console.WriteLine((new Point(1, 4) + new Point(8, 5)).x);

        }
    }


    class Point
    {
        public int x;
        public int y;
        
        public Point(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x+b.x,b.y+a.y);
        }



    }


}
