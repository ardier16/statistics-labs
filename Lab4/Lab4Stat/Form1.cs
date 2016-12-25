using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace Lab4Stat
{
    public partial class Form1 : Form
    {
        Stopwatch timer = new Stopwatch();
        private Excel.Application ExcelApp;
        private Excel.Workbook WorkBookExcel;
        private Excel.Worksheet WorkSheetExcel;
        private Excel.Range RangeExcel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] x = textBox1.Text.Split(';').Select(t => Convert.ToDouble(t)).ToArray();
            double[] y = textBox2.Text.Split(';').Select(t => Convert.ToDouble(t)).ToArray();

            Calculate(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] x = new double[Convert.ToInt32(numericUpDown1.Value)];

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            double[] y = new double[Convert.ToInt32(numericUpDown1.Value)];
            Random r = new Random();

            double k = Convert.ToDouble(numericUpDown2.Value);
            double b = Convert.ToDouble(numericUpDown3.Value);
            double range = 1 - 0.01 * trackBar1.Value;

            for (int i = 0; i < y.Length; i++)
            {
                double s = k * x[i] + b;
                double y1 = k * (x[x.Length - 1] - x[0]);
                double m = range * y1;

                if (m >= 0)
                    y[i] = s + r.Next((int)-m, (int)m);
                else
                    y[i] = s + r.Next((int)m, (int)-m);
            }


            FillStrings(x, y);
            Calculate(x, y);
        }

        private void FillStrings(double[] x, double[] y)
        {
            string xs = "";

            for (int j = 0; j < x.Length; j++)
            {
                xs += x[j] + "; ";
            }

            string ys = "";

            for (int j = 0; j < y.Length; j++)
            {
                ys += y[j] + "; ";
            }

            textBox1.Text = xs.Substring(0, xs.Length - 2);
            textBox2.Text = ys.Substring(0, ys.Length - 2);

        }

        private void Calculate(double[] x, double[] y)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    timer.Start();

                    LSM myReg = new LSM(x, y);
                    myReg.Polynomial(1);
                    FormulaField.Text = "y = " + Math.Round(myReg.Coeff[1], 3) + "*x + " + Math.Round(myReg.Coeff[0], 3);
                    CorrelationField.Text = myReg.Rky.ToString();
                    DeterminationCoef.Text = myReg.DeterminationCoef.ToString();
                    SSEField.Text = myReg.SSE.ToString();

                    timer.Stop();
                    TimeField.Text = timer.ElapsedMilliseconds + " ms";
                    timer.Reset();

                    DrawGraphs(x, y, myReg.Coeff[1], myReg.Coeff[0]);
                    DrawRests(x, myReg.Rest);
                }
                else
                {
                    timer.Start();

                    Regression r = new Regression(x, y);
                    FormulaField.Text = "y = " + r.K + "*x + " + r.B;
                    CorrelationField.Text = r.Rky.ToString();
                    double[] rr = r.Rest;
                    DeterminationCoef.Text = r.DeterminationCoef.ToString();
                    SSEField.Text = r.SSE.ToString();

                    timer.Stop();
                    TimeField.Text = timer.ElapsedMilliseconds + " ms";
                    timer.Reset();

                    DrawGraphs(x, y, r.K, r.B);
                    DrawRests(x, r.Rest);
                }
            }  
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DrawGraphs(double[] x, double[] y, double k, double b)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            chart1.Palette = ChartColorPalette.EarthTones;
            chart1.ChartAreas.Add(new ChartArea());
            ChartArea area = chart1.ChartAreas[0];


            this.chart1.Series.Add("Graph");

            for (int i = 0; i < x.Length; i++)
            {
                this.chart1.Series[0].Points.AddXY(x[i], b + k * x[i]);
            }

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Color = Color.Black;
            chart1.Series[0].BorderWidth = 3;


            this.chart1.Series.Add("Points");

            chart1.Series[1].Points.DataBindXY(x, y);
            chart1.Series[1].ChartType = SeriesChartType.Point;
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
        }

        private void DrawRests(double[] x, double[] rest)
        {
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();
            chart2.Titles.Clear();

            chart2.Palette = ChartColorPalette.EarthTones;
            chart2.ChartAreas.Add(new ChartArea());

            chart2.Series.Add("Rest");
            chart2.Series[0].Points.DataBindXY(x, rest);

            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[0].Color = Color.Green;
            chart2.Series[0].BorderWidth = 3;
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = System.IO.Path.GetFileName(openDialog.FileName);

                ExcelApp = new Excel.Application();
                //Книга.
                WorkBookExcel = ExcelApp.Workbooks.Open(openDialog.FileName);
                //Таблица.
                // WorkSheetExcel = ExcelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                //    RangeExcel = null;

                List<double> x = new List<double>();
                List<double> y = new List<double>();


                for (int k = 0; k < WorkBookExcel.Sheets.Count; k++)
                {
                    WorkSheetExcel = (Excel.Worksheet)WorkBookExcel.Sheets[k+1];

                    var lastCell = WorkSheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

                    for (int i = 0; i < (int)lastCell.Row; i++)
                    {
                        x.Add(Convert.ToDouble(WorkSheetExcel.Cells[i + 1, 1].Text.ToString()));
                        y.Add(Convert.ToDouble(WorkSheetExcel.Cells[i + 1, 2].Text.ToString()));
                    }
                }

                WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
                ExcelApp.Quit(); // вышел из Excel
                GC.Collect(); // убрал за собой

                FillStrings(x.ToArray(), y.ToArray());
            }
            else
            {
                MessageBox.Show("Файл не выбран!", "Информация");
                return;
            }
            
        }
    }
}
