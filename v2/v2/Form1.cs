﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using v2.Helper;
using v2.Model;
using System.Windows.Forms.DataVisualization.Charting;
using static v2.ProteinExperimentDataReader;
using System.Threading;

namespace v2
{
    public partial class Form1 : Form
    {

        string files_txt_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\files.txt";
        string quant_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.Quant.csv";
        string RateConst_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.RateConst.csv";
        ProteinExperimentDataReader proteinExperimentData;
        Thread allProteinExporterThread;

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string path)
        {
            InitializeComponent();
            if (path.Trim().Length > 0)
            {
                try
                {
                    txt_source.Text = path;

                    string[] filePaths = Directory.GetFiles(path);
                    var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                    if (csvfilePaths.Count == 0)
                    {
                        MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
                    }
                    else
                    {
                        var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                        comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please select valid directory", "Error");
                }
            }
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
            chart1.ChartAreas[0].AxisX.Title = "Time (labeling duration)";
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (labeling duration)";

            chart_peptide.ChartAreas[0].AxisY.Title = "Relative abundance \n of monoisotope";
            chart1.ChartAreas[0].AxisY.Title = "Fractional protein \n synthesis";
            chart_peptide.ChartAreas[0].AxisY.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;

            chart1.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart1.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart_peptide.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart_peptide.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);



            // chart add legend
            chart_peptide.Series["Series3"].LegendText = "Theoretical fit";
            chart_peptide.Series["Series1"].LegendText = "Experimental value";

            chart1.Series["Series2"].LegendText = "Theoretical fit";
            chart1.Series["Series1"].LegendText = "Experimental value";

            // chart legend size
            chart_peptide.Legends[0].Position.Auto = false;
            chart_peptide.Legends[0].Position.X = 65;
            chart_peptide.Legends[0].Position.Y = 10;
            chart_peptide.Legends[0].Position.Width = 30;
            chart_peptide.Legends[0].Position.Height = 15;

            chart1.Legends[0].Position.Auto = false;
            chart1.Legends[0].Position.X = 65;
            chart1.Legends[0].Position.Y = 0;
            chart1.Legends[0].Position.Width = 30;
            chart1.Legends[0].Position.Height = 15;

            // cahrt font
            chart_peptide.Legends[0].Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 9);
            chart1.Legends[0].Font = new Font(chart1.Legends[0].Font.FontFamily, 9);

            // chartline tension
            chart_peptide.Series["Series3"]["LineTension"] = "0.1";
            chart1.Series["Series2"]["LineTension"] = "0.1";

            chart_peptide.Series["Series3"].BorderWidth = 1;
            chart1.Series["Series2"].BorderWidth = 1;

            chart_peptide.Series["Series3"].Color = Color.Navy;
            chart1.Series["Series2"].Color = Color.Navy;



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
            //var step = 0.1;
            var step = temp_maxval / 200.0;
            List<double> yval = new List<double>();

            for (int i = 0; i * step < temp_maxval; i++)
            {
                var temp_x = step * i;
                temp_xval.Add(temp_x);
                yval.Add(1 - Math.Pow(Math.E, (double)(-1 * proteinExperimentData.MeanRateConst_CorrCutOff_mean * temp_x)));
            }

            chart1.Series["Series2"].Points.DataBindXY(temp_xval, yval);

            chart1.ChartAreas[0].AxisX.Interval = temp_maxval / 10;
            chart_peptide.ChartAreas[0].AxisY.Interval = yval.Max() / 5;
            chart_peptide.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";


        }
        public void loadDataGridView()
        {

            // update the datasource for the data gridview
            var selected = (from u in proteinExperimentData.peptides
                            where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                            select u).Distinct().ToList();
            dataGridView_peptide.DataSource = selected;

            //hide some columns from the datasource
            dataGridView_peptide.RowHeadersVisible = false; // hide row selector
            dataGridView_peptide.Columns["UniqueToProtein"].Visible = false;
            dataGridView_peptide.Columns["Total_Labeling"].Visible = false;

            dataGridView_peptide.Columns["M0"].Visible = false;
            dataGridView_peptide.Columns["M1"].Visible = false;
            dataGridView_peptide.Columns["M2"].Visible = false;
            dataGridView_peptide.Columns["M3"].Visible = false;
            dataGridView_peptide.Columns["M4"].Visible = false;
            dataGridView_peptide.Columns["M5"].Visible = false;

            //rename column name
            dataGridView_peptide.Columns["PeptideSeq"].HeaderText = "Peptide";
            dataGridView_peptide.Columns["SeqMass"].HeaderText = "m/z";
            dataGridView_peptide.Columns["IsotopeDeviation"].HeaderText = "Isotope Deviation";
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].HeaderText = "Exchangeable \nHydrogens";
            dataGridView_peptide.Columns["Rateconst"].HeaderText = "Rate \nconstant";
            dataGridView_peptide.Columns["RSquare"].HeaderText = "R" + "\u00B2";
            dataGridView_peptide.Columns["RMSE_value"].HeaderText = "RMSE";

            //set size for the columns
            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.MinimumWidth = 70;
            }
            dataGridView_peptide.Columns["Charge"].Width = 50;
            dataGridView_peptide.Columns["RSquare"].Width = 50;
            dataGridView_peptide.Columns["RMSE_value"].Width = 50;
            dataGridView_peptide.Columns["Rateconst"].Width = 90;
            dataGridView_peptide.Columns["PeptideSeq"].MinimumWidth = 170;
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].MinimumWidth = 100;

            //set number formationg for the columns
            dataGridView_peptide.Columns["RSquare"].DefaultCellStyle.Format = "#0.#0";
            dataGridView_peptide.Columns["RMSE_value"].DefaultCellStyle.Format = "#0.####";

            // resizeable columns
            dataGridView_peptide.AllowUserToResizeColumns = true;
            dataGridView_peptide.ColumnHeadersHeightSizeMode =
         DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            dataGridView_peptide.AllowUserToResizeRows = false;
            dataGridView_peptide.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // column selected hightlight
            dataGridView_peptide.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.HeaderCell.Style.SelectionBackColor = Color.White;
                column.HeaderCell.Style.SelectionForeColor = Color.Black;
            }
        }


        public void loadPeptideChart(string peptideSeq, int charge, double Rateconst, double RSquare, double masstocharge, List<RIA> mergedRIAvalues, List<ExpectedI0Value> expectedI0Valuespassedvalue)
        {
            try
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

                var expected_chart_data = expectedI0Valuespassedvalue.Where(x => x.peptideseq == peptideSeq & x.charge == charge).OrderBy(x => x.time).ToArray();
                List<double> x_val = expected_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                List<double> y_val = expected_chart_data.Select(x => x.value).ToList();
                var pep = proteinExperimentData.peptides.Where(x => x.PeptideSeq == peptideSeq).FirstOrDefault();

                chart_peptide.Series["Series3"].Points.DataBindXY(x_val, y_val);
                // set x axis chart interval
                chart_peptide.ChartAreas[0].AxisX.Interval = x_val.Max() / 10;
                chart_peptide.ChartAreas[0].AxisY.Interval = y_val.Max() / 5;
                chart_peptide.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";



                #endregion

                // chart title
                //chart_peptide.Titles.Add(peptideSeq);    

                Title title = new Title();
                title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                //title.Text = peptideSeq + " (K = " + Rateconst.ToString() + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ")";
                title.Text = peptideSeq + " (k = " + formatdoubletothreedecimalplace(Rateconst) + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ", m/z = " + masstocharge.ToString("#0.###") + ", z = " + charge.ToString() + ")";
                chart_peptide.Titles.Add(title);

                chart_peptide.ChartAreas[0].AxisY.Maximum = y_val.Max() + 0.1;



            }
            catch (Exception e)
            {
                Console.WriteLine("Error => loadPeptideChart(), " + e.Message);
            }

        }

        public bool exportchart(string path, string name)
        {

            name = name.Replace("/", "");

            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
                System.IO.Directory.CreateDirectory(path);

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
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    string path = dialog.SelectedPath + "\\" + comboBox_proteinNameSelector.SelectedValue.ToString();

                    if (exportchart(path, chart_peptide.Titles[0].Text))
                    {
                        MessageBox.Show("Chart Exported!");
                    }
                    else
                    {
                        MessageBox.Show("File not genrated!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void button_exportAllPeptideChart_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    //string path = dialog.SelectedPath;
                    string path = dialog.SelectedPath + "\\" + comboBox_proteinNameSelector.SelectedValue.ToString();
                    //copy chart1
                    System.IO.MemoryStream myStream = new System.IO.MemoryStream();
                    Chart chart2 = new Chart();
                    chart_peptide.Serializer.Save(myStream);
                    chart2.Serializer.Load(myStream);

                    var selected = (from u in proteinExperimentData.peptides
                                    where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                                    select u).Distinct().ToList();
                    int count = 1;
                    //foreach (Peptide p in proteinExperimentData.peptides)
                    foreach (Peptide p in selected)
                    {
                        //clear chart area
                        chart2.Titles.Clear();

                        #region experimental data plot

                        // prepare the chart data
                        var chart_data = this.proteinExperimentData.mergedRIAvalues.Where(x => x.peptideSeq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        chart2.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                        #endregion

                        #region expected data plot 

                        var expected_chart_data = this.proteinExperimentData.expectedI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        //
                        List<double> x_val = expected_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                        List<double> y_val = expected_chart_data.Select(x => x.value).ToList();

                        chart2.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

                        // set x axis chart interval
                        chart2.ChartAreas[0].AxisX.Interval = expected_chart_data.Select(x => x.time).ToArray().Max() / 10;
                        chart2.ChartAreas[0].AxisY.Interval = expected_chart_data.Select(x => x.value).ToArray().Max() / 5;
                        chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";



                        #endregion


                        // chart title
                        //chart2.Titles.Add(p.PeptideSeq + "(K=" + p.Rateconst.ToString() + ", " + ")");
                        Title title = new Title();
                        title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                        //title.Text = p.PeptideSeq + " (K = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ")";
                        title.Text = p.PeptideSeq + " (k = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                        chart2.Titles.Add(title);

                        chart2.ChartAreas[0].AxisY.Maximum = (double)(chart_data.Select(x => x.RIA_value).Max() + 0.1);

                        bool exists = System.IO.Directory.Exists(path);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(path);
                        try
                        {
                            using (Bitmap im = new Bitmap(chart_peptide.Width, chart_peptide.Height))
                            {
                                chart2.DrawToBitmap(im, new Rectangle(0, 0, chart2.Width, chart2.Height));

                                im.Save(path + @"\" + count.ToString() + "_" + p.PeptideSeq + "_" + p.Charge.ToString() + ".jpeg");
                            }

                        }
                        catch (Exception he)
                        {

                            Console.WriteLine("ERROR: exporting chart for " + (count + 1).ToString() + " " + p.PeptideSeq + "===>" + he.Message);
                        }

                        count++;

                    }

                    MessageBox.Show("done!!");

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void dataGridView_peptide_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIndex = e.RowIndex;
            //if (rowIndex >= 0)
            //{
            //    DataGridViewRow row = dataGridView_peptide.Rows[rowIndex];
            //    var temp = dataGridView_peptide.Rows[rowIndex].Cells[0].Value.ToString();
            //    var charge = int.Parse(dataGridView_peptide.Rows[rowIndex].Cells[4].Value.ToString());
            //    var rateconst = double.Parse(dataGridView_peptide.Rows[rowIndex].Cells[2].Value.ToString());
            //    var rsquare = double.Parse(dataGridView_peptide.Rows[rowIndex].Cells[3].Value.ToString());

            //    loadPeptideChart(temp, charge, rateconst, rsquare, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

            //}
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
                    MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
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

            //string files_txt_path = txt_source.Text + @"\files.centroid.txt"; 
            string files_txt_path = txt_source.Text + @"\files.txt";
            string quant_csv_path = txt_source.Text + @"\" + proteinName + ".Quant.csv";
            string RateConst_csv_path = txt_source.Text + @"\" + proteinName + ".RateConst.csv";

            var temppath = files_txt_path.Replace("files.txt", "files.centroid.txt");
            if (File.Exists(temppath)) files_txt_path = temppath;

            if (!File.Exists(files_txt_path)) { MessageBox.Show("filex.txt is not available in the specified directory.", "Error"); return; }
            if (!File.Exists(quant_csv_path)) { MessageBox.Show(proteinName + ".Quant.csv" + " is not available in the specified directory.", "Error"); return; }
            if (!File.Exists(RateConst_csv_path)) { MessageBox.Show(proteinName + ".RateConst.csv" + "filex.txt is not available in the specified directory.", "Error"); return; }

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.mergeMultipleRIAPerDay2();
            proteinExperimentData.computeExpectedCurvePoints();
            proteinExperimentData.computeExpectedCurvePointsBasedOnExperimentalIo();
            proteinExperimentData.computeRSquare();
            ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();
            try
            {


                label4_proteinRateConstantValue.Text = formatdoubletothreedecimalplace((double)proteinExperimentData.MeanRateConst_CorrCutOff_mean) + " \u00B1 " + formatdoubletothreedecimalplace((double)proteinExperimentData.StandDev_NumberPeptides_mean);
                label5_Ic.Text = ((double)proteinExperimentData.TotalIonCurrent_1).ToString("G2");
                loadDataGridView();

                loadProteinchart(chartdata);
                groupBox3_proteinchart.Text = proteinName;

                var p = proteinExperimentData.peptides.First();
                loadPeptideChart(p.PeptideSeq, (int)p.Charge, (double)p.Rateconst, (double)p.RSquare, (double)p.SeqMass, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_expectedI0Values);
            }
            catch (Exception xe)
            {
                Console.WriteLine("Error => computeValuesForEnhancedPerProtienPlot2(), " + xe.Message);
            }

        }

        public string formatdoubletothreedecimalplace(double n)
        {
            var tempval = "";
            if (n > 0.09999999) tempval = String.Format("{0:0.###}", n);
            else if (n > 0.00999999) tempval = String.Format("{0:0.####}", n);
            else if (n > 0.00099999) tempval = String.Format("{0:0.#####}", n);
            else if (n > 0.00009999) tempval = String.Format("{0:0.######}", n);
            else tempval = String.Format("{0:0.######}", n);

            return tempval;
        }

        private void dataGridView_peptide_SelectionChanged(object sender, EventArgs e)
        {
            //////int rowIndex = dataGridView_peptide.SelectedRows[0].Index;
            //////if (rowIndex >= 0)
            //////{
            //////    DataGridViewRow row = dataGridView_peptide.Rows[rowIndex];
            //////    var temp = dataGridView_peptide.Rows[rowIndex].Cells[0].Value.ToString();
            //////    var charge = int.Parse(dataGridView_peptide.Rows[rowIndex].Cells[5].Value.ToString());

            //////    loadPeptideChart(temp, charge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.expectedI0Values);

            //////    //MessageBox.Show(temp);
            //////}

            if (dataGridView_peptide.SelectedRows.Count > 0)
            {

                int indexofselctedrow = dataGridView_peptide.SelectedRows[0].Index;

                if (indexofselctedrow >= 0)
                {
                    DataGridViewRow row = dataGridView_peptide.Rows[indexofselctedrow];
                    var temp = dataGridView_peptide.Rows[indexofselctedrow].Cells[0].Value.ToString();
                    var charge = int.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[4].Value.ToString());
                    var rateconst = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[2].Value.ToString());
                    var rsquare = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[3].Value.ToString());
                    var masstocharge = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[5].Value.ToString());

                    loadPeptideChart(temp, charge, rateconst, rsquare, masstocharge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_expectedI0Values);

                }
            }

        }

        private void button_exportProteinChart_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    string path = dialog.SelectedPath + "\\" + comboBox_proteinNameSelector.SelectedValue.ToString();

                    bool exists = System.IO.Directory.Exists(path);
                    if (!exists)
                        System.IO.Directory.CreateDirectory(path);

                    try
                    {
                        using (Bitmap im = new Bitmap(chart1.Width, chart1.Height))
                        {
                            chart1.DrawToBitmap(im, new Rectangle(0, 0, chart1.Width, chart1.Height));

                            im.Save(path + @"\" + comboBox_proteinNameSelector.SelectedValue.ToString() + ".jpeg");
                        }

                        MessageBox.Show("Chart Exported!");
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("! file not genrated");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ExportAllProteinData

            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    string outputpath = dialog.SelectedPath;
                    string sourcepath = txt_source.Text;

                    MessageBox.Show("This process will take longer time to complete. please check the exported graphs in " + outputpath, "Message");

                    if (outputpath.Length > 0 & sourcepath.Length > 0)
                    {
                        ExportAllProteinData exp = new ExportAllProteinData(sourcepath, outputpath);
                        //exp.Export_all_ProteinChart(chart_peptide, progressBar_exportall);

                        //allProteinExporterThread = new Thread(new ThreadStart(exp.Export_all_ProteinChart(chart_peptide, progressBar_exportall)));
                        allProteinExporterThread = new Thread(() => exp.Export_all_ProteinChart(progressBar_exportall));
                        allProteinExporterThread.Start();

                    }
                    else
                    {
                        MessageBox.Show("Please select a valid directory!");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void button2_cancelled_Click(object sender, EventArgs e)
        {
            if (allProteinExporterThread == null) return;
            try
            {
                allProteinExporterThread.Abort();
                MessageBox.Show("Export cacelled!");
                progressBar_exportall.Value = 0;
                allProteinExporterThread = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message);
                allProteinExporterThread = null;
            }
        }
    }
}
