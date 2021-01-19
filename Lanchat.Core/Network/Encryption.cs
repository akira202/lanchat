using System;
using System.IO;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Encryption : IDisposable
    {
        private readonly Aes localAes;
        private readonly RSA localRsa;
        private readonly Aes remoteAes;
        private readonly RSA remoteRsa;

        public Encryption()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create();
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }

        public void Dispose()
        {
            localAes?.Dispose();
            localRsa?.Dispose();
            remoteAes?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        internal PublicKey ExportPublicKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = Convert.ToBase64String(parameters.Modulus),
                RsaExponent = Convert.ToBase64String(parameters.Exponent)
            };
        }

        internal void ImportPublicKey(PublicKey publicKey)
        {
            var parameters = new RSAParameters
            {
                Modulus = Convert.FromBase64String(publicKey.RsaModulus),
                Exponent = Convert.FromBase64String(publicKey.RsaExponent)
            };
            remoteRsa.ImportParameters(parameters);
        }

        internal KeyInfo ExportAesKey()
        {
            return new()
            {
                AesKey = RsaEncrypt(localAes.Key),
                AesIv = RsaEncrypt(localAes.IV)
            };
        }

        internal void ImportAesKey(KeyInfo keyInfo)
        {
            remoteAes.Key = RsaDecrypt(keyInfo.AesKey);
            remoteAes.IV = RsaDecrypt(keyInfo.AesIv);
        }

        internal string Encrypt(string text)
        {
            var encryptor = remoteAes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            var encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        internal string Decrypt(string text)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(text);
                var decryptor = localAes.CreateDecryptor();
                using var msDecrypt = new MemoryStream(encryptedBytes);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                var plaintext = srDecrypt.ReadToEnd();
                return plaintext;
            }
            catch (Exception e)
            {
                if (e is not CryptographicException && e is not FormatException) throw;
                return null;
            }
        }

        private string RsaEncrypt(byte[] bytes)
        {
            var encryptedBytes = remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        private byte[] RsaDecrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }
    }
}