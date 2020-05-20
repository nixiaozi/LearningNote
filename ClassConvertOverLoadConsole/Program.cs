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

        }
    }
}
