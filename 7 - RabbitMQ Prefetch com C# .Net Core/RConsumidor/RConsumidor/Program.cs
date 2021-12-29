﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RConsumidor
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                var channel = CreateChannel(connection);

                channel.QueueDeclare(queue: "order",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                BuildAndRunWorker(channel, $"Worker - A");
                BuildAndRunWorker(channel, $"Worker - B");

                Console.ReadLine();
            }
        }

        public static IModel CreateChannel(IConnection connection)
        {
            var channel = connection.CreateModel();
            return channel;
        }

        public static void BuildAndRunWorker(IModel channel, string workerName)
        {
            channel.BasicQos(0, 7, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"{channel.ChannelNumber} - {workerName} - [x] Received {message}");
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Log ex
                    Console.WriteLine(ex.Message);
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            channel.BasicConsume(queue: "order", autoAck: false, consumer: consumer);

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
    }
}
