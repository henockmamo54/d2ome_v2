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
using v2.Model;

namespace v2
{
    public partial class Main : Form
    {
        List<mzMlmzIDModel> inputdata = new List<mzMlmzIDModel>();
        public Main()
        {
            InitializeComponent();
        }

        public void load_defaultValues()
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

                var path = textBox_outputfolderpath.Text;

                #region files.txt 

                TextWriter tw = new StreamWriter(path + "\\files.txt");
                //TextWriter tw = new StreamWriter("files.txt");
                string fileContent = "";
                foreach (var x in inputdata)
                {
                    fileContent += x.T.ToString() + " " + x.mzML + " " + x.mzID + " " + x.BWE.ToString() + "\n";
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


                Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
                thread.Start();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }



        }

        private void WorkThreadFunction()
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
                p.OutputDataReceived += p_OutputDataReceived;
                p.ErrorDataReceived += p_ErrorDataReceived;

                p.EnableRaisingEvents = true;
                p.Exited += P_Exited;

                // start process
                p.Start();

                //wait
                p.WaitForExit();
            }



            //using (Process p = new Process())
            //{
            //    // set start info
            //    p.StartInfo = new ProcessStartInfo(sExecsFolder + "\\d2ome.exe ", textBox_outputfolderpath.Text + "\\files.txt")
            //    {
            //        //RedirectStandardInput = true,
            //        UseShellExecute = false,
            //        WorkingDirectory = textBox_outputfolderpath.Text
            //    };
            //    // event handlers for output & error
            //    p.OutputDataReceived += p_OutputDataReceived;
            //    p.ErrorDataReceived += p_ErrorDataReceived;

            //    p.EnableRaisingEvents = true;
            //    p.Exited += P_Exited;

            //    // start process
            //    p.Start();

            //    //wait
            //    p.WaitForExit();
            //}



            ////Process p = new Process();
            ////p.EnableRaisingEvents = true;
            ////p.Exited += P_Exited;

            ////ProcessStartInfo processStartInfo = new ProcessStartInfo(sExecsFolder + "\\d2ome.exe ", textBox_outputfolderpath.Text + "\\files.txt");
            ////processStartInfo.RedirectStandardInput = true;
            ////processStartInfo.UseShellExecute = false;

            ////p.StartInfo = processStartInfo;
            ////p.OutputDataReceived += p_OutputDataReceived;
            ////p.ErrorDataReceived += p_ErrorDataReceived;

            ////p.Start();
            ////p.WaitForExit();


        }

        public void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;
            Console.WriteLine(e.Data);
        }

        public void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;

            Console.WriteLine(e.Data);


        }




        private void P_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            var temp = p.ExitCode;
            MessageBox.Show(temp.ToString());
        }

        private void Main_Load(object sender, EventArgs e)
        {
            load_defaultValues();
            inputdata = new List<mzMlmzIDModel>();
            this.dataGridView1_records.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
                                var mzmlIDfilerecord = new mzMlmzIDModel();
                                mzmlIDfilerecord.mzML = textBox_mzmlfile.Text.Trim();
                                mzmlIDfilerecord.mzID = textBox_mzidfile.Text.Trim();
                                mzmlIDfilerecord.T = double.Parse(textBox_T.Text.Trim());
                                mzmlIDfilerecord.BWE = double.Parse(textBox_BWE.Text.Trim());

                                if (mzmlIDfilerecord.BWE < 0 || mzmlIDfilerecord.BWE > 1)
                                {
                                    MessageBox.Show("BWE should be non-negative and less than 1.0\n"); return;
                                }
                                if (mzmlIDfilerecord.T < 0)
                                {
                                    MessageBox.Show("T should be non-negative \n"); return;
                                }

                                inputdata.Add(mzmlIDfilerecord);

                                dataGridView1_records.DataSource = inputdata.ToList();

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
            dataGridView1_records.DataSource = null;
            inputdata = new List<mzMlmzIDModel>();
            dataGridView1_records.DataSource = inputdata;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                var index = dataGridView1_records.SelectedRows[0].Index;
                if (index < inputdata.Count & index >= 0)
                {
                    dataGridView1_records.DataSource = null;
                    inputdata.RemoveAt(index);
                    dataGridView1_records.DataSource = inputdata;
                }

            }
            catch (Exception ex1)
            {
                MessageBox.Show("Please select the row you want to delete. " + ex1.Message, "Error");
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
                    MessageBox.Show("ERROR", "mzML and mzid files should be in matched pairs");
                    return;
                }

                if (mzml.Count() == 0 | mzid.Count() == 0)
                {
                    MessageBox.Show("The folder does not contain the required files (.mzML and .mzid files)", "ERROR");
                    return;
                }

                else
                {
                    inputdata = new List<mzMlmzIDModel>();
                    foreach (var mz in mzml)
                    {
                        mzMlmzIDModel k = new mzMlmzIDModel();
                        k.mzML = mz;
                        k.mzID = mz.Replace(".mzML", ".mzid");
                        k.T = 0;
                        k.BWE = 0;
                        inputdata.Add(k);
                    }

                    //comboBox_mzidfilelist.DataSource = mzid;
                    //comboBox_mzmlfilelist.DataSource = mzml;
                }

                dataGridView1_records.DataSource = inputdata;
                //this.dataGridView1_records.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
        }

        private void dataGridView1_records_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Console.WriteLine("test");

            string headerText =
        dataGridView1_records.Columns[e.ColumnIndex].HeaderText.Trim();

            if (headerText.Equals("mzML"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText =
                        "mzML file name must not be empty";
                    e.Cancel = true;
                }
                else if (!(Path.GetExtension(e.FormattedValue.ToString()).Trim().Equals(".mzML")))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText =
                        "mzML file is not found in this path. Please check the file path";
                    e.Cancel = true;
                }
            }
            if (headerText.Equals("mzID"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText =
                        "mzID file name must not be empty";
                    e.Cancel = true;
                }
                else if (!(Path.GetExtension(e.FormattedValue.ToString()).Trim().Equals(".mzid")))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText =
                        "mzID file is not found in this path. Please check the file path";
                    e.Cancel = true;
                }
            }
            if (headerText.Equals("T"))
            {
                double i = 0;
                if (!double.TryParse(e.FormattedValue.ToString(), out i))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText = ("Time value is Not valid.");
                    e.Cancel = true;
                }
                else if (double.Parse(e.FormattedValue.ToString()) < 0)
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText = ("Time value is Not valid.");
                    e.Cancel = true;
                }

            }
            if (headerText.Equals("BWE"))
            {
                double i = 0;
                if (!double.TryParse(e.FormattedValue.ToString(), out i))
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText = ("BWE value is Not valid.");
                    e.Cancel = true;
                }
                else if (double.Parse(e.FormattedValue.ToString()) < 0 || double.Parse(e.FormattedValue.ToString()) > 1)
                {
                    dataGridView1_records.Rows[e.RowIndex].ErrorText = ("BWE value is Not valid.");
                    e.Cancel = true;
                }
            }

        }

        private void dataGridView1_records_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView1_records_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_records.Rows[e.RowIndex].ErrorText = String.Empty;
        }
    }
}
