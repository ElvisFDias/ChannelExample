using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProducerConsumerAsyncEnumerable
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

    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = Channel.CreateUnbounded<MessageItem>();

            for (int i = 0; i < 10; i++)
            {
                var message = new MessageItem() { Id = i, Content = $"Content with id: {i}" };

                await channel.Writer.WriteAsync(message);
            }

            channel.Writer.Complete();

            await foreach (var message in channel.Reader.ReadAllAsync())
            {
                Console.WriteLine(message);
            }

            Console.WriteLine($"End of messages");
 
            Console.ReadLine();
        }
    }
}
