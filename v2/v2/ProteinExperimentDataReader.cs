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
        List<Peptide> peptides = new List<Peptide>();
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

        public ProteinExperimentDataReader() { }
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


    }
}
