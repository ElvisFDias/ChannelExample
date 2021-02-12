using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SimpleProducerConsumer
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

            while (true)
            {
                try
                {
                    var message = await channel.Reader.ReadAsync();

                    Console.WriteLine(message);
                }
                catch (ChannelClosedException)
                {

                    Console.WriteLine($"End of messages");
                    break;
                }
            }

            Console.ReadLine();
        }
    }
}
