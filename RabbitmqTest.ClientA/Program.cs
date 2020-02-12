using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitmqTest.ClientA
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                //定义一个队列，队列名为hello
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false,
                    autoDelete: false, arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);


                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}",message);

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
