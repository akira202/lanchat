using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class NicknameUpdateHandlerTests

    {
        private NicknameUpdateHandler nicknameUpdateHandler;
        private NodeMock nodeMock;

        [SetUp]
        public void Setup()
        {
            nodeMock = new NodeMock();
            nicknameUpdateHandler = new NicknameUpdateHandler(nodeMock);
        }

        [Test]
        public void NewNickname()
        {
            var nicknameUpdate = new NicknameUpdate
            {
                NewNickname = "new-nickname"
            };

            nicknameUpdateHandler.Handle(nicknameUpdate);
            Assert.AreEqual(nicknameUpdate.NewNickname, nodeMock.Nickname);
        }
    }
}