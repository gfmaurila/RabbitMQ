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
            var queueName = "RabbitMQ_Persistence_Message_e_Durable_Queue";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes($"Hello World! Data/Hora: {DateTime.Now}");

                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: basicProperties, body: body);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
