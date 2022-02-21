using IsotopomerDynamics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using v2.Model;
using static v2.ProteinExperimentDataReader;

namespace v2.Helper
{
    public class ExportAllProteinsData
    {
        string outputPath = "";
        string sourcePath = "";
        public ExportAllProteinsData(string sourcePath, string outputPath)
        {
            this.sourcePath = sourcePath;
            this.outputPath = outputPath;
        }

        public void Export_all_ProteinChart(System.Windows.Forms.ProgressBar progressBar_exportall)
        {

            string[] filePaths = Directory.GetFiles(sourcePath);
            var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

            if (csvfilePaths.Count == 0)
            {
                //MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
            }
            else
            {
                var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).Distinct().ToList();

                int counter = 0;

                progressBar_exportall.Invoke(new Action(() =>
                  progressBar_exportall.Maximum = temp.Count));

                progressBar_exportall.Invoke(new Action(() =>
                  progressBar_exportall.Value = temp.Count));

                //progressBar_exportall.Maximum = temp.Count;
                //  progressBar_exportall.Value = 0;

                // for each file prepare the datasource for ploting
                foreach (string proteinName in temp)
                {
                    progressBar_exportall.Invoke(new Action(() =>
                      progressBar_exportall.Value = counter));

                    counter = counter + 1;

                    //string files_txt_path = txt_source.Text + @"\files.centroid.txt"; 
                    string files_txt_path = sourcePath + @"\files.txt";
                    string quant_csv_path = sourcePath + @"\" + proteinName + ".Quant.csv";
                    string RateConst_csv_path = sourcePath + @"\" + proteinName + ".RateConst.csv";

                    var temppath = files_txt_path.Replace("files.txt", "files.centroid.txt");
                    if (File.Exists(temppath)) files_txt_path = temppath;


                    if (!File.Exists(files_txt_path)) { MessageBox.Show("filex.txt is not available in the specified directory.", "Error"); continue; }
                    else { try { string[] lines = System.IO.File.ReadAllLines(files_txt_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

                    if (!File.Exists(quant_csv_path)) { MessageBox.Show(proteinName + ".Quant.csv" + " is not available in the specified directory.", "Error"); continue; }
                    else { try { string[] lines = System.IO.File.ReadAllLines(quant_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

                    if (!File.Exists(RateConst_csv_path)) { MessageBox.Show(proteinName + ".RateConst.csv" + "filex.txt is not available in the specified directory.", "Error"); continue; }
                    else { try { string[] lines = System.IO.File.ReadAllLines(RateConst_csv_path); } catch (Exception ex) { MessageBox.Show(ex.Message); continue; } }

                    var proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


                    proteinExperimentData.loadAllExperimentData();
                    proteinExperimentData.computeAverageA0();
                    proteinExperimentData.computeRIAPerExperiment();
                    proteinExperimentData.mergeMultipleRIAPerDay2();
                    proteinExperimentData.computeTheoreticalCurvePoints();
                    proteinExperimentData.computeTheoreticalCurvePointsBasedOnExperimentalI0();
                    proteinExperimentData.computeRSquare();
                    ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();

                    // for each peptide draw the chart 
                    draw_peptideChart(proteinExperimentData, outputPath + "\\" + proteinName);

                    //progressBar_exportall.Value = progressBar_exportall.Value + 1;


                }
                MessageBox.Show("Done exporting proteins!!");
                progressBar_exportall.Invoke(new Action(() =>
                     progressBar_exportall.Value = 0));
            }

        }

        public Chart preppare_chart()
        {

            Chart chart2 = new Chart();

            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            Series series2 = new Series();
            //ChartArea chartArea2 = new ChartArea();
            //Legend legend2 = new Legend();
            //Series series3 = new Series();
            //Series series4 = new Series();

            chart2.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart2.Legends.Add(legend1);
            chart2.Location = new System.Drawing.Point(6, 16);
            chart2.Name = "chart_peptide";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.MarkerSize = 7;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Purple;
            series2.Legend = "Legend1";
            series2.Name = "Series3";
            chart2.Series.Add(series1);
            chart2.Series.Add(series2);
            chart2.Size = new System.Drawing.Size(662, 316);
            chart2.TabIndex = 0;
            chart2.Text = "chart1";


            // remove grid lines
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = false;




            // chart labels added  
            chart2.ChartAreas[0].AxisX.Title = "Time (labeling duration)";

            chart2.ChartAreas[0].AxisY.Title = "Relative abundance \n of monoisotope";
            chart2.ChartAreas[0].AxisY.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;

            chart2.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart2.Legends[0].Font.FontFamily, 10);
            chart2.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart2.Legends[0].Font.FontFamily, 10);



            // chart add legend
            chart2.Series["Series3"].LegendText = "Theoretical fit";
            chart2.Series["Series1"].LegendText = "Experimental value";


            // chart legend size
            chart2.Legends[0].Position.Auto = false;
            chart2.Legends[0].Position.X = 65;
            chart2.Legends[0].Position.Y = 10;
            chart2.Legends[0].Position.Width = 30;
            chart2.Legends[0].Position.Height = 15;


            // cahrt font
            chart2.Legends[0].Font = new Font(chart2.Legends[0].Font.FontFamily, 9);

            // chartline tension
            chart2.Series["Series3"]["LineTension"] = "0.1";

            chart2.Series["Series3"].BorderWidth = 1;

            chart2.Series["Series3"].Color = Color.Navy;

            chart2.ChartAreas[0].AxisX.Minimum = 0;

            return chart2;
        }

        public void computeDeuteriumenrichmentInPeptide(List<Peptide> selected,List<ExperimentRecord> experimentRecords,string outputpath)
        {
            // this function computes pX(t)
            // which is the deuterium enrichment in a peptide from the heavy water at the
            // labeling duration time t
            bool exists = System.IO.Directory.Exists(outputpath);
            if (!exists)
                System.IO.Directory.CreateDirectory(outputpath);

            double ph = 1.5574E-4;
            foreach (Peptide peptide in selected)
            {
                //List<double> _neh_list = new List<double>();
                //List<dataloader> _dataErrorlist = new List<dataloader>();

                var experimentRecordsPerPeptide = experimentRecords.Where(p => p.PeptideSeq == peptide.PeptideSeq
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
                            "SSE, ABS(SE), E(t), px(t)_f,NEH_f, \u0394A10, SSE_421,SSE_211,SSE_221," +
                            "SSE_212 ,SSE_511 ,SSE_531,SSE_532,SSE_732," +
                            "SSE_242 , SSE_261 , SSE_241," +
                            "SSE_224 , SSE_216 , SSE_124,A_NEH,rsquared,I0_average  \n";
                        //string newfileval = "pxt,neh,diff\n";
                        List<string> newfileval_list = new List<string>();


                        int Nall_Hyd = MIDyn.NumberOfHydrogens(peptide.PeptideSeq);
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

                        var maxneh = selected.Select(x => x.Exchangeable_Hydrogens).Max() + 1;
                        for (double neh = 1; neh < maxneh; neh = neh + 0.1)
                        {
                            for (float fBWE = (float)0.001; fBWE <= 0.06; fBWE = fBWE + (float)0.001)
                            //for (float fBWE = (float)0.03; fBWE <= 0.04; fBWE = fBWE + (float)0.001)
                            {
                                //var tempc = neh / (del_a_1_0 * (1 - ph));
                                //float fBWE = (float)((float)(1.0 - ph) / (1.0 + tempc));
                                //if (fBWE > 0.05 | fBWE < 0) continue;

                                //float fBWE = (float)(0.000 + (it - 1) * 0.35 / 100.0);
                                //float fBWE = (float)(0.0335);

                                //var tempc = neh / (del_a_1_0 * (1 - ph));
                                //float fBWE = (float)((1.0 - ph) / (1.0 + tempc));




                                ////=======================================
                                ////=======================================
                                ////=======================================

                                //c = a2_a0_t_0 - a2_a0_t - al_a0_t_0 * ((ph * NEH) / (1 - ph)) + (Math.Pow((ph / (1 - ph)), 2)) * (NEH * (NEH + 1)) * 0.5;
                                //a = -0.5 * NEH * (NEH + 1);
                                //b = NEH * al_a0_t;
                                //// a2/a0 + a1/ao
                                //c = c + al_a0_t_0 - al_a0_t;
                                //b = b + (NEH / (1 - ph));

                                //temp = Math.Sqrt((b * b) - 4 * a * c);
                                //y = (-b + temp) / (2 * a);
                                //new_px_t = (y * (1 + ph) - ph) / (1 + y);
                                //float fBWE = (float)(new_px_t);



                                MIDyn.CalculateMIDynamics(fNatIsotopes, fLabIsotopes, fBWE, (float)neh, Nall_Hyd);

                                var new_a3_a0_t = fLabIsotopes[3] / fLabIsotopes[0];
                                var new_a2_a0_t = fLabIsotopes[2] / fLabIsotopes[0];
                                var new_a1_a0_t = fLabIsotopes[1] / fLabIsotopes[0];

                                var a3diff = new_a3_a0_t - a3_a0_t; var a3diff_s = a3diff * a3diff;
                                var a2diff = new_a2_a0_t - a2_a0_t; var a2diff_s = a2diff * a2diff;
                                var a1diff = new_a1_a0_t - al_a0_t; var a1diff_s = a1diff * a1diff;

                                var a1a2a3s = a3diff_s + a2diff_s + a1diff_s;
                                //var a1a2a3s = a2diff_s + a1diff_s;
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
                                    + "," + peptide.Exchangeable_Hydrogens + "," + peptide.RSquare + "," + peptide.A0_average 
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



                        //TextWriter tw = new StreamWriter("test/_" + peptide.PeptideSeq + er.ExperimentTime.ToString() + "_" + peptide.Charge.ToString() + ".csv");
                        TextWriter tw = new StreamWriter(outputpath+@"\_" + peptide.PeptideSeq +"_"+ er.ExperimentTime.ToString() + "_" + peptide.Charge.ToString() + ".csv");
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







                    }

                } 
            }
        }


        private void draw_peptideChart(ProteinExperimentDataReader proteinExperimentData, string outputpath)
        {
            try
            {


                {

                    Chart chart2 = preppare_chart();

                    var selected = (from u in proteinExperimentData.peptides
                                    where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                                    select u).Distinct().ToList();


                    #region modification for test
                    selected = selected.Where(p => p.RSquare > 0.95).ToList();
                    selected = selected.Where(p => p.A0_average > 1E7).ToList();
                    computeDeuteriumenrichmentInPeptide(selected, proteinExperimentData.experimentRecords, outputpath);
                    return;
                    #endregion

                    int count = 1;
                    //foreach (Peptide p in proteinExperimentData.peptides)
                    foreach (Peptide p in selected)
                    {
                        //clear chart area
                        chart2.Titles.Clear();

                        #region experimental data plot

                        // prepare the chart data
                        var chart_data = proteinExperimentData.mergedRIAvalues.Where(x => x.PeptideSeq == p.PeptideSeq & x.Charge == p.Charge).OrderBy(x => x.Time).ToArray();
                        chart2.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.Time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                        #endregion

                        #region expected data plot 

                        var expected_chart_data = proteinExperimentData.theoreticalI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        //
                        List<double> x_val = expected_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                        List<double> y_val = expected_chart_data.Select(x => x.value).ToList();



                        {
                            chart2.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

                            // set x axis chart interval
                            chart2.ChartAreas[0].AxisX.Interval = (int)expected_chart_data.Select(x => x.time).ToArray().Max() / 10;
                            chart2.ChartAreas[0].AxisX.Maximum = x_val.Max() + 0.01;

                            chart2.ChartAreas[0].AxisY.Interval = expected_chart_data.Select(x => x.value).ToArray().Max() / 5;
                            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

                        }



                        #endregion


                        // chart title
                        //chart2.Titles.Add(p.PeptideSeq + "(K=" + p.Rateconst.ToString() + ", " + ")");
                        Title title = new Title();
                        title.Font = new Font(chart2.Legends[0].Font.FontFamily, 8, System.Drawing.FontStyle.Bold);
                        //title.Text = p.PeptideSeq + " (K = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ")";
                        title.Text = p.PeptideSeq + " (k = " + p.Rateconst.ToString() + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";
                        chart2.Titles.Add(title);

                        chart2.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.07;
                        chart2.ChartAreas[0].AxisY.Interval = chart2.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                        chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

                        bool exists = System.IO.Directory.Exists(outputpath);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(outputpath);
                        try
                        {
                            using (Bitmap im = new Bitmap(chart2.Width, chart2.Height))
                            {
                                try
                                {
                                    chart2.DrawToBitmap(im, new Rectangle(0, 0, chart2.Width, chart2.Height));
                                    im.Save(outputpath + @"\" + count.ToString() + "_" + p.PeptideSeq + "_" + p.Charge.ToString() + ".jpeg");
                                }
                                catch (ThreadAbortException e)
                                { }
                            }

                        }
                        catch (Exception he)
                        {

                            Console.WriteLine("ERROR: exporting chart for " + (count + 1).ToString() + " " + p.PeptideSeq + "===>" + he.Message);
                        }

                        count++;

                    }



                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error"); 
            }
        }
    }
}
