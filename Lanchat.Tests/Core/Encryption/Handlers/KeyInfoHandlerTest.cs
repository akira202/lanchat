using Lanchat.Core.Encryption;
using Lanchat.Core.Encryption.Handlers;
using Lanchat.Core.Encryption.Models;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption.Handlers
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
            var eventRaised = false;
            nodeMock.Connected += (_, _) =>
            {
                eventRaised = true;
            };
            
            var keyInfo = symmetricEncryption.ExportKey();
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(keyInfoHandler.Disabled);
            Assert.IsTrue(nodeMock.Ready);
            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void InvalidKeyInfo()
        {
            var eventRaised = false;
            nodeMock.CannotConnect += (_, _) =>
            {
                eventRaised = true;
            };
            
            var keyInfo = new KeyInfo
            {
                AesKey = new byte[] {0x10},
                AesIv = new byte[] {0x10}
            };
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void BlankKeyInfo()
        {
            var eventRaised = false;
            nodeMock.CannotConnect += (_, _) =>
            {
                eventRaised = true;
            };
            
            var keyInfo = new KeyInfo();
            keyInfoHandler.Handle(keyInfo);

            Assert.IsTrue(eventRaised);
        }
    }
}