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
        public List<RateConstant> rateConstants = new List<RateConstant>();
        public double? MeanRateConst_CorrCutOff_mean;
        double? MeanRateConst_CorrCutOff_median;
        double? MedianRateConst_RMSSCutOff_mean;
        double? MedianRateConst_RMSSCutOff_median;
        double? StandDev_NumberPeptides_mean;
        double? StandDev_NumberPeptides_median;
        double? TotalIonCurrent_1;

        // computed values
        List<RIA> RIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvalues = new List<RIA>();
        public List<ExpectedI0Value> expectedI0Values = new List<ExpectedI0Value>();

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

            // add rate constant values to peptied list
            foreach (Peptide p in peptides)
            {
                var rateconst = rateConstants.Where(x => x.PeptideSeq == p.PeptideSeq).ToList();
                if (rateconst.Count > 0) p.Rateconst = rateconst[0].RateConstant_value;
            }
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
                    ria.charge = er.charge;

                    //get the experiment time from files.txt values
                    var temp = filecontents.Where(x => x.experimentID == er.experimentName).Select(t => t.time).ToArray();
                    if (temp.Length > 0) ria.time = temp[0];

                    RIAvalues.Add(ria);
                }
            }

        }
        public void mergeMultipleRIAPerDay()
        {
            //var peptides = RIAvalues.Select(x => new { peptideSeq = x.peptideSeq, charge = x.charge }).Distinct().ToList();



            foreach (Peptide p in this.peptides)
            {
                foreach (int t in this.Experiment_time)
                {
                    var temp = RIAvalues.Where(x => x.peptideSeq == p.PeptideSeq & x.charge == p.Charge & x.time == t).ToList();

                    if (temp.Count == 1)
                    {
                        RIA ria = new RIA();
                        ria.experimentNames = new List<string>();

                        ria.peptideSeq = p.PeptideSeq;
                        ria.charge = p.Charge;
                        ria.time = t;
                        ria.experimentNames.Add(temp[0].experimentName);
                        ria.RIA_value = temp[0].RIA_value;
                        mergedRIAvalues.Add(ria);
                    }
                    else if (temp.Count > 1)
                    {
                        RIA ria = new RIA();
                        ria.experimentNames = new List<string>();

                        ria.peptideSeq = p.PeptideSeq;
                        ria.charge = p.Charge;
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
        public void computeExpectedCurvePoints()
        {
            double ph = 1.5574E-4;
            double pw = filecontents[filecontents.Count - 1].BWE;
            double io = 0;
            double neh = 0;
            double k = 0;

            for (int i = 0; i < peptides.Count(); i++)
            {
                foreach (int t in this.Experiment_time)
                {
                    try
                    {
                        io = (double)(peptides[i].M0 / 100);
                        neh = (double)(peptides[i].Exchangeable_Hydrogens);
                        k = (double)(peptides[i].Rateconst);

                        var val1 = io * Math.Pow(1 - (pw / (1 - pw)), neh);
                        var val2 = io * Math.Pow(Math.E, -1 * k * t) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));


                        var val = val1 + val2;

                        ExpectedI0Value expectedI0Value = new ExpectedI0Value(peptides[i].PeptideSeq, t, val);
                        expectedI0Values.Add(expectedI0Value);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }


        }

        public void computeRSquare()
        {

            //foreach (RateConstant r in rateConstants)
            foreach (Peptide r in this.peptides)
            {
                try
                {
                    var experimentalvalue = mergedRIAvalues.Where(x => x.peptideSeq == r.PeptideSeq & x.charge == r.Charge).ToList();

                    var meanval_ria = experimentalvalue.Average(x => x.RIA_value);

                    double ss = 0;
                    double rss = 0;

                    foreach (var p in experimentalvalue)
                    {
                        if (p.RIA_value != null)
                        {
                            var computedRIAValue = expectedI0Values.Where(x => x.peptideseq == p.peptideSeq & x.time == p.time).First().value;
                            ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                            rss = rss + Math.Pow((double)(p.RIA_value - computedRIAValue), 2);
                        }
                    }

                    double RSquare = 1 - (rss / ss);
                    r.RSquare = RSquare;

                    //var temp = peptides.Where(x => x.PeptideSeq.Trim() == r.PeptideSeq.Trim() & x.Charge = r.Charge).ToList();
                    //if (temp.Count > 1)
                    //{
                    //    Console.WriteLine("tre");
                    //}
                    //foreach (var t in temp) t.RSquare = RSquare;
                }
                catch (Exception e) {
                    Console.WriteLine("error==>"+e.Message);
                }
            }

        }

        public ProtienchartDataValues computeValuesForEnhancedPerProtienPlot()
        {
            List<double> xval = new List<double>();
            List<double> yval = new List<double>();

            double ph = 1.5574E-4;
            double pw = filecontents[filecontents.Count - 1].BWE;
            double io = 0;
            double neh = 0;
            double k = 0;

            for (int i = 0; i < peptides.Count(); i++)
            {
                foreach (int t in this.Experiment_time)
                {
                    try
                    {
                        io = (double)(peptides[i].M0 / 100);
                        neh = (double)(peptides[i].Exchangeable_Hydrogens);
                        k = (double)(peptides[i].Rateconst);

                        var ria_val2 = mergedRIAvalues.Where(x => x.peptideSeq.Trim() == peptides[i].PeptideSeq.Trim() & x.time == t).ToList();
                        if (ria_val2.Count > 0)
                        {
                            var ria_val = ria_val2.First().RIA_value;

                            var dn = io * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
                            var nu = io - ria_val;
                            var fv = nu / dn;

                            xval.Add(t);
                            yval.Add((double)fv);
                        }

                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }
            //ProtienchartDataValues pd = new ProtienchartDataValues(xval, yval);
            return new ProtienchartDataValues(xval, yval);

        }

        public struct ProtienchartDataValues
        {
            public List<double> x;
            public List<double> y;

            public ProtienchartDataValues(List<double> x, List<double> y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public struct ExpectedI0Value
        {
            public string peptideseq;
            public int time;
            public double value;

            public ExpectedI0Value(string peptideSeq, int t, double val)
            {
                peptideseq = peptideSeq;
                time = t;
                value = val;
            }

        }
    }
}
