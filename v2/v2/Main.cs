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

        public bool check_inputDatagridIsIntheRightFormat()
        {
            try
            {
                List<mzMlmzIDModel> gridviewboundedData = (List<mzMlmzIDModel>)dataGridView1_records.DataSource;
            }
            catch (Exception e) {
                MessageBox.Show("");
            }

            return false;

        }

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

                    comboBox_mzidfilelist.DataSource = mzid;
                    comboBox_mzmlfilelist.DataSource = mzml;
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
        }
    }
}
