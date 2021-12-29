using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RProdutor
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var manualResetEvent = new ManualResetEvent(false);

            manualResetEvent.Reset();
            using (var connection = factory.CreateConnection())
            {
                var queueName = "order";
                var channel1 = SetupChannel(connection);
                BuildPublishers(channel1, queueName, "Produtor A", manualResetEvent);

                manualResetEvent.WaitOne();
                Console.ReadLine();
            }
        }

        public static IModel SetupChannel(IConnection connection)
        {
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "logs", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "finance_orders", durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.ExchangeDeclare("order", "fanout");

            channel.QueueBind("order", "order", "");
            channel.QueueBind("logs", "order", "");
            channel.QueueBind("finance_orders", "order", "");

            return channel;
        }

        public static void BuildPublishers(IModel channel, string queue, string publisherName, ManualResetEvent manualResetEvent)
        {
            Task.Run(() => {

                channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                int count = 0;
                while (true)
                {
                    try
                    {
                        Console.WriteLine($"Pressione qualquer tecla para produzi 10 msg");
                        Console.ReadLine();

                        for (int i = 0; i < 10; i++)
                        {
                            string message = $"OrderNumber - {count++} from {publisherName}";

                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish("order", "", null, body);

                            Console.WriteLine($" {publisherName} - [x] Sent {count}", message);
                            //System.Threading.Thread.Sleep(1000);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        manualResetEvent.Set();
                    }
                }

            });
        }
    }
}
