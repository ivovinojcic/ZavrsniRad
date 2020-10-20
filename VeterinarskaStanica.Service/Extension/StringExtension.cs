using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VeterinarskaStanica.Service.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Generate hash SHA512 from string
        /// </summary>
        /// <returns>The 512 hash.</returns>
        /// <param name="inputString">Input string.</param>
        public static string SHA512Hash(this string inputString)
        {
            // Create byte from input string
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);

            // Crete hash from bytes
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            // Return hash value
            return result.ToString();
        }
    }
}
