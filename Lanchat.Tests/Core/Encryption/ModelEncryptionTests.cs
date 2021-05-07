using Lanchat.Core.Encryption;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class ModelEncryptionTests
    {
        private ModelEncryption modelEncryption;

        [SetUp]
        public void Setup()
        {
            modelEncryption = new ModelEncryption(new SymmetricEncryptionMock());
        }

        [Test]
        public void Encrypt()
        {
            var model = new EncryptedModel
            {
                Property = "1234"
            };

            modelEncryption.EncryptObject(model);
            Assert.AreEqual("4321", model.Property);
        }

        [Test]
        public void Decrypt()
        {
            var model = new EncryptedModel
            {
                Property = "1234"
            };

            modelEncryption.EncryptObject(model);
            modelEncryption.DecryptObject(model);
            Assert.AreEqual("1234", model.Property);
        }
    }
}