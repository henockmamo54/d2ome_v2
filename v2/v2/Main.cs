﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        //public bool check_inputDatagridIsIntheRightFormat()
        //{
        //    try
        //    {
        //        List<mzMlmzIDModel> gridviewboundedData = (List<mzMlmzIDModel>)dataGridView1_records.DataSource;
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("");
        //    }

        //    return false;

        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog dialog = new FolderBrowserDialog();
        //    if (DialogResult.OK == dialog.ShowDialog())
        //    {
        //        string path = dialog.SelectedPath;

        //        textBox1_mzmlidfiles.Text = path;

        //        string[] filePaths = Directory.GetFiles(path);

        //        var mzml = filePaths.Where(x => x.Contains(".mzML")).ToList();
        //        var mzid = filePaths.Where(x => x.Contains(".mzid")).ToList();

        //        if (mzml.Count() != mzid.Count())
        //        {
        //            MessageBox.Show("ERROR", "mzML and mzid files should be in matched pairs");
        //            return;
        //        }

        //        else
        //        {

        //            foreach (var mz in mzml)
        //            {
        //                mzMlmzIDModel k = new mzMlmzIDModel();
        //                k.mzML = mz;
        //                k.mzID = mz.Replace(".mzML", ".mzid");
        //                inputdata.Add(k);
        //            }

        //        }

        //        dataGridView1_records.DataSource = inputdata;
        //        this.dataGridView1_records.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;


        //        //string[] filePaths = Directory.GetFiles(path);
        //        //var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

        //        //if (csvfilePaths.Count == 0)
        //        //{
        //        //    MessageBox.Show("This directory doesn't contain the necessary files. Please select another diroctory.");
        //        //}
        //        //else
        //        //{
        //        //    var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
        //        //    comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
        //        //}
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            #region files.txt
            Console.WriteLine("test");

            TextWriter tw = new StreamWriter("SavedLists.txt");
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

            TextWriter tw2 = new StreamWriter("quant.state");


            tw2.WriteLine(quantstatefile);
            tw2.Close();

            #endregion

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

                                inputdata.Add(mzmlIDfilerecord);

                                dataGridView1_records.DataSource = inputdata.ToList();

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
            inputdata.Clear();
            dataGridView1_records.DataSource = inputdata;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                var index = dataGridView1_records.SelectedRows[0].Index;
                inputdata.RemoveAt(index);
                dataGridView1_records.DataSource = inputdata;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select the row you want to delete.", "Error");
            }
        }
    }
}
