using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Rabbitmq.WorkB
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue", //定义一个新队列 task_queue
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null); // 不包含其它参数

                channel.BasicQos(  //通道预定义
                    prefetchSize: 0,   //
                    prefetchCount: 1,  //一次只传递一个消息
                    global: false); 

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000); // 通过挂起线程模拟高耗时任务

                    Console.WriteLine(" [x] Done");

                    //使用了手动确认模式
                    channel.BasicAck(
                        deliveryTag: ea.DeliveryTag, //确认的tag
                        multiple: false);  //不要一次性确认多个标签
                };
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,    //不要自动确认
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
