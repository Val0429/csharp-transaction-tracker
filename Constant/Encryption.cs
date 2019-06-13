using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Constant
{
    public enum Encryption : ushort
    {
        Basic = 1,
        Plain = 2,
        Digest = 3,
    }

    public static class Encryptions
    {
        public static Encryption ToIndex(String value)
        {
            foreach (KeyValuePair<Encryption, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static String ToString(Encryption index)
        {
            foreach (KeyValuePair<Encryption, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<Encryption, String> List = new Dictionary<Encryption, String>
                                                             {
                                                                 { Encryption.Basic, "BASIC" },
                                                                 { Encryption.Plain, "PLAIN" },
                                                                 { Encryption.Digest, "DIGEST" },
                                                             };

        public static String EncryptDES(String original)
        {
            return EncryptDES(original, "#$*D%^3(");
        }

        public static String DecryptDES(String encrypt)
        {
            return DecryptDES(encrypt, "#$*D%^3(");
        }

        public static String EncryptDES(String original, String password)
        {
            try
            {
                if (original == null) return null;

                var bysData = Encoding.UTF8.GetBytes(original);
                if (bysData.Length == 0) return "";

                var bysFixSizeData = new Byte[Math.Max((Int32)Math.Ceiling(bysData.Length / 8.0) * 8, 8)];
                Array.Copy(bysData, bysFixSizeData, bysData.Length);

                var des = new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(password),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.None
                };

                return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(bysFixSizeData, 0, bysFixSizeData.Length));
            }
            catch
            {
                return original;
            }
        }

        public static String DecryptDES(String encrypt, String password)
        {
            try
            {
                if (encrypt == null) return null;
                var bysData = Convert.FromBase64String(encrypt);
                if (bysData.Length == 0)
                    return "";

                var des = new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(password),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.None
                };

                var desData = des.CreateDecryptor().TransformFinalBlock(bysData, 0, bysData.Length);
                Int32 i = desData.Length - 1;
                while (i >= 0 && desData[i] == 0)
                    i--;

                if (i < 0) return "";

                Byte[] trimData = new Byte[i + 1];
                Array.Copy(desData, trimData, trimData.Length);

                return Encoding.UTF8.GetString(trimData);
            }
            catch
            {
                return encrypt;
            }
        }
    }
}