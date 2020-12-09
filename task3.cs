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
            string str = Console.ReadLine();
            if (str.Split(' ').Length < 3 || str.Split(' ').Length % 2 == 0)
            {
                Console.WriteLine("Enter an odd number of words separated by a space. Words must be at least 3.");
                return;
            }
            List<string> listOfMoves = new List<string>();
            foreach (var item in str.Split(' '))
            {
                if (!listOfMoves.ConvertAll(l => l.ToUpper()).Contains(item.ToUpper()))
                {
                    listOfMoves.Add(item);
                }
                else
                {
                    Console.WriteLine("You cannot enter duplicate words.");
                    return;
                }
            }
            var key = new byte[16];
            RandomNumberGenerator.Create().GetBytes(key);
            int compMove = RandomNumberGenerator.GetInt32(1, listOfMoves.Count + 1);
            Console.WriteLine($"HMAC: {Encode(listOfMoves[compMove - 1], key)}");
            int move;
            while (true)
            {
                Console.WriteLine("Available moves:");
                foreach (var item in listOfMoves)
                {
                    Console.WriteLine($"{listOfMoves.IndexOf(item) + 1} - {item}");
                }
                Console.WriteLine("0 - exit");
                Console.Write("Enter your move: ");
                bool result = Int32.TryParse(Console.ReadLine(), out move);
                if (!result)
                    continue;
                if (move == 0)
                    return;
                if (result && move >= 0 && move <= listOfMoves.Count)
                {
                    Console.WriteLine($"Your move: {listOfMoves[move - 1]}");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter a number from the proposed.");
                }
            }
            Console.WriteLine($"Computer move: {listOfMoves[compMove - 1]}");
            WinnerDetermination(move, compMove, listOfMoves);
            Console.Write("HMAC key: ");
            foreach (var item in key)
            {
                Console.Write(item);
            }
        }

        public static void WinnerDetermination(int move, int compMove, List<string> list)
        {
            if (move == compMove)
            {
                Console.WriteLine("Draw!");
                return;
            }
            for (int i = 0; i < list.Count / 2; i++)
            {
                if (move == list.Count)
                {
                    move = 0;
                }
                if (list[compMove - 1] == list[move])
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
