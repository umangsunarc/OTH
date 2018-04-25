using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Wollo.Base.Utilities
{
    public class Cipher
    {
        private readonly RijndaelManaged _rijndaelManaged;

        public Cipher(string salt = null, string inputKey = null)
        {
            if (String.IsNullOrWhiteSpace(salt))
            {
                salt = "C96E358A-F3B1-4766-9E1F-259B96AC258E";
            }

            if (String.IsNullOrWhiteSpace(inputKey))
            {
                inputKey = "18B5C658-4126-4AF8-940D-E73D52E532B0";
            }

            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(inputKey, saltBytes);

            _rijndaelManaged = new RijndaelManaged();

            _rijndaelManaged.Key = key.GetBytes(_rijndaelManaged.KeySize / 8);

            _rijndaelManaged.IV = key.GetBytes(_rijndaelManaged.BlockSize / 8);
        }

        public string Decrypt(string cipherText)
        {
            string result = null;


            try
            {
                if (!isBase64String(cipherText))
                {
                    throw new Exception("The cipherText input parameter is not base64 encoded");
                }
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    ICryptoTransform cryptoTransform = _rijndaelManaged.CreateDecryptor();

                    using (CryptoStream cryptStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        StreamReader reader = new StreamReader(cryptStream);

                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                throw;
            }

            return result;
        }

        public string Encrypt(string plainText)
        {
            string result = null;

            try
            {
                ICryptoTransform cryptoTransform = _rijndaelManaged.CreateEncryptor();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptStream))
                        {
                            writer.Write(plainText);
                        }
                    }

                    result = Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                throw;
            }

            return result;
        }


        private bool isBase64String(string base64String)
        {
            base64String = base64String.Trim();

            return (base64String.Length % 4 == 0) && Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

    }
}
