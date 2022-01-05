using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using v2.Model;

namespace v2.Helper
{
    public class ReadExperiments
    {
        string path = @"F:\workplace\Data\temp_Mouse_Liver_0104_2022\CPSM_MOUSE.Quant.csv";

        List<Peptide> peptides = new List<Peptide>();
        List<ExperimentRecord> experimentRecords = new List<ExperimentRecord>();
        List<string> experimentNames = new List<string>();

        public ReadExperiments()
        {
            readExperimentCSVFile(path);
        }

        public ReadExperiments(string path)
        {
            this.path = path;
            readExperimentCSVFile(path);
        }

        public void readExperimentCSVFile(string path)
        {
            //Extracts the information from csv file

            //check if the file exists
            //string path = this.path;
            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    //line #1 & #2 are protein name and desciption

                    //line #3 contains the name of the experiments
                    experimentNames = getExperimentNames(lines[2].Trim());


                    //extract the experimental data line by line
                    //the experimental data starts from line 5 (index=4)
                    for (int i = 4; i < lines.Length; i++)
                    {
                        readRow(lines[i]);
                    }


                }
                catch (Exception e)
                {

                    Console.WriteLine("error ==>" + e.Message);

                    MessageBox.Show("error reading files.txt ==> " + e.Message);
                }
            }
        }

        public void readRow(string row)
        {
            try
            {
                // read each row and upate peptide and experiment records
                var columns = row.Trim().Split(',');
                columns = columns.Select(x => x.Trim()).ToArray();

                // the first 12 columns are peptide information
                Peptide p = getPeptideInfo(columns);
                peptides.Add(p);
                                
            }

            catch (Exception e)
            {

                Console.WriteLine("error ==>" + e.Message);

                MessageBox.Show("error reading files.txt ==> " + e.Message);
            }

        }

        public List<string> getExperimentNames(string line3)
        {
            line3 = Regex.Replace(line3, @",+", " ").Trim();
            line3 = Regex.Replace(line3, @"\s+", " ");
            return line3.Split(' ').ToList();
        }

        public Peptide getPeptideInfo(string[] columns) {


            try
            {
                
                // the first 12 columns are peptide information
                Peptide p = new Peptide();
                p.PeptideSeq = columns[0];
                if (columns[1].Trim().Length != 0) p.UniqueToProtein = (columns[1].Trim() == "Yes") ? true : false;
                if (columns[2].Trim().Length != 0) p.Exchangeable_Hydrogens = double.Parse(columns[2]);
                if (columns[3].Trim().Length != 0) p.Charge = double.Parse(columns[3]);
                if (columns[4].Trim().Length != 0) p.SeqMass = double.Parse(columns[4]);
                if (columns[5].Trim().Length != 0) p.M0 = double.Parse(columns[5]);
                if (columns[6].Trim().Length != 0) p.M1 = double.Parse(columns[6]);
                if (columns[7].Trim().Length != 0) p.M2 = double.Parse(columns[7]);
                if (columns[8].Trim().Length != 0) p.M3 = double.Parse(columns[8]);
                if (columns[9].Trim().Length != 0) p.M4 = double.Parse(columns[9]);
                if (columns[10].Trim().Length != 0) p.M5 = double.Parse(columns[10]);
                if (columns[11].Trim().Length != 0) p.Total_Labeling = double.Parse(columns[11]);

                return p;
            }

            catch (Exception e)
            {

                Console.WriteLine("error ==>" + e.Message);

                MessageBox.Show("error reading files.txt ==> " + e.Message);
                return null;
            }

        }
    }
}
