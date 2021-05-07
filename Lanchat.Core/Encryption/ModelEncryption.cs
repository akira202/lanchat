using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lanchat.Core.Extensions;

namespace Lanchat.Core.Encryption
{
    internal class ModelEncryption : IModelEncryption
    {
        private readonly ISymmetricEncryption symmetricEncryption;

        public ModelEncryption(ISymmetricEncryption symmetricEncryption)
        {
            this.symmetricEncryption = symmetricEncryption;
        }

        public void EncryptObject(object data)
        {
            var props = GetPropertiesWithEncryptAttribute(data);
            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, symmetricEncryption.EncryptString(value), null);
            });
        }

        public void DecryptObject(object data)
        {
            var props = GetPropertiesWithEncryptAttribute(data);
            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, symmetricEncryption.DecryptString(value), null);
            });
        }

        private static IEnumerable<PropertyInfo> GetPropertiesWithEncryptAttribute(object data)
        {
            var props = data
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(EncryptAttribute)));
            return props;
        }
    }
}