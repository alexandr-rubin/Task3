using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace task3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0)
            {
                Console.WriteLine("Enter an odd number of words separated by a space. Words must be at least 3.");
                return;
            }
            foreach (var item in args)
            {
                if (args.ToString().ToUpper().Contains(item.ToUpper()))
                {
                    Console.WriteLine("You cannot enter duplicate words.");
                    return;
                }
            }
            var key = new byte[16];
            RandomNumberGenerator.Create().GetBytes(key);
            int compMove = RandomNumberGenerator.GetInt32(1, args.Length + 1);
            Console.WriteLine($"HMAC: {Encode(args[compMove - 1], key)}");
            int move;
            while (true)
            {
                Console.WriteLine("Available moves:");
                foreach (var item in args)
                {
                    Console.WriteLine($"{Array.IndexOf(args, item) + 1} - {item}");
                }
                Console.WriteLine("0 - exit");
                Console.Write("Enter your move: ");
                bool result = Int32.TryParse(Console.ReadLine(), out move);
                if (!result)
                    continue;
                if (move == 0)
                    return;
                if (result && move >= 0 && move <= args.Length)
                {
                    Console.WriteLine($"Your move: {args[move - 1]}");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter a number from the proposed.");
                }
            }
            Console.WriteLine($"Computer move: {args[compMove - 1]}");
            WinnerDetermination(move, compMove, args);
            Console.Write("HMAC key: ");
            foreach (var item in key)
            {
                Console.Write(item);
            }
        }

        public static void WinnerDetermination(int move, int compMove, string[] arr)
        {
            if (move == compMove)
            {
                Console.WriteLine("Draw!");
                return;
            }
            for (int i = 0; i < arr.Length / 2; i++)
            {
                if (move == arr.Length)
                {
                    move = 0;
                }
                if (arr[compMove - 1] == arr[move])
                {
                    Console.WriteLine("You lose!");
                    return;
                }
                move++;
            }
            Console.WriteLine("You win!");
        }

        public static string Encode(string input, byte[] key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(input);
            HMACSHA256 hash = new HMACSHA256(key);
            Byte[] hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
