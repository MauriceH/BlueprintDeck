using System.Security.Cryptography;
using System.Text;

namespace BlueprintDeck
{
    internal static class SHA1Extensions
    {
        public static string ComputeHash(this SHA1 sha1, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = sha1.ComputeHash(valueBytes);
            return ByteArrayToString(hashBytes);
        }
        
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}