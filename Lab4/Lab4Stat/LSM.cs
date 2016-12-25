using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4Stat
{
    public class LSM
    {
        public double[] X { get; set; }
        public double[] Y { get; set; }

        private double[] coeff;
        public double[] Coeff { get { return coeff; } }
        
        public LSM(double[] x, double[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException("X and Y arrays should be equal!");
            X = new double[x.Length];
            Y = new double[y.Length];

            for (int i = 0; i < x.Length; i++)
            {
                X[i] = x[i];
                Y[i] = y[i];
            }
        }

        public void Polynomial(int m)
        {
            if (m <= 0) throw new ArgumentException("Порядок полинома должен быть больше 0");
            if (m >= X.Length) throw new ArgumentException("Порядок полинома должен быть на много меньше количества точек!");

            double[,] basic = new double[X.Length, m + 1];

            for (int i = 0; i < basic.GetLength(0); i++)
                for (int j = 0; j < basic.GetLength(1); j++)
                    basic[i, j] = Math.Pow(X[i], j);

            Matrix basicFuncMatr = new Matrix(basic);
            Matrix transBasicFuncMatr = basicFuncMatr.Transposition();
            Matrix lambda = transBasicFuncMatr * basicFuncMatr;
            Matrix beta = transBasicFuncMatr * new Matrix(Y);
            Matrix a = lambda.InverseMatrix() * beta;

            coeff = new double[a.Row];

            for (int i = 0; i < coeff.Length; i++)
            {
                coeff[i] = a.Args[i, 0];
            }
        }

        private double getDelta()
        {

            double[] dif = new double[Y.Length];
            double[] f = new double[X.Length];

            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < coeff.Length; j++)
                {
                    f[i] += coeff[j] * Math.Pow(X[i], j);
                }
                dif[i] = Math.Pow((f[i] - Y[i]), 2);
            }
            return Math.Sqrt(dif.Sum() / X.Length);
        }

        public double[] Rest
        {
            get
            {
                double[] rest = new double[X.Length];

                for (int i = 0; i < rest.Length; i++)
                {
                    rest[i] = Math.Round(Y[i] - (coeff[1] * X[i] + coeff[0]), 3);
                }

                return rest;
            }
        }

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


        public double DispersionX
        {
            get
            {
                return X.Select(x => x * x).Average() - AvgX * AvgX;
            }
        }
        public double DispersionY
        {
            get
            {
                return Y.Select(y => y * y).Average() - AvgY * AvgY;
            }
        }


        public double DeltaX { get { return Math.Sqrt(DispersionX); } }
        public double DeltaY { get { return Math.Sqrt(DispersionY); } }


        public double Rky
        {
            get
            {
                if (DeltaY == 0)
                    return 1;

                return Math.Round(coeff[1]*DeltaX/DeltaY, 3);
            }
        }
        public double DeterminationCoef { get { return Math.Round(Rky * Rky, 3); } }

        public double SSE
        {
            get
            {
                double sse = 0;

                for (int i = 0; i < Y.Length; i++)
                {
                    sse += Math.Pow(Y[i] - (coeff[1] * X[i] + coeff[0]), 2);
                }

                return Math.Round(sse, 3);
            }
        }
    }
}
