using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using static System.Net.Mime.MediaTypeNames;


namespace SSHA
{
    class StringHelper
    {
        private static readonly Random random = new Random();

        private const int randomSymbolsDefaultCount = 8;
        private const string availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static int randomSymbolsIndex = 0;

        public string GetRandomSymbols()
        {
            return GetRandomSymbols(randomSymbolsDefaultCount);
        }

        public string GetRandomSymbols(int count)
        {
            var index = randomSymbolsIndex;
            var result = new string(
                Enumerable.Repeat(availableChars, count)
                          .Select(s => {
                              index += random.Next(s.Length);
                              if (index >= s.Length)
                                  index -= s.Length;
                              return s[index];
                          })
                          .ToArray());
            randomSymbolsIndex = index;
            Console.WriteLine("Salt: " + result);
            return result;
        }
    }

    class SHA256Algorithm
    {
        public string SaltedSHA256Hash(string text)
        {
            StringHelper stringHelper = new StringHelper();

            SHA256 sha256 = SHA256.Create();
            StringBuilder sb = new StringBuilder(text.Length * 2);

            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(text + stringHelper.GetRandomSymbols(16)));

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    class Program
    {
        void runAlgorithms()
        {
            SHA256Algorithm a = new SHA256Algorithm();

            Console.WriteLine("Enter your plaintext: ");
            string plaintext = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("SHA256 Hash: " + a.SaltedSHA256Hash(plaintext));
        }
        public static void Main(string[] args)
        {

            Program p = new Program();

            p.runAlgorithms();
            Console.ReadLine();
        }
    }
}
