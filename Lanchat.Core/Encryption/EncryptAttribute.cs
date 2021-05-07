using System;

namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     String properties with this attribute will be encrypted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptAttribute : Attribute
    { }
}