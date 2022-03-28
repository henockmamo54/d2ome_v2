﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using v2.Helper;
using v2.Model;
using static v2.ProteinExperimentDataReader;
using Labeling_Path;
using MathNet.Numerics.Statistics;
using LBFGS_Library_Call;

namespace v2
{
    public partial class Main : Form
    {
        List<MzMLmzIDFilePair> inputdata = new List<MzMLmzIDFilePair>();
        string files_txt_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\files.txt";
        string quant_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.Quant.csv";
        string RateConst_csv_path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.RateConst.csv";
        ProteinExperimentDataReader proteinExperimentData;
        Thread allProteinExporterThread;
        public bool isvisualizationLoadForThepath = false;

        public Main()
        {
            InitializeComponent();
        }
        public void loadUiProps()
        {
            chart_protein.ChartAreas[0].AxisX.Minimum = 0;
            chart_protein.ChartAreas[0].AxisY.Minimum = 0;
            chart_protein.ChartAreas[0].AxisY.Maximum = 1.5;


            // remove grid lines
            chart_peptide.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_peptide.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            chart_protein.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_protein.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart_protein.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_protein.ChartAreas[0].AxisY.MinorGrid.Enabled = false;



            // chart labels added 
            chart_protein.ChartAreas[0].AxisX.Title = "Time (labeling duration)";
            chart_peptide.ChartAreas[0].AxisX.Title = "Time (labeling duration)";

            chart_peptide.ChartAreas[0].AxisY.Title = "Relative abundance \n of monoisotope";
            chart_protein.ChartAreas[0].AxisY.Title = "Fractional protein \n synthesis";
            chart_peptide.ChartAreas[0].AxisY.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;

            chart_protein.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart_protein.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart_peptide.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);
            chart_peptide.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart_peptide.Legends[0].Font.FontFamily, 10);



            // chart add legend
            chart_peptide.Series["Series3"].LegendText = "Theoretical fit";
            chart_peptide.Series["Series1"].LegendText = "Experimental value";

            chart_protein.Series["Series2"].LegendText = "Theoretical fit";
            chart_protein.Series["Series1"].LegendText = "Experimental value";

            // chart legend size
            chart_peptide.Legends[0].Position.Auto = false;
            chart_peptide.Legends[0].Position.X = 65;
            chart_peptide.Legends[0].Position.Y = 10;
            chart_peptide.Legends[0].Position.Width = 30;
            chart_peptide.Legends[0].Position.Height = 15;

            chart_protein.Legends[0].Position.Auto = false;
            chart_protein.Legends[0].Position.X = 65;
            chart_protein.Legends[0].Position.Y = 0;
            chart_protein.Legends[0].Position.Width = 30;
            chart_protein.Legends[0].Position.Height = 15;

            // cahrt font
            chart_peptide.Legends[0].Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 9);
            chart_protein.Legends[0].Font = new Font(chart_protein.Legends[0].Font.FontFamily, 9);

            // chartline tension
            chart_peptide.Series["Series3"]["LineTension"] = "0.1";
            chart_protein.Series["Series2"]["LineTension"] = "0.1";

            chart_peptide.Series["Series3"].BorderWidth = 2;
            chart_protein.Series["Series2"].BorderWidth = 2;

            chart_peptide.Series["Series3"].Color = Color.Purple;
            chart_protein.Series["Series2"].Color = Color.Purple;



        }

        #region computation

        public void loadDefaultValues()
        {
            comboBox_Enrichment.SelectedIndex = 0;
            comboBox_MS1Data.SelectedIndex = 0;
            comboBox_Rate_Constant_Method.SelectedIndex = 0;
            textBox_massAccuracy.Text = "20.0";
            textBox_ElutionWindow.Text = "1.0";
            textBox_peptideConsistency.Text = "4";
            textBox_peptideScore.Text = "20";
            textBox_protein_score.Text = "40";
            textBox_protein_consistency.Text = "4";

            comboBox_Enrichment.Enabled = false;

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputdata.Count == 0) { MessageBox.Show("Please input mzML and mzID files records!", "Error"); return; }
                if (textBox_outputfolderpath.Text.Length == 0) { MessageBox.Show("Please select a valid output directory!", "Error"); return; }

                string[] filePaths = Directory.GetFiles(textBox_outputfolderpath.Text);
                var csvfilePaths = filePaths.Where(x => x.Contains(".csv") || (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                if (csvfilePaths.Count != 0)
                {
                    MessageBox.Show("There are *.csv files in the output folder: " +
                                    textBox_outputfolderpath.Text + "\n\r" + "They may interfere with output files.\r\n" +
                                    "Remove the csv files from the folder and run the program again.\n\r", "Error");
                }


                var path = textBox_outputfolderpath.Text;
                sortInputDataGridView();

                #region files.txt 

                TextWriter tw = new StreamWriter(path + "\\files.txt");
                //TextWriter tw = new StreamWriter("files.txt");
                string fileContent = "";
                foreach (var x in inputdata)
                {
                    fileContent += x.Time.ToString() + " " + x.MzML_FileName + " " + x.MzID_FileName + " " + x.BWE.ToString() + "\n";
                }

                tw.WriteLine(fileContent);
                tw.Close();

                #endregion

                #region quant.csv

                // massaccuracy
                double massaccuracy = 0;
                if (textBox_massAccuracy.Text.Length > 0)
                {
                    try
                    {
                        massaccuracy = double.Parse(textBox_massAccuracy.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Mass Accuracy. " + textBox_massAccuracy.Text +
                            " Please, enter mass accuracy\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Mass Accuracy. " + textBox_massAccuracy.Text +
                                " Please, enter mass accuracy\n");
                    return;
                }

                // elutionwindow

                double elutionwindow = 0;
                if (textBox_massAccuracy.Text.Length > 0)
                {
                    try
                    {
                        elutionwindow = double.Parse(textBox_ElutionWindow.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Elution Window. " + textBox_ElutionWindow.Text +
                            " Please, re-enter Elution Window\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Elution Window. " + textBox_ElutionWindow.Text +
                           " Please, re-enter Elution Window\n");
                    return;
                }

                // peptidescore

                double peptidescore = 0;
                if (textBox_peptideScore.Text.Length > 0)
                {
                    try
                    {
                        peptidescore = double.Parse(textBox_peptideScore.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Peptide Identification Score. " + textBox_peptideScore.Text +
                            " Please, re-enter Elution Window\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Peptide Identification Score. " + textBox_peptideScore.Text +
                           " Please, re-enter Elution Window\n");
                    return;
                }

                // peptideconsistency

                double peptideconsistency = 0;
                if (textBox_peptideConsistency.Text.Length > 0)
                {
                    try
                    {
                        peptideconsistency = double.Parse(textBox_peptideConsistency.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Peptide Consistency. " + textBox_peptideConsistency.Text +
                            " Please, re-enter. Peptide Consistency value of 4 or higher is suggested\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Peptide Consistency. " + textBox_peptideConsistency.Text +
                            " Please, re-enter. Peptide Consistency value of 4 or higher is suggested\n");
                    return;
                }

                //================================================================
                //================================================================

                // peptidescore

                double protienscore = 0;
                if (textBox_protein_score.Text.Length > 0)
                {
                    try
                    {
                        protienscore = double.Parse(textBox_protein_score.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Protein Identification Score. " + textBox_protein_score.Text +
                            " Please, re-enter Elution Window\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Protein Identification Score. " + textBox_protein_score.Text +
                           " Please, re-enter Elution Window\n");
                    return;
                }

                // peptideconsistency

                double protienconsistency = 0;
                if (textBox_protein_consistency.Text.Length > 0)
                {
                    try
                    {
                        protienconsistency = double.Parse(textBox_protein_consistency.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Protein Consistency. " + textBox_protein_consistency.Text +
                            " Please, re-enter. Peptide Consistency value of 4 or higher is suggested\n");

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Protein Consistency. " + textBox_peptideConsistency.Text +
                            " Please, re-enter. Peptide Consistency value of 4 or higher is suggested\n");
                    return;
                }
                //================================================================
                //================================================================

                // rate constant
                int rate_constant_choice = 0;

                if (comboBox_Rate_Constant_Method.Text == "One Parameter")
                    rate_constant_choice = 1;
                else if (comboBox_Rate_Constant_Method.Text == "Two Parameter")
                    rate_constant_choice = 2;
                else
                {
                    MessageBox.Show("Invalid Rate Constant. " + rate_constant_choice +
                        "  Please, re-enter Rate Constant\n");
                    return;
                }

                double MS1_Type = 0;
                if (comboBox_MS1Data.Text == "Profile")
                    MS1_Type = 0;
                else if (comboBox_MS1Data.Text == "Centroid")
                    MS1_Type = 1;


                string quantstatefile = string.Format(@"mass_accuracy =  {0:f1} ppm  // mass accuracy: either in ppm or Da 
MS1_Type = {1}	// data type of MS1, 1 - centroid, 0 - profile  
protein_score       = {6:f1}     //minimum protein score
peptide_score =  {2:f1} 	// minimum peptide score, ion score in Mascot, default is 1
peptide_expectation = 0.05     // maximum peptide expectation in Mascot
elutiontimewindow   =   {3}  // time window  (mins) to search for elution peak. From the time that highest scoring MS2 was triggered
protein_consistency = {7}  // minimum number of experiments for protein consistency
peptide_consistency = {4}   //mininum number of experiments for a peptide consistency
NParam_RateConst_Fit = {5}	// The model for fitting rate constant. Values are 1, and 2", massaccuracy, MS1_Type, peptidescore,
elutionwindow, peptideconsistency, rate_constant_choice, protienscore, protienconsistency);

                TextWriter tw2 = new StreamWriter(path + "\\quant.state");
                //TextWriter tw2 = new StreamWriter("quant.state");


                tw2.WriteLine(quantstatefile);
                tw2.Close();

                #endregion


                Thread thread = new Thread(new ThreadStart(workThreadFunction));
                thread.Start();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }



        }

        private void workThreadFunction()
        {
            var sExecsFolder = Directory.GetCurrentDirectory();
            Console.WriteLine(sExecsFolder);
            var sCommandFile = sExecsFolder + "\\d2ome.exe " + "files.txt";

            using (Process p = new Process())
            {
                // set start info
                p.StartInfo = new ProcessStartInfo(sExecsFolder + "\\d2ome.exe ", textBox_outputfolderpath.Text + "\\files.txt")
                {
                    //RedirectStandardInput = true,
                    UseShellExecute = true,
                    WorkingDirectory = textBox_outputfolderpath.Text
                };
                // event handlers for output & error
                //p.OutputDataReceived += outputDataReceived;
                //p.ErrorDataReceived += errorDataReceived;

                p.EnableRaisingEvents = true;
                p.Exited += P_Exited;

                // start process
                p.Start();

                button_start.Invoke(new Action(() =>
                 button_start.Enabled = false));

                //wait
                p.WaitForExit();
            }

        }

        public void errorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;
            Console.WriteLine(e.Data);
        }

        public void outputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;

            Console.WriteLine(e.Data);
        }


        private void P_Exited(object sender, EventArgs e)
        {
            try
            {
                Process p = (Process)sender;
                var ret = p.ExitCode;

                button_start.Invoke(new Action(() =>
                     button_start.Enabled = true));

                if (ret == 0)
                {
                    MessageBox.Show("Finished... Please check the for the results in " + textBox_outputfolderpath.Text + " folder.");

                }
                else if (-10 == ret)
                {
                    var stError = "Program terminates with error.\n";
                    //if (1 == this->MS1_type)
                    if (comboBox_MS1Data.SelectedValue.ToString().Trim() == "Centroid")
                    {
                        stError = stError + "The specified MS1 data type is Centroid \n";

                        stError = stError + "It does not match with the MS1 type in mzML file\n";
                    }

                    MessageBox.Show(stError);

                }
                else
                {

                    var stError = "Program terminates with error.\n";

                    MessageBox.Show(stError);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            loadUiProps();
            loadDefaultValues();
            inputdata = new List<MzMLmzIDFilePair>();
            this.dataGridView1_mzMLmzIDData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_mzmlfile.Text.Length > 0)
                {
                    if (File.Exists(textBox_mzmlfile.Text.Trim()))
                    {
                        if (textBox_mzidfile.Text.Length > 0)
                        {
                            if (File.Exists(textBox_mzidfile.Text.Trim()))
                            {
                                var mzmlIDfilerecord = new MzMLmzIDFilePair();
                                mzmlIDfilerecord.MzML_FileName = textBox_mzmlfile.Text.Trim();
                                mzmlIDfilerecord.MzID_FileName = textBox_mzidfile.Text.Trim();
                                mzmlIDfilerecord.Time = double.Parse(textBox_T.Text.Trim());
                                mzmlIDfilerecord.BWE = double.Parse(textBox_BWE.Text.Trim());

                                if (mzmlIDfilerecord.BWE < 0 || mzmlIDfilerecord.BWE > 1)
                                {
                                    MessageBox.Show("BWE should be non-negative and less than 1.0\n"); return;
                                }
                                if (mzmlIDfilerecord.Time < 0)
                                {
                                    MessageBox.Show("T should be non-negative \n"); return;
                                }

                                inputdata.Add(mzmlIDfilerecord);

                                var temp = inputdata;
                                temp = temp.OrderBy(x => x.Time).ToList();
                                dataGridView1_mzMLmzIDData.DataSource = temp;
                                inputdata = new List<MzMLmzIDFilePair>();
                                inputdata = temp;

                                //dataGridView1_records.DataSource = inputdata.ToList();

                                //sucssful insertion of record. clear the input
                                textBox_mzmlfile.Text = "";
                                textBox_mzidfile.Text = "";
                                textBox_T.Text = "";
                                textBox_BWE.Text = "";

                            }
                            else
                            {
                                MessageBox.Show("Please check your input!", "Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please check your input!", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Please check your input!", "Error");
                }
            }
            catch { MessageBox.Show("Please check your input!", "Error"); }
        }

        private void button_mzmlbrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            //dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.Filter = "txt files (*.mzML)|*.mzML";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.FileName;
                textBox_mzmlfile.Text = path;
            }
        }

        private void button_mzidbrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.mzid)|*.mzid";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.FileName;
                textBox_mzidfile.Text = path;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            dataGridView1_mzMLmzIDData.DataSource = null;
            inputdata = new List<MzMLmzIDFilePair>();
            dataGridView1_mzMLmzIDData.DataSource = inputdata;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> selectedRows = new List<int>();
                foreach (DataGridViewRow r in dataGridView1_mzMLmzIDData.SelectedRows) selectedRows.Add(r.Index);
                foreach (DataGridViewCell c in dataGridView1_mzMLmzIDData.SelectedCells) selectedRows.Add(c.RowIndex);
                selectedRows = selectedRows.Distinct().ToList();

                foreach (int index in selectedRows)
                {
                    dataGridView1_mzMLmzIDData.DataSource = null;
                    inputdata.RemoveAt(index);
                    dataGridView1_mzMLmzIDData.DataSource = inputdata;
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Please select the row you want to delete. \n" + ex1.Message, "Error");
            }
        }

        private void button_browseoutputfolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (textBox_outputfolderpath.Text.Length > 0 & Directory.Exists(textBox_outputfolderpath.Text))
                dialog.SelectedPath = textBox_outputfolderpath.Text;

            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;

                txt_source.Text = path;

                if (Directory.Exists(path))
                {
                    string[] filePaths = Directory.GetFiles(path);
                    var csvfilePaths = filePaths.Where(x => x.Contains(".csv") || (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                    if (csvfilePaths.Count == 0)
                    {
                        textBox_outputfolderpath.Text = path;

                    }
                    else
                    {
                        MessageBox.Show("There are *.csv files in the output folder: " +
                                        path + "\n\r" + "They may interfere with output files.\r\n" +
                                        "Remove the csv files from the folder and run the program again.\n\r", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid path.");
                }
            }

        }

        private void button_autofillBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (textBox1_mzmlidfiles.Text.Length > 0 & Directory.Exists(textBox1_mzmlidfiles.Text))
                dialog.SelectedPath = textBox1_mzmlidfiles.Text;

            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;

                textBox1_mzmlidfiles.Text = path;

                string[] filePaths = Directory.GetFiles(path);

                var mzml = filePaths.Where(x => x.Contains(".mzML")).ToList();
                var mzid = filePaths.Where(x => x.Contains(".mzid")).ToList();

                if (mzml.Count() != mzid.Count())
                {
                    MessageBox.Show("mzML and mzid files should be in matched pairs", "Error");
                    return;
                }

                if (mzml.Count() == 0 | mzid.Count() == 0)
                {
                    MessageBox.Show("The folder does not contain the required files (.mzML and .mzid files)", "Error");
                    return;
                }

                else
                {
                    inputdata = new List<MzMLmzIDFilePair>();
                    foreach (var mz in mzml)
                    {
                        MzMLmzIDFilePair k = new MzMLmzIDFilePair();
                        k.MzML_FileName = mz;
                        k.MzID_FileName = mz.Replace(".mzML", ".mzid");
                        k.Time = 0;
                        k.BWE = 0;
                        inputdata.Add(k);
                    }

                    //comboBox_mzidfilelist.DataSource = mzid;
                    //comboBox_mzmlfilelist.DataSource = mzml;
                }

                inputdata = inputdata.OrderBy(x => x.Time).ToList();
                dataGridView1_mzMLmzIDData.DataSource = inputdata;
                //this.dataGridView1_records.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
        }

        private void dataGridView1_records_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Console.WriteLine("test");

            string headerText =
        dataGridView1_mzMLmzIDData.Columns[e.ColumnIndex].HeaderText.Trim();

            if (headerText.Equals("mzML"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText =
                        "mzML file name must not be empty";
                    e.Cancel = true;
                }
                else if (!(Path.GetExtension(e.FormattedValue.ToString()).Trim().Equals(".mzML")))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText =
                        "mzML file is not found in this path. Please check the file path";
                    e.Cancel = true;
                }
            }
            if (headerText.Equals("mzID"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText =
                        "mzID file name must not be empty";
                    e.Cancel = true;
                }
                else if (!(Path.GetExtension(e.FormattedValue.ToString()).Trim().Equals(".mzid")))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText =
                        "mzID file is not found in this path. Please check the file path";
                    e.Cancel = true;
                }
            }
            if (headerText.Equals("T"))
            {
                double i = 0;
                if (!double.TryParse(e.FormattedValue.ToString(), out i))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText = ("Time value is Not valid.");
                    e.Cancel = true;
                }
                else if (double.Parse(e.FormattedValue.ToString()) < 0)
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText = ("Time value is Not valid.");
                    e.Cancel = true;
                }

            }
            if (headerText.Equals("BWE"))
            {
                double i = 0;
                if (!double.TryParse(e.FormattedValue.ToString(), out i))
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText = ("BWE value is Not valid.");
                    e.Cancel = true;
                }
                else if (double.Parse(e.FormattedValue.ToString()) < 0 || double.Parse(e.FormattedValue.ToString()) > 1)
                {
                    dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText = ("BWE value is Not valid.");
                    e.Cancel = true;
                }
            }

        }

        private void dataGridView1_records_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView1_records_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_mzMLmzIDData.Rows[e.RowIndex].ErrorText = String.Empty;
        }
        #endregion

        #region visualization
        public void loadProteinchart(ProtienchartDataValues chartdata)
        {
            chart_protein.Series["Series1"].Points.DataBindXY(chartdata.x, chartdata.y);
            //chart1.ChartAreas[0].AxisX.Maximum = proteinExperimentData.Experiment_time.Max();

            var temp_xval = new List<double>();
            var temp_maxval = proteinExperimentData.experiment_time.Max();
            //var step = 0.1;
            var step = temp_maxval / 200.0;
            List<double> yval = new List<double>();

            for (int i = 0; i * step < temp_maxval; i++)
            {
                var temp_x = step * i;
                temp_xval.Add(temp_x);
                yval.Add(1 - Math.Pow(Math.E, (double)(-1 * proteinExperimentData.MeanRateConst * temp_x)));
            }

            chart_protein.Series["Series2"].Points.DataBindXY(temp_xval, yval);

            chart_protein.ChartAreas[0].AxisX.Interval = (int)temp_maxval / 10;
            chart_protein.ChartAreas[0].AxisX.Maximum = temp_maxval + 0.01;

            chart_peptide.ChartAreas[0].AxisY.Interval = yval.Max() / 5;
            chart_peptide.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

            chart_protein.ChartAreas[0].AxisY.Minimum = -0.1;
            chart_protein.ChartAreas[0].AxisY.Maximum = 2;


        }

        public void computeNewProtienRateConstant(ProtienchartDataValues chartdata)
        {

            label24_totalperppetide.Text = proteinExperimentData.peptides.Count().ToString();
            //label22_rsquarecountPerPeptide.Text = proteinExperimentData.peptides.Where(x => x.RSquare >= 0.8).Count().ToString();
            //label22_stdcount.Text = proteinExperimentData.peptides.Where(x => x.Rateconst > 0).Where((x) => x.RSquare < 0.8 && x.RSquare > 0.4 && (10 * x.std_k / x.Rateconst) < 30).Count().ToString();

            var temp = proteinExperimentData.peptides.Where(x => x.NDP >= 4 && x.RMSE_value <= 0.05).ToList();
            List<Peptide> filtered_peptidelist = new List<Peptide>();
            List<Peptide> filtered_peptidelist_lowRsquared = new List<Peptide>();

            var RSquaredThreshold = 0.75;

            foreach (var peptide in temp)
            {
                //var half_life = Math.Log(2) / proteinExperimentData.experiment_time[1];


                if (peptide.Rateconst > 2 * Math.Log(2) / proteinExperimentData.experiment_time[1])
                {
                    if (peptide.RSquare >= RSquaredThreshold && peptide.RMSE_value <= 0.05) filtered_peptidelist.Add(peptide);

                    else if ((peptide.RSquare >= 0.16 && peptide.RMSE_value <= 0.05))
                        filtered_peptidelist_lowRsquared.Add(peptide);
                }
                else if (peptide.Rateconst < 0.1 * Math.Log(2) / proteinExperimentData.experiment_time[proteinExperimentData.experiment_time.Count - 1])
                {
                    if (peptide.RMSE_value <= 0.04) filtered_peptidelist.Add(peptide);
                }
                else
                {
                    //if ((peptide.RSquare >= 0.85 && peptide.RMSE_value <= 0.05 && peptide.std_k / peptide.Rateconst <= 0.35) ||
                    //    (peptide.RSquare <= 0.85 && peptide.RSquare >= 0.5 && peptide.RMSE_value <= 0.05 && peptide.std_k / peptide.Rateconst <= 0.35))
                    if ((peptide.RSquare >= 0.5 && peptide.RMSE_value <= 0.05 && peptide.std_k / peptide.Rateconst <= 0.35))
                        filtered_peptidelist.Add(peptide);

                    else if ((peptide.RSquare >= 0.16 && peptide.std_k / peptide.Rateconst <= 0.35) ||
                        (peptide.RSquare <= RSquaredThreshold && peptide.RSquare >= 0.5 && peptide.std_k / peptide.Rateconst <= 0.35))
                        filtered_peptidelist_lowRsquared.Add(peptide);
                }
            }

            label22_rsquarecountPerPeptide.Text = filtered_peptidelist.Count.ToString();

            if (filtered_peptidelist.Count > 0)
            {
                if (filtered_peptidelist.Where(x => x.RSquare >= 0.85).Count() >= 5)
                {
                    var selected_k = filtered_peptidelist.OrderByDescending(x => x.RSquare).Where(x => x.RSquare >= 0.8).Select(x => (double)x.Rateconst).Take(5).ToList();
                    var median = BasicFunctions.getMedian(selected_k);
                    var sd = BasicFunctions.getStandardDeviation(selected_k);

                    label22_stdcount.Text = median.ToString() + " +/- " + sd.ToString();
                }
                else
                {
                    var selected_k = filtered_peptidelist.Select(x => (double)x.Rateconst).ToList();
                    var median = BasicFunctions.getMedian(selected_k);
                    var sd = BasicFunctions.getStandardDeviation(selected_k);

                    label22_stdcount.Text = median.ToString() + " +/- " + sd.ToString();
                }
            }
            else
            {
                // if nothing passed, look for low rsquared values
                if (filtered_peptidelist_lowRsquared.Count > 0)
                {
                    var selected_k = filtered_peptidelist_lowRsquared.OrderByDescending(x => x.RSquare).Select(x => (double)x.Rateconst).Take(3).ToList();
                    var median = BasicFunctions.getMedian(selected_k);
                    var sd = BasicFunctions.getStandardDeviation(selected_k);

                    label22_stdcount.Text = median.ToString() + " +/- " + sd.ToString();
                }
                else
                    label22_stdcount.Text = "NA";
            }


            /*
            // compute the median for each time point 
            var median_value = new List<double>();
            median_value.Add(0);

            var ttt = chartdata.y.Select((value, index) => (value, chartdata.x[index])).ToList();


            foreach (var t in proteinExperimentData.experiment_time.Where(x => x != 0).ToList())
            {

                //var _values = proteinExperimentData.mergedRIAvalues.Where(x => x.Time == t && x.RIA_value >= 0).Select(x => (double)x.RIA_value).OrderBy(x => x).ToList();
                var _values = ttt.Where(x => x.Item2 == t).Select(x => (double)x.Item1).OrderBy(x => x).ToList();

                median_value.Add(Helper.BasicFunctions.getMedian(_values));
            }

            if (chart_protein.Series.FindByName("new_k") != null)
                chart_protein.Series.Remove(chart_protein.Series.FindByName("new_k"));

            Series s1 = new Series();
            s1.Name = "new_k";
            //s1.Points.DataBindXY(proteinExperimentData.experiment_time, median_value);
            for (int k = 0; k < median_value.Count; k++)
            {
                if (!double.IsNaN(median_value[k]))
                    s1.Points.AddXY(proteinExperimentData.experiment_time[k], median_value[k]);
            }
            s1.ChartType = SeriesChartType.Line;
            s1.Color = Color.Red;
            s1.BorderWidth = 2;
            chart_protein.Series.Add(s1);
            */
        }

        public void loadDataGridView()
        {

            // update the datasource for the data gridview
            //var selected = (from u in proteinExperimentData.peptides
            //                where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
            //                select u).Distinct().ToList();
            //dataGridView_peptide.DataSource = selected;
            dataGridView_peptide.DataSource = proteinExperimentData.peptides.OrderByDescending(x => x.RSquare).ToList();

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
            dataGridView_peptide.Columns["order"].Visible = false;

            //rename column name
            dataGridView_peptide.Columns["PeptideSeq"].HeaderText = "Peptide";
            dataGridView_peptide.Columns["SeqMass"].HeaderText = "m/z";
            dataGridView_peptide.Columns["IsotopeDeviation"].HeaderText = "Isotope Deviation";
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].HeaderText = "Exchangeable Hydrogens";
            dataGridView_peptide.Columns["Rateconst"].HeaderText = "Rate \nconstant";
            dataGridView_peptide.Columns["RSquare"].HeaderText = "R" + "\u00B2";
            dataGridView_peptide.Columns["RMSE_value"].HeaderText = "RMSE";
            //dataGridView_peptide.Columns["std_k"].HeaderText = "std (k)";
            dataGridView_peptide.Columns["std_k"].HeaderText = "\u03C3 (k)";

            // enable AutoSizeColumnsMode
            //dataGridView_peptide.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //set size for the columns
            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.MinimumWidth = 55;
                column.Width = 56;
            }
            //dataGridView_peptide.Columns["Charge"].Width = 55;
            //dataGridView_peptide.Columns["RSquare"].Width = 55;
            //dataGridView_peptide.Columns["RMSE_value"].Width = 55;
            //dataGridView_peptide.Columns["Rateconst"].Width = 55;
            dataGridView_peptide.Columns["PeptideSeq"].MinimumWidth = 200;
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].MinimumWidth = 80;
            dataGridView_peptide.Columns["Exchangeable_Hydrogens"].Width = 81;
            dataGridView_peptide.Columns["Abundance"].MinimumWidth = 70;
            dataGridView_peptide.Columns["std_k"].MinimumWidth = 70;

            //set number formationg for the columns
            dataGridView_peptide.Columns["RSquare"].DefaultCellStyle.Format = "#0.#0";
            dataGridView_peptide.Columns["RMSE_value"].DefaultCellStyle.Format = "#0.###";
            dataGridView_peptide.Columns["SeqMass"].DefaultCellStyle.Format = "#0.###";
            dataGridView_peptide.Columns["IsotopeDeviation"].DefaultCellStyle.Format = "#0.###";
            dataGridView_peptide.Columns["Abundance"].DefaultCellStyle.Format = "G2";
            dataGridView_peptide.Columns["std_k"].DefaultCellStyle.Format = "G2";

            // resizeable columns
            dataGridView_peptide.AllowUserToResizeColumns = true;
            dataGridView_peptide.ColumnHeadersHeightSizeMode =
         DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            dataGridView_peptide.AllowUserToResizeRows = false;
            dataGridView_peptide.RowHeadersWidthSizeMode =
                DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // column selected hightlight
            dataGridView_peptide.EnableHeadersVisualStyles = false;
            dataGridView_peptide.Columns["PeptideSeq"].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;

            foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            {
                column.HeaderCell.Style.SelectionBackColor = Color.White;
                column.HeaderCell.Style.SelectionForeColor = Color.Black;
            }
            ////set size for the columns
            //foreach (DataGridViewColumn column in dataGridView_peptide.Columns)
            //{ 
            //    column.Width = 51;
            //}
        }
        public unsafe void computation(RIA[] chart_data, string peptideSeq, int charge)
        {

            try
            {
                var scale = 60;
                chart_data = chart_data.Where(x => (double)x.RIA_value > 0).ToArray();

                float[] TimeCourseDates = chart_data.Select(x => (float)(x.Time)).ToArray();
                float[] TimeCourseI0Isotope = chart_data.Select(x => float.Parse(x.RIA_value.ToString())).ToArray();

                var current_peptide = proteinExperimentData.peptides.Where(x => x.PeptideSeq == peptideSeq & x.Charge == charge).FirstOrDefault();
                var current_peptide_M0 = current_peptide.M0 / 100;
                var experiment_peptide_I0 = chart_data.Where(x => x.Time == 0).Select(x => x.RIA_value).FirstOrDefault();

                double selected_Io = 0;
                if (experiment_peptide_I0.HasValue)
                    selected_Io = (double)experiment_peptide_I0;
                else
                    selected_Io = (double)current_peptide_M0;

                if (experiment_peptide_I0.HasValue && Math.Abs((double)current_peptide_M0 - (double)experiment_peptide_I0) > 0.1)
                    selected_Io = (double)current_peptide_M0;

                double pw = proteinExperimentData.filecontents[proteinExperimentData.filecontents.Count - 1].BWE;
                var neh = (double)current_peptide.Exchangeable_Hydrogens;
                var I0_AtAsymptote = selected_Io * Math.Pow((1 - (pw / 1 - Helper.Constants.ph)), neh);
                float rkd, rks, fx1;

                var new_k = computeRate(TimeCourseDates.ToList(), TimeCourseI0Isotope.ToList(), (double)selected_Io, (double)I0_AtAsymptote, pw, neh);



                Drawewpeptideplot(new_k, (double)selected_Io, (double)I0_AtAsymptote, neh, pw, TimeCourseDates.ToList());

                //float[] new_TimeCourseDates = new float[500];

                //for (int i = 0; i < TimeCourseDates.Length; i++)
                //{
                //    new_TimeCourseDates[i] = TimeCourseDates[i];
                //}

                float[] new_TimeCourseDates = TimeCourseDates.Select(x => x / scale).ToArray();
                fixed (float* ptr_TimeCourseDates = new_TimeCourseDates)
                fixed (float* ptr_TimeCourseI0Isotope = TimeCourseI0Isotope)
                {
                    LBFGS lbfgs = new LBFGS(ptr_TimeCourseDates, TimeCourseDates.Count(), 1, "One_Compartment_exponential");
                    lbfgs.InitializeTime();
                    var nret = lbfgs.Optimize(ptr_TimeCourseI0Isotope, (float)I0_AtAsymptote, (float)(selected_Io - I0_AtAsymptote), &rkd, &rks, &fx1);
                    double fDegradationConstant = Math.Exp(lbfgs.fParams[0]) / 60;

                    Console.WriteLine(nret.ToString() + "=======>" + fDegradationConstant.ToString());


                    //for (int i = 0; i < 2; i++)
                    //{
                    //    Console.WriteLine("Address of list[{0}]={1}", i, (int)(ptr_TimeCourseI0Isotope + i));
                    //    Console.WriteLine("Value of list[{0}]={1}", i, *(ptr_TimeCourseI0Isotope + i));
                    //}

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }

        private void Drawewpeptideplot(double k, double io_nature, double io_assymptot, double neh, double pw, List<float> experiment_time)
        {
            try
            {
                double ph = 1.5574E-4;
                List<double> mytimelist = new List<double>();
                var comptedvalue = new List<double>();

                var temp_maxval = experiment_time.Max();
                var step = temp_maxval / 200.0;
                for (int j = 0; j * step < temp_maxval; j++)
                { mytimelist.Add(j * step); }

                foreach (double t in mytimelist)
                {
                    //var val1 = io * Math.Pow(1 - (pw / (1 - ph)), neh);
                    //var val2 = io * Math.Pow(Math.E, -1 * k * t) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
                    var val = io_assymptot + (io_nature - io_assymptot) * Math.Exp(-k * t);
                    comptedvalue.Add(val);
                }

                chart_peptide.Series.Remove(chart_peptide.Series.FindByName("new_k"));

                Series s1 = new Series();
                s1.Name = "new_k";
                s1.Points.DataBindXY(mytimelist, comptedvalue);
                s1.ChartType = SeriesChartType.Line;
                s1.Color = Color.Red;
                s1.BorderWidth = 2;
                //chart_peptide.Series.Add(s1);


            }
            catch (Exception e) { Console.WriteLine("Error => computeExpectedCurvePoints(), " + e.Message); }

        }

        public static double computeRate(List<float> TimeCourseDates, List<float> TimeCourseI0Isotope, double nature_Io, double I0_AtAsymptote,
            double pw, double neh)
        {
            double ph = 1.5574E-4;
            float rkd, rks, fx1;

            double previous_teta = 0;
            double teta = Math.Log(1E-5);
            double fit_error = 1;

            while (Math.Abs(teta - previous_teta) > 1E-8)
            {
                float k = (float)Math.Exp(teta);

                // compute derivatives for each time point
                List<double> y = new List<double>();
                foreach (var t in TimeCourseDates)
                {
                    double temp = (double)((nature_Io - I0_AtAsymptote) * Math.Exp(-k * t) * (-t) * k);
                    y.Add(temp);
                }

                //comute del y
                List<double> del_y = new List<double>();
                for (int i = 0; i < TimeCourseI0Isotope.Count; i++)
                {
                    var fit_value = I0_AtAsymptote + (nature_Io - I0_AtAsymptote) * Math.Exp(-k * TimeCourseDates[i]);
                    del_y.Add((double)(TimeCourseI0Isotope[i] - fit_value));
                }

                // compute yT*y
                double val_1 = y.Select(x => x * x).ToList().Sum();

                //compute yT*del_y
                double val_2 = 0;
                for (int i = 0; i < del_y.Count; i++)
                {
                    val_2 = val_2 + (del_y[i] * y[i]);
                }

                double del_teta = val_2 / val_1;
                previous_teta = teta;
                teta = teta + 0.001 * del_teta;

                //Console.WriteLine(Math.Exp(teta) + " , " + Math.Abs(teta - previous_teta) + " , fit_error = " + fit_error);

                if (double.IsNaN(teta))
                {
                    Console.WriteLine("Teta is null");
                }


            }

            Console.WriteLine("==> k = " + Math.Exp(teta).ToString());

            return Math.Exp(teta);

        }


        public void loadPeptideChart(string peptideSeq, int charge, double masstocharge, List<RIA> mergedRIAvalues, List<TheoreticalI0Value> theoreticalI0Valuespassedvalue, double Rateconst = Double.NaN, double RSquare = Double.NaN)
        {
            try
            {
                //clear chart area
                chart_peptide.Titles.Clear();

                #region experimental data plot

                // prepare the chart data
                var chart_data = mergedRIAvalues.Where(x => x.PeptideSeq == peptideSeq & x.Charge == charge).OrderBy(x => x.Time).ToArray();
                chart_peptide.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.Time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                chart_peptide.ChartAreas[0].AxisX.Minimum = 0;
                //chart_peptide.ChartAreas[0].AxisX.IsMarginVisible = false;

                computation(chart_data, peptideSeq, charge);


                #endregion

                #region expected data plot 

                var theoretical_chart_data = theoreticalI0Valuespassedvalue.Where(x => x.peptideseq == peptideSeq & x.charge == charge).OrderBy(x => x.time).ToArray();
                List<double> x_val = theoretical_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                List<double> y_val = theoretical_chart_data.Select(x => x.value).ToList();
                var pep = proteinExperimentData.peptides.Where(x => x.PeptideSeq == peptideSeq).FirstOrDefault();

                chart_peptide.Series["Series3"].Points.DataBindXY(x_val, y_val);

                // set x axis chart interval
                if (x_val.Count > 0)
                {
                    chart_peptide.ChartAreas[0].AxisX.Interval = (int)x_val.Max() / 10;
                    chart_peptide.ChartAreas[0].AxisX.Maximum = proteinExperimentData.experiment_time.Max() + 0.01;
                }


                #endregion

                // chart title
                //chart_peptide.Titles.Add(peptideSeq);    

                Title title = new Title();
                title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                //title.Text = peptideSeq + " (K = " + Rateconst.ToString() + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ")";
                if (Rateconst != double.NaN)
                {
                    title.Text = peptideSeq + " (k = " + formatdoubletothreedecimalplace(Rateconst) + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ", m/z = " + masstocharge.ToString("#0.###") + ", z = " + charge.ToString() + ")";
                }
                else
                {
                    title.Text = peptideSeq + " (m/z = " + masstocharge.ToString("#0.###") + ", z = " + charge.ToString() + ")";
                }
                chart_peptide.Titles.Add(title);

                //chart_peptide.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.07;

                var max_y_list = new List<double>();
                foreach (var series in chart_peptide.Series)
                {
                    if (series.Points.Count > 0)
                    {
                        var maxvalue = series.Points.FindMaxByValue();
                        if (maxvalue != null)
                            max_y_list.Add(maxvalue.YValues[0]);
                    }
                }
                chart_peptide.ChartAreas[0].AxisY.Maximum = max_y_list.Max() + 0.08;

                chart_peptide.ChartAreas[0].AxisY.Interval = chart_peptide.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                chart_peptide.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";



            }
            catch (Exception e)
            {
                Console.WriteLine("Error => loadPeptideChart(), " + e.Message);
            }

        }
        public bool exportChart(string path, string name)
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

                    if (exportChart(path, chart_peptide.Titles[0].Text))
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

                    //var selected = (from u in proteinExperimentData.peptides
                    //                where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                    //                select u).Distinct().ToList();
                    var selected = proteinExperimentData.peptides;

                    int count = 1;
                    //foreach (Peptide p in proteinExperimentData.peptides)
                    foreach (Peptide p in selected)
                    {
                        //clear chart area
                        chart2.Titles.Clear();

                        Console.WriteLine(p.PeptideSeq);

                        #region experimental data plot

                        // prepare the chart data
                        var chart_data = this.proteinExperimentData.mergedRIAvalues.Where(x => x.PeptideSeq == p.PeptideSeq & x.Charge == p.Charge).OrderBy(x => x.Time).ToArray();
                        chart2.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.Time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                        #endregion

                        #region expected data plot 

                        var theoretical_chart_data = this.proteinExperimentData.theoreticalI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        //
                        List<double> x_val = theoretical_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                        List<double> y_val = theoretical_chart_data.Select(x => x.value).ToList();

                        chart2.Series["Series3"].Points.DataBindXY(theoretical_chart_data.Select(x => x.time).ToArray(), theoretical_chart_data.Select(x => x.value).ToArray());

                        // set x axis chart interval
                        if (x_val.Count > 0)
                        {
                            chart2.ChartAreas[0].AxisX.Interval = (int)theoretical_chart_data.Select(x => x.time).ToArray().Max() / 10;
                            chart2.ChartAreas[0].AxisX.Maximum = x_val.Max() + 0.01;
                        }

                        //chart2.ChartAreas[0].AxisY.Interval = theoretical_chart_data.Select(x => x.value).ToArray().Max() / 5;
                        //chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";



                        #endregion


                        // chart title
                        //chart2.Titles.Add(p.PeptideSeq + "(K=" + p.Rateconst.ToString() + ", " + ")");
                        Title title = new Title();
                        title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                        //title.Text = p.PeptideSeq + " (K = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ")";
                        //title.Text = p.PeptideSeq + " (k = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                        try
                        {
                            if (p.Rateconst != null)
                            {
                                title.Text = p.PeptideSeq + " (k = " + formatdoubletothreedecimalplace((double)p.Rateconst) + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                            }
                            else
                            {
                                title.Text = p.PeptideSeq + " (m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                            }
                        }
                        catch (Exception fex)
                        {
                            Console.WriteLine(fex.Message);
                        }
                        chart2.Titles.Add(title);

                        //chart2.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.07;
                        //chart2.ChartAreas[0].AxisY.Interval = chart2.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                        //chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

                        var max_y_list = new List<double>();
                        foreach (var series in chart2.Series)
                        {
                            if (series.Points.Count > 0)
                            {
                                var maxvalue = series.Points.FindMaxByValue();
                                if (maxvalue != null)
                                    max_y_list.Add(maxvalue.YValues[0]);
                            }
                        }
                        chart2.ChartAreas[0].AxisY.Maximum = max_y_list.Max() + 0.08;

                        chart2.ChartAreas[0].AxisY.Interval = chart2.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                        chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";


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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
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

            if (txt_source.Text.Length > 0 & Directory.Exists(txt_source.Text))
                dialog.SelectedPath = txt_source.Text;

            if (DialogResult.OK == dialog.ShowDialog())
            {
                string path = dialog.SelectedPath;

                txt_source.Text = path;

                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Please select a valid path.");
                    return;
                }

                string[] filePaths = Directory.GetFiles(path);
                var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                if (csvfilePaths.Count == 0)
                {
                    MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
                }
                //else
                //{
                //    var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                //    comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                //}
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
            else { try { string[] lines = System.IO.File.ReadAllLines(files_txt_path); } catch (Exception ex) { MessageBox.Show(ex.Message); return; } }

            if (!File.Exists(quant_csv_path)) { MessageBox.Show(proteinName + ".Quant.csv" + " is not available in the specified directory.", "Error"); return; }
            else { try { string[] lines = System.IO.File.ReadAllLines(quant_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); return; } }

            if (!File.Exists(RateConst_csv_path)) { MessageBox.Show(proteinName + ".RateConst.csv" + "filex.txt is not available in the specified directory.", "Error"); return; }
            else { try { string[] lines = System.IO.File.ReadAllLines(RateConst_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); return; } }

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.computeAverageA0();
            proteinExperimentData.mergeMultipleRIAPerDay2();
            proteinExperimentData.computeTheoreticalCurvePoints();
            proteinExperimentData.computeTheoreticalCurvePointsBasedOnExperimentalI0();
            proteinExperimentData.computeRSquare();
            ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();

            computeNewProtienRateConstant(chartdata);
            //preparedDataForBestPathSearch(chartdata);
            try
            {


                label4_proteinRateConstantValue.Text = formatdoubletothreedecimalplace((double)proteinExperimentData.MeanRateConst) + " \u00B1 " + formatdoubletothreedecimalplace((double)proteinExperimentData.StandDev_NumberPeptides_StandDev);
                label5_Ic.Text = ((double)proteinExperimentData.TotalIonCurrent).ToString("G2");
                loadDataGridView();

                loadProteinchart(chartdata);
                groupBox3_proteinchart.Text = proteinName;
                button_exportAllPeptideChart.Text = "Export " + proteinName;

                var p = proteinExperimentData.peptides.First();
                loadPeptideChart(p.PeptideSeq, (int)p.Charge, (double)p.SeqMass, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_theoreticalI0Values, (double)p.Rateconst, (double)p.RSquare);
            }
            catch (Exception xe)
            {
                Console.WriteLine("Error => computeValuesForEnhancedPerProtienPlot2(), " + xe.Message);
            }

        }

        private void preparedDataForBestPathSearch(ProtienchartDataValues chartdata)
        {

            // clear previous plots
            while (chart_protein.Series.Count > 2)
                chart_protein.Series.RemoveAt(chart_protein.Series.Count - 1);


            var experimentTime = chartdata.x.Distinct().OrderBy(x => x).ToList();
            var inputForBestPathSearch = new float[chartdata.x.Count / experimentTime.Count, experimentTime.Count];
            for (int i = 0; i < chartdata.x.Count; i++)
            {
                inputForBestPathSearch[(int)i / experimentTime.Count, experimentTime.IndexOf((int)chartdata.x[i])] =
                    ((Double.IsNaN(chartdata.y[i]))) ? float.NaN : (float)chartdata.y[i];
            }



            Labeling_Path.Label_Path lp = new Label_Path();
            var bestpaths = lp.Labeling_Path_Fractional_Synthesis2(inputForBestPathSearch, inputForBestPathSearch.GetLength(0), experimentTime.Count(), 0, 0);



            #region linking path
            var best_scores = new List<float>();
            var best_paths = new List<List<float>>();

            foreach (var path in bestpaths)
            {

                var path_score = path.Trim().Split('#');

                var path_indexs = path_score[0];
                var score = path_score[1];

                var temp_yval = new List<float>();
                var value_indexs = path_indexs.Split(' ');

                ////foreach (var index in value_indexs)
                ////{
                ////    var temp = index.Split(',');
                ////    temp_yval.Add(inputForBestPathSearch[int.Parse(temp[0]), int.Parse(temp[1])]);
                ////}

                ////best_scores.Add(float.Parse(score));
                ////best_paths.Add(temp_yval);



                Series s = new Series();
                s.Name = score.ToString() + path;
                //s.Points.DataBindXY(experimentTime, temp_yval);

                for (int i = 0; i < value_indexs.Length; i++)
                {
                    var temp = value_indexs[i].Split(',');
                    if (i == 0)
                    {
                        if (double.IsNaN(inputForBestPathSearch[int.Parse(temp[0]), int.Parse(temp[1])]))
                            s.Points.AddXY(experimentTime[i], 0);
                        else s.Points.AddXY(experimentTime[i], inputForBestPathSearch[int.Parse(temp[0]), int.Parse(temp[1])]);
                    }
                    else if (int.Parse(temp[0]) != 0 && int.Parse(temp[1]) != 0 &&
                        !double.IsNaN(inputForBestPathSearch[int.Parse(temp[0]), int.Parse(temp[1])]))
                    {
                        s.Points.AddXY(experimentTime[i], inputForBestPathSearch[int.Parse(temp[0]), int.Parse(temp[1])]);
                    }
                }


                s.ChartType = SeriesChartType.Line;
                chart_protein.Series.Add(s);

            }

            #endregion


            #region median calculation
            var medialdata = new List<double>();
            var experimenttime_withdata = new List<int>();

            foreach (int t in experimentTime)
            {
                var indices = Enumerable.Range(0, chartdata.x.Count).Where(i => chartdata.x[i] == t).ToList();   //chartdata.x.Where(x => x == 72).ToList();
                var temp = new List<double>();
                foreach (int i in indices)
                {
                    if (!double.IsNaN(chartdata.y[i]))
                        temp.Add(chartdata.y[i]);
                }
                var computed_median = temp.Median(); // Helper.BasicFunctions.getMedian(temp);
                if (!double.IsNaN(computed_median))
                {
                    medialdata.Add(computed_median);
                    experimenttime_withdata.Add(t);
                }
                else if (t == 0)
                {
                    medialdata.Add(0);
                    experimenttime_withdata.Add(0);
                }
            }

            Series s1 = new Series();
            s1.Name = "Median";
            s1.Points.DataBindXY(experimenttime_withdata, medialdata);
            s1.ChartType = SeriesChartType.Line;
            s1.Color = Color.Red;
            s1.BorderWidth = 4;
            chart_protein.Series.Add(s1);

            #endregion
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
            try
            {
                if (dataGridView_peptide.SelectedRows.Count > 0)
                {

                    int indexofselctedrow = dataGridView_peptide.SelectedRows[0].Index;

                    if (indexofselctedrow >= 0)
                    {
                        DataGridViewRow row = dataGridView_peptide.Rows[indexofselctedrow];
                        //var temp = dataGridView_peptide.Rows[indexofselctedrow].Cells[0].Value.ToString();
                        //var charge = int.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[4].Value.ToString());
                        //var rateconst = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[2].Value.ToString());
                        //var rsquare = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[3].Value.ToString());
                        //var masstocharge = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[5].Value.ToString());
                        //loadPeptideChart(temp, charge, rateconst, rsquare, masstocharge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_theoreticalI0Values);

                        // check for rate constant value 
                        var rateconstvaluefromgrid = dataGridView_peptide.Rows[indexofselctedrow].Cells[2].Value;
                        if (rateconstvaluefromgrid == null)
                        {
                            var temp = dataGridView_peptide.Rows[indexofselctedrow].Cells[0].Value.ToString();
                            var charge = int.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[4].Value.ToString());
                            //var rateconst = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[2].Value.ToString());
                            //var rsquare = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[3].Value.ToString());
                            var masstocharge = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[5].Value.ToString());

                            loadPeptideChart(temp, charge, masstocharge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_theoreticalI0Values);
                        }
                        else
                        {
                            var temp = dataGridView_peptide.Rows[indexofselctedrow].Cells[0].Value.ToString();
                            var charge = int.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[4].Value.ToString());
                            var rateconst = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[2].Value.ToString());
                            var rsquare = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[3].Value.ToString());
                            var masstocharge = double.Parse(dataGridView_peptide.Rows[indexofselctedrow].Cells[5].Value.ToString());

                            loadPeptideChart(temp, charge, masstocharge, proteinExperimentData.mergedRIAvalues, proteinExperimentData.temp_theoreticalI0Values, rateconst, rsquare);
                        }

                    }
                }
            }
            catch (Exception ex) { }

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
                        using (Bitmap im = new Bitmap(chart_protein.Width, chart_protein.Height))
                        {
                            chart_protein.DrawToBitmap(im, new Rectangle(0, 0, chart_protein.Width, chart_protein.Height));

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

                    MessageBox.Show("This process will take a few minutes to complete. \nPlease check the exported graphs in " + outputpath, "Message");

                    if (outputpath.Length > 0 & sourcepath.Length > 0)
                    {
                        ExportAllProteinsData exp = new ExportAllProteinsData(sourcepath, outputpath);
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

        #endregion

        private void dataGridView1_records_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //List<mzMlmzIDModel> temp = (List<mzMlmzIDModel>)dataGridView1_records.DataSource;
            //temp = temp.OrderBy(x => x.T).ToList();
            //dataGridView1_records.DataSource = temp;
            //inputdata = new List<mzMlmzIDModel>();
            //inputdata = temp;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 1)
            if (!isvisualizationLoadForThepath)
            {

                var path = txt_source.Text;

                if (!Directory.Exists(path))
                {
                    return;
                }
                string[] filePaths = Directory.GetFiles(path);
                var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                if (csvfilePaths.Count == 0)
                {
                    return;
                }
                else
                {
                    var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                    comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                }

            }
        }

        private void textBox_outputfolderpath_TextChanged(object sender, EventArgs e)
        {
            if (textBox_outputfolderpath.Text.Length > 0 & Directory.Exists(textBox_outputfolderpath.Text))
                txt_source.Text = textBox_outputfolderpath.Text;
        }

        private void txt_source_TextChanged(object sender, EventArgs e)
        {
            var path = txt_source.Text;

            if (!Directory.Exists(path))
            {
                isvisualizationLoadForThepath = false;
                return;
            }
            string[] filePaths = Directory.GetFiles(path);
            var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

            if (csvfilePaths.Count == 0)
            {
                isvisualizationLoadForThepath = false;
                return;
            }
            else
            {
                var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                isvisualizationLoadForThepath = true;
            }
        }

        private void dataGridView1_records_Leave(object sender, EventArgs e)
        {
            //List<mzMlmzIDModel> temp = (List<mzMlmzIDModel>)dataGridView1_records.DataSource;
            //temp = temp.OrderBy(x => x.Time).ToList();
            //dataGridView1_records.DataSource = temp;
            //inputdata = new List<mzMlmzIDModel>();
            //inputdata = temp;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            sortInputDataGridView();
        }

        public void sortInputDataGridView()
        {
            List<MzMLmzIDFilePair> temp = (List<MzMLmzIDFilePair>)dataGridView1_mzMLmzIDData.DataSource;
            temp = temp.OrderBy(x => x.Time).ToList();
            dataGridView1_mzMLmzIDData.DataSource = temp;
            inputdata = new List<MzMLmzIDFilePair>();
            inputdata = temp;
        }
        /*
        private void button1_Click_1(object sender, EventArgs e)
        {
            var there_l = new Thread(() => dummy(txt_source.Text));
            there_l.Start();

            //mynewproteinExperimentData.loadAllExperimentData();
            //mynewproteinExperimentData.computeRIAPerExperiment();
            //mynewproteinExperimentData.computeAverageA0();
            //mynewproteinExperimentData.mergeMultipleRIAPerDay2();
            //mynewproteinExperimentData.computeTheoreticalCurvePoints();
            //mynewproteinExperimentData.computeTheoreticalCurvePointsBasedOnExperimentalI0();
            //mynewproteinExperimentData.computeRSquare();
        }

        
        //public void dummy(string sourcePath)
        //{

        //    int count_r = 0;
        //    int count_s = 0;
        //    int count_t = 0;

        //    string[] filePaths = Directory.GetFiles(sourcePath);
        //    var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

        //    if (csvfilePaths.Count == 0)
        //    {
        //        //MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
        //    }
        //    else
        //    {
        //        var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).Distinct().ToList();

        //        int counter = 0;

        //        //progressBar_exportall.Invoke(new Action(() =>
        //        //  progressBar_exportall.Maximum = temp.Count));

        //        //progressBar_exportall.Invoke(new Action(() =>
        //        //  progressBar_exportall.Value = temp.Count));

        //        //progressBar_exportall.Maximum = temp.Count;
        //        //  progressBar_exportall.Value = 0;

        //        // for each file prepare the datasource for ploting
        //        foreach (string proteinName in temp)
        //        {
        //            //progressBar_exportall.Invoke(new Action(() =>
        //            //  progressBar_exportall.Value = counter));

        //            counter = counter + 1;
        //            try
        //            {
        //                label24_progress.Invoke(new Action(() => label24_progress.Text = counter.ToString() + "/" + temp.Count.ToString()));
        //            }
        //            catch (Exception ex) { }

        //            //string files_txt_path = txt_source.Text + @"\files.centroid.txt"; 
        //            string files_txt_path = sourcePath + @"\files.txt";
        //            string quant_csv_path = sourcePath + @"\" + proteinName + ".Quant.csv";
        //            string RateConst_csv_path = sourcePath + @"\" + proteinName + ".RateConst.csv";

        //            var temppath = files_txt_path.Replace("files.txt", "files.centroid.txt");
        //            if (File.Exists(temppath)) files_txt_path = temppath;


        //            if (!File.Exists(files_txt_path)) { MessageBox.Show("filex.txt is not available in the specified directory.", "Error"); continue; }
        //            else { try { string[] lines = System.IO.File.ReadAllLines(files_txt_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

        //            if (!File.Exists(quant_csv_path)) { MessageBox.Show(proteinName + ".Quant.csv" + " is not available in the specified directory.", "Error"); continue; }
        //            else { try { string[] lines = System.IO.File.ReadAllLines(quant_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

        //            if (!File.Exists(RateConst_csv_path)) { MessageBox.Show(proteinName + ".RateConst.csv" + "filex.txt is not available in the specified directory.", "Error"); continue; }
        //            else { try { string[] lines = System.IO.File.ReadAllLines(RateConst_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

        //            var mynewproteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


        //            mynewproteinExperimentData.loadAllExperimentData();
        //            mynewproteinExperimentData.computeRIAPerExperiment();
        //            mynewproteinExperimentData.computeAverageA0();
        //            mynewproteinExperimentData.mergeMultipleRIAPerDay2();
        //            mynewproteinExperimentData.computeTheoreticalCurvePoints();
        //            mynewproteinExperimentData.computeTheoreticalCurvePointsBasedOnExperimentalI0();
        //            mynewproteinExperimentData.computeRSquare();

        //            // for each peptide draw the chart 

        //            count_r += mynewproteinExperimentData.peptides.Where(x => x.RSquare >= 0.8).Count();
        //            count_t += mynewproteinExperimentData.peptides.Count();
        //            count_s += mynewproteinExperimentData.peptides.Where(x => x.Rateconst > 0).Where((x) => x.RSquare < 0.8 && x.RSquare > 0.4 && (10 * x.std_k / x.Rateconst) < 30).Count();

        //            //progressBar_exportall.Value = progressBar_exportall.Value + 1;

        //            //label22_total.Text = count_t.ToString();
        //            //label24_rate.Text = count_r.ToString();
        //            //label25_std.Text = count_s.ToString();

        //            try
        //            {
        //                label22_total.Invoke(new Action(() => label22_total.Text = count_t.ToString()));
        //                label24_rate.Invoke(new Action(() => label24_rate.Text = count_r.ToString()));
        //                label25_std.Invoke(new Action(() => label25_std.Text = count_s.ToString()));
        //            }
        //            catch (Exception ex) { }



        //        }
        //        //MessageBox.Show("Done exporting proteins!!");
        //        //progressBar_exportall.Invoke(new Action(() =>
        //        //     progressBar_exportall.Value = 0));


        //    }
        //}
        */
    }
}
