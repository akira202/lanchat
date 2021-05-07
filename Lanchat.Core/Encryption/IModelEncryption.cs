namespace Lanchat.Core.Encryption
{
    internal interface IModelEncryption
    {
        void EncryptObject(object data);
        void DecryptObject(object data);
    }
}