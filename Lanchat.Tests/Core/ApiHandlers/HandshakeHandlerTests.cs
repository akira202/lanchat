using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock.Api;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class HandshakeHandlerTests
    {
        private HandshakeHandler handshakeHandler;
        private NodeMock nodeMock;
        private OutputMock outputMock;
        private PublicKeyEncryption publicKeyEncryption;
        private SymmetricEncryptionMock symmetricEncryptionMock;

        [SetUp]
        public void Setup()
        {
            publicKeyEncryption = new PublicKeyEncryption();
            symmetricEncryptionMock = new SymmetricEncryptionMock();
            outputMock = new OutputMock();
            nodeMock = new NodeMock();

            handshakeHandler = new HandshakeHandler(
                publicKeyEncryption,
                symmetricEncryptionMock,
                outputMock,
                nodeMock);
        }

        [Test]
        public void ValidHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = "test",
                Status = Status.Online,
                PublicKey = publicKeyEncryption.ExportKey()
            };
            handshakeHandler.Handle(handshake);

            publicKeyEncryption.Encrypt(new byte[] {0x10});
            Assert.IsTrue(handshakeHandler.Disabled);
            Assert.AreEqual(handshake.Status, nodeMock.Status);
            Assert.IsTrue(nodeMock.HandshakeSent);
            Assert.NotNull(outputMock.LastOutput);
        }

        [Test]
        public void InvalidKey()
        {
            var handshake = new Handshake
            {
                Nickname = "test",
                Status = Status.Online,
                PublicKey = new PublicKey()
            };
            handshakeHandler.Handle(handshake);

            Assert.IsTrue(nodeMock.CannotConnectEvent);
        }
    }
}