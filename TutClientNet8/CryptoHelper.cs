using System;
using System.Text;

namespace TutClientNet8
{
    public static class CryptoHelper
    {
        private const byte Key = 0x5A;

        public static string SimpleXor(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= Key;
            return Convert.ToBase64String(bytes);
        }
    }
}