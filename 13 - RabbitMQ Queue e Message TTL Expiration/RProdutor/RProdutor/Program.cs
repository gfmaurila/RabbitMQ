using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RProdutor
{
    class Program
    {
        static void Main()
        {
            var queueName = "test_time_to_live";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var arguments = new Dictionary<string, object>();
                    arguments.Add("x-message-ttl", 22000);

                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: arguments);

                    var body = Encoding.UTF8.GetBytes($"Hello World! Data/Hora: {DateTime.Now}");


                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);


                    //var props = channel.CreateBasicProperties();
                    //props.Expiration = "20000";
                    //channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: props, body: body);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
