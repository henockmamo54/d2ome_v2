﻿using System;
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

        private void Form1_Load(object sender, EventArgs e)
        {

            //proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);
            //proteinExperimentData.loadAllExperimentData();
            //proteinExperimentData.computeRIAPerExperiment();
            //proteinExperimentData.mergeMultipleRIAPerDay();
            //proteinExperimentData.computeExpectedCurvePoints();
            //proteinExperimentData.computeRSquare();
            //ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot();



            //loadDataGridView();
            //loadPeptideChart("TSVNVVR", proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);
            //loadProteinchart(chartdata);

            //ReadFilesInfo_txt filesinfo = new ReadFilesInfo_txt();
            //ReadExperiments experiInfoReader = new ReadExperiments();
            //ReadRateConstants experiInfoReader = new ReadRateConstants();

        }
        public void loadProteinchart(ProtienchartDataValues chartdata)
        {

            chart1.Series["Series1"].Points.DataBindXY(chartdata.x, chartdata.y);
            chart_peptide.ChartAreas[0].AxisX.Minimum = 0;

            List<double> yval = new List<double>();
            foreach (int t in proteinExperimentData.Experiment_time)
            {
                yval.Add(1 - Math.Pow(Math.E, -1 * 0.135301 * t));
            }

            chart1.Series["Series2"].Points.DataBindXY(proteinExperimentData.Experiment_time, yval);
            chart1.ChartAreas[0].AxisX.Minimum = 0;
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

            chart_peptide.ChartAreas[0].AxisX.Minimum = 0;
            //chart_peptide.ChartAreas[0].AxisX.IsMarginVisible = false;

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
            chart_peptide.ChartAreas[0].AxisY.Title = "Relative Isotope abundance of monoisotope";

            // chart add legend
            chart_peptide.Series["Series3"].LegendText = "Expected Value";
            chart_peptide.Series["Series1"].LegendText = "Experimental value";

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

                loadPeptideChart(temp, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

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
                var csvfilePaths = filePaths.Where(x => x.Contains(".csv")).ToList();

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

            string files_txt_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\files.txt";
            string quant_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.Quant.csv";
            string RateConst_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.RateConst.csv"; 

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);
            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.mergeMultipleRIAPerDay();
            proteinExperimentData.computeExpectedCurvePoints();
            proteinExperimentData.computeRSquare();
            ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot();



            loadDataGridView();
            loadPeptideChart(proteinExperimentData.peptides.First().PeptideSeq, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);
            loadProteinchart(chartdata);

        }

    }
}
