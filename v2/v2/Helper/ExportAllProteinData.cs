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
    public class ExportAllProteinData
    {
        string outputPath = "";
        string sourcePath = "";
        public ExportAllProteinData(string sourcePath, string outputPath)
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
                    proteinExperimentData.computeRIAPerExperiment();
                    proteinExperimentData.mergeMultipleRIAPerDay2();
                    proteinExperimentData.computeExpectedCurvePoints();
                    proteinExperimentData.computeExpectedCurvePointsBasedOnExperimentalIo();
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
        private void draw_peptideChart(ProteinExperimentDataReader proteinExperimentData, string outputpath)
        {
            try
            {


                {

                    Chart chart2 = preppare_chart();

                    var selected = (from u in proteinExperimentData.peptides
                                    where proteinExperimentData.rateConstants.Select(x => x.PeptideSeq).ToList().Contains(u.PeptideSeq)
                                    select u).Distinct().ToList();
                    int count = 1;
                    //foreach (Peptide p in proteinExperimentData.peptides)
                    foreach (Peptide p in selected)
                    {
                        //clear chart area
                        chart2.Titles.Clear();

                        #region experimental data plot

                        // prepare the chart data
                        var chart_data = proteinExperimentData.mergedRIAvalues.Where(x => x.peptideSeq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        chart2.Series["Series1"].Points.DataBindXY(chart_data.Select(x => x.time).ToArray(), chart_data.Select(x => x.RIA_value).ToArray());

                        #endregion

                        #region expected data plot 

                        var expected_chart_data = proteinExperimentData.expectedI0Values.Where(x => x.peptideseq == p.PeptideSeq & x.charge == p.Charge).OrderBy(x => x.time).ToArray();
                        //
                        List<double> x_val = expected_chart_data.Select(x => x.time).ToList().ConvertAll(x => (double)x);
                        List<double> y_val = expected_chart_data.Select(x => x.value).ToList();



                        {
                            chart2.Series["Series3"].Points.DataBindXY(expected_chart_data.Select(x => x.time).ToArray(), expected_chart_data.Select(x => x.value).ToArray());

                            // set x axis chart interval
                            chart2.ChartAreas[0].AxisX.Interval = expected_chart_data.Select(x => x.time).ToArray().Max() / 10;
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

                        chart2.ChartAreas[0].AxisY.Maximum = Math.Max((double)y_val.Max(), (double)chart_data.Select(x => x.RIA_value).Max()) + 0.1;

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
