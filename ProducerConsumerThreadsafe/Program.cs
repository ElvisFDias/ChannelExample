using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProducerConsumerThreadsafe
{
    public class MessageItem
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Content: {Content}";
        }
    }

    public class Producer
    {
        private readonly ChannelWriter<MessageItem> writer;

        public Producer(ChannelWriter<MessageItem> writer) => this.writer = writer;

        public async Task ProduceAsync()
        {

            for (int i = 0; i < 10; i++)
            {
                var message = new MessageItem() { Id = i, Content = $"Content with id: {i}" };

                AddItem(message);
            }

            writer.Complete();
        }


        private async Task AddItem(MessageItem message)
        {
            await Task.Delay(1000);

            await writer.WriteAsync(message);
        }

    }

    public class Consumer
    {
        private readonly ChannelReader<MessageItem> reader;

        public Consumer(ChannelReader<MessageItem> reader) => this.reader = reader;

        public async Task ReadAsync()
        {

            await foreach (var message in reader.ReadAllAsync())
            {
                Console.WriteLine(message);
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = Channel.CreateUnbounded<MessageItem>();
            var producer = new Producer(channel.Writer);
            var consumer = new Consumer(channel.Reader);

            await producer.ProduceAsync();

            await consumer.ReadAsync();

            Console.WriteLine($"End of messages");

            Console.ReadLine();
        }
    }
}
