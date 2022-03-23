﻿using System;
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
        public List<int> experiment_time = new List<int>(); //contains unique time values
        public List<string> experimentIDs = new List<string>();
        public List<FileContent> filecontents = new List<FileContent>();

        // properties from Protein.quant.csv 
        public List<Peptide> peptides = new List<Peptide>();
        List<ExperimentRecord> experimentRecords = new List<ExperimentRecord>();

        // propeties from Protein.Rateconst.csv
        public List<RateConstant> rateConstants = new List<RateConstant>();
        public double? MeanRateConst;
        double? MeanRateConst_CorrCutOff;
        double? MedianRateConst;
        double? MedianRateConst_RMSSCutOff;
        public double? StandDev_NumberPeptides_StandDev;
        double? StandDev_NumberPeptides_NumberPeptides;
        public double? TotalIonCurrent;

        // computed values
        List<RIA> RIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvalues = new List<RIA>();
        public List<TheoreticalI0Value> theoreticalI0Values = new List<TheoreticalI0Value>();
        public List<TheoreticalI0Value> theoreticalI0Values_withExperimentalIO = new List<TheoreticalI0Value>();
        public List<TheoreticalI0Value> temp_theoreticalI0Values = new List<TheoreticalI0Value>();

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

            this.experiment_time = filesinfo.time;
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
            this.MeanRateConst = rateConstInfoReader.MeanRateConst;
            this.MeanRateConst_CorrCutOff = rateConstInfoReader.MeanRateConst_CorrCutOff;
            this.MedianRateConst = rateConstInfoReader.MedianRateConst;
            this.MedianRateConst_RMSSCutOff = rateConstInfoReader.MedianRateConst_RMSSCutOff;
            this.StandDev_NumberPeptides_StandDev = rateConstInfoReader.StandDev_NumberPeptides_StandDev;
            this.StandDev_NumberPeptides_NumberPeptides = rateConstInfoReader.StandDev_NumberPeptides_NumberPeptides;
            this.TotalIonCurrent = rateConstInfoReader.TotalIonCurrent;

            // add rate constant values to peptied list
            foreach (Peptide p in peptides)
            {
                var rateconst = rateConstants.Where(x => x.PeptideSeq == p.PeptideSeq).ToList();
                if (rateconst.Count == 1)
                {
                    p.Rateconst = rateconst[0].RateConstant_value;

                    var AbsoluteIsotopeError = (double)rateconst[0].AbsoluteIsotopeError;
                    if (AbsoluteIsotopeError == -100) p.IsotopeDeviation = 1.0;
                    else p.IsotopeDeviation = AbsoluteIsotopeError;

                }
                else if (rateconst.Count > 1)
                {
                    var mult_pep = peptides.Where(x => x.PeptideSeq == p.PeptideSeq).OrderBy(x => x.Order).ToList();
                    rateconst = rateconst.OrderBy(x => x.order).ToList();
                    for (int t = 0; t < rateconst.Count; t++)
                    {
                        var rp = mult_pep[t];
                        rp.Rateconst = rateconst[t].RateConstant_value;

                        var AbsoluteIsotopeError = (double)rateconst[t].AbsoluteIsotopeError;
                        if (AbsoluteIsotopeError == -100) rp.IsotopeDeviation = 1.0;
                        else rp.IsotopeDeviation = AbsoluteIsotopeError;
                    }


                }
            }
            //for (int i = 0; i < peptides.Count; i++)
            //{



            //    Console.WriteLine(i.ToString());

            //    var p = peptides[i];
            //    var rateconst = rateConstants.Where(x => x.PeptideSeq == p.PeptideSeq).ToList();

            //    Console.WriteLine(p.PeptideSeq + rateconst.Count.ToString());

            //    if (p.Rateconst == null & rateconst.Count > 0)
            //    {
            //        int countofRC = rateconst.Count();
            //        double AbsoluteIsotopeError = 0;
            //        for (int j = 0; j < countofRC; j++)
            //        {
            //            try
            //            {
            //                p = peptides[i + j];
            //                p.Rateconst = rateconst[j].RateConstant_value;

            //                AbsoluteIsotopeError = (double)rateconst[j].AbsoluteIsotopeError;
            //                if (AbsoluteIsotopeError == -100) p.IsotopeDeviation = 1.0;
            //                else p.IsotopeDeviation = AbsoluteIsotopeError;
            //            }
            //            catch { }

            //        }



            //        Console.WriteLine(i.ToString());
            //    }

            //}
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
                    ria.ExperimentName = er.ExperimentName;
                    ria.PeptideSeq = er.PeptideSeq;
                    ria.RIA_value = er.I0 / sum_val;
                    ria.I0 = er.I0;
                    ria.Charge = er.Charge;

                    //get the experiment time from files.txt values
                    var temp = filecontents.Where(x => x.experimentID == er.ExperimentName).Select(t => t.time).ToArray();
                    if (temp.Length > 0) ria.Time = temp[0];

                    RIAvalues.Add(ria);
                }
            }

        }
        public void mergeMultipleRIAPerDay()
        {
            //var peptides = RIAvalues.Select(x => new { peptideSeq = x.peptideSeq, charge = x.charge }).Distinct().ToList();



            foreach (Peptide p in this.peptides)
            {
                foreach (int t in this.experiment_time)
                {
                    var temp = RIAvalues.Where(x => x.PeptideSeq == p.PeptideSeq & x.Charge == p.Charge & x.Time == t).ToList();

                    if (temp.Count == 1)
                    {
                        RIA ria = new RIA();
                        ria.ExperimentNames = new List<string>();

                        ria.PeptideSeq = p.PeptideSeq;
                        ria.Charge = p.Charge;
                        ria.Time = t;
                        ria.ExperimentNames.Add(temp[0].ExperimentName);
                        ria.RIA_value = temp[0].RIA_value;
                        mergedRIAvalues.Add(ria);
                    }
                    else if (temp.Count > 1)
                    {
                        RIA ria = new RIA();
                        ria.ExperimentNames = new List<string>();

                        ria.PeptideSeq = p.PeptideSeq;
                        ria.Charge = p.Charge;
                        ria.Time = t;

                        var sum_io = temp.Sum(x => x.I0);
                        var sum_ioria = temp.Sum(x => x.I0 * x.RIA_value);
                        var new_ria = sum_ioria / sum_io;

                        ria.RIA_value = new_ria;
                        ria.ExperimentNames = temp.Select(x => x.ExperimentName).ToList();
                        mergedRIAvalues.Add(ria);
                    }

                }
                // add unique count of unique data points
                var peptides = mergedRIAvalues.Select(x => new { peptideSeq = x.PeptideSeq, charge = x.Charge }).Distinct().ToList();
                p.NDP = peptides.Count();
            }


        }
        public void normalizeRIAValues()
        {

            List<RIA> normalizedRIAvalues = new List<RIA>();
            foreach (Peptide p in this.peptides)
            {
                var datapoints = mergedRIAvalues.AsParallel().Where(x => x.PeptideSeq == p.PeptideSeq && x.Charge == p.Charge).ToList();

                //select proper io
                double I0 = (double)p.M0 / 100;
                var zeroTimePoint = datapoints.Where(x => x.Time == 0).ToList();
                if (zeroTimePoint.Any())
                {
                    if (Math.Abs((double)zeroTimePoint.FirstOrDefault().I0 - I0) < 0.1) { I0 = (double)zeroTimePoint.FirstOrDefault().I0; }
                }

                //

                var BWE = filecontents[filecontents.Count - 1].BWE;
                var IO_asymptote = I0 * (1 - (BWE / (1 - Helper.Constants.ph)) * p.Exchangeable_Hydrogens);

                // normalize each time point
                foreach (var d in datapoints)
                {
                    var BWE_t = filecontents.Where(x => x.time == d.Time).FirstOrDefault().BWE;
                    var IO_t_asymptote = I0 * (1 - (BWE_t / (1 - Helper.Constants.ph)) * p.Exchangeable_Hydrogens);

                    // compute the new value 
                    var I0_t = IO_asymptote + (d.RIA_value - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote);

                    //update the value
                    d.RIA_value = I0_t;
                    normalizedRIAvalues.Add(d);
                }

            }

            this.mergedRIAvalues = normalizedRIAvalues;



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
                    if (r.RIA_value != null & r.PeptideSeq == p.PeptideSeq & r.Charge == p.Charge) temp_RIAvalues.Add(r);
                }


                foreach (int t in this.experiment_time)
                {
                    //var temp_RIAvalues_pertime = temp_RIAvalues.Where(x => x.time == t).ToList();
                    List<RIA> temp_RIAvalues_pertime = temp_RIAvalues.Where(x => x.Time == t).ToList();

                    RIA ria = new RIA();
                    ria.ExperimentNames = new List<string>();

                    ria.PeptideSeq = p.PeptideSeq;
                    ria.Charge = p.Charge;
                    ria.Time = t;

                    //var sum_io = temp_RIAvalues_pertime.Sum(x => x.I0);
                    //var sum_ioria = temp_RIAvalues_pertime.Sum(x => x.I0 * x.RIA_value);
                    double sum_io = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_io = (double)(sum_io + temp_RIAvalues_pertime[i].I0);
                    double sum_ioria = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_ioria = (double)(
                            sum_ioria + (temp_RIAvalues_pertime[i].I0 * temp_RIAvalues_pertime[i].RIA_value));
                    var new_ria = sum_ioria / sum_io;

                    ria.RIA_value = new_ria;
                    ria.ExperimentNames = temp_RIAvalues_pertime.Select(x => x.ExperimentName).ToList();
                    mergedRIAvalues.Add(ria);

                }

                // add unique count of unique data points
                var peptides = mergedRIAvalues.Where(x => x.PeptideSeq == p.PeptideSeq && x.Charge == p.Charge && x.RIA_value >= 0).Distinct().ToList();
                p.NDP = peptides.Count();
            }


        }
        public void computeTheoreticalCurvePoints()
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
                    if (peptides[i].Rateconst == null) continue;

                    List<double> mytimelist = new List<double>();
                    foreach (var x in experiment_time) mytimelist.Add(x);

                    var temp_maxval = experiment_time.Max();
                    var step = temp_maxval / 200.0;
                    for (int j = 0; j * step < temp_maxval; j++)
                    { mytimelist.Add(j * step); }


                    foreach (double t in mytimelist)
                    {
                        //foreach (int t in this.Experiment_time)
                        //{
                        try
                        {
                            io = (double)(peptides[i].M0 / 100);
                            neh = (double)(peptides[i].Exchangeable_Hydrogens);
                            k = (double)(peptides[i].Rateconst);

                            var val1 = io * Math.Pow(1 - (pw / (1 - ph)), neh);
                            var val2 = io * Math.Pow(Math.E, -1 * k * t) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));


                            var val = val1 + val2;

                            TheoreticalI0Value theoreticalI0Value = new TheoreticalI0Value(peptides[i].PeptideSeq, t, val, (double)peptides[i].Charge);
                            theoreticalI0Values.Add(theoreticalI0Value);
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

        public void computeAverageA0()
        {
            foreach (Peptide peptide in peptides)
            {
                var experimentRecordsPerPeptide = this.experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
                            & p.Charge == peptide.Charge & p.IonScore != 0 & p.I0 != null & p.I0 > 0).Select(x => x.I0).ToList();
                if (experimentRecordsPerPeptide.Count() == 0) peptide.Abundance = 0;
                else
                {
                    var average_value = experimentRecordsPerPeptide.Average().Value;
                    peptide.Abundance = average_value;
                }
            }
        }

        public void computeTheoreticalCurvePointsBasedOnExperimentalI0()
        {
            try
            {
                double ph = 1.5574E-4;
                double pw = filecontents[filecontents.Count - 1].BWE;
                double io = 0;
                double neh = 0;
                double k = 0;

                var experimentalvalue_at_t0 = mergedRIAvalues.Where(x => x.Time == 0).ToList();



                for (int i = 0; i < peptides.Count(); i++)
                {
                    if (peptides[i].Rateconst == null) continue;

                    var r = peptides[i];
                    var experimentalvalue = experimentalvalue_at_t0.Where(x => x.Charge == r.Charge).ToList();
                    experimentalvalue = experimentalvalue.Where(x => x.PeptideSeq == r.PeptideSeq).ToList();
                    var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();

                    if (temp_experimentalvalue.Count != 1) continue;

                    List<double> mytimelist = new List<double>();
                    foreach (var x in experiment_time) mytimelist.Add(x);

                    var temp_maxval = experiment_time.Max();
                    var step = temp_maxval / 200.0;
                    for (int j = 0; j * step < temp_maxval; j++)
                    { mytimelist.Add(j * step); }


                    foreach (double t in mytimelist)
                    {
                        try
                        {
                            io = (double)(temp_experimentalvalue.FirstOrDefault().RIA_value);
                            neh = (double)(peptides[i].Exchangeable_Hydrogens);
                            k = (double)(peptides[i].Rateconst);

                            var val1 = io * Math.Pow(1 - (pw / (1 - ph)), neh);
                            var val2 = io * Math.Pow(Math.E, -1 * k * t) * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));


                            var val = val1 + val2;

                            TheoreticalI0Value theoreticalI0Value_withExperimentalIO = new TheoreticalI0Value(peptides[i].PeptideSeq, t, val, (double)peptides[i].Charge);
                            theoreticalI0Values_withExperimentalIO.Add(theoreticalI0Value_withExperimentalIO);
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
            temp_theoreticalI0Values = new List<TheoreticalI0Value>();

            //foreach (RateConstant r in rateConstants)
            foreach (Peptide r in this.peptides)
            {
                try
                {
                    var experimentalvalue = mergedRIAvalues.Where(x => x.Charge == r.Charge).ToList();
                    experimentalvalue = experimentalvalue.Where(x => x.PeptideSeq == r.PeptideSeq).ToList();
                    var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();
                    var meanval_ria = temp_experimentalvalue.Average(x => x.RIA_value);

                    var temp_computedRIAValue = theoreticalI0Values.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();
                    var temp_computedRIAValue_withexperimentalIO = theoreticalI0Values_withExperimentalIO.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();


                    if (temp_computedRIAValue_withexperimentalIO.Count == 0)
                    {

                        double ss = 0;
                        double rss = 0;
                        double RSquare = double.NaN;
                        double diff = 0; 

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss = rss + Math.Pow((double)(p.RIA_value - computedRIAValue), 2);
                                diff = computedRIAValue > 0 ? diff + Math.Abs((double)p.RIA_value - computedRIAValue) : diff;
                            }
                        }

                        if (r.Rateconst > 0.0006) RSquare = 1 - (rss / ss);
                        else RSquare = 1 - (diff);

                        if (RSquare == double.PositiveInfinity || RSquare == double.NegativeInfinity)
                            RSquare = double.NaN;




                        r.RSquare = RSquare;
                        r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());
                        //temp_expectedI0Values.AddRange(temp_computedRIAValue);
                        foreach (var x in temp_computedRIAValue) temp_theoreticalI0Values.Add(x);
                    }

                    else
                    {
                        double ss = 0;
                        double rss_mo = 0;
                        double rss_io = 0;
                        double RSquare = double.NaN;
                        double diff_mo = 0;
                        double diff_io = 0;

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue_mo = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                var computedRIAValue_io = temp_computedRIAValue_withexperimentalIO.Where(x => x.time == p.Time).First().value;

                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss_mo = rss_mo + Math.Pow((double)(p.RIA_value - computedRIAValue_mo), 2);
                                rss_io = rss_io + Math.Pow((double)(p.RIA_value - computedRIAValue_io), 2);

                                diff_mo = computedRIAValue_mo > 0 ? diff_mo + Math.Abs((double)p.RIA_value - computedRIAValue_mo) : diff_mo;
                                diff_io = computedRIAValue_io > 0 ? diff_io + Math.Abs((double)p.RIA_value - computedRIAValue_io) : diff_io;

                            }
                        }

                        var rss = rss_mo < rss_io ? rss_mo : rss_io;

                        if (r.Rateconst > 0.0006)
                        {
                            RSquare = 1 - (rss / ss);
                            if (RSquare == double.PositiveInfinity || RSquare == double.NegativeInfinity)
                                RSquare = double.NaN;
                            r.RSquare = RSquare;
                            r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());
                        }
                        else
                        {
                            var dif = rss_mo < rss_io ? diff_mo : diff_io;
                            RSquare = 1 - dif;
                        }

                        r.RSquare = RSquare;
                        r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());

                        if (rss_mo < rss_io)
                            foreach (var x in temp_computedRIAValue) temp_theoreticalI0Values.Add(x);
                        else
                            foreach (var x in temp_computedRIAValue_withexperimentalIO) temp_theoreticalI0Values.Add(x);


                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error => computeRSquare(), " + e.Message);
                }
            }

            theoreticalI0Values = new List<TheoreticalI0Value>();
            theoreticalI0Values = temp_theoreticalI0Values;

        }
        ////public ProtienchartDataValues computeValuesForEnhancedPerProtienPlot()
        ////{
        ////    List<double> xval = new List<double>();
        ////    List<double> yval = new List<double>();

        ////    double ph = 1.5574E-4;
        ////    double pw = filecontents[filecontents.Count - 1].BWE;
        ////    double io = 0;
        ////    double neh = 0;
        ////    double k = 0;

        ////    for (int i = 0; i < peptides.Count(); i++)
        ////    {
        ////        foreach (int t in this.experiment_time)
        ////        {
        ////            try
        ////            {
        ////                io = (double)(peptides[i].M0 / 100);
        ////                neh = (double)(peptides[i].Exchangeable_Hydrogens);
        ////                k = (double)(peptides[i].Rateconst);

        ////                var ria_val2 = mergedRIAvalues.Where(x => x.PeptideSeq.Trim() == peptides[i].PeptideSeq.Trim() & x.Time == t & x.Charge == peptides[i].Charge).ToList();
        ////                if (ria_val2.Count > 0)
        ////                {
        ////                    var ria_val = ria_val2.First().RIA_value;

        ////                    var dn = io * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
        ////                    var nu = io - ria_val;
        ////                    var fv = nu / dn;

        ////                    xval.Add(t);
        ////                    yval.Add((double)fv);
        ////                }

        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                continue;
        ////            }

        ////        }
        ////    }
        ////    //ProtienchartDataValues pd = new ProtienchartDataValues(xval, yval);
        ////    return new ProtienchartDataValues(xval, yval);

        ////}

        public ProtienchartDataValues computeValuesForEnhancedPerProtienPlot2()
        {
            List<double> xval = new List<double>();
            List<double> yval = new List<double>();
            List<string> PeptideSeq = new List<string>();

            try
            {
                double ph = 1.5574E-4;
                double pw = filecontents[filecontents.Count - 1].BWE;
                double io = 0;
                double neh = 0;
                double k = 0;
                var temp_pep = this.peptides; // this.peptides.Where(x => x.RSquare > 0.25);
                foreach (RIA r in mergedRIAvalues)
                {

                    Peptide p = temp_pep.Where(x => x.PeptideSeq == r.PeptideSeq & x.Charge == r.Charge).FirstOrDefault();
                    if (p != null)
                    {
                        io = (double)(p.M0 / 100);
                        neh = (double)(p.Exchangeable_Hydrogens);

                        //if (p.Rateconst == null) continue;
                        //else k = (double)(p.Rateconst);



                        var dn = io * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
                        var nu = io - r.RIA_value;
                        var fv = nu / dn;

                        xval.Add(r.Time);
                        yval.Add((double)fv);
                        PeptideSeq.Add(p.PeptideSeq);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error => computeValuesForEnhancedPerProtienPlot2(), " + e.Message);
            }

            //ProtienchartDataValues pd = new ProtienchartDataValues(xval, yval);
            return new ProtienchartDataValues(xval, yval, PeptideSeq);

        }
        public struct ProtienchartDataValues
        {
            public List<double> x;
            public List<double> y;
            public List<string> PeptideSeq;

            public ProtienchartDataValues(List<double> x, List<double> y, List<string> PeptideSeq)
            {
                this.x = x;
                this.y = y;
                this.PeptideSeq = PeptideSeq;
            }
        }
        public struct TheoreticalI0Value
        {
            public string peptideseq;
            public double time;
            public double value;
            public double? charge;

            public TheoreticalI0Value(string peptideSeq, double t, double val, double charge)
            {
                peptideseq = peptideSeq;
                time = t;
                value = val;
                this.charge = charge;
            }

        }
    }
}
