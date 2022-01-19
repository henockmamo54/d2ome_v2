using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
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

                else
                {

                    foreach (var mz in mzml)
                    {
                        mzMlmzIDModel k = new mzMlmzIDModel();
                        k.mzML = mz;
                        k.mzID = mz.Replace(".mzML", ".mzid");
                        inputdata.Add(k);
                    }

                }

                dataGridView1_records.DataSource = inputdata;
                this.dataGridView1_records.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;


                //string[] filePaths = Directory.GetFiles(path);
                //var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

                //if (csvfilePaths.Count == 0)
                //{
                //    MessageBox.Show("This directory doesn't contain the necessary files. Please select another diroctory.");
                //}
                //else
                //{
                //    var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).ToList();
                //    comboBox_proteinNameSelector.DataSource = temp.Distinct().ToList();
                //}
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("test");
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
            if (textBox1_mzmlidfiles.Text.Trim().Count() > 0)
            {
                try
                {
                    dialog.InitialDirectory = textBox1_mzmlidfiles.Text.Trim();
                }
                catch
                {
                    dialog.InitialDirectory = "c:\\";
                }
            }

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
            if (textBox1_mzmlidfiles.Text.Trim().Count() > 0)
            {
                try
                {
                    dialog.InitialDirectory = textBox1_mzmlidfiles.Text.Trim();
                }
                catch
                {
                    dialog.InitialDirectory = "c:\\";
                }
            }

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
