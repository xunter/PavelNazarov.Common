using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace PavelNazarov.Common.Security
{
    public static class MembershipUtil
    {
        public static byte[] GetHashAsBytes(string hash)
        {
            int len = (int)Math.Ceiling((double)hash.Length / 2);
            byte[] bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                var currToken = hash.Substring(i * 2, 2);
                var b = Byte.Parse(currToken, NumberStyles.HexNumber);
                bytes[i] = b;
            }
            return bytes;
        }

        public static byte[] CreateSalt()
        {
            byte[] bytes = new byte[64];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        public static byte[] CreatePasswordHashWithSalt(byte[] passwordHash, byte[] salt)
        {
            var bytes = new byte[passwordHash.Length + salt.Length];
            Array.Copy(passwordHash, bytes, passwordHash.Length);
            Array.Copy(salt, 0, bytes, passwordHash.Length, salt.Length);
            var hashAsBytes = HashFactory<SHA512>.Create(bytes);
            return hashAsBytes;
        }

        public static string CreatePasswordHashWithSaltAsString(byte[] passwordHash, byte[] salt)
        {
            var bytes = CreatePasswordHashWithSalt(passwordHash, salt);
            return BytesUtil.BytesToString(bytes);
        }

        public static bool ValidatePasswordHashForAccount(string passwordHashToValidate, byte[] salt, byte[] passwordHash)
        {
            var passwordHashAsBytes = GetHashAsBytes(passwordHashToValidate);
            var hashWithSalt = CreatePasswordHashWithSalt(passwordHashAsBytes, salt);
            return IsHashesInBytesEqual(hashWithSalt, passwordHash);
        }

        public static bool IsHashesInBytesEqual(byte[] hashLeft, byte[] hashRight)
        {
            if (hashLeft.Length != hashRight.Length)
                return false;

            for (int i = 0; i < hashLeft.Length; i++)
                if (hashLeft[i] != hashRight[i])
                    return false;

            return true;
        }

        public static string CreateConfirmKeyAsGuidBytes()
        {
            var guid = Guid.NewGuid();
            var guidBytes = guid.ToByteArray();
            var emailApprovedKeyForAccount = BytesUtil.BytesToString(guidBytes);
            return emailApprovedKeyForAccount;
        }

        public static string CreateRNDPasswordAsBase64()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();
            var bytesStringAsBase64 = Convert.ToBase64String(bytes);
            return bytesStringAsBase64;
        }

        public static byte[] CreatePasswordHashWithSalt(string newPassword)
        {
            return HashFactory<SHA512>.Create(newPassword);
        }
    }
}
