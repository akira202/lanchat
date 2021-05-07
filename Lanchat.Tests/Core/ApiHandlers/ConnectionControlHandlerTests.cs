using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock.Tcp;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class ConnectionControlHandlerTests
    {
        private ConnectionControlHandler connectionControlHandler;
        private HostMock hostElement;

        [SetUp]
        public void Setup()
        {
            hostElement = new HostMock();
            connectionControlHandler = new ConnectionControlHandler(hostElement);
        }

        [Test]
        public void RemoteClose()
        {
            var data = new ConnectionControl
            {
                Status = ConnectionStatus.RemoteDisconnect
            };
            connectionControlHandler.Handle(data);
            Assert.IsTrue(hostElement.Closed);
        }
    }
}