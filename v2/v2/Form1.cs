using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadcharts();
            loadGridarea();
        }

        public void loadcharts()
        {

            //# compute chart values 
            double[] k_val = { 0.23, 0.25, 0.31, 0.35 };
            int[] t_val = { 0, 1, 3, 5, 7, 21 };
            List<List<double>> y_vals = new List<List<double>>();

            foreach (double k in k_val)
            {
                List<double> temp_y = new List<double>();
                foreach (int t in t_val)
                {
                    var val = 1 - Math.Pow(Math.E, (-k * t));
                    temp_y.Add(val);
                }
                y_vals.Add(temp_y);

            }


            for (int i = 0; i < k_val.Length; i++)
            {
                for (int j = 0; j < t_val.Length; j++)
                {
                    switch (i)
                    {
                        case 0:
                            chart1.Series["Series1"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 1:
                            chart1.Series["Series2"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 2:
                            chart1.Series["Series3"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 3:
                            chart1.Series["Series4"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                    }

                }
            }

            // add lables 
            //chart1.Series["Series1"].Label = " k = " + k_val[0];
            //chart1.Series["Series2"].Label = " k = " + k_val[1];
            //chart1.Series["Series3"].Label = " k = " + k_val[2];
            //chart1.Series["Series4"].Label = " k = " + k_val[3];

            // remove grid lines
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart add legend
            chart1.Series["Series1"].LegendText = " k = " + k_val[0];
            chart1.Series["Series2"].LegendText = " k = " + k_val[1];
            chart1.Series["Series3"].LegendText = " k = " + k_val[2];
            chart1.Series["Series4"].LegendText = " k = " + k_val[3];

            // chart labels added 
            chart1.ChartAreas[0].AxisX.Title = "Time (days)";
            chart1.ChartAreas[0].AxisY.Title = "Lys0/LysTotal";
        }

        public void loadGridarea()
        {

            //prepare the raw data

            DataTable dt = new DataTable();
            dt = new DataTable();
            dt.Columns.Add("Peptide", typeof(string));
            dt.Rows.Add("sample Peptide 1");
            dt.Rows.Add("sample Peptide 2");
            dt.Rows.Add("sample Peptide 3");
            dt.Rows.Add("sample Peptide 4");
            dt.Rows.Add("sample Peptide 5");
            dt.Rows.Add("sample Peptide 6");
            dt.Rows.Add("sample Peptide 7");
            dt.Rows.Add("sample Peptide 8");
            dt.Rows.Add("sample Peptide 9");

            //Bind data to grid view
            dataGridView1.DataSource = dt;


            // hide row selector
            dataGridView1.RowHeadersVisible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                var temp = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

                double[] k_val = { 0.23, 0.25, 0.31, 0.35 };
                int[] t_val = { 0, 1, 3, 5, 7, 21 };

                t_val = t_val.Select(x => x * (1+rowIndex)).ToArray();
                k_val = k_val.Select(x => x + (x/(rowIndex+1))).ToArray();

                loadcharts(k_val, t_val);

                MessageBox.Show(temp);
            }


        }

        public void loadcharts(double[] k_val, int[] t_val)
        {

            //double[] k_val = { 0.23, 0.25, 0.31, 0.35 };
            //int[] t_val = { 0, 1, 3, 5, 7, 21 };

            //clear the chart 
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
                chart1.Update();

                
            }


            //# compute chart values 
            List<List<double>> y_vals = new List<List<double>>();

            foreach (double k in k_val)
            {
                List<double> temp_y = new List<double>();
                foreach (int t in t_val)
                {
                    var val = 1 - Math.Pow(Math.E, (-k * t));
                    temp_y.Add(val);
                }
                y_vals.Add(temp_y);

            }


            for (int i = 0; i < k_val.Length; i++)
            {
                for (int j = 0; j < t_val.Length; j++)
                {
                    switch (i)
                    {
                        case 0:
                            chart1.Series["Series1"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 1:
                            chart1.Series["Series2"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 2:
                            chart1.Series["Series3"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 3:
                            chart1.Series["Series4"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                    }

                }
            }

            // add lables 
            //chart1.Series["Series1"].Label = " k = " + k_val[0];
            //chart1.Series["Series2"].Label = " k = " + k_val[1];
            //chart1.Series["Series3"].Label = " k = " + k_val[2];
            //chart1.Series["Series4"].Label = " k = " + k_val[3];

            // remove grid lines
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart add legend
            chart1.Series["Series1"].LegendText = " k = " + k_val[0];
            chart1.Series["Series2"].LegendText = " k = " + k_val[1];
            chart1.Series["Series3"].LegendText = " k = " + k_val[2];
            chart1.Series["Series4"].LegendText = " k = " + k_val[3];

            // chart labels added 
            chart1.ChartAreas[0].AxisX.Title = "Time (days)";
            chart1.ChartAreas[0].AxisY.Title = "Lys0/LysTotal";

            chart1.Update();
        }

    }
}
