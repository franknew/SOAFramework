using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
// <OWNER>Microsoft</OWNER>
// 

//
//  RSA.cs
//

namespace System.Security.Cryptography
{
    using System.IO;
    using System.Text;
    using System.Runtime.Serialization;
    using System.Security.Util;
    using System.Globalization;
    using System.Diagnostics.Contracts;

    // We allow only the public components of an RSAParameters object, the Modulus and Exponent
    // to be serializable.
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public struct RSAParameters
    {
        public byte[] Exponent;
        public byte[] Modulus;
#if SILVERLIGHT
        public byte[] P;
        public byte[] Q;
        public byte[] DP;
        public byte[] DQ;
        public byte[] InverseQ;
        public byte[] D;
#else // SILVERLIGHT
        [NonSerialized] public byte[] P;
        [NonSerialized] public byte[] Q;
        [NonSerialized] public byte[] DP;
        [NonSerialized] public byte[] DQ;
        [NonSerialized] public byte[] InverseQ;
        [NonSerialized] public byte[] D;
#endif // SILVERLIGHT
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class RSA : AsymmetricAlgorithm
    {
        protected RSA() { }
        //
        // public methods
        //

        new static public RSA Create()
        {
            return Create("System.Security.Cryptography.RSA");
        }

        new static public RSA Create(String algName)
        {
            return (RSA)CryptoConfig.CreateFromName(algName);
        }

        //
        // New RSA encrypt/decrypt/sign/verify RSA abstractions in .NET 4.6+ and .NET Core
        //
        // Methods that throw DerivedClassMustOverride are effectively abstract but we
        // cannot mark them as such as it would be a breaking change. We'll make them
        // abstract in .NET Core.

        public virtual byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            throw DerivedClassMustOverride();
        }

        public virtual byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
        {
            throw DerivedClassMustOverride();
        }

        public virtual byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            throw DerivedClassMustOverride();
        }

        public virtual bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            throw DerivedClassMustOverride();
        }

        protected virtual byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
        {
            throw DerivedClassMustOverride();
        }

        protected virtual byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
        {
            throw DerivedClassMustOverride();
        }

        public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return SignData(data, 0, data.Length, hashAlgorithm, padding);
        }

        public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0 || offset > data.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || count > data.Length - offset)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (String.IsNullOrEmpty(hashAlgorithm.Name))
            {
                throw HashAlgorithmNameNullOrEmpty();
            }
            if (padding == null)
            {
                throw new ArgumentNullException("padding");
            }

            byte[] hash = HashData(data, offset, count, hashAlgorithm);
            return SignHash(hash, hashAlgorithm, padding);
        }

        public virtual byte[] SignData(Stream data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (String.IsNullOrEmpty(hashAlgorithm.Name))
            {
                throw HashAlgorithmNameNullOrEmpty();
            }
            if (padding == null)
            {
                throw new ArgumentNullException("padding");
            }

            byte[] hash = HashData(data, hashAlgorithm);
            return SignHash(hash, hashAlgorithm, padding);
        }

        public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return VerifyData(data, 0, data.Length, signature, hashAlgorithm, padding);
        }

        public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset < 0 || offset > data.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || count > data.Length - offset)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            if (String.IsNullOrEmpty(hashAlgorithm.Name))
            {
                throw HashAlgorithmNameNullOrEmpty();
            }
            if (padding == null)
            {
                throw new ArgumentNullException("padding");
            }

            byte[] hash = HashData(data, offset, count, hashAlgorithm);
            return VerifyHash(hash, signature, hashAlgorithm, padding);
        }

        public bool VerifyData(Stream data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            if (String.IsNullOrEmpty(hashAlgorithm.Name))
            {
                throw HashAlgorithmNameNullOrEmpty();
            }
            if (padding == null)
            {
                throw new ArgumentNullException("padding");
            }

            byte[] hash = HashData(data, hashAlgorithm);
            return VerifyHash(hash, signature, hashAlgorithm, padding);
        }
        
                //
        // These should also be obsolete (on the base). They aren't well defined nor are they used
        // anywhere in the FX apart from checking that they're not null.
        //
        // For new derived RSA classes, we'll just return "RSA" which is analagous to what ECDsa
        // and ECDiffieHellman do.
        //
        // Note that for compat, RSACryptoServiceProvider still overrides and returns RSA-PKCS1-KEYEX 
        // and http://www.w3.org/2000/09/xmldsig#rsa-sha1
        //

        public override string KeyExchangeAlgorithm
        {
            get { return "RSA"; }
        }

        public override string SignatureAlgorithm
        {
            get { return "RSA"; }
        }
        

        // We can provide a default implementation of ToXmlString because we require 
        // every RSA implementation to implement ImportParameters
        // If includePrivateParameters is false, this is just an XMLDSIG RSAKeyValue
        // clause.  If includePrivateParameters is true, then we extend RSAKeyValue with 
        // the other (private) elements.
        public override String ToXmlString(bool includePrivateParameters)
        {
            // From the XMLDSIG spec, RFC 3075, Section 6.4.2, an RSAKeyValue looks like this:
            /* 
               <element name="RSAKeyValue"> 
                 <complexType> 
                   <sequence>
                     <element name="Modulus" type="ds:CryptoBinary"/> 
                     <element name="Exponent" type="ds:CryptoBinary"/>
                   </sequence> 
                 </complexType> 
               </element>
            */
            // we extend appropriately for private components
            RSAParameters rsaParams = this.ExportParameters(includePrivateParameters);
            StringBuilder sb = new StringBuilder();
            sb.Append("<RSAKeyValue>");
            // Add the modulus
            sb.Append("<Modulus>" + Convert.ToBase64String(rsaParams.Modulus) + "</Modulus>");
            // Add the exponent
            sb.Append("<Exponent>" + Convert.ToBase64String(rsaParams.Exponent) + "</Exponent>");
            if (includePrivateParameters)
            {
                // Add the private components
                sb.Append("<P>" + Convert.ToBase64String(rsaParams.P) + "</P>");
                sb.Append("<Q>" + Convert.ToBase64String(rsaParams.Q) + "</Q>");
                sb.Append("<DP>" + Convert.ToBase64String(rsaParams.DP) + "</DP>");
                sb.Append("<DQ>" + Convert.ToBase64String(rsaParams.DQ) + "</DQ>");
                sb.Append("<InverseQ>" + Convert.ToBase64String(rsaParams.InverseQ) + "</InverseQ>");
                sb.Append("<D>" + Convert.ToBase64String(rsaParams.D) + "</D>");
            }
            sb.Append("</RSAKeyValue>");
            return (sb.ToString());
        }

        abstract public RSAParameters ExportParameters(bool includePrivateParameters);

        abstract public void ImportParameters(RSAParameters parameters);

        internal static Exception HashAlgorithmNameNullOrEmpty()
        {
            return new ArgumentException("HashAlgorithmNameNullOrEmpty");
        }

        private static Exception DerivedClassMustOverride()
        {
            return new NotImplementedException("DerivedClassMustOverride");
        }
    }
}
