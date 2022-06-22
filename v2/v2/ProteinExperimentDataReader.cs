using IsotopomerDynamics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        string quant_state_file_path = @"";

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
        public double? MedianRateConst;
        double? MedianRateConst_RMSSCutOff;
        public double? StandDev_NumberPeptides_StandDev;
        double? StandDev_NumberPeptides_NumberPeptides;
        public double? TotalIonCurrent;

        // computed values
        public List<RIA> RIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvalues = new List<RIA>();
        public List<RIA> mergedRIAvaluesWithZeroIonScore = new List<RIA>();
        public List<TheoreticalI0Value> theoreticalI0Values = new List<TheoreticalI0Value>();
        public List<TheoreticalI0Value> theoreticalI0Values_withExperimentalIO = new List<TheoreticalI0Value>();
        public List<TheoreticalI0Value> temp_theoreticalI0Values = new List<TheoreticalI0Value>();
        public string labelingDuration = "Labeling Duration";

        public ProteinExperimentDataReader(string files_txt_path, string quant_csv_path, string RateConst_csv_path, String quant_state_file_path)
        {
            this.files_txt_path = files_txt_path;
            this.quant_csv_path = quant_csv_path;
            this.RateConst_csv_path = RateConst_csv_path;
            this.quant_state_file_path = quant_state_file_path;
        }
        public void loadAllExperimentData()
        {

            //Files.txt reader
            ReadFilesInfo_txt filesinfo = new ReadFilesInfo_txt(files_txt_path);
            filesinfo.readFile();

            this.experiment_time = filesinfo.experimentTimes;
            this.experimentIDs = filesinfo.experimentIDs;
            this.filecontents = filesinfo.filecontents;

            //qunat.state reader

            ReadQuantState quantstatereader = new ReadQuantState(quant_state_file_path);
            this.labelingDuration = quantstatereader.getLabelingDuration();

            //Protein.Quant.csv reader
            ReadExperiments experiInfoReader = new ReadExperiments(quant_csv_path, filesinfo.experimentTimes_all);
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

        public void Comparison_of_Theoretical_And_Experimental_Spectrum(ProteinExperimentDataReader proteinExperimentDataReader, string protein)
        {
            string filecontent = "Protein,Peptide,charge,time,RSS,theta\n";
            foreach (var er in proteinExperimentDataReader.experimentRecords)
            {
                try
                {
                    var peptide = proteinExperimentDataReader.peptides.Where(x => x.PeptideSeq == er.PeptideSeq && x.Charge == er.Charge).FirstOrDefault();
                    if (peptide != null)
                    {
                        double[] theoretical_vals = { (double)peptide.M0/100, (double)peptide.M1/100, (double)peptide.M2/100, (double)peptide.M3/100,
                        (double)peptide.M4/100, (double)peptide.M5/100 };

                        double[] experimental_vals = {(double)(er.I0/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)),
                    (double)(er.I1/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)),
                    (double)(er.I2/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)),
                    (double)(er.I3/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)),
                    (double)(er.I4/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)),
                    (double)(er.I5/(er.I0+er.I1+er.I2+er.I3+er.I4+er.I5)) };

                        #region compute theoretical spectrum values for time t and px_t

                        float[] fNatIsotopes = new float[10];
                        float[] fLabIsotopes = new float[10];
                        fNatIsotopes[0] = (float)((double)peptide.M0 / 100);
                        fNatIsotopes[1] = (float)((double)peptide.M1 / 100);
                        fNatIsotopes[2] = (float)((double)peptide.M2 / 100);
                        fNatIsotopes[3] = (float)((double)peptide.M3 / 100);
                        fNatIsotopes[4] = (float)((double)peptide.M4 / 100);
                        fNatIsotopes[5] = (float)((double)peptide.M5 / 100);
                        MassIsotopomers MIDyn = new MassIsotopomers();
                        var bwe = (float)filecontents.Where(x => x.experimentID == er.ExperimentName).Select(x => x.BWE).FirstOrDefault();
                        int Nall_Hyd = MIDyn.NumberOfHydrogens(peptide.PeptideSeq);
                        MIDyn.CalculateMIDynamics(fNatIsotopes, fLabIsotopes, bwe, (float)peptide.Exchangeable_Hydrogens, Nall_Hyd);

                        double[] theoretical_vals_at_t = { fLabIsotopes[0], fLabIsotopes[1], fLabIsotopes[2], fLabIsotopes[3],
                        fLabIsotopes[4], fLabIsotopes[5] };

                        double min_rss = 100;
                        double min_theta = double.NaN;

                        for (double i = 0; i < 1; i = i + 0.01)
                        {
                            var temp_t = theoretical_vals_at_t.Select(x => x * i).ToList();
                            var temp = theoretical_vals.Select(x => x * (1 - i)).ToList();
                            double error = 0;
                            for (int j = 0; j < 3; j++)
                            {
                                error += Math.Pow(temp_t[j] + temp[j] - experimental_vals[j], 2);
                            }
                            if (error < min_rss)
                            {
                                min_rss = error/3;
                                min_theta = i;
                            }


                        }
                        Console.WriteLine(er.PeptideSeq + " c" + er.Charge + " t = " + er.ExperimentTime.ToString() + " RSS = " + min_rss + " theta" + min_theta);

                        //filecontent = "Protein,Peptide,charge,time,RSS,theta";
                        filecontent += protein + "," + er.PeptideSeq + "," + er.Charge + "," + er.ExperimentTime + "," + min_rss + "," + min_theta + "\n";
                        #endregion
                    }

                }
                catch (Exception ex) { continue; }
            }
            using (StreamWriter writer = new StreamWriter("_compare" + protein + ".csv"))
            {
                writer.WriteLine(filecontent);
            }
        }

        public void computeDeuteriumenrichmentInPeptide()
        {
            // this function computes pX(t)
            // which is the deuterium enrichment in a peptide from the heavy water at the
            // labeling duration time t

            double ph = Helper.Constants.ph;

            foreach (Peptide peptide in peptides)
            {
                var experimentRecordsPerPeptide = this.experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
                            & p.Charge == peptide.Charge).ToList();
                float NEH = (float)peptide.Exchangeable_Hydrogens;


                if (experimentRecordsPerPeptide.Count > 0)
                {
                    float[] fNatIsotopes = new float[10];
                    float[] fLabIsotopes = new float[10];

                    fNatIsotopes[0] = (float)((double)peptide.M0 / 100);
                    fNatIsotopes[1] = (float)((double)peptide.M1 / 100);
                    fNatIsotopes[2] = (float)((double)peptide.M2 / 100);
                    fNatIsotopes[3] = (float)((double)peptide.M3 / 100);
                    fNatIsotopes[4] = (float)((double)peptide.M4 / 100);
                    fNatIsotopes[5] = (float)((double)peptide.M5 / 100);
                    MassIsotopomers MIDyn = new MassIsotopomers();
                    int Nall_Hyd = MIDyn.NumberOfHydrogens(peptide.PeptideSeq);

                    List<float> theo_a2 = new List<float>();
                    List<float> theo_a1 = new List<float>();
                    List<float> theo_a0 = new List<float>();
                    List<double> pxts = new List<double>();

                    // compute theoretical values
                    for (double bwe = 0.0001; bwe < 0.05; bwe = bwe + 0.0005)
                    {
                        MIDyn.CalculateMIDynamics(fNatIsotopes, fLabIsotopes, (float)bwe, (float)NEH, Nall_Hyd);

                        var new_a2_t = fLabIsotopes[2];
                        var new_a1_t = fLabIsotopes[1];
                        var new_a0_t = fLabIsotopes[0];

                        theo_a2.Add(new_a2_t);
                        theo_a1.Add(new_a1_t);
                        theo_a0.Add(new_a0_t);
                        pxts.Add(bwe);
                    }


                    foreach (ExperimentRecord er in experimentRecordsPerPeptide)
                    {
                        double experiment_BWE = filecontents.Where(x => x.experimentID == er.ExperimentName).Select(x => x.BWE).FirstOrDefault();

                        var experimentsAt_t = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == er.ExperimentTime && t.I0 != null && t.I0 > 0 &&
                        (er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5) > 0
                        ).ToList();
                        if (experimentsAt_t.Count == 0) continue;

                        double sum_A2_t = experimentsAt_t.Sum(x => x.I2).Value;
                        double sum_A2_r_t = experimentsAt_t.Sum(x => (x.I2 * (x.I2 / (x.I0 + x.I1 + x.I2 + x.I3 + x.I4 + x.I5)))).Value;
                        double exp_A2 = sum_A2_r_t / sum_A2_t;

                        double sum_A1_t = experimentsAt_t.Sum(x => x.I1).Value;
                        double sum_A1_r_t = experimentsAt_t.Sum(x => (x.I1 * (x.I1 / (x.I0 + x.I1 + x.I2 + x.I3 + x.I4 + x.I5)))).Value;
                        double exp_A1 = sum_A1_r_t / sum_A1_t;

                        double sum_A0_t = experimentsAt_t.Sum(x => x.I0).Value;
                        double sum_A0_r_t = experimentsAt_t.Sum(x => (x.I0 * (x.I0 / (x.I0 + x.I1 + x.I2 + x.I3 + x.I4 + x.I5)))).Value;
                        double exp_A0 = sum_A0_r_t / sum_A0_t;

                        // compute al/a0
                        if (exp_A0 > 0)
                        {
                            var exp_a1_a0 = exp_A1 / exp_A0;
                            var diff = theo_a1.Select((value, index) => Math.Abs((value / theo_a0[index]) - exp_a1_a0)).ToList();
                            double min_value = diff.Min();
                            int index_min_value = diff.IndexOf(min_value);
                            var selected_pxt = pxts[index_min_value];
                            er.I0_t_fromA1A0 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (selected_pxt / (1 - ph))), (double)NEH));
                            //er.I0_t_fromA1A0_pxt = experiment_BWE;
                        }

                        // compute a2/a0
                        if (exp_A0 > 0)
                        {
                            var exp_a2_a0 = exp_A2 / exp_A0;
                            var diff = theo_a2.Select((value, index) => Math.Abs((value / theo_a0[index]) - exp_a2_a0)).ToList();
                            double min_value = diff.Min();
                            int index_min_value = diff.IndexOf(min_value);
                            var selected_pxt = pxts[index_min_value];
                            er.I0_t_fromA2A0 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (selected_pxt / (1 - ph))), (double)NEH));
                            //er.I0_t_fromA2A0_pxt = experiment_BWE;
                        }


                        // compute a2/a1
                        if (exp_A0 > 0)
                        {
                            var exp_a2_a1 = exp_A2 / exp_A1;
                            var diff = theo_a2.Select((value, index) => Math.Abs((value / theo_a1[index]) - exp_a2_a1)).ToList();
                            double min_value = diff.Min();
                            int index_min_value = diff.IndexOf(min_value);
                            var selected_pxt = pxts[index_min_value];
                            er.I0_t_fromA2A1 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (selected_pxt / (1 - ph))), (double)NEH));
                            //er.I0_t_fromA2A1_pxt = experiment_BWE;
                        }


                    }

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
                    ria.ExperimentName = er.ExperimentName;
                    ria.Time = er.ExperimentTime;
                    ria.PeptideSeq = er.PeptideSeq;
                    ria.RIA_value = er.I0 / sum_val;
                    ria.I0 = er.I0;
                    ria.Charge = er.Charge;
                    ria.IonScore = er.IonScore;

                    ria.I0_t_fromA1A0 = er.I0_t_fromA1A0;
                    ria.I0_t_fromA2A0 = er.I0_t_fromA2A0;
                    ria.I0_t_fromA2A1 = er.I0_t_fromA2A1;


                    //ria.I0_t_fromA1A0_pxt = er.I0_t_fromA1A0_pxt;
                    //ria.I0_t_fromA2A0_pxt = er.I0_t_fromA2A0_pxt;
                    //ria.I0_t_fromA2A1_pxt = er.I0_t_fromA2A1_pxt;

                    //get the experiment time from files.txt values
                    var temp = filecontents.Where(x => x.experimentID == er.ExperimentName).Select(t => t.time).ToArray();
                    if (temp.Length > 0) ria.Time = temp[0];

                    //var peptide_MO = (double)this.peptides.Where((x) => x.PeptideSeq == er.PeptideSeq && x.Charge == er.Charge).FirstOrDefault().M0 / 100;
                    //if (ria.Time != 0 && ria.RIA_value > peptide_MO) continue;
                    //else if (ria.Time == 0 && Math.Abs(peptide_MO - (double)ria.RIA_value) > 0.1) continue;
                    //else RIAvalues.Add(ria);

                    RIAvalues.Add(ria);
                }
            }

        }

        public void normalizeRIAValuesForAllPeptides()
        {
            List<RIA> normalizedRIAvalues = new List<RIA>();

            foreach (var p in peptides)
            {
                var datapoints = RIAvalues.AsParallel().Where(x => x.PeptideSeq == p.PeptideSeq && x.Charge == p.Charge).ToList();

                var I0 = 0.0;
                var zeroIimePoints = datapoints.Where(x => x.Time == 0 && x.RIA_value != null && x.RIA_value > 0).ToList();
                if (zeroIimePoints.Count > 0)
                    I0 = (double)zeroIimePoints.Select(x => x.RIA_value).Average();

                var temp_mo = (double)p.M0 / 100;
                if (Math.Abs(I0 - temp_mo) / temp_mo > 0.1) { I0 = temp_mo; }

                var IO_asymptote = I0 * Math.Pow(1 - (filecontents[filecontents.Count - 1].BWE / (1 - Helper.Constants.ph)), (double)p.Exchangeable_Hydrogens);

                foreach (var datapoint in datapoints)
                {
                    var BWE_t = filecontents.Where(x => x.experimentID == datapoint.ExperimentName).Select(x => x.BWE).FirstOrDefault();

                    if (datapoint.Time != 0 && BWE_t == 0) continue;


                    var IO_t_asymptote = I0 * Math.Pow(1 - (BWE_t / (1 - Helper.Constants.ph)), (double)p.Exchangeable_Hydrogens);

                    var I0_t = (datapoint.Time != 0) ? IO_asymptote + (datapoint.RIA_value - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : datapoint.RIA_value;

                    datapoint.RIA_value = I0_t;

                    datapoint.I0_t_fromA1A0 = (datapoint.Time != 0) ? IO_asymptote + (datapoint.I0_t_fromA1A0 - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : datapoint.I0_t_fromA1A0;
                    datapoint.I0_t_fromA2A0 = (datapoint.Time != 0) ? IO_asymptote + (datapoint.I0_t_fromA2A0 - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : datapoint.I0_t_fromA2A0;
                    datapoint.I0_t_fromA2A1 = (datapoint.Time != 0) ? IO_asymptote + (datapoint.I0_t_fromA2A1 - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : datapoint.I0_t_fromA2A1;

                    normalizedRIAvalues.Add(datapoint);
                    //fTempI0[i] = fI0_Asymptote_final + (I0_Natural - fI0_Asymptote_final) * (fTempI0[i] - fI0_Asymptote_Temp) / (I0_Natural - fI0_Asymptote_Temp);
                }
            }

            this.RIAvalues = normalizedRIAvalues;

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
                var zeroTimePoint = datapoints.Where(x => x.Time == 0 && !double.IsNaN((double)x.RIA_value)).ToList();
                if (zeroTimePoint.Any())
                {
                    if (Math.Abs((double)zeroTimePoint.FirstOrDefault().RIA_value - I0) < 0.1) { I0 = (double)zeroTimePoint.FirstOrDefault().RIA_value; }
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
                    var I0_t = (d.Time != 0) ? IO_asymptote + (d.RIA_value - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : d.RIA_value;

                    //update the value
                    d.RIA_value = I0_t;
                    normalizedRIAvalues.Add(d);
                }

            }

            this.mergedRIAvalues = normalizedRIAvalues;
        }
        public void mergeMultipleRIAPerDay2()
        {
            foreach (Peptide p in this.peptides)
            {
                List<RIA> temp_RIAvalues = new List<RIA>();
                foreach (RIA r in RIAvalues)
                {
                    if (r.RIA_value != null & r.PeptideSeq == p.PeptideSeq & r.Charge == p.Charge) temp_RIAvalues.Add(r);
                }


                foreach (int t in this.experiment_time)
                {
                    List<RIA> temp_RIAvalues_pertime = temp_RIAvalues.Where(x => x.Time == t).ToList();

                    // check the number of experiments with zero ion score
                    int countOfNonZeroIonScore = temp_RIAvalues_pertime.Where(x => x.IonScore > 0).Count();
                    if (countOfNonZeroIonScore > 0) temp_RIAvalues_pertime = temp_RIAvalues_pertime.Where(x => x.IonScore > 0).ToList();


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

                    #region compute the modified I0_t

                    //ria.I0_t_fromA1 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().I0_t_fromA1 : null;
                    //ria.I0_t_fromA1A2 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().I0_t_fromA1A2 : null;
                    //ria.pX_greaterthanThreshold = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().pX_greaterthanThreshold : null;

                    ria.I0_t_fromA1A0 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA1A0).Average() : null;
                    ria.I0_t_fromA2A0 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA2A0).Average() : null;
                    ria.I0_t_fromA2A1 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA2A1).Average() : null;

                    //ria.I0_t_fromA1A0_pxt = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA1A0_pxt).Average() : null;
                    //ria.I0_t_fromA2A0_pxt = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA2A0_pxt).Average() : null;
                    //ria.I0_t_fromA2A1_pxt = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.Select(x => x.I0_t_fromA2A1_pxt).Average() : null;
                    #endregion

                    ria.RIA_value = new_ria;
                    ria.ExperimentNames = temp_RIAvalues_pertime.Select(x => x.ExperimentName).ToList();
                    mergedRIAvalues.Add(ria);

                    if (countOfNonZeroIonScore == 0)
                    {
                        mergedRIAvaluesWithZeroIonScore.Add(ria);
                    }

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
                    //var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();
                    var temp_experimentalvalue = experimentalvalue.ToList();
                    var meanval_ria = temp_experimentalvalue.Where(x => !double.IsNaN((double)x.RIA_value)).Average(x => x.RIA_value);

                    var temp_computedRIAValue = theoreticalI0Values.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();
                    var temp_computedRIAValue_withexperimentalIO = theoreticalI0Values_withExperimentalIO.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();


                    if (temp_computedRIAValue_withexperimentalIO.Count == 0)
                    {

                        double ss = 0;
                        double rss = 0;
                        double RSquare = double.NaN;
                        double diff = 0;
                        double dn = 0;
                        double IO_assymptot = (double)(r.M0 / 100 * Math.Pow((1 - (filecontents[filecontents.Count - 1].BWE / (1 - Helper.Constants.ph))), (double)r.Exchangeable_Hydrogens));

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (!double.IsNaN((double)p.RIA_value))
                            {
                                var computedRIAValue = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss = rss + Math.Pow((double)(p.RIA_value - computedRIAValue), 2);
                                diff = computedRIAValue > 0 ? diff + Math.Abs((double)p.RIA_value - computedRIAValue) : diff;
                                //dn = dn + Math.Pow(p.Time, 2) * Math.Pow((double)(p.RIA_value - IO_assymptot), 2);
                                dn = dn + Math.Pow(p.Time, 2) * Math.Pow((double)(computedRIAValue - IO_assymptot), 2);
                            }
                        }

                        //if (r.Rateconst > 0.0006) RSquare = 1 - (rss / ss);
                        //else RSquare = 1 - (diff);

                        RSquare = 1 - (rss / ss);

                        if (RSquare == double.PositiveInfinity || RSquare == double.NegativeInfinity)
                            RSquare = double.NaN;


                        var var_k = dn == 0 ? double.NaN : (rss / temp_experimentalvalue.Count()) / (dn);


                        r.RSquare = RSquare;
                        r.std_k = Math.Sqrt(var_k);
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

                        double dn_mo = 0;
                        double dn_io = 0;
                        var io_experimental = temp_experimentalvalue.Where(x => x.Time == 0).Select(x => x.RIA_value).FirstOrDefault().Value;
                        double IO_assymptot_mo = (double)(r.M0 / 100 * Math.Pow((1 - (filecontents[filecontents.Count - 1].BWE / (1 - Constants.ph))), (double)r.Exchangeable_Hydrogens));
                        double IO_assymptot_io = (double)(io_experimental * Math.Pow((1 - (filecontents[filecontents.Count - 1].BWE / (1 - Constants.ph))), (double)r.Exchangeable_Hydrogens));




                        foreach (var p in temp_experimentalvalue)
                        {
                            if (!double.IsNaN((double)p.RIA_value))
                            {
                                var computedRIAValue_mo = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                var computedRIAValue_io = temp_computedRIAValue_withexperimentalIO.Where(x => x.time == p.Time).First().value;

                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss_mo = rss_mo + Math.Pow((double)(p.RIA_value - computedRIAValue_mo), 2);
                                rss_io = rss_io + Math.Pow((double)(p.RIA_value - computedRIAValue_io), 2);

                                diff_mo = computedRIAValue_mo > 0 ? diff_mo + Math.Abs((double)p.RIA_value - computedRIAValue_mo) : diff_mo;
                                diff_io = computedRIAValue_io > 0 ? diff_io + Math.Abs((double)p.RIA_value - computedRIAValue_io) : diff_io;

                                //dn_io = dn_io + Math.Pow(p.Time, 2) * Math.Pow((double)(p.RIA_value - IO_assymptot_io), 2);
                                //dn_mo = dn_mo + Math.Pow(p.Time, 2) * Math.Pow((double)(p.RIA_value - IO_assymptot_mo), 2);

                                dn_io = dn_io + Math.Pow(p.Time, 2) * Math.Pow((double)(computedRIAValue_io - IO_assymptot_io), 2);
                                dn_mo = dn_mo + Math.Pow(p.Time, 2) * Math.Pow((double)(computedRIAValue_mo - IO_assymptot_mo), 2);

                            }
                        }


                        //var rss = rss_mo < rss_io ? rss_mo : rss_io;
                        double rss = double.NaN;
                        double var_k = double.NaN;
                        if (rss_mo < rss_io)
                        {
                            rss = rss_mo;
                            var_k = dn_mo == 0 ? double.NaN : (rss / temp_experimentalvalue.Count()) / (dn_mo);
                        }
                        else
                        {
                            rss = rss_io;
                            var_k = dn_io == 0 ? double.NaN : (rss / temp_experimentalvalue.Count()) / (dn_io);
                        }



                        RSquare = 1 - (rss / ss);

                        //if (r.Rateconst > 0.0006) 
                        //    RSquare = 1 - (rss / ss); 
                        //else
                        //{
                        //    var dif = rss_mo < rss_io ? diff_mo : diff_io;
                        //    RSquare = 1 - dif;
                        //}

                        if (RSquare == double.PositiveInfinity || RSquare == double.NegativeInfinity)
                            RSquare = double.NaN;

                        r.RSquare = RSquare;
                        r.std_k = Math.Sqrt(var_k);
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
