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
using static v2.ProteinExperimentDataReader;

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

        public void loaduiprops()
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1.5;


            // remove grid lines
            chart_peptide.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart labels added 
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (days)";
            chart_peptide.ChartAreas[0].AxisY.Title = "Relative Isotope abundance of monoisotope";

            // chart add legend
            chart_peptide.Series["Series3"].LegendText = "Theoretical value";
            chart_peptide.Series["Series1"].LegendText = "Experimental Value";

            chart1.Series["Series2"].LegendText = "Theoretical value";
            chart1.Series["Series1"].LegendText = "Experimental Value";

            //chart_peptide.Legends[0].Position.Auto = false;
            chart_peptide.Legends[0].Position.X = 65;
            chart_peptide.Legends[0].Position.Y = 10;
            chart_peptide.Legends[0].Position.Width = 20;
            chart_peptide.Legends[0].Position.Height = 15;

            //chart1.Legends[0].Position.Auto = false;
            chart1.Legends[0].Position.X = 65;
            chart1.Legends[0].Position.Y = 0;
            chart1.Legends[0].Position.Width = 20;
            chart1.Legends[0].Position.Height = 15;

            // chartline tension
            chart_peptide.Series["Series3"]["LineTension"] = "0.0";
            chart1.Series["Series2"]["LineTension"] = "0.0";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loaduiprops();
            //proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);
            //proteinExperimentData.loadAllExperimentData();
            //proteinExperimentData.computeRIAPerExperiment();
            //proteinExperimentData.mergeMultipleRIAPerDay2();
            //proteinExperimentData.computeExpectedCurvePoints();
            //proteinExperimentData.computeRSquare();
            //ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();



            //loadDataGridView();
            //loadPeptideChart("TSVNVVR", 2, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);
            //loadProteinchart(chartdata);

            ////ReadFilesInfo_txt filesinfo = new ReadFilesInfo_txt();
            ////ReadExperiments experiInfoReader = new ReadExperiments();
            ////ReadRateConstants experiInfoReader = new ReadRateConstants();


        }
        public void loadProteinchart(ProtienchartDataValues chartdata)
        {
            chart1.Series["Series1"].Points.DataBindXY(chartdata.x, chartdata.y);
            //chart1.ChartAreas[0].AxisX.Maximum = proteinExperimentData.Experiment_time.Max();

            var temp_xval = new List<double>();
            var temp_maxval = proteinExperimentData.Experiment_time.Max();
            var step = 0.1;
            List<double> yval = new List<double>();

            for (int i = 0; i * step < temp_maxval; i++)
            {
                var temp_x = step * i;
                temp_xval.Add(temp_x);
                yval.Add(1 - Math.Pow(Math.E, (double)(-1 * proteinExperimentData.MeanRateConst_CorrCutOff_mean * temp_x)));
            }

            chart1.Series["Series2"].Points.DataBindXY(temp_xval, yval);

        }
        public void loadDataGridView()
        {
            var selected = (from u in proteinExperimentData.peptides
                            where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                            select u).Distinct().ToList();


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

            //set size for the columns
            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.MinimumWidth = 70;
            }
            dataGridView_peptide.Columns["Charge"].Width = 50;
            dataGridView_peptide.Columns["PeptideSeq"].MinimumWidth = 170;

            //set number formationg for the columns
            dataGridView_peptide.Columns["RSquare"].DefaultCellStyle.Format = "#0.#0";

            // resizeable columns
            dataGridView_peptide.AllowUserToResizeColumns = true;
            dataGridView_peptide.ColumnHeadersHeightSizeMode =
         DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            dataGridView_peptide.AllowUserToResizeRows = false;
            dataGridView_peptide.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }


        public void loadPeptideChart(string peptideSeq, int charge, List<RIA> mergedRIAvalues, List<ProteinExperimentDataReader.ExpectedI0Value> expectedI0Values)
        {
            //clear chart area
            chart_peptide.Titles.Clear();

            #region experimental data plot

            // prepare the chart data
            var chart_data = mergedRIAvalues.Where(x => x.peptideSeq == peptideSeq & x.charge == charge).OrderBy(x => x.time).ToArray();
            chart_peptide.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

            chart_peptide.ChartAreas[0].AxisX.Minimum = 0;
            //chart_peptide.ChartAreas[0].AxisX.IsMarginVisible = false;

            #endregion

            #region expected data plot 

            var expected_chart_data = expectedI0Values.Where(x => x.peptideseq == peptideSeq).OrderBy(x => x.time).ToArray();
            chart_peptide.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

            #endregion

            // chart title
            chart_peptide.Titles.Add(peptideSeq);

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

        private void button_exportPeptideChart_Click(object sender, EventArgs e)
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

        private void button_exportAllPeptideChart_Click(object sender, EventArgs e)
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

        private void dataGridView_peptide_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_peptide.Rows[rowIndex];
                var temp = dataGridView_peptide.Rows[rowIndex].Cells[0].Value.ToString();
                var charge = int.Parse(dataGridView_peptide.Rows[rowIndex].Cells[5].Value.ToString());

                loadPeptideChart(temp, charge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

                //MessageBox.Show(temp);
            }
        }

        private void btn_Browsefolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;

                txt_source.Text = path;

                string[] filePaths = Directory.GetFiles(path);
                var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                if (csvfilePaths.Count == 0)
                {
                    MessageBox.Show("This directory doesn't contain the necessary files. Please select another diroctory.");
                }
                else
                {
                    var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                    comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                }
            }
        }

        private void comboBox_proteinNameSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBox_proteinNameSelector.SelectedValue.ToString());
            // plot chart inofrormation for the selected protien
            string proteinName = comboBox_proteinNameSelector.SelectedValue.ToString();
            string files_txt_path = txt_source.Text + @"\files.txt";
            string quant_csv_path = txt_source.Text + @"\" + proteinName + ".Quant.csv";
            string RateConst_csv_path = txt_source.Text + @"\" + proteinName + ".RateConst.csv";

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.mergeMultipleRIAPerDay2();
            proteinExperimentData.computeExpectedCurvePoints();
            proteinExperimentData.computeRSquare();
            ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();
            try
            {
                label4_proteinRateConstantValue.Text = proteinExperimentData.MeanRateConst_CorrCutOff_mean.ToString();
                loadDataGridView();

                loadProteinchart(chartdata);
                groupBox3_proteinchart.Text = proteinName;

                var p = proteinExperimentData.peptides.First();
                loadPeptideChart(p.PeptideSeq, (int)p.Charge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);
            }
            catch (Exception xe)
            {
                Console.WriteLine("error ploting grpah => " + xe.Message);
            }

        }

        private void dataGridView_peptide_SelectionChanged(object sender, EventArgs e)
        {
            //int rowIndex = dataGridView_peptide.SelectedRows[0].Index;
            //if (rowIndex >= 0)
            //{
            //    DataGridViewRow row = dataGridView_peptide.Rows[rowIndex];
            //    var temp = dataGridView_peptide.Rows[rowIndex].Cells[0].Value.ToString();
            //    var charge = int.Parse(dataGridView_peptide.Rows[rowIndex].Cells[5].Value.ToString());

            //    loadPeptideChart(temp, charge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

            //    //MessageBox.Show(temp);
            //}

        }
    }
}
