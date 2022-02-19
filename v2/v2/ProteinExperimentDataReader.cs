using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using v2.Helper;
using v2.Model;
using static v2.Helper.ReadFilesInfo_txt;
using IsotopomerDynamics;

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

        struct dataloader
        {
            public double neh;
            public double ext;
            public double error;
        }

        public void computeDeuteriumenrichmentInPeptide()
        {
            // this function computes pX(t)
            // which is the deuterium enrichment in a peptide from the heavy water at the
            // labeling duration time t

            double ph = 1.5574E-4;
            foreach (Peptide peptide in peptides)
            {
                //List<double> _neh_list = new List<double>();
                //List<dataloader> _dataErrorlist = new List<dataloader>();

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

                    double sum_a3_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I3 / x.I0))).Value;
                    double a3_a0_t_0 = sum_a3_ao_t_0 / sum_io_t_0;
                    
                    double sum_a4_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I4 / x.I0))).Value;
                    double a4_a0_t_0 = sum_a4_ao_t_0 / sum_io_t_0;

                    //double n = 0;
                    //double d = 0;
                    //foreach (var er in experimentsAt_t_0)
                    //{
                    //    var tempsum = er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5;
                    //    double sum_val = tempsum != null ? (double)(er.I0 + er.I1 + er.I2 + er.I3 + er.I4 + er.I5) : 0;
                    //    var ria = er.I0 / sum_val;

                    //    n += (double)(ria * er.I0);
                    //    d += (double)er.I0;
                    //}

                    //double io_0 = n / d;



                    foreach (ExperimentRecord er in experimentRecordsPerPeptide)
                    {
                        if (er.I0_t != null) continue;
                        var experimentsAt_t = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == er.ExperimentTime & t.I0 != null & t.I0 > 0).ToList();
                        if (experimentsAt_t.Count == 0) continue;

                        //n = 0;
                        //d = 0;
                        //foreach (var r in experimentsAt_t)
                        //{
                        //    var tempsum = r.I0 + r.I1 + r.I2 + r.I3 + r.I4 + r.I5;
                        //    double sum_val = tempsum != null ? (double)(r.I0 + r.I1 + r.I2 + r.I3 + r.I4 + r.I5) : 0;
                        //    var ria = er.I0 / sum_val;

                        //    n += (double)(ria * r.I0);
                        //    d += (double)r.I0;
                        //}

                        //double io_t = n / d;

                        //var new_val = 1 - Math.Pow();

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

                        double sum_a3_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I3 / x.I0))).Value;
                        double a3_a0_t = sum_a3_ao_t / sum_io_t;

                        double sum_a4_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I4 / x.I0))).Value;
                        double a4_a0_t = sum_a4_ao_t / sum_io_t;

                        var del_a_1_0 = al_a0_t - al_a0_t_0;
                        var del_a_2_0 = a2_a0_t - a2_a0_t_0;

                        //////var a = del_a_2_0 - (al_a0_t_0 * ph * del_a_1_0) - (al_a0_t * (1 - ph) * del_a_1_0) + 0.5 * ((-Math.Pow(del_a_1_0, 2) * (2 * ph - 1)) + (del_a_1_0 * ((2 * ph - 1) / (1 - ph))));
                        ////var a = del_a_2_0;
                        ////a = a - (al_a0_t_0 * ph * del_a_1_0);
                        ////a = a - (al_a0_t * (1 - ph) * del_a_1_0);
                        ////a = a + 0.5 * (-1 * (Math.Pow(del_a_1_0, 2) * (2 * ph - 1)) + (del_a_2_0 * ((2 * ph - 1) / (1 - ph))));


                        //////var b = (-1 * del_a_2_0 * (1 - ph)) + (al_a0_t_0 * ph * del_a_1_0 * 2 * (1 - ph)) + ((1 - 2 * ph) * al_a0_t * (1 - ph) * del_a_1_0) +
                        //////    0.5 * ((Math.Pow(del_a_1_0, 2) * (1 - ph) * (2 * ph - 1)) + (Math.Pow(del_a_1_0, 2) * 2 * ph * (1 - ph)) - (del_a_1_0 * 2 * ph));
                        ////var b = (-1 * del_a_2_0 * (1 - ph));
                        ////b = b + (al_a0_t_0 * ph * del_a_1_0 * 2 * (1 - ph));
                        ////b = b + ((1 - 2 * ph) * al_a0_t * (1 - ph) * del_a_1_0);
                        ////b = b + 0.5 * ((Math.Pow(del_a_2_0, 2) * (1 - ph) * (2 * ph - 1)) + (Math.Pow(del_a_1_0, 2) * 2 * ph * (1 - ph)) - (del_a_1_0 * 2 * ph));

                        ////var new_px_t = -b / a;
                        ////var new_temp_neh = Math.Round(((1 - ph - new_px_t) / new_px_t) * del_a_1_0 * (1 - ph));
                        ////Console.WriteLine("999999999999990000000000000000000000 => " + new_px_t + " , " + new_temp_neh);
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


                        double I0_t_new_a2 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (new_px_t / (1 - ph))), (double)NEH));
                        er.I0_t_new_a2 = I0_t_new_a2;

                        //Console.WriteLine("New Px_t " + peptide.PeptideSeq + " " + er.ExperimentTime.ToString() + " = " + new_px_t.ToString());
                        var computedNeh = ((1 - ph - new_px_t) / new_px_t) * del_a_1_0 * (1 - ph);
                        Console.WriteLine("======================================== " + er.ExperimentTime.ToString() + " new_px_t = " + new_px_t + " NEH = " + NEH +
                            " computedNeh = " + computedNeh + " , " + (new_px_t * NEH)
                            );
                        
                        #endregion

                        #region new trial with MassIsotopomers dll

                        float[] fNatIsotopes = new float[10];
                        float[] fLabIsotopes = new float[10];

                        fNatIsotopes[0] = (float)((double)peptide.M0);
                        fNatIsotopes[1] = (float)((double)peptide.M1);
                        fNatIsotopes[2] = (float)((double)peptide.M2);
                        fNatIsotopes[3] = (float)((double)peptide.M3);
                        fNatIsotopes[4] = (float)((double)peptide.M4);
                        fNatIsotopes[5] = (float)((double)peptide.M5);
                        MassIsotopomers MIDyn = new MassIsotopomers();

                        string newfileval = "px(t),NEH,A3/A0 (Exp.),A3/A0 (Theo.),A2/A0 (Exp.),A2/A0 (Theo.),A1/A0 (Exp.),A1/A0 (Theo.),A3/A0 (Exp.)-A3/A0 (Theo.),A2/A0 (Exp.)-A2/A0 (Theo.),A1/A0 (Exp.)-A1/A0 (Theo.)," +
                            " SSE, ABS(SE), E(t), px(t)_f,NEH_f, \u0394A10, SSE_421,SSE_211,SSE_221," +
                            "SSE_212 ,SSE_511 ,SSE_531,SSE_532,SSE_732," +
                            "SSE_242 , SSE_261 , SSE_241," +
                            "SSE_224 , SSE_216 , SSE_124  \n";
                        //string newfileval = "pxt,neh,diff\n";
                        List<string> newfileval_list = new List<string>();


                        int Nall_Hyd=MIDyn.NumberOfHydrogens(peptide.PeptideSeq);
                        //int Nall_Hyd = 128;

                        if (peptide.PeptideSeq == "VTVLEGDILDTQYLR")
                            Nall_Hyd = 128;
                        else if (peptide.PeptideSeq == "QTILDVNLK")
                            Nall_Hyd = 83;
                        else if (peptide.PeptideSeq == "PGWScLVTGAGGFLGQR")
                            Nall_Hyd = 117;
                        else if (peptide.PeptideSeq == "KEFFNLETSIK")
                            Nall_Hyd = 99;
                        else if (peptide.PeptideSeq == "DLGYEPLVSWEEAK")
                            Nall_Hyd = 111;
                        else if (peptide.PeptideSeq == "AVLAANGSMLK")
                            Nall_Hyd = 84;

                        for (double neh = 1; neh < 45; neh = neh + 0.1)
                        {
                            //for (float fBWE = (float)0.001; fBWE <= 0.06; fBWE = fBWE + (float)0.001)
                            //for (float fBWE = (float)0.03; fBWE <= 0.04; fBWE = fBWE + (float)0.001)
                            {
                                //var tempc = neh / (del_a_1_0 * (1 - ph));
                                //float fBWE = (float)((float)(1.0 - ph) / (1.0 + tempc));
                                //if (fBWE > 0.05 | fBWE < 0) continue;

                                //float fBWE = (float)(0.000 + (it - 1) * 0.35 / 100.0);
                                float fBWE = (float)(0.0335);

                                MIDyn.CalculateMIDynamics(fNatIsotopes, fLabIsotopes, fBWE, (float)neh, Nall_Hyd);

                                var new_a3_a0_t = fLabIsotopes[3] / fLabIsotopes[0];
                                var new_a2_a0_t = fLabIsotopes[2] / fLabIsotopes[0];
                                var new_a1_a0_t = fLabIsotopes[1] / fLabIsotopes[0];

                                var a3diff = new_a3_a0_t - a3_a0_t; var a3diff_s = a3diff * a3diff;
                                var a2diff = new_a2_a0_t - a2_a0_t; var a2diff_s = a2diff * a2diff;
                                var a1diff = new_a1_a0_t - al_a0_t; var a1diff_s = a1diff * a1diff;

                                var a1a2a3s = a3diff_s + a2diff_s + a1diff_s;
                                var a1a2a3abs = Math.Abs(a3diff + a2diff + a1diff);

                                var a1a2a3s_421 = 4 * a3diff_s + 2 * a2diff_s + a1diff_s;
                                var a1a2a3s_211 = 2 * a3diff_s + a2diff_s + a1diff_s;
                                var a1a2a3s_221 = 2 * a3diff_s + 2 * a2diff_s + a1diff_s;

                                var a1a2a3s_212 = 2 * a3diff_s + 1 * a2diff_s + 2 * a1diff_s;
                                var a1a2a3s_511 = 5 * a3diff_s + 1 * a2diff_s + 1 * a1diff_s;
                                var a1a2a3s_531 = 5 * a3diff_s + 3 * a2diff_s + 1 * a1diff_s;
                                var a1a2a3s_532 = 5 * a3diff_s + 3 * a2diff_s + 2 * a1diff_s;
                                var a1a2a3s_732 = 7 * a3diff_s + 3 * a2diff_s + 2 * a1diff_s;

                                var a1a2a3s_242 = 2 * a3diff_s + 4 * a2diff_s + 2 * a1diff_s;
                                var a1a2a3s_261 = 2 * a3diff_s + 6 * a2diff_s + a1diff_s;
                                var a1a2a3s_241 = 2 * a3diff_s + 4 * a2diff_s + a1diff_s;

                                var a1a2a3s_224 = 2 * a3diff_s + 2 * a2diff_s + 4 * a1diff_s;
                                var a1a2a3s_216 = 2 * a3diff_s + 1 * a2diff_s + 6 * a1diff_s;
                                var a1a2a3s_124 = a3diff_s + 2 * a2diff_s + 4 * a1diff_s;

                                var e_t = fBWE * neh;
                                var px_f = 1 - ph - (e_t / (del_a_1_0 * (1 - ph)));
                                var neh_f = e_t / px_f;

                                newfileval_list.Add(fBWE + "," + neh + "," + a3_a0_t + "," + new_a3_a0_t + "," + a2_a0_t + "," + new_a2_a0_t + "," + al_a0_t + "," + new_a1_a0_t + "," + a3diff + "," + a2diff + "," + a1diff + "," + a1a2a3s + "," + a1a2a3abs
                                    + "," + e_t + "," + px_f + "," + neh_f + "," + del_a_1_0
                                    + "," + a1a2a3s_421 + "," + a1a2a3s_211 + "," + a1a2a3s_221
                                    + "," + a1a2a3s_212 + "," + a1a2a3s_511 + "," + a1a2a3s_531 + "," + a1a2a3s_532 + "," + a1a2a3s_732
                                    + "," + a1a2a3s_242 + "," + a1a2a3s_261 + "," + a1a2a3s_241
                                    + "," + a1a2a3s_224 + "," + a1a2a3s_216 + "," + a1a2a3s_124
                                    + "\n");
                                //newfileval += (fBWE + "," + neh + "," + a3_a0_t + "," + new_a3_a0_t + "," + a2_a0_t + "," + new_a2_a0_t + "," + al_a0_t + "," + new_a1_a0_t + "," + a3diff + "," + a2diff + "," + a1diff + "\n");
                                //newfileval += (fBWE + "," + neh + "," + (a3diff + a2diff + a1diff) + "\n");

                                //dataloader dl = new dataloader();
                                //dl.neh = neh;
                                //dl.ext = neh * fBWE;
                                //dl.error = Math.Abs(a2diff);
                                //_dataErrorlist.Add(dl);

                            }
                        }

                        //if (er.ExperimentTime != 0)
                        //{
                        //    var best_neh = _dataErrorlist.OrderBy(re => re.error).Select(u => u.neh).ToList().Distinct().Take(15);
                        //    _neh_list.AddRange(best_neh);

                        //}



                        TextWriter tw = new StreamWriter("test/_" + peptide.PeptideSeq + er.ExperimentTime.ToString() + "_" + peptide.Charge.ToString() + ".csv");
                        tw.WriteLine(newfileval.Trim());
                        foreach (var l in newfileval_list)
                            tw.WriteLine(l.Trim());
                        tw.Close();

                        #endregion

                        #region test 

                        //var _d = al_a0_t * del_a_1_0 * (1 - ph);
                        //var _c = del_a_1_0 * (1 - ph);
                        //var _b = 0.5 * (Math.Pow(ph / (1 - ph), 2));
                        //var _a = -al_a0_t_0 * (ph / (1 - ph));

                        ////var coeff = new double[] { 2 * _b, _a + _b, _d-_c * _c  , -0.5*_c * _c };
                        //var coeff = new double[] { _b, _a, (-del_a_2_0 + _b - 0.5 * _c * _c + _d), -0.5 * _c * _c };

                        ////var sol = RealPolynomialRootFinder.FindRoots(coeff);

                        #endregion

                        #region varying pxt only

                        ////var temp2 = a2_a0_t;
                        ////var temp1 = al_a0_t;

                        //////Console.WriteLine("======================================== " + er.ExperimentTime.ToString() + " new_px_t = " + new_px_t + " NEH = " + NEH);

                        ////var px_t_new = new_px_t;
                        ////var NEH_new = NEH;

                        ////var temp1_t = al_a0_t_0 + ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));

                        ////var temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                        ////    (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                        ////   (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                        ////   (NEH_new * (px_t_new + ph) * temp1_t / (1 - ph - px_t_new));

                        //////Console.WriteLine(temp2_t + temp1_t - temp2 - temp1);

                        ////List<double> difflist = new List<double>();
                        ////List<double> pxtlist = new List<double>();
                        ////string fileval = "";

                        ////double step = 0.0005;
                        ////for (int i = 1; i * step < 0.06; i++)
                        ////{
                        ////    px_t_new = i * step;

                        ////    temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                        ////       (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                        ////      (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                        ////      (NEH_new * (px_t_new + ph) * al_a0_t / (1 - ph - px_t_new));
                        ////    temp1_t = al_a0_t_0 + ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));
                        ////    var diff = Math.Abs(temp2_t + temp1_t - temp2 - temp1);
                        ////    difflist.Add(diff);
                        ////    pxtlist.Add(px_t_new);

                        ////    fileval += (i * step + "," + diff + "\n");
                        ////}
                        ////double minval = difflist.Min();
                        ////int minimumValueIndex = difflist.IndexOf(minval);
                        ////Console.WriteLine("==min== " + minval + " px = " + pxtlist[minimumValueIndex] + " , " + (pxtlist[minimumValueIndex] * NEH));

                        //////TextWriter tw = new StreamWriter(peptide.PeptideSeq + er.ExperimentTime.ToString() + ".csv");
                        //////tw.WriteLine(fileval.Trim());
                        //////tw.Close();

                        #endregion

                        #region varying pxt AND NEH

                        ////difflist = new List<double>();
                        ////pxtlist = new List<double>();
                        ////var NEHlist = new List<double>();
                        ////fileval = "";

                        ////step = 0.001;
                        ////for (int k = 1; k < 35; k++)
                        ////{
                        ////    for (int i = 1; i * step <= 0.06; i++)
                        ////    {
                        ////        px_t_new = i * step;
                        ////        NEH_new = k;

                        ////        //px_t_new = i * step;
                        ////        //NEH_new = Math.Round(((1 - ph - px_t_new) / px_t_new) * del_a_1_0 * (1 - ph));
                        ////        //var k = NEH_new;
                        ////        //if (k > 50) { /*Console.WriteLine("==neh>30== " + minval + " px = " + px_t_new + " neh="+k); */ continue; }


                        ////        //NEH_new = k;
                        ////        //var tempc = NEH_new / (del_a_1_0 * (1 - ph));
                        ////        //px_t_new = (1.0 - ph) / (1.0 + tempc);
                        ////        //if (px_t_new > 0.05 | px_t_new < 0) continue;

                        ////        temp1_t = al_a0_t_0 + ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));

                        ////        temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                        ////           (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                        ////          (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                        ////          (NEH_new * (px_t_new + ph) * temp1_t / (1 - ph - px_t_new));


                        ////        var diff = Math.Abs(temp2_t + temp1_t - temp2 - temp1);


                        ////        //if (diff < 0.1)
                        ////        {

                        ////            difflist.Add(diff);
                        ////            pxtlist.Add(px_t_new);
                        ////            NEHlist.Add(k);
                        ////            fileval += (px_t_new + "," + k + "," + diff + "\n");
                        ////        }
                        ////    }
                        ////}
                        ////if (difflist.Count > 0)
                        ////{
                        ////    minval = difflist.Min();
                        ////    minimumValueIndex = difflist.IndexOf(minval);
                        ////    Console.WriteLine("==min==2  " + minval + " px = " + pxtlist[minimumValueIndex] + " NEH=" + NEHlist[minimumValueIndex] + ", " + (pxtlist[minimumValueIndex] * NEHlist[minimumValueIndex]));
                        ////}

                        ////TextWriter tw2 = new StreamWriter(peptide.PeptideSeq + er.ExperimentTime.ToString() + ".csv");
                        ////tw2.WriteLine(fileval.Trim());
                        ////tw2.Close();

                        #endregion


                        //var test = ((-del_a_1_0 * del_a_1_0) / ((2 * del_a_2_0) + (del_a_1_0 * del_a_1_0) - (2 * del_a_1_0 * al_a0_t)));
                        //Console.WriteLine("temp test ======//*/*/*/*/======> " + test);

                        //#region a3/a0

                        //double sum_a3_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I3 / x.I0))).Value;
                        //double a3_a0_t = sum_a2_ao_t / sum_io_t;


                        //var ph_px_const = (new_px_t + ph) / (1 - ph - new_px_t);
                        //var a_t = binomialCoefficient(NEH_new, 3) * Math.Pow(ph_px_const, 3);
                        //var b_t = binomialCoefficient(NEH_new, 2) * Math.Pow(ph_px_const, 2);
                        //var c_t = NEH_new * ph_px_const;
                        //var nt = 0;

                        //var temp3_t = a_t + b_t * (al_a0_t - c_t);
                        //temp3_t += c_t * (a2_a0_t - c_t * (al_a0_t - c_t) - b);
                        //temp3_t += a3_a0_t - NEH_new * (ph / (1 - ph)) * (a2_a0_t_0 - (((NEH_new + 1) * 0.5 * ph) / (1 - ph)) * (al_a0_t_0 - ((nt * ph) / (1 - ph))) - 
                        //    (3*nt*NEH_new+3*nt-NEH_new*NEH_new-3*NEH_new-2)*(Math.Pow(ph/1-ph,2))/6  );

                        //#endregion

                        //////string fileval = "";

                        //////var step = 0.001;
                        //////List<double> testvals = new List<double>();

                        //////List<double> temp2_t_list = new List<double>();
                        //////List<double> temp1_t_list = new List<double>();
                        //////List<double> neh_list = new List<double>();

                        //////for (int i = 0; i * step < 0.06; i++)
                        //////{
                        //////    for (int k = 0; k < 30; k++)
                        //////    {
                        //////        double px_t_new = step * i;
                        //////        double NEH_new = k; // ((1 - ph - px_t_new) / px_t_new) * del_a_1_0 * (1.0 - ph);

                        //////        //var temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                        //////        //    (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                        //////        //   (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                        //////        //   (NEH_new * (px_t_new + ph) * al_a0_t / (1 - ph - px_t_new));

                        //////        var temp1_t = al_a0_t_0 + ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));

                        //////        var temp2_t = a2_a0_t_0 - (al_a0_t_0 * (ph * NEH_new) / (1 - ph)) +
                        //////            (Math.Pow((ph / (1 - ph)), 2) * NEH_new * (NEH_new + 1) / 2) -
                        //////           (Math.Pow((px_t_new + ph) / (1 - ph - px_t_new), 2) * NEH_new * (NEH_new + 1) / 2) +
                        //////           (NEH_new * (px_t_new + ph) * (temp1_t) / (1 - ph - px_t_new));

                        //////        var way = ((px_t_new * NEH_new) / ((1.0 - ph) * (1.0 - ph - px_t_new)));

                        //////        //var diff = Math.Abs(temp2 - temp2_t) + Math.Abs(temp1 - temp1_t); ;
                        //////        var diff = (temp2 - temp2_t) + (temp1 - temp1_t); ;
                        //////        //var diff = Math.Abs(temp2 - temp2_t) ;
                        //////        //var diff = Math.Abs(temp1 - temp1_t); ;

                        //////        //System.Console.WriteLine(i * step + "," + k + "," + diff);
                        //////        fileval += (i * step + "," + k + "," + diff + "\n");

                        //////        if (i != 0)
                        //////        {
                        //////            testvals.Add(diff);
                        //////            temp2_t_list.Add(temp2_t);
                        //////            temp1_t_list.Add(temp1_t);
                        //////            neh_list.Add(NEH_new);
                        //////        }
                        //////    }
                        //////}
                        //////Console.WriteLine("===================" + (testvals.Min().ToString()) + "=====================");

                        //////TextWriter tw = new StreamWriter(peptide.PeptideSeq + er.ExperimentTime.ToString() + ".csv");

                        //////tw.WriteLine(fileval.Trim());
                        //////tw.Close();





                    }

                }
                //_neh_list = _neh_list.Distinct().ToList();
                //peptide.possibleNehs = _neh_list;
            }
        }

        public void computeDeuteriumenrichmentInPeptide2()
        {

            double ph = 1.5574E-4;
            foreach (Peptide peptide in peptides)
            {
                var experimentRecordsPerPeptide = this.experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
                            & p.Charge == peptide.Charge).OrderBy(x => x.ExperimentTime).ToList();

                if (experimentRecordsPerPeptide.Count > 0)
                {


                    foreach (var NEH in peptide.possibleNehs)
                    {
                        //var NEH = (double)peptide.Exchangeable_Hydrogens;

                        List<double> possibleIOs = new List<double>();
                        foreach (var t in experiment_time.Distinct().ToList()) possibleIOs.Add(Double.NaN);

                        //experiments at t=0
                        var experimentsAt_t_0 = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == 0 & t.I0 != null & t.I0 > 0).ToList();
                        double sum_io_t_0 = experimentsAt_t_0.Sum(x => x.I0).Value;

                        double sum_a1_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I1 / x.I0))).Value;
                        double al_a0_t_0 = sum_a1_ao_t_0 / sum_io_t_0;

                        double sum_a2_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I2 / x.I0))).Value;
                        double a2_a0_t_0 = sum_a2_ao_t_0 / sum_io_t_0;

                        double sum_a3_ao_t_0 = experimentsAt_t_0.Sum(x => (x.I0 * (x.I3 / x.I0))).Value;
                        double a3_a0_t_0 = sum_a3_ao_t_0 / sum_io_t_0;

                        List<int> experimentTime = new List<int>();
                        foreach (ExperimentRecord er in experimentRecordsPerPeptide)
                        {
                            //if (er.I0_t != null) continue;
                            if (experimentTime.Contains((int)er.ExperimentTime)) continue;
                            else experimentTime.Add((int)er.ExperimentTime);

                            var experimentsAt_t = experimentRecordsPerPeptide.Where(t => t.ExperimentTime == er.ExperimentTime & t.I0 != null & t.I0 > 0).ToList();
                            if (experimentsAt_t.Count == 0) continue;

                            #region A1(t)/A0(t)
                            double sum_io_t = experimentsAt_t.Sum(x => x.I0).Value;
                            double sum_a1_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I1 / x.I0))).Value;
                            double al_a0_t = sum_a1_ao_t / sum_io_t;
                            #endregion

                            #region A2(t)/A1(t) 
                            double sum_a2_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I2 / x.I0))).Value;
                            double a2_a0_t = sum_a2_ao_t / sum_io_t;
                            double sum_a3_ao_t = experimentsAt_t.Sum(x => (x.I0 * (x.I3 / x.I0))).Value;
                            double a3_a0_t = sum_a3_ao_t / sum_io_t;

                            var del_a_1_0 = al_a0_t - al_a0_t_0;
                            var del_a_2_0 = a2_a0_t - a2_a0_t_0;

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


                            double I0_t_new_a2 = (double)((peptide.M0 / 100.0) * Math.Pow((double)(1 - (new_px_t / (1 - ph))), (double)NEH));

                            var indexofdata = this.experiment_time.IndexOf((int)er.ExperimentTime);
                            possibleIOs[indexofdata] = I0_t_new_a2;

                            #endregion
                            //er.I0_t_new_a2 = I0_t_new_a2;

                            ////Console.WriteLine("New Px_t " + peptide.PeptideSeq + " " + er.ExperimentTime.ToString() + " = " + new_px_t.ToString());
                            //var computedNeh = ((1 - ph - new_px_t) / new_px_t) * del_a_1_0 * (1 - ph);
                            //Console.WriteLine("======================================== " + er.ExperimentTime.ToString() + " new_px_t = " + new_px_t + " NEH = " + NEH +
                            //    " computedNeh = " + computedNeh + " , " + (new_px_t * NEH));

                        }

                        peptide.possibleI0s.Add(possibleIOs);

                    }
                }
            }
        }

        public void getBestNehValue()
        {


            foreach (Peptide r in this.peptides)
            {
                var rsqs = new List<double>();

                //for (int uy = 0; uy < r.possibleNehs.Count(); uy++)
                foreach (var possibleI0s in r.possibleI0s)
                {
                    try
                    {
                        //var experimentalvalue = mergedRIAvalues.Where(x => x.Charge == r.Charge).ToList();
                        //experimentalvalue = experimentalvalue.Where(x => x.PeptideSeq == r.PeptideSeq).ToList();
                        //var temp_experimentalvalue = experimentalvalue.Where(x => x.RIA_value >= 0).ToList();


                        var temp_experimentalvalue = possibleI0s.ToList();
                        var meanval_ria = temp_experimentalvalue.Where(x => x >= 0).ToList().Average();

                        var temp_computedRIAValue = theoreticalI0Values.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();
                        var temp_computedRIAValue_withexperimentalIO = theoreticalI0Values_withExperimentalIO.Where(x => x.peptideseq == r.PeptideSeq & x.charge == r.Charge).ToList();

                        if (temp_computedRIAValue_withexperimentalIO.Count == 0)
                        {

                            double ss = 0;
                            double rss = 0;

                            for (int t = 0; t < experiment_time.Count(); t++)
                            {
                                try
                                {
                                    var computedRIAValue = temp_computedRIAValue.Where(x => x.time == experiment_time[t]).First().value;
                                    if (Double.IsNaN(temp_experimentalvalue[t])) continue;
                                    ss = ss + Math.Pow((double)(temp_experimentalvalue[t] - meanval_ria), 2);
                                    rss = rss + Math.Pow((double)(temp_experimentalvalue[t] - computedRIAValue), 2);
                                }
                                catch (Exception exx) { continue; }
                            }

                            double RSquare = 1 - (rss / ss);
                            //r.RSquare = RSquare;
                            //r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());
                            rsqs.Add(RSquare);
                            ////temp_expectedI0Values.AddRange(temp_computedRIAValue);
                            //foreach (var x in temp_computedRIAValue) temp_theoreticalI0Values.Add(x);
                        }

                        else
                        {
                            double ss = 0;
                            double rss_mo = 0;
                            double rss_io = 0;

                            for (int t = 0; t < experiment_time.Count(); t++)
                            {
                                try
                                {
                                    var computedRIAValue_mo = temp_computedRIAValue.Where(x => x.time == experiment_time[t]).First().value;
                                    var computedRIAValue_io = temp_computedRIAValue_withexperimentalIO.Where(x => x.time == experiment_time[t]).First().value;
                                    if (Double.IsNaN(temp_experimentalvalue[t])) continue;
                                    else
                                    {
                                        ss = ss + Math.Pow((double)(temp_experimentalvalue[t] - meanval_ria), 2);
                                        rss_mo = rss_mo + Math.Pow((double)(temp_experimentalvalue[t] - computedRIAValue_mo), 2);
                                        rss_io = rss_io + Math.Pow((double)(temp_experimentalvalue[t] - computedRIAValue_io), 2);
                                    }
                                }
                                catch (Exception ex) { continue; }
                            }

                            var rss = rss_mo < rss_io ? rss_mo : rss_io;
                            double RSquare = 1 - (rss / ss);
                            //r.RSquare = RSquare;
                            //r.RMSE_value = Math.Sqrt(rss / temp_experimentalvalue.Count());
                            rsqs.Add(RSquare);

                            //if (rss_mo < rss_io)
                            //{
                            //    foreach (var x in temp_computedRIAValue) temp_theoreticalI0Values.Add(x);
                            //    //temp_expectedI0Values.AddRange(temp_computedRIAValue);
                            //}
                            //else
                            //{
                            //    //temp_expectedI0Values.AddRange(temp_computedRIAValue_withexperimentalIO);
                            //    foreach (var x in temp_computedRIAValue_withexperimentalIO) temp_theoreticalI0Values.Add(x);
                            //}

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error => computeRSquare(), " + e.Message);
                    }
                }

                Console.WriteLine("********************4$$$$$$$$$$$$$$$$$$$$$$$$$$ " + rsqs.Max() + ", " + r.possibleNehs[rsqs.IndexOf(rsqs.Max())]);

            }




        }

        public double binomialCoefficient(double N, int K)
        {
            double result = 1;
            for (int i = 1; i <= K; i++)
            {
                result *= N - (K - i);
                result /= i;
            }
            return result;
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

                //double io_0 = (double)temp_RIAvalues.Where(x => x.Time == 0).FirstOrDefault().RIA_value;
                double io_0 = 0;

                //Console.WriteLine("//////////////////////////////////////*********************" + p.PeptideSeq);

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

                    if (t == 0) io_0 = (double)ria.RIA_value;

                    var tempvals = 1 - (Math.Pow((double)(ria.RIA_value / io_0), 1 / (double)p.Exchangeable_Hydrogens));

                    //System.Console.WriteLine(p.PeptideSeq + " " + t + " = " + tempvals);

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
