using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v2.Helper;
using v2.Model;
using static v2.Helper.ReadFilesInfo_txt;

namespace v2
{
    public class ProteinExperimentDataReader
    {
        string files_txt_path = @"";
        string quant_csv_path = @"";
        string RateConst_csv_path = @"";

        // propperites from Protein.files.txt
        public List<int> Experiment_time = new List<int>(); //contains unique time values
        public List<string> experimentIDs = new List<string>();
        public List<FileContent> filecontents = new List<FileContent>();

        // properties from Protein.quant.csv 
        public List<Peptide> peptides = new List<Peptide>();
        List<ExperimentRecord> experimentRecords = new List<ExperimentRecord>();

        // propeties from Protein.Rateconst.csv
        List<RateConstant> rateConstants = new List<RateConstant>();
        double? MeanRateConst_CorrCutOff_mean;
        double? MeanRateConst_CorrCutOff_median;
        double? MedianRateConst_RMSSCutOff_mean;
        double? MedianRateConst_RMSSCutOff_median;
        double? StandDev_NumberPeptides_mean;
        double? StandDev_NumberPeptides_median;
        double? TotalIonCurrent_1;

        // computed values
        List<RIA> RIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvalues = new List<RIA>();
                
        public ProteinExperimentDataReader(string files_txt_path, string quant_csv_path, string RateConst_csv_path)
        {
            this.files_txt_path = files_txt_path;
            this.quant_csv_path = quant_csv_path;
            this.RateConst_csv_path = RateConst_csv_path;
        }
        public void loadAllExperimentData()
        {

            //Files.txt reader
            ReadFilesInfo_txt filesinfo = new ReadFilesInfo_txt(files_txt_path);
            filesinfo.readFile();

            this.Experiment_time = filesinfo.time;
            this.experimentIDs = filesinfo.experimentIDs;
            this.filecontents = filesinfo.filecontents;

            //Protein.Quant.csv reader
            ReadExperiments experiInfoReader = new ReadExperiments(quant_csv_path);
            experiInfoReader.readExperimentCSVFile();

            this.peptides = experiInfoReader.peptides;
            this.experimentRecords = experiInfoReader.experimentRecords;


            //Protein.Quant.csv reader
            ReadRateConstants rateConstInfoReader = new ReadRateConstants(RateConst_csv_path);
            rateConstInfoReader.readRateConstantsCSV();

            this.rateConstants = rateConstInfoReader.rateConstants;
            this.MeanRateConst_CorrCutOff_mean = rateConstInfoReader.MeanRateConst_CorrCutOff_mean;
            this.MeanRateConst_CorrCutOff_median = rateConstInfoReader.MeanRateConst_CorrCutOff_median;
            this.MedianRateConst_RMSSCutOff_mean = rateConstInfoReader.MedianRateConst_RMSSCutOff_mean;
            this.MedianRateConst_RMSSCutOff_median = rateConstInfoReader.MedianRateConst_RMSSCutOff_median;
            this.StandDev_NumberPeptides_mean = rateConstInfoReader.StandDev_NumberPeptides_mean;
            this.StandDev_NumberPeptides_median = rateConstInfoReader.StandDev_NumberPeptides_median;
            this.TotalIonCurrent_1 = rateConstInfoReader.TotalIonCurrent_1;

        }

        public void computeRIAPerExperiment()
        {

            foreach (ExperimentRecord er in experimentRecords)
            {
                double sum_val = (double)(er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5);
                if (sum_val != 0)
                {
                    RIA ria = new RIA();
                    ria.experimentName = er.experimentName;
                    ria.peptideSeq = er.PeptideSeq;
                    ria.RIA_value = er.I0 / sum_val;
                    ria.I0 = er.I0;

                    //get the experiment time from files.txt values
                    var temp = filecontents.Where(x => x.experimentID == er.experimentName).Select(t => t.time).ToArray();
                    if (temp.Length > 0) ria.time = temp[0];

                    RIAvalues.Add(ria);
                }
            }

        }

        public void mergeMultipleRIAPerDay()
        {
            var peptides = RIAvalues.Select(x => x.peptideSeq).Distinct().ToList();

            for (int i = 0; i < peptides.Count(); i++)
            {
                foreach (int t in this.Experiment_time)
                {
                    var temp = RIAvalues.Where(x => x.peptideSeq == peptides[i] & x.time == t).ToList();

                    if (temp.Count == 1)
                    {
                        RIA ria = new RIA();
                        ria.experimentNames = new List<string>();

                        ria.peptideSeq = peptides[i];
                        ria.time = t;
                        ria.experimentNames.Add(temp[0].experimentName);
                        ria.RIA_value = temp[0].RIA_value;
                        mergedRIAvalues.Add(ria);
                    }
                    else if (temp.Count > 1)
                    {
                        RIA ria = new RIA();
                        ria.experimentNames = new List<string>();

                        ria.peptideSeq = peptides[i];
                        ria.time = t;

                        var sum_io = temp.Sum(x => x.I0);
                        var sum_ioria = temp.Sum(x => x.I0 * x.RIA_value);
                        var new_ria = sum_ioria / sum_io;

                        ria.RIA_value = new_ria;
                        ria.experimentNames = temp.Select(x => x.experimentName).ToList();
                        mergedRIAvalues.Add(ria);
                    }

                }
            }


        }
    }
}
