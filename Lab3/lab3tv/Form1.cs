using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Troschuetz.Random;

namespace lab3tv
{
    public partial class Distributions : Form
    {
        Random r = new Random();
        Stopwatch time = new Stopwatch();

        public Distributions()
        {
            InitializeComponent();
        }

        double Uniform(double a, double b)
        {
            return a + r.NextDouble() * (b - a);
        }

        double Exponential(double lambda)
        {
            return -Math.Log(Uniform(0, 1)) / lambda;
        }

        double BoxMuller(bool isFirst, double mean, double dev)
        {
            double u, v, s;

            do {
                u = Uniform(-1, 1);
                v = Uniform(-1, 1);
                s = u * u + v * v;
            } while (s >= 1);

            double r = Math.Sqrt(-2 * Math.Log(s) / s);

            if (isFirst)
                return r * v * dev + mean;
            else
                return r * u  * dev + mean;
        }


        Dictionary<double, int> GetDict(double[] arr)
        {
            Dictionary<double, int> dict = new Dictionary<double, int>();

            Array.Sort(arr);
            for (int i = 0; i < arr.Length; i++)
            {

                if (dict.ContainsKey(arr[i]))
                {
                    dict[arr[i]]++;
                }
                else
                    dict.Add(arr[i], 1);
            }

            return dict;
        }

        double[] FillUniformArray(int l, int dec, double a, double b)
        {
            double[] arr = new double[l];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Math.Round(Uniform(a, b), dec);
            }

            return arr;
        }

        double[] FillExpArray(int l, int dec)
        {
            double[] a = new double[l];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = Math.Round(Exponential(4), dec);
            }

            return a;
        }

        double[] FillZigArr(int l, int dec)
        {
            double[] a = new double[l];
            setupNormalTables();

            for (int i = 0; i < a.Length / 2; i++)
            {
                //a[i] = Math.Round(BoxMuller(i % 2 == 0, 0.5, 0.09), 3);
                a[i] = Math.Round(Normal(0.5, 0.125), dec);
                a[a.Length - i - 1] = -Math.Round(Normal(0.5, 0.125), dec) + 1;
            }

            return a;
        }

        double[] FillMullerArr(int l, int dec)
        {
            double[] a = new double[l];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = Math.Round(BoxMuller(i % 2 == 0, 0.5, 0.125), dec);
            }

            return a;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //time.Start();
            //double[] array = FillUniformArray(Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value),
            //                                        (double)numericUpDown1.Value, (double)numericUpDown2.Value);
            //time.Stop();
            //label2.Text = time.ElapsedMilliseconds + " ms";
            //time.Reset();

            //Dictionary<double, int> d = GetDict(array);
            //List<double> keys = d.Keys.ToList();
            //List<int> values = d.Values.ToList();

            ////this.chart1.Palette = ChartColorPalette.Fire;
            ////ChartArea area = chart1.ChartAreas[0];
            ////area.AxisX.Minimum = 0;
            ////area.AxisX.Maximum = 1;
            //////area.AxisX.Minimum = keys[0];
            //////area.AxisX.Maximum = keys[keys.Count - 1];
            ////this.chart1.Titles.Add("Title");

            ////chart1.Series.Add(new Series("Palette"));
            ////this.chart1.Series[0].Points.DataBindXY(keys, values);
            //////for (int i = 0; i < keys.Count; i++)
            ////{
            ////    if (keys[i] > 1)
            ////        break;

            ////    Series series = this.chart1.Series.Add(keys[i].ToString());

            ////    // Add point.
            ////    series.Points.Add(values[i]);
            ////}

            //this.chart1.Palette = ChartColorPalette.Fire;
            //ChartArea area = chart1.ChartAreas[0];
            //area.AxisX.Minimum = 0;
            //area.AxisX.Maximum = 1;
            //chart1.Series.Add(new Series());
            //chart1.Series[0].ChartType = SeriesChartType.Line;

            //for (int i = 0; i < array.Length; i++)
            //{
            //    chart1.Series[0].Points.AddXY(array[i], (array[i] - (double) numericUpDown1.Value) / (double) (numericUpDown2.Value - numericUpDown1.Value));
            //}
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        #region Ziggurat
        static double[] stairWidth = new double[257], stairHeight = new double[256];
        const double x1 = 3.6541528853610088;
        const double A = 4.92867323399e-3; 

        void setupNormalTables()
        {
            // coordinates of the implicit rectangle in base layer
            stairHeight[0] = Math.Exp(-0.5 * x1 * x1);
            stairWidth[0] = A / stairHeight[0];
            // implicit value for the top layer
            stairWidth[256] = 0;
            for (int i = 1; i <= 255; ++i)
            {
                // such x_i that f(x_i) = y_{i-1}
                stairWidth[i] = Math.Sqrt(-2 * Math.Log(stairHeight[i - 1]));
                stairHeight[i] = stairHeight[i - 1] + A / stairWidth[i];
            }
        }

        double NormalZiggurat()
        {
            int iter = 0;
            do
            {
                int B = r.Next();
                int stairId = B & 255;
                double x = Uniform(0, stairWidth[stairId]);

                if (x < stairWidth[stairId + 1])
                    return (B > 0) ? x : -x;
                if (stairId == 0) 
                {
                    double z = -1;
                    double y;

                    if (z > 0) 
                    {
                        x = Exponential(x1);
                        z -= 0.5 * x * x;
                    }
                    if (z <= 0) 
                    {
                        do
                        {
                            x = Exponential(x1);
                            y = Exponential(1);
                            z = y - 0.5 * x * x; 
                        } while (z <= 0);
                    }
                    x += x1;
                    return (B > 0) ? x : -x;
                }
                
                if (Uniform(stairHeight[stairId - 1], stairHeight[stairId]) < Math.Exp(-.5 * x * x))
                    return (B > 0) ? x : -x;
            } while (++iter <= 1e9);

            return -1; 
        }

        double Normal(double mu, double sigma)
        {
            return mu + NormalZiggurat() * sigma;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            time.Start();
            double[] array = FillExpArray(Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value));
            time.Stop();
            label2.Text = time.ElapsedMilliseconds + " ms";
            time.Reset();

            Dictionary<double, int> d = GetDict(array);
            List<double> keys = d.Keys.ToList();
            List<int> values = d.Values.ToList();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            this.chart1.Palette = ChartColorPalette.Fire;
            chart1.ChartAreas.Add(new ChartArea());
            ChartArea area = chart1.ChartAreas[0];
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 1;
            this.chart1.Titles.Add("Exponential Distribution");

            chart1.Series.Add(new Series("Exponential"));
            this.chart1.Series[0].Points.DataBindXY(keys, values);

            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();

            this.chart2.Palette = ChartColorPalette.Fire;
            chart2.ChartAreas.Add(new ChartArea());
            ChartArea area1 = chart2.ChartAreas[0];
            area1.AxisX.Minimum = 0;
            area1.AxisX.Maximum = 1;
            chart2.Series.Add(new Series());
            chart2.Series[0].Name = "Practical";
            chart2.Series[0].ChartType = SeriesChartType.Line;

            for (int i = 0; i < array.Length; i++)
            {
                chart2.Series[0].Points.AddXY(array[i], (1 - Math.Pow(Math.E, -4 * array[i])));
            }

            chart2.Series.Add(new Series());
            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[1].Name = "Teoretical";

            for (double i = 0; i < 1; i+=0.001)
            {
                chart2.Series[1].Points.AddXY(i, (1 - Math.Pow(Math.E, -4 * i)));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            time.Start();
            double[] array = FillZigArr(Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value));
            time.Stop();
            label2.Text = time.ElapsedMilliseconds + " ms";
            time.Reset();

            Dictionary<double, int> d = GetDict(array);
            List<double> keys = d.Keys.ToList();
            List<int> values = d.Values.ToList();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            this.chart1.Palette = ChartColorPalette.Fire;
            chart1.ChartAreas.Add(new ChartArea());
            ChartArea area = chart1.ChartAreas[0];
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 1;
            this.chart1.Titles.Add("Ziggurat Distribution");

            chart1.Series.Add(new Series("Ziggurat"));
            this.chart1.Series[0].Points.DataBindXY(keys, values);


            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();

            this.chart2.Palette = ChartColorPalette.Fire;
            chart2.ChartAreas.Add(new ChartArea());
            ChartArea area1 = chart2.ChartAreas[0];

            chart2.Series.Add(new Series());
            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[0].Name = "Practical";


            for (int i = 0; i < array.Length; i++)
            {
                chart2.Series[0].Points.AddXY(array[i], 0.5 * (1 + Erf((array[i] - 0.5) / (Math.Sqrt(2 * 0.09)))));
            }

            chart2.Series.Add(new Series());
            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[1].Name = "Teoretical";

            for (double i = 0; i < 1; i += 0.001)
            {
                chart2.Series[1].Points.AddXY(i, 0.5 * (1 + Erf((i - 0.5) / (Math.Sqrt(2 * 0.09)))));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            time.Start();
            double[] array = FillMullerArr(Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value));
            time.Stop();
            label2.Text = time.ElapsedMilliseconds + " ms";
            time.Reset();

            Dictionary<double, int> d = GetDict(array);
            List<double> keys = d.Keys.ToList();
            List<int> values = d.Values.ToList();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            this.chart1.Palette = ChartColorPalette.Fire;
            chart1.ChartAreas.Add(new ChartArea());
            ChartArea area = chart1.ChartAreas[0];
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 1;
            this.chart1.Titles.Add("Box-Muller Distribution");

            chart1.Series.Add(new Series("Box-Muller"));
            this.chart1.Series[0].Points.DataBindXY(keys, values);


            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();

            this.chart2.Palette = ChartColorPalette.Fire;
            chart2.ChartAreas.Add(new ChartArea());
            ChartArea area1 = chart2.ChartAreas[0];

            chart2.Series.Add(new Series());
            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[0].Name = "Practical";


            for (int i = 0; i < array.Length; i++)
            {
                chart2.Series[0].Points.AddXY(array[i], 0.5 * (1 + Erf((array[i] - 0.5) / (Math.Sqrt(2 * 0.09)))));
            }

            chart2.Series.Add(new Series());
            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[1].Name = "Teoretical";

            for (double i = 0; i < 1; i += 0.001)
            {
                chart2.Series[1].Points.AddXY(i, 0.5 * (1 + Erf((i - 0.5) / (Math.Sqrt(2 * 0.09)))));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            time.Start();
            double[] array = FillUniformArray(Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value),
                                                    (double)numericUpDown1.Value, (double)numericUpDown2.Value);
            time.Stop();
            label2.Text = time.ElapsedMilliseconds + " ms";
            time.Reset();

            Dictionary<double, int> d = GetDict(array);
            List<double> keys = d.Keys.ToList();
            List<int> values = d.Values.ToList();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            this.chart1.Palette = ChartColorPalette.Fire;
            chart1.ChartAreas.Add(new ChartArea());
            ChartArea area = chart1.ChartAreas[0];
            area.AxisX.Minimum = (double)numericUpDown1.Value;
            area.AxisX.Maximum = (double)numericUpDown2.Value;
            this.chart1.Titles.Add("Uniform Distribution");

            chart1.Series.Add(new Series("Uniform"));
            this.chart1.Series[0].Points.DataBindXY(keys, values);


            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();


            this.chart2.Palette = ChartColorPalette.Fire;
            
            chart2.ChartAreas.Add(new ChartArea());
            ChartArea area1 = chart2.ChartAreas[0];
            
            area1.AxisX.Minimum = (double)numericUpDown1.Value;
            area1.AxisX.Maximum = (double)numericUpDown2.Value;
            chart2.Series.Add(new Series());
            chart2.Series[0].Name = "Practical";
            chart2.Series[0].ChartType = SeriesChartType.Line;

            for (int i = 0; i < array.Length; i++)
            {
                chart2.Series[0].Points.AddXY(array[i], (array[i] - (double)numericUpDown1.Value) / (double)(numericUpDown2.Value - numericUpDown1.Value));
            }

            //chart2.Series.Add(new Series());
            //chart2.Series[1].ChartType = SeriesChartType.Line;
            //chart2.Series[1].Name = "Teoretical";
            //chart2.Series[1].Points.AddXY(numericUpDown1.Value, 0);
            //chart2.Series[1].Points.AddXY(numericUpDown2.Value, 1);

        }



        static double Erf(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }
}
