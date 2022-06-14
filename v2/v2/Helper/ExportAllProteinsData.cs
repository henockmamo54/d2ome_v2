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
                int counter = 0;
                var temp = csvfilePaths.Select(x => x.Split('\\').Last().Replace(".Quant.csv", "").Replace(".RateConst.csv", "")).Distinct().ToList();
                progressBar_exportall.Invoke(new Action(() => progressBar_exportall.Maximum = temp.Count));
                progressBar_exportall.Invoke(new Action(() => progressBar_exportall.Value = temp.Count));

                // for each file prepare the datasource for ploting
                foreach (string proteinName in temp)
                {
                    progressBar_exportall.Invoke(new Action(() => progressBar_exportall.Value = counter));
                    counter = counter + 1;

                    //string files_txt_path = txt_source.Text + @"\files.centroid.txt"; 
                    string files_txt_path = sourcePath + @"\files.txt";
                    string quant_state_file_path = sourcePath + @"\quant.state";
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

                    var proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path, quant_state_file_path);

                    proteinExperimentData.loadAllExperimentData();
                    proteinExperimentData.computeDeuteriumenrichmentInPeptide();
                    proteinExperimentData.computeRIAPerExperiment();
                    proteinExperimentData.normalizeRIAValuesForAllPeptides();
                    proteinExperimentData.computeAverageA0();
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

            chart2.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea1);
            ////legend1.Name = "Legend1";
            ////chart2.Legends.Add(legend1);
            chart2.Location = new System.Drawing.Point(6, 16);
            chart2.Name = "chart_peptide";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            ////series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.MarkerSize = 50;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            series2.BorderWidth = 9;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Purple;
            ////series2.Legend = "Legend1";
            series2.Name = "Series3";
            chart2.Series.Add(series1);
            chart2.Series.Add(series2);
            chart2.Size = new System.Drawing.Size((int)(562 * 6), (int)(310 * 6));
            chart2.TabIndex = 0;
            chart2.Text = "chart1";

            //chart2.Series[0].Font = new Font(chart2.Font.FontFamily, 32, System.Drawing.FontStyle.Bold);


            // chartline tension
            chart2.Series["Series3"]["LineTension"] = "0.1";

            // remove grid lines
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

            // chart labels added  
            chart2.ChartAreas[0].AxisX.Title = "Time (labeling duration)";
            chart2.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font(chart2.Font.FontFamily, 72);
            chart2.ChartAreas[0].AxisX.LineWidth = 3;
            chart2.ChartAreas[0].AxisX.LabelStyle.Font = new Font(chart2.Font.FontFamily, 72);
            chart2.ChartAreas[0].AxisX.Minimum = 0;


            chart2.ChartAreas[0].AxisY.Title = "Relative abundance \n of monoisotope";
            chart2.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font(chart2.Font.FontFamily, 72);
            chart2.ChartAreas[0].AxisY.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;
            chart2.ChartAreas[0].AxisY.LineWidth = 3;
            chart2.ChartAreas[0].AxisY.LabelStyle.Font = new Font(chart2.Font.FontFamily, 72);


            /*
            // chart add legend
            chart2.Series["Series3"].LegendText = "Theoretical fit";
            chart2.Series["Series1"].LegendText = "Experimental value";

            // chart legend size
            chart2.Legends[0].Position.Auto = false;
            chart2.Legends[0].Position.X = 65;
            chart2.Legends[0].Position.Y = 10;
            chart2.Legends[0].Position.Width = 30;
            chart2.Legends[0].Position.Height = 20;

            // cahrt font
            chart2.Legends[0].Font = new Font(chart2.Legends[0].Font.FontFamily, 14); */

            //chart2.Series["Series3"].BorderWidth = 8;
            //chart2.Series["Series3"].Color = Color.Purple;

            return chart2;
        }
        private void draw_peptideChart(ProteinExperimentDataReader proteinExperimentData, string outputpath)
        {
            try
            {


                {

                    Chart chart2 = preppare_chart();
                    var selected = proteinExperimentData.peptides;
                    var temp = outputpath.Split('\\');
                    var protein_name = temp[temp.Length - 1];

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

                        #region zero ion score
                        /*
                        // ion score == 0 plot
                        var chart_data2 = proteinExperimentData.mergedRIAvaluesWithZeroIonScore.Where(x => x.PeptideSeq == p.PeptideSeq & x.Charge == p.Charge).OrderBy(x => x.Time).ToArray();

                        if (chart2.Series.FindByName("Zero Ion score") != null)
                            chart2.Series.Remove(chart2.Series.FindByName("Zero Ion score"));

                        Series s1 = new Series();
                        s1.Name = "Zero Ion score";
                        if (chart_data2.Length > 0)
                            s1.Points.DataBindXY(chart_data2.Select(x => x.Time).ToArray(), chart_data2.Select(x => x.RIA_value).ToArray());

                        s1.ChartType = SeriesChartType.FastPoint;
                        s1.Color = Color.Red;
                        s1.MarkerSize = 7;
                        s1.MarkerStyle = MarkerStyle.Circle;
                        chart2.Series.Add(s1);
                        */
                        #endregion

                        #endregion

                        #region expected data plot 

                        var theoretical_chart_data = proteinExperimentData.theoreticalI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        //
                        List<double> x_val = theoretical_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                        List<double> y_val = theoretical_chart_data.Select(x => x.value).ToList();

                        chart2.Series["Series3"].Points.DataBindXY(theoretical_chart_data.Select(x => x.time).ToArray(), theoretical_chart_data.Select(x => x.value).ToArray());

                        // set x axis chart interval
                        if (x_val.Count > 0)
                        {
                            chart2.ChartAreas[0].AxisX.Interval = (int)theoretical_chart_data.Select(x => x.time).ToArray().Max() / 10;
                            chart2.ChartAreas[0].AxisX.Maximum = x_val.Max() + 0.01;
                        }



                        #endregion



                        var max_y_list = new List<double>();
                        foreach (var series in chart2.Series)
                        {
                            if (series.Points.Count > 0)
                            {
                                var maxvalue = series.Points.FindMaxByValue();
                                if (maxvalue != null)
                                    max_y_list.Add(maxvalue.YValues[0]);
                            }
                        }

                        chart2.ChartAreas[0].AxisY.Maximum = max_y_list.Max() + 0.08;
                        chart2.ChartAreas[0].AxisY.Interval = chart2.ChartAreas[0].AxisY.Maximum / 5 - 0.005;
                        chart2.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";
                        chart2.ChartAreas[0].AxisX.Title = "Time (" + proteinExperimentData.labelingDuration + ")";


                        #region find best fit

                        var current_peptide = p;

                        double new_RS = findBestFits(chart2, proteinExperimentData, current_peptide,
                            chart_data.Select(x => x.I0_t_fromA1A0).ToList(),
                            chart_data.Select(x => x.I0_t_fromA2A0).ToList(),
                            chart_data.Select(x => x.I0_t_fromA2A1).ToList(),
                            chart_data.Select(x => x.RIA_value).ToList(),
                            proteinExperimentData.theoreticalI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).Select(x => x.value).Take(proteinExperimentData.experiment_time.Count).ToList());

                        if ( (double.IsNaN(new_RS) || new_RS < 0.9) || (p.RSquare > 0.9)) continue;

                        #endregion


                        #region update chart title
                        Title title = new Title();
                        title.Font = new Font(chart2.Font.FontFamily, 72, System.Drawing.FontStyle.Regular);

                        var chargestring = "";
                        switch (p.Charge)
                        {
                            case 0: chargestring = "\u207A\u2070"; break;
                            case 1: chargestring = "\u207A\u2071"; break;
                            case 2: chargestring = "\u207A\u00B2"; break;
                            case 3: chargestring = "\u207A\u00B3"; break;
                            case 4: chargestring = "\u207A\u2074"; break;
                            case 5: chargestring = "\u207A\u2075"; break;
                            case 6: chargestring = "\u207A\u2076"; break;
                            case 7: chargestring = "\u207A\u2077"; break;
                            case 8: chargestring = "\u207A\u2078"; break;
                            case 9: chargestring = "\u207A\u2079"; break;
                            default: chargestring = ""; break;
                        }

                        if (p.Rateconst != double.NaN)
                        {
                            title.Text = protein_name + ":" + p.PeptideSeq + chargestring + " (k = " + BasicFunctions.formatdoubletothreedecimalplace((double)p.Rateconst) + " \u00B1 " + ((double)p.std_k).ToString("G2") + ", R" + "\u00B2" + " = " + ((double)p.RSquare).ToString("#0.#0") + " : " + ((double)new_RS).ToString("#0.#0") + ", m/z = " + ((double)p.SeqMass).ToString("#0.###") + ")";
                        }
                        else
                        {
                            title.Text = protein_name + ":" + p.PeptideSeq + " (m/z = " + ((double)p.SeqMass).ToString("#0.###") + ", z = " + ((double)p.Charge).ToString() + ")";

                        }
                        //clear chart title
                        chart2.Titles.Clear();
                        chart2.Titles.Add(title);
                        #endregion

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
                                catch (Exception e)
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

        public double findBestFits(Chart chart2, ProteinExperimentDataReader proteinExperimentData, Peptide current_peptide, List<double?> a1ao, List<double?> a2ao, List<double?> a1a2, List<double?> experimental_RIA, List<double> theoretical_RIA, bool verbose = true)
        {
            try
            {
                var selected_points = new List<double>();
                var selected_A1A0_count = 0;
                var selected_A2A0_count = 0;
                var selected_A2A1_count = 0;
                for (int i = 0; i < proteinExperimentData.experiment_time.Count; i++)
                {
                    var theoretical_val = (double)theoretical_RIA[i];
                    var candidate_points = new List<double>();

                    candidate_points.Add(a1ao[i] == null || double.IsNaN((double)a1ao[i]) ? double.MaxValue : Math.Abs((double)a1ao[i] - theoretical_val));
                    candidate_points.Add(a2ao[i] == null || double.IsNaN((double)a2ao[i]) ? double.MaxValue : Math.Abs((double)a2ao[i] - theoretical_val));
                    candidate_points.Add(a1a2[i] == null || double.IsNaN((double)a1a2[i]) ? double.MaxValue : Math.Abs((double)a1a2[i] - theoretical_val));
                    candidate_points.Add(experimental_RIA[i] == null || double.IsNaN((double)experimental_RIA[i]) ? double.MaxValue : Math.Abs((double)experimental_RIA[i] - theoretical_val));

                    // index of minimum point
                    var min_val = candidate_points.Min();

                    // add the minimum error point to the selected list for the specific time point
                    if (min_val == double.MaxValue) selected_points.Add(double.NaN);
                    else
                    {
                        var index_min_val = candidate_points.IndexOf(min_val);
                        switch (index_min_val)
                        {
                            case 0: selected_points.Add((double)a1ao[i]); selected_A1A0_count += 1; break;
                            case 1: selected_points.Add((double)a2ao[i]); selected_A2A0_count += 1; break;
                            case 2: selected_points.Add((double)a1a2[i]); selected_A2A1_count += 1; break;
                            case 3: selected_points.Add((double)experimental_RIA[i]); break;
                            default: selected_points.Add(double.NaN); break;
                        }
                    }
                }


                var rsquared = Helper.BasicFunctions.computeRsquared(selected_points, theoretical_RIA);
                var rsquaredw = Helper.BasicFunctions.computeRsquared(experimental_RIA.Select(x => (double)x).ToList(), theoretical_RIA);

                var test = Helper.BasicFunctions.computeRsquared(experimental_RIA.Select(x => (double)x).ToList(), theoretical_RIA);
                Console.WriteLine("test new rsquared => " + rsquared.ToString());

                if (chart2.Series.FindByName("selected") != null)
                    chart2.Series.Remove(chart2.Series.FindByName("selected"));
                Series s_pxt = new Series();
                s_pxt.Name = "selected";
                s_pxt.Points.DataBindXY(proteinExperimentData.experiment_time.ToArray(), selected_points.ToArray());
                s_pxt.ChartType = SeriesChartType.FastPoint;
                s_pxt.Color = Color.DodgerBlue;
                s_pxt.MarkerSize = 50;
                s_pxt.MarkerStyle = MarkerStyle.Cross;
                chart2.Series.Add(s_pxt);

                return rsquared;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return double.NaN;
        }

    }
}
