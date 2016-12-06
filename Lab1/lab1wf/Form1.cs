using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1wf
{
    public partial class Form1 : Form
    {
        List<double> data = new List<double>();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                data.Add(Double.Parse(textBox1.Text));
                label1.Text += textBox1.Text + "; ";
                textBox1.Clear();
                textBox1.Focus();
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = Mean(data).ToString();
        }


        public static double Mean(List<double> data)
        {
            return data.Average();
        }


        public static double Median(List<double> data)
        {
            List<double> res = new List<double>();
            res.AddRange(data);
            res.Sort();

            if (res.Count % 2 == 1)
                return res[(res.Count - 1) / 2];

            return (res[res.Count / 2] + res[res.Count / 2 - 1]) / 2;
        }


        public static double Dispersion(List<double> data)
        {
            double dispers = 0;

            foreach (double item in data)
            {
                dispers += Math.Pow(item - Mean(data), 2);
            }

            return dispers / (data.Count - 1);
        }


        public static double Deviation(List<double> data)
        {
            return Math.Sqrt(Dispersion(data));
        }

        public static double MaxValue(List<double> data)
        {
            return data.Max();
        }


        public static double MinValue(List<double> data)
        {
            return data.Min();
        }

        public static double Range(List<double> data)
        {
            return MaxValue(data) - MinValue(data);
        }

        public static double Mode(List<double> data)
        {
            int maxCounted = 0, counter = 0;
            double ModeValue, currentValue;
            int j = 0;
            double[] temp = new double[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                temp[i] = data[i];
            }
            Array.Sort(temp);
            currentValue = temp[0];
            ModeValue = currentValue;
            while (j < data.Count)
            {
                if (temp[j] == currentValue)
                {
                    counter++;
                }
                else
                {
                    if (maxCounted < counter)
                    {
                        maxCounted = counter;
                        ModeValue = currentValue;
                    }
                    currentValue = temp[j];
                    counter = 1;
                }
                j++;
            }

            if (maxCounted < counter)
            {
                maxCounted = counter;
                ModeValue = currentValue;
            }

            return ModeValue;
        }


        public static double Quantile(List<double> data, double q)
        {
            List<double> res = new List<double>();
            res.AddRange(data);
            res.Sort();

            if (q * res.Count < 1)
                return res[0];

            else
                return res[(int)Math.Ceiling(q * res.Count)-1];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = Dispersion(data).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = Median(data).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Text = Deviation(data).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label3.Text = Mode(data).ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label3.Text = Range(data).ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label3.Text = MaxValue(data).ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label3.Text = MinValue(data).ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            data.Clear();
            label1.Text = "Values: ";
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Return")
                button1_Click(null, null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                label3.Text = Quantile(data, Double.Parse(textBox2.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                Random r = new Random();
                int x = r.Next(100);
                data.Add(x);
                label1.Text += x + "; ";
                textBox1.Clear();
                textBox1.Focus();
            }
            catch
            {

            }
        }
    }
}
