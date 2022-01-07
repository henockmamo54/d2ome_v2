using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using v2.Helper;
using v2.Model;
using System.Windows.Forms.DataVisualization.Charting;

namespace v2
{
    public partial class Form1 : Form
    {

        string files_txt_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\files.txt";
        string quant_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.Quant.csv";
        string RateConst_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.RateConst.csv";
        ProteinExperimentDataReader proteinExperimentData;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);
            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.mergeMultipleRIAPerDay();
            proteinExperimentData.computeExpectedCurvePoints();


            loadDataGridView();
            loadPeptideChart("TSVNVVR", proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

            //ReadFilesInfo_txt filesinfo = new ReadFilesInfo_txt();
            //ReadExperiments experiInfoReader = new ReadExperiments();
            //ReadRateConstants experiInfoReader = new ReadRateConstants();



        }

        public void loadDataGridView()
        {
            var selected = (from u in proteinExperimentData.peptides
                            where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                            select u).ToList();


            dataGridView_peptide.DataSource = selected;
            dataGridView_peptide.RowHeadersVisible = false; // hide row selector
            dataGridView_peptide.Columns["UniqueToProtein"].Visible = false;
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].Visible = false;
            //dataGridView1.Columns["M0"].Visible = false;
            //dataGridView1.Columns["M1"].Visible = false;
            //dataGridView1.Columns["M2"].Visible = false;
            //dataGridView1.Columns["M3"].Visible = false;
            //dataGridView1.Columns["M4"].Visible = false;
            //dataGridView1.Columns["M5"].Visible = false;


            dataGridView_peptide.Columns["PeptideSeq"].HeaderText = "Peptide";
            dataGridView_peptide.Columns["SeqMass"].HeaderText = "m/z";
            dataGridView_peptide.Columns["Total_Labeling"].HeaderText = "Total Labeling";

            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.MinimumWidth = 70;
            }
            dataGridView_peptide.Columns["Charge"].Width = 50;
            dataGridView_peptide.Columns["PeptideSeq"].MinimumWidth = 170;
        }

        public void loadMultiplePeptideChart(List<string> peptideSeqs, List<RIA> mergedRIAvalues)
        {

            foreach (string peptideSeq in peptideSeqs)
            {

            }

        }

        public void loadPeptideChart(string peptideSeq, List<RIA> mergedRIAvalues, List<ProteinExperimentDataReader.ExpectedI0Value> expectedI0Values)
        {
            //clear chart area
            chart_peptide.Titles.Clear();

            #region experimental data plot

            // prepare the chart data
            var chart_data = mergedRIAvalues.Where(x => x.peptideSeq == peptideSeq).OrderBy(x => x.time).ToArray();
            chart_peptide.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

            #endregion

            #region expected data plot 

            var expected_chart_data = expectedI0Values.Where(x => x.peptideseq == peptideSeq).OrderBy(x => x.time).ToArray();
            chart_peptide.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

            #endregion

            // remove grid lines
            chart_peptide.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart labels added 
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (days)";
            chart_peptide.ChartAreas[0].AxisY.Title = "RIA";

            // chart add legend
            chart_peptide.Series["Series3"].LegendText = "Expected Value";
            chart_peptide.Series["Series1"].LegendText = "Experimental value";

            // chart title
            chart_peptide.Titles.Add(peptideSeq);
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
                            chart_peptide.Series["Series1"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 1:
                            chart_peptide.Series["Series2"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 2:
                            chart_peptide.Series["Series3"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 3:
                            chart_peptide.Series["Series4"].Points.AddXY(t_val[j], y_vals[i][j]);
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
            chart_peptide.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart add legend
            chart_peptide.Series["Series1"].LegendText = " k = " + k_val[0];
            chart_peptide.Series["Series2"].LegendText = " k = " + k_val[1];
            chart_peptide.Series["Series3"].LegendText = " k = " + k_val[2];
            chart_peptide.Series["Series4"].LegendText = " k = " + k_val[3];

            // chart labels added 
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (days)";
            chart_peptide.ChartAreas[0].AxisY.Title = "Lys0/LysTotal";
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
            dataGridView_peptide.DataSource = dt;


            // hide row selector
            dataGridView_peptide.RowHeadersVisible = false;
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_peptide.Rows[rowIndex];
                var temp = dataGridView_peptide.Rows[rowIndex].Cells[0].Value.ToString();

                loadPeptideChart(temp, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

                MessageBox.Show(temp);
            }
        }

        public void loadcharts(double[] k_val, int[] t_val)
        {

            //double[] k_val = { 0.23, 0.25, 0.31, 0.35 };
            //int[] t_val = { 0, 1, 3, 5, 7, 21 };

            //clear the chart 
            foreach (var series in chart_peptide.Series)
            {
                series.Points.Clear();
                chart_peptide.Update();


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
                            chart_peptide.Series["Series1"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 1:
                            chart_peptide.Series["Series2"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 2:
                            chart_peptide.Series["Series3"].Points.AddXY(t_val[j], y_vals[i][j]);
                            break;
                        case 3:
                            chart_peptide.Series["Series4"].Points.AddXY(t_val[j], y_vals[i][j]);
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
            chart_peptide.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart add legend
            chart_peptide.Series["Series1"].LegendText = " k = " + k_val[0];
            chart_peptide.Series["Series2"].LegendText = " k = " + k_val[1];
            chart_peptide.Series["Series3"].LegendText = " k = " + k_val[2];
            chart_peptide.Series["Series4"].LegendText = " k = " + k_val[3];

            // chart labels added 
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (days)";
            chart_peptide.ChartAreas[0].AxisY.Title = "Lys0/LysTotal";

            chart_peptide.Update();
        }

        public bool exportchart()
        {
            try
            {
                using (Bitmap im = new Bitmap(chart_peptide.Width, chart_peptide.Height))
                {
                    chart_peptide.DrawToBitmap(im, new Rectangle(0, 0, chart_peptide.Width, chart_peptide.Height));
                    //using (Graphics gr = Graphics.FromImage(im))
                    //{
                    //    gr.DrawString("Test",
                    //        new Font(FontFamily.GenericSerif, 10, FontStyle.Bold),
                    //        new SolidBrush(Color.Red), new PointF(10, 10));
                    //}
                    im.Save("F:\\workplace\\d2ome_v2\\test.jpeg");


                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool exportchart(string name)
        {
            try
            {
                using (Bitmap im = new Bitmap(chart_peptide.Width, chart_peptide.Height))
                {
                    chart_peptide.DrawToBitmap(im, new Rectangle(0, 0, chart_peptide.Width, chart_peptide.Height));
                    using (Graphics gr = Graphics.FromImage(im))
                    {
                        gr.DrawString(name,
                            new Font(FontFamily.GenericSerif, 10, FontStyle.Bold),
                            new SolidBrush(Color.Red), new PointF(10, 10));
                    }
                    im.Save("F:\\workplace\\d2ome_v2\\" + name + ".jpeg");
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool exportchart(string path, string name)
        {
            try
            {
                using (Bitmap im = new Bitmap(chart_peptide.Width, chart_peptide.Height))
                {
                    chart_peptide.DrawToBitmap(im, new Rectangle(0, 0, chart_peptide.Width, chart_peptide.Height));

                    im.Save(path + @"\" + name + ".jpeg");
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;

                if (exportchart(path, chart_peptide.Titles[0].Text))
                {
                    MessageBox.Show("Chart Exported!");
                }
                else
                {
                    MessageBox.Show("! file not genrated");
                }
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;
                //copy chart1
                System.IO.MemoryStream myStream = new System.IO.MemoryStream();
                Chart chart2 = new Chart();
                chart_peptide.Serializer.Save(myStream);
                chart2.Serializer.Load(myStream);

                foreach (Peptide p in proteinExperimentData.peptides)
                {

                    //clear chart area
                    chart2.Titles.Clear();

                    #region experimental data plot

                    // prepare the chart data
                    var chart_data = this.proteinExperimentData.mergedRIAvalues.Where(x => x.peptideSeq == p.PeptideSeq).OrderBy(x => x.time).ToArray();
                    chart2.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                    #endregion

                    #region expected data plot 

                    var expected_chart_data = this.proteinExperimentData.expectedI0Values.Where(x => x.peptideseq == p.PeptideSeq).OrderBy(x => x.time).ToArray();
                    chart2.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

                    #endregion

                    // remove grid lines
                    chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

                    // chart labels added 
                    chart2.ChartAreas[0].AxisX.Title = "Time (days)";
                    chart2.ChartAreas[0].AxisY.Title = "RIA";

                    // chart add legend
                    chart2.Series["Series3"].LegendText = "Expected Value";
                    chart2.Series["Series1"].LegendText = "Experimental value";

                    // chart title
                    chart2.Titles.Add(p.PeptideSeq);

                    try
                    {
                        using (Bitmap im = new Bitmap(chart_peptide.Width, chart_peptide.Height))
                        {
                            chart2.DrawToBitmap(im, new Rectangle(0, 0, chart2.Width, chart2.Height));

                            im.Save(path + @"\" + p.PeptideSeq + ".jpeg");
                        }

                    }
                    catch (Exception he)
                    {

                    }



                }

                MessageBox.Show("done!!");

            }


        }
    }
}
