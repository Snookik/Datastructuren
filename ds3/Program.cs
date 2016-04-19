using System;
using System.IO;

namespace Datastructuren3
{
    // Petar Kostic
    // Studentennummer: 4075897
    class Program
    {
        static void Main(string[] args)
        {
            // First line for the amount of fractions
            int aantal_lines = int.Parse(Console.ReadLine());

            // Declare local jagged array
            double[][] jaggedArray = new double[3][];

            //Create new arrays in the jagged array
            jaggedArray[0] = new double[aantal_lines]; // Teller
            jaggedArray[1] = new double[aantal_lines]; // Noemer
            jaggedArray[2] = new double[aantal_lines]; // Deelsom (breuk)

            for (int i = 0; i < aantal_lines; i++)
            {
                string input_string = Console.ReadLine();
                string[] input = input_string.Split(' ');

                double teller = double.Parse(input[0]);
                double noemer = double.Parse(input[1]);
                double breuk = teller / noemer;

                jaggedArray[0][i] = teller;
                jaggedArray[1][i] = noemer;
                jaggedArray[2][i] = breuk;
            }

            // Voer een quicksort uit op de 3de array
            HybridSort(jaggedArray, 0, jaggedArray[2].Length - 1);

            //// WRITE ALL DATA
            //for (int j = 0; j < jaggedArray.Length; j++)
            //{
            //    double[] innerArray = jaggedArray[j];
            //    for (int a = 0; a < innerArray.Length; a++)
            //    {
            //        Console.Write(innerArray[a] + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.ReadLine();

            // WRITE DOMJUDGE ACCEPTED INPUT
            for (int i = 0; i < aantal_lines; i++)
            {
                Console.WriteLine(jaggedArray[0][i].ToString() + " " + jaggedArray[1][i].ToString());
            }
        }

        static void HybridSort(double[][] jaggedArray, int left, int right)
        {
            if ((right - left) < 9)
                InsertionSort(jaggedArray, left, right);
            else
                QuickSort(jaggedArray, left, right);
        }

        public static void QuickSort(double[][] jaggedArray, int left, int right)
        {
            if (left < right)
            {
                double q = RandomPartition(jaggedArray, left, right);
                QuickSort(jaggedArray, left, (int)q - 1);
                QuickSort(jaggedArray, (int)q + 1, right);
            }
        }

        private static double RandomPartition(double[][] jaggedArray, int left, int right)
        {
            Random random = new Random();
            int i = random.Next(left, right);

            double[] pivot = new double[3];
            pivot[0] = jaggedArray[0][i];
            pivot[1] = jaggedArray[1][i];
            pivot[2] = jaggedArray[2][i];

            jaggedArray[2][i] = jaggedArray[2][right];
            // ---------------------------------------ook voor array [1] en [0]
            jaggedArray[1][i] = jaggedArray[1][right];
            jaggedArray[0][i] = jaggedArray[0][right];
            // ---------------------------------------
            jaggedArray[2][right] = pivot[2];
            // ---------------------------------------ook voor array [1] en [0]
            jaggedArray[1][right] = pivot[1];
            jaggedArray[0][right] = pivot[0];
            // ---------------------------------------

            return Partition(jaggedArray, left, right);
        }

        private static double Partition(double[][] jaggedArray, int left, int right)
        {
            double[] pivot = new double[3];
            pivot[0] = jaggedArray[0][right];
            pivot[1] = jaggedArray[1][right];
            pivot[2] = jaggedArray[2][right];
            double temp;

            int i = left;
            for (int j = left; j < right; j++)
            {
                if (jaggedArray[2][j] <= pivot[2]) // Alleen if-statement voor array [2] en pivot[2]
                {
                    temp = jaggedArray[2][j];
                    jaggedArray[2][j] = jaggedArray[2][i];
                    jaggedArray[2][i] = temp;
                    // --------------------------------------- ook voor array [1] en [0]
                    temp = jaggedArray[1][j];
                    jaggedArray[1][j] = jaggedArray[1][i];
                    jaggedArray[1][i] = temp;

                    temp = jaggedArray[0][j];
                    jaggedArray[0][j] = jaggedArray[0][i];
                    jaggedArray[0][i] = temp;
                    // ---------------------------------------- 
                    i++;
                }
            }

            jaggedArray[2][right] = jaggedArray[2][i];
            // ------------------------------------------ ook voor array [1] en [0]
            jaggedArray[1][right] = jaggedArray[1][i];
            jaggedArray[0][right] = jaggedArray[0][i];
            // ------------------------------------------ 

            jaggedArray[2][i] = pivot[2];
            // ------------------------------------------ ook voor array [1] en [0]
            jaggedArray[1][i] = pivot[1];
            jaggedArray[0][i] = pivot[0];
            // ------------------------------------------

            return i;
        }

        static void InsertionSort(double[][] jaggedArray, int left, int right)
        {
            for (int i = left; i < right + 1; i++)
            {
                double[] temp = new double[3];
                temp[0] = jaggedArray[0][i];
                temp[1] = jaggedArray[1][i];
                temp[2] = jaggedArray[2][i];

                int j = i - 1;
                while (j >= 0 && temp[2] < jaggedArray[2][j])
                {
                    jaggedArray[2][j + 1] = jaggedArray[2][j];
                    // ------------------------------------------ ook voor array [1] en [0]
                    jaggedArray[1][j + 1] = jaggedArray[1][j];
                    jaggedArray[0][j + 1] = jaggedArray[0][j];
                    // ------------------------------------------ 
                    j--;
                }
                jaggedArray[2][j + 1] = temp[2];
                // ------------------------------------------ ook voor array [1] en [0]
                jaggedArray[1][j + 1] = temp[1];
                jaggedArray[0][j + 1] = temp[0];
                // ------------------------------------------ 
            }
        }
    }
}




