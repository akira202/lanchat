using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class KeyInfoHandlerTest
    {
        private KeyInfoHandler keyInfoHandler;
        private NodeMock nodeMock;
        private SymmetricEncryption symmetricEncryption;

        [SetUp]
        public void Setup()
        {
            var publicKeyEncryption = new PublicKeyEncryption();
            publicKeyEncryption.ImportKey(publicKeyEncryption.ExportKey());
            symmetricEncryption = new SymmetricEncryption(publicKeyEncryption);
            nodeMock = new NodeMock();
            keyInfoHandler = new KeyInfoHandler(symmetricEncryption, nodeMock);
        }

        [Test]
        public void ValidKeyInfo()
        {
            var keyInfo = symmetricEncryption.ExportKey();
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(keyInfoHandler.Disabled);
            Assert.IsTrue(nodeMock.Ready);
            Assert.IsTrue(nodeMock.ConnectedEvent);
        }

        [Test]
        public void InvalidKeyInfo()
        {
            var keyInfo = new KeyInfo
            {
                AesKey = new byte[] {0x10},
                AesIv = new byte[] {0x10}
            };
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(nodeMock.CannotConnectEvent);
        }

        [Test]
        public void BlankKeyInfo()
        {
            var keyInfo = new KeyInfo();
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(nodeMock.CannotConnectEvent);
        }
    }
}