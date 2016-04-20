using System;
using System.IO;

namespace Datastructuren2
{
    // Petar Kostic
    // Studentennummer: 4075897
    class Program
    {
        public static void Main(string[] args)
        {
            string firstline = Console.ReadLine();
            string[] aby = firstline.Split(' ');

            // Parses van eerste line, zodat we dit niet constant hoeven te doen.
            long alpha = long.Parse(aby[0]);
            long beta = long.Parse(aby[1]);
            int gamma = int.Parse(aby[2]);

            // Array aanmaken die de het aantal levels respectievelijk met het aantal punten bijhoudt.
            long[] userInput = new long[gamma];
            for (int counter = 1; counter < gamma; counter++)
            {
                userInput[counter] = alpha;
                alpha = alpha + (long)(Math.Ceiling((double)alpha / (double)beta));
            }

            bool check = true;
            while (check == true)
            {
                // --------------------------------------
                // |       Check if valid string        |
                // --------------------------------------
                string value_string = Console.ReadLine();
                long value;
                check = long.TryParse(value_string, out value);
                if (!check)
                    break;

                // --------------------------------------
                // |            Binary Search           |
                // --------------------------------------
                int min = 0, max = gamma - 1;
                while (min < max)
                {
                    int mid = (min + max) / 2;

                    if (userInput[mid] < value)
                        min = mid + 1;
                    else
                        max = mid;
                } 
                // -------------------------------------
                // |            Output                 |
                // -------------------------------------
                
                if (userInput[min] == value)
                    Console.WriteLine(min + 1);
                // Het zal vaak voorkomen dat de input niet overeenkomt met
                // het aantal punten in de array, daarom moeten we de vorige index uit de array pakken
                // Dit kunnen we eenvoudig doen door value (de input) te checken met wat is overgebleven
                // van de binarysearch.
                else if (userInput[max] > value)
                    Console.WriteLine(min);
                else if (userInput[max] < value)
                    Console.WriteLine(gamma);
            }
        }
    }
}

