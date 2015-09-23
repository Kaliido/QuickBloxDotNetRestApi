using System;
using System.Security.Cryptography;
using System.Text;

namespace Kaliido.QuickBlox
{
    public static class GlobalHelper
    {

        public static readonly Encoding encoding = Encoding.ASCII;

        public static int getNonce()
        {
            return new Random().Next(1, 10000);
        }

        public static string getTimestamp()
        {
            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;

            return ticks.ToString();
        }

        public static byte[] getHash(string inputString, string key)
        {
            var keyByte = encoding.GetBytes(key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(encoding.GetBytes(inputString));

                //Console.WriteLine("Result: {0}", ByteToString(hmacsha256.Hash));
                return hmacsha1.Hash;
            }

        }

    }
}