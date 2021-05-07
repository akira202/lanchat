using System;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal class PublicKeyEncryption : IPublicKeyEncryption
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;

        internal PublicKeyEncryption()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create();
        }

        public void Dispose()
        {
            localRsa?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        public PublicKey ExportKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = parameters.Modulus,
                RsaExponent = parameters.Exponent
            };
        }

        public void ImportKey(PublicKey publicKey)
        {
            var parameters = new RSAParameters
            {
                Modulus = publicKey.RsaModulus,
                Exponent = publicKey.RsaExponent
            };

            remoteRsa.ImportParameters(parameters);
            TestKeys();
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        }

        public byte[] Decrypt(byte[] encryptedBytes)
        {
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }

        private void TestKeys()
        {
            remoteRsa.Encrypt(new byte[] {0x10}, RSAEncryptionPadding.Pkcs1);
        }
    }
}