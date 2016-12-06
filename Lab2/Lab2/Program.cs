using System;

namespace Lab2
{
    class Program
    {
        public static double Average(int[][] arr)
        {
            double sum = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[0].Length; j++)
                {
                    sum += arr[i][j];
                }
            }

            return sum / (arr.Length * arr[0].Length);
        }

        public static double OverallSum(int[][] arr)
        {
            double sum = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[0].Length; j++)
                {
                    sum += Math.Pow(arr[i][j] - Average(arr), 2);
                }
            }

            return sum;
        }

        public static double FactSum(double[] arr, int q, double avg)
        {
            double sum = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                sum += Math.Pow(arr[i] - avg, 2);
            }

            return q * sum;
        } 

        public static double RestSum(double overallSum, double factSum)
        {
            return overallSum - factSum;
        }

        public static double FactDispersion(double factsum, int p)
        {
            return factsum / (p - 1);
        }

        public static double RestDispersion(double restSum, int p, int q)
        {
            return restSum / (p * (q - 1));
        }

        public static double FisherCriteria(double factDispersion, double restDispersion)
        {
            return factDispersion / restDispersion;
        }

        static void Main(string[] args)
        {
            int[][] arr = new int[5][];
            arr[0] = new int[] { 36, 56, 52, 39 };
            arr[1] = new int[] { 47, 61, 57, 57 };
            arr[2] = new int[] { 50, 64, 59, 63 };
            arr[3] = new int[] { 58, 66, 58, 61 };
            arr[4] = new int[] { 67, 66, 79, 65 };

            double[] avgs = new double[] { 51.6, 62.6, 61.0, 57.0 };
            int p = arr[0].Length;
            int q = arr.Length;
            double avg = Average(arr);
            double overallSum = OverallSum(arr);
            double factSum = FactSum(avgs, q, avg);
            double restSum = RestSum(overallSum, factSum);
            double factDispersion = FactDispersion(factSum, p);
            double restDispersion = RestDispersion(restSum, p, q);
            double fisherCriteria = FisherCriteria(factDispersion, restDispersion);

            // Fк = F(a, p-1, p*(q-1)) = F(0.05, 3, 16) = 3.24

            double Fk = 3.24;

            Console.WriteLine("Average: " + avg);
            Console.WriteLine("Overall Sum: " + overallSum);
            Console.WriteLine("Factor Sum: " + factSum);
            Console.WriteLine("Rest Sum: " + restSum);
            Console.WriteLine("Factor Dispersion: " + factDispersion);
            Console.WriteLine("Rest Dispersion: " + restDispersion);
            Console.WriteLine("Fisher-Snedecor Criteria: " + fisherCriteria);
            Console.WriteLine("Fcr: " + Fk);

            if (fisherCriteria < Fk)
                Console.WriteLine("Null Hypothesis is accepted");
            else
                Console.WriteLine("Null Hypothesis is rejected");


            Console.Read();
        }
    }
}
