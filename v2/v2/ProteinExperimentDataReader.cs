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
            // update experiment time for all records
            foreach (var fc in filecontents)
            {
                experimentRecords.Where(x => x.ExperimentName == fc.experimentID).ToList().ForEach(x => x.ExperimentTime = fc.time);
            }


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

        }
        public void computeAverageA0()
        {
            foreach (Peptide peptide in peptides)
            {
                var experimentRecordsPerPeptide = this.experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
                            & p.Charge == peptide.Charge & p.IonScore != 0 & p.I0 != null & p.I0 > 0).Select(x => x.I0).ToList();
                if (experimentRecordsPerPeptide.Count() == 0) peptide.A0_average = 0;
                else
                {
                    var average_value = experimentRecordsPerPeptide.Average().Value;
                    peptide.A0_average = average_value;
                }
            }
        }
        public void computeDeuteriumenrichmentInPeptide()
        {
            // this function computes pX(t)
            // which is the deuterium enrichment in a peptide from the heavy water at the
            // labeling duration time t

            double ph = 1.5574E-4;
            foreach (Peptide peptide in peptides)
            {
                var experimentRecordsPerPeptide = this.experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
                            & p.Charge == peptide.Charge).ToList();

                Console.WriteLine("//////////////////////////////////////********************* " + peptide.PeptideSeq);

                if (experimentRecordsPerPeptide.Count > 0)
                {
                    var NEH = (double)peptide.Exchangeable_Hydrogens;

                    //experiments at t=0
                    var experimentsAt_t_0 = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == 0 & t.I0 != null & t.I0 > 0).ToList();
                    double sum_io_t_0 = experimentsAt_t_0.Sum(x => x.I0).Value;

                    double sum_a1_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I1 / x.I0))).Value;
                    double al_a0_t_0 = sum_a1_ao_t_0 / sum_io_t_0;

                    double sum_a2_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I2 / x.I0))).Value;
                    double a2_a0_t_0 = sum_a2_ao_t_0 / sum_io_t_0;

                    foreach (ExperimentRecord er in experimentRecordsPerPeptide)
                    {
                        if (er.I0_t != null) continue;
                        var experimentsAt_t = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == er.ExperimentTime & t.I0 != null & t.I0 > 0).ToList();
                        if (experimentsAt_t.Count == 0) continue;

                        #region A1(t)/A0(t)
                        double sum_io_t = experimentsAt_t.Sum(x => x.I0).Value;
                        double sum_a1_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I1 / x.I0))).Value;
                        double al_a0_t = sum_a1_ao_t / sum_io_t;

                        var k_t = (1 / NEH) * (al_a0_t - al_a0_t_0);
                        var px_t = (k_t * (1 - ph)) / (1 + k_t);

                        er.Deuteriumenrichment = px_t;

                        // compute modified I0(t)
                        double I0_t = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (px_t / (1 - ph))), (double)NEH));
                        er.I0_t = I0_t;

                        if (px_t > 0.05 | px_t < (-0.2))
                        {
                            Console.WriteLine("test");
                            er.pX_greaterthanThreshold = 0;
                        }
                        #endregion

                        #region A2(t)/A1(t) 

                        double sum_a2_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I2 / x.I0))).Value;
                        double a2_a0_t = sum_a2_ao_t / sum_io_t;

                        var del_a_1_0 = al_a0_t - al_a0_t_0;
                        var del_a_2_0 = a2_a0_t - a2_a0_t_0;

                        ////var a = del_a_2_0 - (al_a0_t_0 * ph * del_a_1_0) - (al_a0_t * (1 - ph) * del_a_1_0) + 0.5 * ((-Math.Pow(del_a_1_0, 2) * (2 * ph - 1)) + (del_a_1_0 * ((2 * ph - 1) / (1 - ph))));
                        //var a = del_a_2_0;
                        //a = a - (al_a0_t_0 * ph * del_a_1_0);
                        //a = a - (al_a0_t * (1 - ph) * del_a_1_0);
                        //a = a + 0.5 * (-1 * (Math.Pow(del_a_1_0, 2) * (2 * ph - 1)) + (del_a_2_0 * ((2 * ph - 1) / (1 - ph))));


                        ////var b = (-1 * del_a_2_0 * (1 - ph)) + (al_a0_t_0 * ph * del_a_1_0 * 2 * (1 - ph)) + ((1 - 2 * ph) * al_a0_t * (1 - ph) * del_a_1_0) +
                        ////    0.5 * ((Math.Pow(del_a_1_0, 2) * (1 - ph) * (2 * ph - 1)) + (Math.Pow(del_a_1_0, 2) * 2 * ph * (1 - ph)) - (del_a_1_0 * 2 * ph));
                        //var b = (-1 * del_a_2_0 * (1 - ph));
                        //b = b + (al_a0_t_0 * ph * del_a_1_0 * 2 * (1 - ph));
                        //b = b + ((1 - 2 * ph) * al_a0_t * (1 - ph) * del_a_1_0);
                        //b = b + 0.5 * ((Math.Pow(del_a_2_0, 2) * (1 - ph) * (2 * ph - 1)) + (Math.Pow(del_a_1_0, 2) * 2 * ph * (1 - ph)) - (del_a_1_0 * 2 * ph));

                        //=======================================
                        //=======================================
                        //=======================================
                        // a2/a0
                        double c = a2_a0_t_0 - a2_a0_t - al_a0_t_0 * ((ph * NEH) / (1 - ph)) + (Math.Pow((ph / (1 - ph)), 2)) * (NEH * (NEH + 1)) * 0.5;
                        double a = -0.5 * NEH * (NEH + 1);
                        double b = NEH * al_a0_t;


                        //=======================================
                        //=======================================
                        //=======================================
                        // a2/a0 + a1/ao
                        c = c + al_a0_t_0 - al_a0_t;
                        b = b + (NEH / (1 - ph));

                        double temp = Math.Sqrt((b * b) - 4 * a * c);
                        double y = (-b + temp) / (2 * a);
                        double new_px_t = (y * (1 + ph) - ph) / (1 + y);

                        //var new_px_t = -b / a;
                        double I0_t_new_a2 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (new_px_t / (1 - ph))), (double)NEH));
                        er.I0_t_new_a2 = I0_t_new_a2;


                        var temp2 = a2_a0_t;
                        var temp1 = al_a0_t;

                        Console.WriteLine("======================================== " + er.ExperimentTime.ToString());

                        var step = 0.001;
                        List<double> testvals = new List<double>();

                        List<double> temp2_t_list = new List<double>();
                        List<double> temp1_t_list = new List<double>();
                        List<double> neh_list = new List<double>();

                        for (int i = 0; i * step < 0.06; i++)
                        {
                            for (int k = 0; k < 20; k++)
                            {
                                double px_t_new = step * i;
                                double NEH_new = k; // ((1 - ph - px_t_new) / px_t_new) * del_a_1_0 * (1.0 - ph);

                                var temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                                    (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                                   (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                                   (NEH_new * (px_t_new + ph) * al_a0_t / (1 - ph - px_t_new));

                                var temp1_t = al_a0_t_0 + ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));
                                var way = ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));

                                var diff = Math.Abs(temp2 - temp2_t) + Math.Abs(temp1 - temp1_t); ;

                                System.Console.WriteLine(i+","+k+","+diff);
                                if (i != 0)
                                {
                                    testvals.Add(diff);
                                    temp2_t_list.Add(temp2_t);
                                    temp1_t_list.Add(temp1_t);
                                    neh_list.Add(NEH_new);
                                }
                            }
                        }
                        Console.WriteLine("===================" + (testvals.Min().ToString()) + "=====================");





                        #endregion



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
                    ria.PeptideSeq = er.PeptideSeq;
                    ria.RIA_value = er.I0 / sum_val;
                    ria.I0 = er.I0;
                    ria.Charge = er.Charge;
                    ria.I0_t = er.I0_t;
                    ria.I0_t_new_a2 = er.I0_t_new_a2;
                    ria.pX_greaterthanThreshold = er.pX_greaterthanThreshold;

                    //get the experiment time from files.txt values
                    var temp = filecontents.Where(x => x.experimentID == er.ExperimentName).Select(t => t.time).ToArray();
                    if (temp.Length > 0)
                    {
                        ria.Time = temp[0];
                        er.ExperimentTime = temp[0]; // add experiment time to Experiment records
                    }

                    RIAvalues.Add(ria);
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

                    #region original IO computaion
                    ////var sum_io = temp_RIAvalues_pertime.Sum(x => x.I0);
                    ////var sum_ioria = temp_RIAvalues_pertime.Sum(x => x.I0 * x.RIA_value);
                    double sum_io = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_io = (double)(sum_io + temp_RIAvalues_pertime[i].I0);
                    double sum_ioria = 0; for (int i = 0; i < temp_RIAvalues_pertime.Count(); i++) sum_ioria = (double)(
                            sum_ioria + (temp_RIAvalues_pertime[i].I0 * temp_RIAvalues_pertime[i].RIA_value));
                    var new_ria = sum_ioria / sum_io;
                    ria.RIA_value = new_ria;
                    #endregion

                    #region compute the modified I0_t

                    ria.I0_t = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().I0_t : null;
                    ria.I0_t_new_a2 = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().I0_t_new_a2 : null;
                    ria.pX_greaterthanThreshold = temp_RIAvalues_pertime.Count > 0 ? temp_RIAvalues_pertime.FirstOrDefault().pX_greaterthanThreshold : null;
                    #endregion

                    ria.ExperimentNames = temp_RIAvalues_pertime.Select(x => x.ExperimentName).ToList();
                    mergedRIAvalues.Add(ria);

                }
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

                            var val1 = io * Math.Pow(1 - (pw / (1 - pw)), neh);
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

                            var val1 = io * Math.Pow(1 - (pw / (1 - pw)), neh);
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

                    if (r.PeptideSeq == "VASGQALAAFcLTEPSSGSDVASIR" | r.PeptideSeq == "GILLYGTK")
                    {
                        Console.WriteLine("Test");
                    }

                    if (temp_computedRIAValue_withexperimentalIO.Count == 0)
                    {

                        double ss = 0;
                        double rss = 0;

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                ss = ss + Math.Pow((double)(p.RIA_value - meanval_ria), 2);
                                rss = rss + Math.Pow((double)(p.RIA_value - computedRIAValue), 2);
                            }
                        }

                        double RSquare = 1 - (rss / ss);
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

                        foreach (var p in temp_experimentalvalue)
                        {
                            if (p.RIA_value != null)
                            {
                                var computedRIAValue_mo = temp_computedRIAValue.Where(x => x.time == p.Time).First().value;
                                var computedRIAValue_io = temp_computedRIAValue_withexperimentalIO.Where(x => x.time == p.Time).First().value;

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
                            foreach (var x in temp_computedRIAValue) temp_theoreticalI0Values.Add(x);
                            //temp_expectedI0Values.AddRange(temp_computedRIAValue);
                        }
                        else
                        {
                            //temp_expectedI0Values.AddRange(temp_computedRIAValue_withexperimentalIO);
                            foreach (var x in temp_computedRIAValue_withexperimentalIO) temp_theoreticalI0Values.Add(x);
                        }

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
                foreach (int t in this.experiment_time)
                {
                    try
                    {
                        io = (double)(peptides[i].M0 / 100);
                        neh = (double)(peptides[i].Exchangeable_Hydrogens);
                        k = (double)(peptides[i].Rateconst);

                        var ria_val2 = mergedRIAvalues.Where(x => x.PeptideSeq.Trim() == peptides[i].PeptideSeq.Trim() & x.Time == t & x.Charge == peptides[i].Charge).ToList();
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

                    Peptide p = temp_pep.Where(x => x.PeptideSeq == r.PeptideSeq & x.Charge == r.Charge).FirstOrDefault();
                    if (p != null)
                    {
                        io = (double)(p.M0 / 100);
                        neh = (double)(p.Exchangeable_Hydrogens);
                        k = (double)(p.Rateconst);



                        var dn = io * (1 - (Math.Pow(1 - (pw / (1 - ph)), neh)));
                        var nu = io - r.RIA_value;
                        var fv = nu / dn;

                        xval.Add(r.Time);
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
