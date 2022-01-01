using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RProdutor
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                try
                {
                    // canal confirmador
                    channel.ConfirmSelect();

                    channel.BasicAcks += Channel_BasicAcks;
                    channel.BasicNacks += Channel_BasicNacks;
                    channel.BasicReturn += Channel_BasicReturn;


                    channel.QueueDeclare(queue: "order",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = $"{DateTime.UtcNow:o} -> Olá";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "order_teste ",
                                         basicProperties: null,
                                         body: body,
                                         mandatory: true);

                    channel.WaitForConfirms(new TimeSpan(0, 0, 5));

                    Console.WriteLine(" [x] Sent {0}", message);
                } catch (Exception ex)
                {
                    // TRatar error
                    // gerar logs 
                    // verificar se o canal foi fechado para fazer uma re conexão 
                    // Abrir o canal e reconectar ao consumidor 
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void Channel_BasicReturn(object sender, RabbitMQ.Client.Events.BasicReturnEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"{DateTime.UtcNow:o} -> Basic Return -> {body}");
        }

        private static void Channel_BasicNacks(object sender, RabbitMQ.Client.Events.BasicNackEventArgs e)
        {
            Console.WriteLine($"{DateTime.UtcNow:o} -> Basic Nack");
        }

        private static void Channel_BasicAcks(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
        {
            Console.WriteLine($"{DateTime.UtcNow:o} -> Basic Ack "); 
        }
    }
}
