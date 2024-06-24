using SecretVault.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SecretVault.Infrastucture.Services
{
    public class EncryptionService:IEncryptionService
    {
        
            private readonly byte[] _key;

            public EncryptionService(string key)
            {

                _key = Encoding.UTF8.GetBytes(key);
            }

            public string Encrypt(string plainText)
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                var iv = aes.IV;

                var encryptedContent = ms.ToArray();

                var result = new byte[iv.Length + encryptedContent.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

                return Convert.ToBase64String(result);
            }

            public string Decrypt(string encryptedText)
            {


                var fullCipher = Convert.FromBase64String(encryptedText);

                using var aes = Aes.Create();
                aes.Key = _key;
                var iv = new byte[aes.BlockSize / 8];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(cipher);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);

                var valorDecripy = sr.ReadToEnd();


                return valorDecripy;
            }
        }


    }
 