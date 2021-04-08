using System;
using System.Security.Cryptography;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class PublicKeyEncryptionTests
    {
        private PublicKeyEncryption publicKeyEncryptionTest;

        [SetUp]
        public void Setup()
        {
            publicKeyEncryptionTest = new PublicKeyEncryption();
            publicKeyEncryptionTest.ImportKey(publicKeyEncryptionTest.ExportKey());
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = publicKeyEncryptionTest.Encrypt(testBytes);
            var decryptedBytes = publicKeyEncryptionTest.Decrypt(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void ImportInvalidRsa()
        {
            Assert.Catch<CryptographicException>(() =>
            {
                publicKeyEncryptionTest.ImportKey(new PublicKey
                {
                    RsaExponent = new byte[] {0x10},
                    RsaModulus = new byte[] {0x10}
                });
            });
        }

        [Test]
        public void Dispose()
        {
            publicKeyEncryptionTest.Dispose();
            Assert.Catch<ObjectDisposedException>(() => { publicKeyEncryptionTest.Encrypt(new byte[] {0x10}); });
        }
    }
}