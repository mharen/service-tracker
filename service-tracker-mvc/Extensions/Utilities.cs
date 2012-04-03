using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace service_tracker_mvc.Extensions
{
    public static class Utilities
    {
        /// <summary>
        /// Generate a decently long string o random characters, suitable for tokens
        /// </summary>
        /// <returns>a string of gobbledygook</returns>
        public static string GenerateKey()
        {
            var RandomBytes = new byte[
                6 * 10 // use a multiple of 6 to get a full base64 output http://en.wikipedia.org/wiki/Base64
                - 16 // compensate for the 16-byte guid we're going to add in 
                ];

            // fill the buffer with garbage (this is threadsafe)
            BetterRandom.GetBytes(RandomBytes);

            // get a guid, which will be unique enough for us
            var UniqueBytes = Guid.NewGuid().ToByteArray();

            // encode the garbage as friendly, printable characters
            var AllBytes = new byte[RandomBytes.Length + UniqueBytes.Length];
            UniqueBytes.CopyTo(AllBytes, 0);
            RandomBytes.CopyTo(AllBytes, UniqueBytes.Length);

            return Convert.ToBase64String(AllBytes);
        }
        static RandomNumberGenerator BetterRandom = new RNGCryptoServiceProvider();
    }
}