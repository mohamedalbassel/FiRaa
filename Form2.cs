using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;

namespace Egyptian_FLS_code
{
    public partial class Form2 : Form
    {
        public static List<string> nclas = new List<string>();
     
        
        public int cnull = 0;
        public int c = 0;
        public int count = nclas.Count();
        public Form2()
        {


            InitializeComponent();
            nclas= Egyptian_FLS_code.Form1.eclas;
            foreach (string item in nclas)
            {
                if (item == "null")
                {
                    cnull += 1;

                }
                else
                {
                    c += 1;
                }



            }

            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            //// Define a collection of items to display in the chart 
            SeriesCollection piechartData = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Assigned",
                        Values = new ChartValues<double> {c},

                        DataLabels = true,
                        LabelPoint = labelPoint
                    },
                    new PieSeries
                    {
                        Title = "Missing",
                        Values = new ChartValues<double> {cnull},
                        DataLabels = true,
                        LabelPoint = labelPoint
                    },
                };

            pieChart1.Series = piechartData;
            pieChart1.LegendLocation = LegendLocation.Bottom;



        }
        private void fillChart()
        {

            //chart1.Series["Accuracy"].Points.AddXY("Assigned", c);
            //chart1.Series["Accuracy"].Points.AddXY("Missing", cnull);

            //chart1.Titles.Add("Accuracy Chart");

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            fillChart();
            MessageBox.Show(nclas.Count().ToString(),"Room Count");      


        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void pieChart1_ChildChanged_1(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void pieChart1_ChildChanged_2(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
