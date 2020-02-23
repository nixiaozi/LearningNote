using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbitmq.WorkA
{
    public static class RabbitmqBase
    {
        public static IModel model;


        static RabbitmqBase()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            connection.


        }


    }
}
