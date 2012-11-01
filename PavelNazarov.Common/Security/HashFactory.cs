using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PavelNazarov.Common.Security
{
    public static class HashFactory<THashAlgorithm> where THashAlgorithm : HashAlgorithm
    {
        public static byte[] Create(byte[] binaryData)
        {
            using (THashAlgorithm hashAlgorithm = (THashAlgorithm)typeof(THashAlgorithm).GetMethod("Create", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[] { }, null).Invoke(null, null))
            {
                return hashAlgorithm.ComputeHash(binaryData);
            }
        }

        public static byte[] Create(string text, string encodingName = "utf-8")
        {
            Encoding encoding = Encoding.GetEncoding(encodingName);
            byte[] binaryData = encoding.GetBytes(text);

            byte[] bytes = Create(binaryData);
            return bytes;
        }

        public static string CreateText(byte[] binaryData)
        {
            return Create(binaryData).Aggregate<byte, StringBuilder, string>(new StringBuilder(), (sb, b) => sb.Append(b.ToString("x2")), sb => sb.ToString());
        }

        public static string CreateText(string textToHash, string encodingName = "utf-8")
        {
            Encoding encoding = Encoding.GetEncoding(encodingName);
            byte[] bytes = encoding.GetBytes(textToHash);
            return CreateText(bytes);
        }

        public static string CreateText(params Object[] objects)
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    var currObj = objects[i];
                    if (currObj == null)
                        throw new NullReferenceException(String.Format("Item with index {0} is null!", i));
                    writer.Write(currObj);
                }
            }
            return CreateText(sb.ToString());
        }
    }
}
