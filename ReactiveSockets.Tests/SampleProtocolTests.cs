namespace ReactiveSockets.Tests
{
    using Moq;
    using ReactiveProtocol;
    using ReactiveSockets;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using Xunit;

    public class SampleProtocolTests
    {
        [Fact]
        public void when_parsing_bytes_then_raises_messages()
        {
            var bytes = new BlockingCollection<byte>();
            var socket = Mock.Of<IReactiveSocket>(x => x.Receiver == bytes.GetConsumingEnumerable().ToObservable(TaskPoolScheduler.Default));

            var protocol = new StringChannel(socket);
            var message = "";

            protocol.Receiver.SubscribeOn(TaskPoolScheduler.Default).Subscribe(s => message = s);

            protocol.Convert("Hello").ToList().ForEach(b => bytes.Add(b));

            Thread.Sleep(200);

            Assert.NotNull(message);
            Assert.Equal("Hello", message);
        }
    }
}
