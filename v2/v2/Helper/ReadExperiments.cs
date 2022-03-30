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
        string path = @"";
        public List<Peptide> peptides = new List<Peptide>();
        public List<ExperimentRecord> experimentRecords = new List<ExperimentRecord>();
        public List<string> experimentNames = new List<string>();
        public List<int> experimentTimes = new List<int>();

        public ReadExperiments(string path, List<int> experiment_time)
        {
            this.path = path;
            this.experimentTimes = experiment_time;
        }

        public void readExperimentCSVFile()
        {
            readExperimentCSVFile(this.path);
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
                    //first check the csv file is in the correct format
                    // for now we assume "Peptide" should be on the fourth row


                    if (lines.Length > 2 & (lines[1].Trim().Contains("Peptide, UniqueToProtein") ||
                        lines[1].Trim().Contains("Peptide,UniqueToProtein") ||
                        lines[1].Trim().Contains("Peptide ,UniqueToProtein")))
                    {
                        // the other varation of quant file drops the first two lines (line #1 & #2), which are protein name and desciption.
                        // to handel this, we strat read from first line as name of experiment
                        experimentNames = getExperimentNames(lines[0].Trim());

                        //extract the experimental data line by line
                        //the experimental data starts from line 5 (index=4)
                        for (int i = 2; i < lines.Length; i++)
                        {
                            readRow(lines[i]);
                        }

                    }
                    else if (lines.Length > 3 & (lines[3].Trim().Contains("Peptide, UniqueToProtein") ||
                         lines[3].Trim().Contains("Peptide,UniqueToProtein") ||
                         lines[3].Trim().Contains("Peptide ,UniqueToProtein")))
                    {
                        experimentNames = getExperimentNames(lines[2].Trim());

                        //extract the experimental data line by line
                        //the experimental data starts from line 5 (index=4)
                        for (int i = 4; i < lines.Length; i++)
                        {
                            readRow(lines[i]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error => .Quant.csv File is not in the right format");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("error ==>" + e.Message);
                    MessageBox.Show("Error => .Quant.csv File is not in the right format. " + e.Message);

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

                //read all the experiment values from the row
                //each experiment has 16 columns
                // starting from the 13th column (index = 12) read the experiment values by block of 16
                int start_index = 12;
                int current_index = 0;
                for (int i = 0; i < experimentNames.Count; i++)
                {
                    current_index = start_index + (i * 16);
                    ExperimentRecord experimentRecord = getExperimentsValuePerPeptide(columns, current_index);
                    experimentRecord.PeptideSeq = p.PeptideSeq;
                    experimentRecord.Charge = p.Charge;
                    experimentRecord.ExperimentName = experimentNames[i];
                    //experimentRecord.ExperimentTime = experimentTimes[i];
                    experimentRecords.Add(experimentRecord);
                }


            }

            catch (Exception e)
            {

                Console.WriteLine("error ==>" + e.Message);

                //MessageBox.Show("Error reading .Quant.csv ==> " + e.Message);
            }

        }

        public List<string> getExperimentNames(string line3)
        {
            line3 = Regex.Replace(line3, @",+", " ").Trim();
            line3 = Regex.Replace(line3, @"\s+", " ");
            return line3.Split(' ').ToList();
        }

        public Peptide getPeptideInfo(string[] columns)
        {
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
                MessageBox.Show("Error reading .Quant.csv ==> " + e.Message);
                return null;
            }

        }

        public ExperimentRecord getExperimentsValuePerPeptide(string[] columns, int index)
        {
            try
            {
                ExperimentRecord experimentRecord = new ExperimentRecord();
                if (columns[index].Trim().Length != 0) experimentRecord.SpecMass = double.Parse(columns[index]);
                if (columns[index + 1].Trim().Length != 0) experimentRecord.IonScore = double.Parse(columns[index + 1]);
                if (columns[index + 2].Trim().Length != 0) experimentRecord.Expectn = double.Parse(columns[index + 2]);
                if (columns[index + 3].Trim().Length != 0) experimentRecord.Error = double.Parse(columns[index + 3]);
                if (columns[index + 4].Trim().Length != 0) experimentRecord.Scan = double.Parse(columns[index + 4]);
                if (columns[index + 5].Trim().Length != 0) experimentRecord.I0 = double.Parse(columns[index + 5]);
                if (columns[index + 6].Trim().Length != 0) experimentRecord.I1 = double.Parse(columns[index + 6]);
                if (columns[index + 7].Trim().Length != 0) experimentRecord.I2 = double.Parse(columns[index + 7]);
                if (columns[index + 8].Trim().Length != 0) experimentRecord.I3 = double.Parse(columns[index + 8]);
                if (columns[index + 9].Trim().Length != 0) experimentRecord.I4 = double.Parse(columns[index + 9]);
                if (columns[index + 10].Trim().Length != 0) experimentRecord.I5 = double.Parse(columns[index + 10]);
                if (columns[index + 11].Trim().Length != 0) experimentRecord.Start_Elution = double.Parse(columns[index + 11]);
                if (columns[index + 12].Trim().Length != 0) experimentRecord.End_Elution = double.Parse(columns[index + 12]);
                if (columns[index + 13].Trim().Length != 0) experimentRecord.I0_Peak_Width = double.Parse(columns[index + 13]);
                if (columns[index + 14].Trim().Length != 0) experimentRecord.Total_Labeling = double.Parse(columns[index + 14]);
                if (columns[index + 15].Trim().Length != 0) experimentRecord.Net_Labeling = double.Parse(columns[index + 15]);

                return experimentRecord;
            }

            catch (Exception e)
            {
                Console.WriteLine("error ==>" + e.Message);
                MessageBox.Show("Error reading .Quant.csv ==> " + e.Message);
            }

            return null;
        }
    }
}
