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
        public double? StandDev_NumberPeptides_mean;
        double? StandDev_NumberPeptides_median;
        public double? TotalIonCurrent_1;

        // computed values
        List<RIA> RIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvalues = new List<RIA>();
        public List<ExpectedI0Value> expectedI0Values = new List<ExpectedI0Value>();
        public List<ExpectedI0Value> expectedI0Values_withExperimentalIO = new List<ExpectedI0Value>();
        public List<ExpectedI0Value> temp_expectedI0Values = new List<ExpectedI0Value>();

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
            this.MeanRateConst_CorrCutOff_median = rateConstInfoReader.MeanRateConst_CorrCutOff_CorrCutOff;
            this.MedianRateConst_RMSSCutOff_mean = rateConstInfoReader.MedianRateConst_RMSSCutOff_Median;
            this.MedianRateConst_RMSSCutOff_median = rateConstInfoReader.MedianRateConst_RMSSCutOff_RMSSCutOff;
            this.StandDev_NumberPeptides_mean = rateConstInfoReader.StandDev_NumberPeptides_StandDev;
            this.StandDev_NumberPeptides_median = rateConstInfoReader.StandDev_NumberPeptides_NumberPeptides;
            this.TotalIonCurrent_1 = rateConstInfoReader.TotalIonCurrent_1;

            // add rate constant values to peptied list
            foreach (Peptide p in peptides)
            {
                var rateconst = rateConstants.Where(x => x.PeptideSeq == p.PeptideSeq).ToList();
                if (rateconst.Count > 0)
                {
                    p.Rateconst = rateconst[0].RateConstant_value;

                    var AbsoluteIsotopeError = rateconst[0].AbsoluteIsotopeError;
                    if (AbsoluteIsotopeError == -100) p.IsotopeDeviation = 1.0;
                    else p.IsotopeDeviation = AbsoluteIsotopeError;
                }

            }
        }
        public void computeRIAPerExperiment()
        {

            foreach (ExperimentRecord er in experimentRecords)
            {
                var tempsum = er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5;
                double sum_val = tempsum != null ? (double)(er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5) : 0;
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
        public void mergeMultipleRIAPerDay2()
        {
            //var peptides = RIAvalues.Select(x => new { peptideSeq = x.peptideSeq, charge = x.charge }).Distinct().ToList(); 

            foreach (Peptide p in this.peptides)
            {
                //var temp_RIAvalues = RIAvalues.Where(x => x.peptideSeq == p.PeptideSeq & x.charge == p.Charge).ToList();
                //var temp_RIAvalues = RIAvalues.Where(x => x.RIA_value != null);
                //temp_RIAvalues = temp_RIAvalues.Where(x => x.peptideSeq == p.PeptideSeq);
                //temp_RIAvalues = temp_RIAvalues.Where(x => x.charge == p.Charge);

                List<RIA> temp_RIAvalues = new List<RIA>();
                foreach (RIA r in RIAvalues)
                {
                    if (r.RIA_value != null & r.peptideSeq == p.PeptideSeq & r.charge == p.Charge) temp_RIAvalues.Add(r);
                }


                foreach (int t in this.Experiment_time)
                {
                    //var temp_RIAvalues_pertime = temp_RIAvalues.Where(x => x.time == t).ToList();
                    List<RIA> temp_RIAvalues_pertime = temp_RIAvalues.Where(x => x.time == t).ToList();

                    RIA ria = new RIA();
                    ria.experimentNames = new List<string>();

                    ria.peptideSeq = p.PeptideSeq;
                    ria.charge = p.Charge;
                    ria.time = t;

                    //var sum_io = temp_RIAvalues_pertime.Sum(x => x.I0);
                    //var sum_ioria = temp_RIAvalues_pertime.Sum(x => x.I0 * x.RIA_value);
                    double sum_io = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_io = (double)(sum_io + temp_RIAvalues_pertime[i].I0);
                    double sum_ioria = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_ioria = (double)(
                            sum_ioria + (temp_RIAvalues_pertime[i].I0 * temp_RIAvalues_pertime[i].RIA_value));
                    var new_ria = sum_ioria / sum_io;

                    ria.RIA_value = new_ria;
                    ria.experimentNames = temp_RIAvalues_pertime.Select(x => x.experimentName).ToList();
                    mergedRIAvalues.Add(ria);

                }
            }


        }
        public void computeExpectedCurvePoints()
        {
            try
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

                            ExpectedI0Value expectedI0Value = new ExpectedI0Value(peptides[i].PeptideSeq, t, val, (double)peptides[i].Charge);
                            expectedI0Values.Add(expectedI0Value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error => computeExpectedCurvePoints(), " + ex.Message);
                            continue;
                        }

                    }
                }
            }
            catch (Exception e) { Console.WriteLine("Error => computeExpectedCurvePoints(), " + e.Message); }


        }

        public void computeExpectedCurvePointsBasedOnExperimentalIo()
        {
            try
            {
                double ph = 1.5574E-4;
                double pw = filecontents[filecontents.Count - 1].BWE;
                double io = 0;
                double neh = 0;
                double k = 0;

                var experimentalvalue_at_t0 = mergedRIAvalues.Where(x => x.time == 0).ToList();



                for (int i = 0; i < peptides.Count(); i++)
                {
                    var r = peptides[i];
                    var experimentalvalue = experimentalvalue_at_t0.Where(x => x.charge == r.Charge).ToList();
                    experimentalvalue = experimentalvalue.Where(x => x.peptideSeq == r.PeptideSeq).ToList();
                    var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();

                    if (temp_experimentalvalue.Count != 1) continue;

                    foreach (int t in this.Experiment_time)
                    {
                        try
                        {
                            io = (double)(temp_experimentalvalue.FirstOrDefault().RIA_value);
                            neh = (double)(peptides[i].Exchangeable_Hydrogens);
                            k = (double)(peptides[i].Rateconst);

                            var val1 = io * Math.Pow(1 - (pw / (1 - pw)), neh);
                            var val2 = io * Math.Pow(Math.E, -1 * k * t) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));


                            var val = val1 + val2;

                            ExpectedI0Value expectedI0Value_withExperimentalIO = new ExpectedI0Value(peptides[i].PeptideSeq, t, val, (double)peptides[i].Charge);
                            expectedI0Values_withExperimentalIO.Add(expectedI0Value_withExperimentalIO);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error => computeExpectedCurvePoints(), " + ex.Message);
                            continue;
                        }

                    }
                }
            }
            catch (Exception e) { Console.WriteLine("Error => computeExpectedCurvePoints(), " + e.Message); }


        }

        public void computeRSquare()
        {
            temp_expectedI0Values = new List<ExpectedI0Value>();

            //foreach (RateConstant r in rateConstants)
            foreach (Peptide r in this.peptides)
            {
                try
                {
                    var experimentalvalue = mergedRIAvalues.Where(x => x.charge == r.Charge).ToList();
                    experimentalvalue = experimentalvalue.Where(x => x.peptideSeq == r.PeptideSeq).ToList();
                    var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();
                    var meanval_ria = temp_experimentalvalue.Average(x => x.RIA_value);

                    var temp_computedRIAValue = expectedI0Values.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();
                    var temp_computedRIAValue_withexperimentalIO = expectedI0Values_withExperimentalIO.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();

                    if (temp_computedRIAValue_withexperimentalIO.Count == 0)
                    {

                        double ss = 0;
                        double rss = 0;

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue = temp_computedRIAValue.Where(x => x.time == p.time).First().value;
                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss = rss + Math.Pow((double)(p.RIA_value - computedRIAValue), 2);
                            }
                        }

                        double RSquare = 1 - (rss / ss);
                        r.RSquare = RSquare;
                        r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());
                        //temp_expectedI0Values.AddRange(temp_computedRIAValue);
                        foreach (var x in temp_computedRIAValue) temp_expectedI0Values.Add(x);
                    }

                    else
                    {
                        double ss = 0;
                        double rss_mo = 0;
                        double rss_io = 0;

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue_mo = temp_computedRIAValue.Where(x => x.time == p.time).First().value;
                                var computedRIAValue_io = temp_computedRIAValue_withexperimentalIO.Where(x => x.time == p.time).First().value;

                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss_mo = rss_mo + Math.Pow((double)(p.RIA_value - computedRIAValue_mo), 2);
                                rss_io = rss_io + Math.Pow((double)(p.RIA_value - computedRIAValue_io), 2);
                            }
                        }

                        var rss = rss_mo < rss_io ? rss_mo : rss_io;
                        double RSquare = 1 - (rss / ss);
                        r.RSquare = RSquare;
                        r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());

                        if (rss_mo < rss_io)
                        {
                            foreach (var x in temp_computedRIAValue) temp_expectedI0Values.Add(x);
                            //temp_expectedI0Values.AddRange(temp_computedRIAValue);
                        }
                        else
                        {
                            //temp_expectedI0Values.AddRange(temp_computedRIAValue_withexperimentalIO);
                            foreach (var x in temp_computedRIAValue_withexperimentalIO) temp_expectedI0Values.Add(x);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error => computeRSquare(), " + e.Message);
                }
            }

            expectedI0Values = new List<ExpectedI0Value>();
            expectedI0Values = temp_expectedI0Values;

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

                        var ria_val2 = mergedRIAvalues.Where(x => x.peptideSeq.Trim() == peptides[i].PeptideSeq.Trim() & x.time == t & x.charge == peptides[i].Charge).ToList();
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

        public ProtienchartDataValues computeValuesForEnhancedPerProtienPlot2()
        {
            List<double> xval = new List<double>();
            List<double> yval = new List<double>();

            try
            {
                double ph = 1.5574E-4;
                double pw = filecontents[filecontents.Count - 1].BWE;
                double io = 0;
                double neh = 0;
                double k = 0;
                var temp_pep = this.peptides.Where(x => x.RSquare > 0.8);
                foreach (RIA r in mergedRIAvalues)
                {

                    Peptide p = temp_pep.Where(x => x.PeptideSeq == r.peptideSeq & x.Charge == r.charge).FirstOrDefault();
                    if (p != null)
                    {
                        io = (double)(p.M0 / 100);
                        neh = (double)(p.Exchangeable_Hydrogens);
                        k = (double)(p.Rateconst);



                        var dn = io * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
                        var nu = io - r.RIA_value;
                        var fv = nu / dn;

                        xval.Add(r.time);
                        yval.Add((double)fv);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error => computeValuesForEnhancedPerProtienPlot2(), " + e.Message);
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
            public double? charge;

            public ExpectedI0Value(string peptideSeq, int t, double val, double charge)
            {
                peptideseq = peptideSeq;
                time = t;
                value = val;
                this.charge = charge;
            }

        }
    }
}
