using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using v2.Helper;
using v2.Model;
using static v2.ProteinExperimentDataReader;

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

            chart_peptide.Series["Series3"].BorderWidth = 1;
            chart_protein.Series["Series2"].BorderWidth = 1;

            chart_peptide.Series["Series3"].Color = Color.Navy;
            chart_protein.Series["Series2"].Color = Color.Navy;



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
protein_score       = 40     //minimum protein score
peptide_score =  {2:f1} 	// minimum peptide score, ion score in Mascot, default is 1
peptide_expectation = 0.05     // maximum peptide expectation in Mascot
elutiontimewindow   =   {3}  // time window  (mins) to search for elution peak. From the time that highest scoring MS2 was triggered
protein_consistency = {4}  // minimum number of experiments for protein consistency
peptide_consistency = {4}   //mininum number of experiments for a peptide consistency
NParam_RateConst_Fit = {5}	// The model for fitting rate constant. Values are 1, and 2", massaccuracy, MS1_Type, peptidescore, elutionwindow, peptideconsistency, rate_constant_choice);

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
                    UseShellExecute = false,
                    WorkingDirectory = textBox_outputfolderpath.Text
                };
                // event handlers for output & error
                p.OutputDataReceived += outputDataReceived;
                p.ErrorDataReceived += errorDataReceived;

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


        }
        public void loadDataGridView()
        {

            //// update the datasource for the data gridview
            //var selected = (from u in proteinExperimentData.peptides
            //                where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
            //                select u).Distinct().ToList();
            //dataGridView_peptide.DataSource = selected;
            dataGridView_peptide.DataSource = proteinExperimentData.peptides;

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
            dataGridView_peptide.Columns["A0_average"].DefaultCellStyle.Format = "G2";

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
                chart_peptide.Series["Series4"].Points.DataBindXY(chart_data.Select(x => x.Time).ToArray(), chart_data.Select(x => x.I0_t).ToArray());
                chart_peptide.Series["Series5"].Points.DataBindXY(chart_data.Select(x => x.Time).ToArray(), chart_data.Select(x => x.pX_greaterthanThreshold).ToArray());

                chart_peptide.ChartAreas[0].AxisX.Minimum = 0;
                //chart_peptide.ChartAreas[0].AxisX.IsMarginVisible = false;

                #endregion


                #region expected data plot 

                var theoretical_chart_data = theoreticalI0Valuespassedvalue.Where(x => x.peptideseq == peptideSeq & x.charge == charge).OrderBy(x => x.time).ToArray();
                if (theoretical_chart_data.Count() > 0)
                {
                    List<double> x_val = theoretical_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                    List<double> y_val = theoretical_chart_data.Select(x => x.value).ToList();

                    chart_peptide.Series["Series3"].Points.DataBindXY(x_val, y_val);
                    // set x axis chart interval
                    chart_peptide.ChartAreas[0].AxisX.Interval = (int)x_val.Max() / 10;
                    chart_peptide.ChartAreas[0].AxisX.Maximum = x_val.Max() + 0.01;

                    chart_peptide.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.07;
                    chart_peptide.ChartAreas[0].AxisY.Interval = chart_peptide.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                    chart_peptide.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";
                }



                #endregion

                /*
                #region temp 
                List<double> x_val_temp = new List<double>();
                List<double> y_val_temp = new List<double>();

                // add additional data points to make the graph smooth
                var pep = proteinExperimentData.peptides.Where(x => x.PeptideSeq == peptideSeq & x.Charge == charge).FirstOrDefault();
                double io = (double)(pep.M0 / 100);
                double neh = (double)(pep.Exchangeable_Hydrogens);
                double k = (double)(pep.Rateconst);
                double ph = 1.5574E-4;
                double pw = proteinExperimentData.filecontents[proteinExperimentData.filecontents.Count - 1].BWE;

                var temp_maxval = proteinExperimentData.experiment_time.Max();
                //var step = 0.1;
                var step = 0.01;
                for (int i = 0; i * step < temp_maxval; i++)
                {
                    double temp_X = step * i;
                    x_val_temp.Add(temp_X);

                    var val1 = io * Math.Pow(1 - (pw / (1 - pw)), neh);
                    var val2 = io * Math.Pow(Math.E, -1 * k * temp_X) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));

                    var val = val1 + val2;
                    y_val_temp.Add(val);
                }
                chart_peptide.Series["Series5"].Points.DataBindXY(x_val_temp.OrderBy(x => x).ToList(), y_val_temp.OrderByDescending(x => x).ToList());

                #endregion
                */

                // chart title
                //chart_peptide.Titles.Add(peptideSeq);    

                Title title = new Title();
                title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                //title.Text = peptideSeq + " (K = " + Rateconst.ToString() + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ")";
                if (Double.IsNaN(Rateconst)) title.Text = peptideSeq + " (k = " + formatdoubletothreedecimalplace(Rateconst) + ", m/z = " + masstocharge.ToString("#0.###") + ", z = " + charge.ToString() + ")";
                else title.Text = peptideSeq + " (k = " + formatdoubletothreedecimalplace(Rateconst) + ", R" + "\u00B2" + " = " + RSquare.ToString("#0.#0") + ", m/z = " + masstocharge.ToString("#0.###") + ", z = " + charge.ToString() + ")";
                chart_peptide.Titles.Add(title);

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
                        chart2.ChartAreas[0].AxisX.Interval = (int)theoretical_chart_data.Select(x => x.time).ToArray().Max() / 10;
                        chart2.ChartAreas[0].AxisX.Maximum = x_val.Max() + 0.01;

                        chart2.ChartAreas[0].AxisY.Interval = theoretical_chart_data.Select(x => x.value).ToArray().Max() / 5;
                        chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";



                        #endregion


                        // chart title
                        //chart2.Titles.Add(p.PeptideSeq + "(K=" + p.Rateconst.ToString() + ", " + ")");
                        Title title = new Title();
                        title.Font = new Font(chart_peptide.Legends[0].Font.FontFamily, 8, FontStyle.Bold);
                        //title.Text = p.PeptideSeq + " (K = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ")";
                        title.Text = p.PeptideSeq + " (k = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                        chart2.Titles.Add(title);

                        chart2.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.07;

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

            // clean peptide chart
            chart_peptide.DataSource = null;
            chart_peptide.Titles.Clear();

            proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);
            proteinExperimentData.loadAllExperimentData();
            proteinExperimentData.computeAverageA0();
            proteinExperimentData.computeDeuteriumenrichmentInPeptide();
            proteinExperimentData.computeRIAPerExperiment();
            proteinExperimentData.mergeMultipleRIAPerDay2();
            proteinExperimentData.computeTheoreticalCurvePoints();
            proteinExperimentData.computeTheoreticalCurvePointsBasedOnExperimentalI0();
            proteinExperimentData.computeRSquare();
            ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();
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
            catch (Exception ex)
            {
                Console.WriteLine("test" + ex.Message);
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
    }
}
