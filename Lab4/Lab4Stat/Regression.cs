using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4Stat
{
     public class Regression
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }

        public double AvgX { get { return X.Average(); } }
        public double AvgY { get { return Y.Average(); } }
        private double AvgXY
        {
            get
            {
                double[] XY = new double[X.Length];

                for (int i = 0; i < XY.Length; i++)
                {
                    XY[i] = X[i] * Y[i];
                }

                return XY.Average();
            }
        }


        public double DispersionX { get { return X.Select(x => x * x).Average() - AvgX * AvgX; } }
        public double DispersionY { get { return Y.Select(y => y * y).Average() - AvgY * AvgY; } }


        public double DeltaX { get { return Math.Sqrt(DispersionX); } }
        public double DeltaY { get { return Math.Sqrt(DispersionY); } }


        public double Rky
        {
            get
            {
                if (DeltaY == 0)
                    return 1;

                return Math.Round((AvgXY - AvgX * AvgY) / (DeltaX * DeltaY), 3);
            }
        }
        public double DeterminationCoef { get { return Math.Round(Rky * Rky, 3); } }


        public double K { get { return Math.Round(Rky * DeltaY / DeltaX, 3); } }
        public double B { get { return Math.Round(AvgY - Rky * AvgX * DeltaY / DeltaX, 3); } }


        public double[] Rest
        {
            get
            {
                double[] rest = new double[X.Length];

                for (int i = 0; i < rest.Length; i++)
                {
                    rest[i] = Math.Round(Y[i] - (K * X[i] + B), 3);
                }

                return rest;
            }
        }

        public double SSE
        {
            get
            {
                double sse = 0;

                for (int i = 0; i < Y.Length; i++)
                {
                    sse += Math.Pow(Y[i] - (K * X[i] + B), 2);
                }

                return Math.Round(sse, 3);
            }
        }

        public Regression(double[] x, double[] y)
        {
            X = x;
            Y = y;
        }
    }
}
