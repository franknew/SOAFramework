using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Service.SDK.Core
{
    public abstract class IServiceEncryptor
    {
        public IServiceEncryptor() { }
        public IServiceEncryptor(string key, string iv)
        {
            this.Key = key;
            this.IV = iv;
        }

        public string Key { get; set; }
        public string IV { get; set; }
        public string Content { get; set; }

        public virtual string Encrypt(string key, string content)
        {
            this.Key = key;
            return Encrypt(content);
        }

        public virtual string Decrypt(string key, string content)
        {
            this.Key = key;
            return Decrypt(content);
        }

        public abstract string Encrypt(string content);
        public abstract string Decrypt(string content);
    }
}
