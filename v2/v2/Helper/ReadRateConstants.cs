using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using v2.Model;
namespace v2.Helper
{
    public class ReadRateConstants
    {
        public List<RateConstant> rateConstants = new List<RateConstant>();
        public double? MeanRateConst_CorrCutOff_mean;
        public double? MeanRateConst_CorrCutOff_median;
        public double? MedianRateConst_RMSSCutOff_mean;
        public double? MedianRateConst_RMSSCutOff_median;
        public double? StandDev_NumberPeptides_mean;
        public double? StandDev_NumberPeptides_median;
        public double? TotalIonCurrent_1;
        public string path = @"";

        public ReadRateConstants(string path)
        {
            this.path = path;
        }

        public void readRateConstantsCSV()
        {
            readRateConstantsCSV(this.path);
        }

        public void readRateConstantsCSV(string path)
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

                    for (int i = 1; i < lines.Length; i++)
                    {
                        // this csv file has structured table data on the top and 
                        //four paramters at the end of the files. first check for those
                        //paramters.

                        string line = lines[i].Trim();

                        if (line.Contains("MeanRateConst/CorrCutOff"))
                        {
                            var columns = line.Split(',');
                            if (columns[1].Trim().Length > 0) MeanRateConst_CorrCutOff_mean = double.Parse(columns[1].Trim());
                            if (columns[2].Trim().Length > 0) MeanRateConst_CorrCutOff_median = double.Parse(columns[2].Trim());
                        }
                        else if (line.Contains("MedianRateConst/RMSSCutOff"))
                        {
                            var columns = line.Split(',');
                            if (columns[1].Trim().Length > 0) MedianRateConst_RMSSCutOff_mean = double.Parse(columns[1].Trim());
                            if (columns[2].Trim().Length > 0) MedianRateConst_RMSSCutOff_median = double.Parse(columns[2].Trim());
                        }
                        else if (line.Contains("StandDev/NumberPeptides"))
                        {
                            var columns = line.Split(',');
                            if (columns[1].Trim().Length > 0) StandDev_NumberPeptides_mean = double.Parse(columns[1].Trim());
                            if (columns[2].Trim().Length > 0) StandDev_NumberPeptides_median = double.Parse(columns[2].Trim());
                        }
                        else if (line.Contains("TotalIonCurrent"))
                        {
                            var columns = line.Split(',');
                            if (columns[1].Trim().Length > 0) TotalIonCurrent_1 = double.Parse(columns[1].Trim());
                        }
                        else
                        {
                            var columns = line.Split(',');

                            RateConstant rateConstant = new RateConstant();
                            rateConstant.PeptideSeq = columns[0].Trim();
                            if (columns[2].Trim().Length != 0) rateConstant.RateConstant_value = double.Parse(columns[2].Trim());
                            if (columns[3].Trim().Length != 0) rateConstant.Correlations = double.Parse(columns[3].Trim());
                            if (columns[4].Trim().Length != 0) rateConstant.RootMeanRSS = double.Parse(columns[4].Trim());
                            if (columns[5].Trim().Length != 0) rateConstant.AbsoluteIsotopeError = double.Parse(columns[5].Trim());
                            rateConstants.Add(rateConstant);
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine("error ==>" + e.Message);
                    MessageBox.Show("error reading files.txt ==> " + e.Message);
                }
            }
        }
    }
}
