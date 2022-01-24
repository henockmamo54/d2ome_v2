using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        public void Export_all_ProteinChart(Chart chart_peptide, System.Windows.Forms.ProgressBar progressBar_exportall)
        {

            string[] filePaths = Directory.GetFiles(sourcePath);
            var csvfilePaths = filePaths.Where(x => x.Contains(".csv") & (x.Contains(".Quant.csv") || x.Contains(".RateConst.csv"))).ToList();

            if (csvfilePaths.Count == 0)
            {
                MessageBox.Show("This directory doesn't contain the necessary files. Please select another directory.");
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

                    if (!File.Exists(files_txt_path)) { MessageBox.Show("filex.txt is not available in the specified directory.", "Error"); return; }
                    if (!File.Exists(quant_csv_path)) { MessageBox.Show(proteinName + ".Quant.csv" + " is not available in the specified directory.", "Error"); return; }
                    if (!File.Exists(RateConst_csv_path)) { MessageBox.Show(proteinName + ".RateConst.csv" + "filex.txt is not available in the specified directory.", "Error"); return; }

                    var proteinExperimentData = new ProteinExperimentDataReader(files_txt_path, quant_csv_path, RateConst_csv_path);


                    proteinExperimentData.loadAllExperimentData();
                    proteinExperimentData.computeRIAPerExperiment();
                    proteinExperimentData.mergeMultipleRIAPerDay2();
                    proteinExperimentData.computeExpectedCurvePoints();
                    proteinExperimentData.computeExpectedCurvePointsBasedOnExperimentalIo();
                    proteinExperimentData.computeRSquare();
                    ProtienchartDataValues chartdata = proteinExperimentData.computeValuesForEnhancedPerProtienPlot2();

                    // for each peptide draw the chart 
                    draw_peptideChart(proteinExperimentData, outputPath + "\\" + proteinName, chart_peptide);

                    //progressBar_exportall.Value = progressBar_exportall.Value + 1;


                }
                MessageBox.Show("done!!");
            }

        }

        private void draw_peptideChart(ProteinExperimentDataReader proteinExperimentData, string outputpath, Chart chart_peptide)
        {
            try
            {


                {

                    //copy chart1
                    System.IO.MemoryStream myStream = new System.IO.MemoryStream();
                    Chart chart2 = new Chart();
                    chart_peptide.Serializer.Save(myStream);
                    chart2.Serializer.Load(myStream);

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

                        chart2.ChartAreas[0].AxisY.Maximum = (double)(chart_data.Select(x => x.RIA_value).Max() + 0.1);

                        bool exists = System.IO.Directory.Exists(outputpath);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(outputpath);
                        try
                        {
                            using (Bitmap im = new Bitmap(chart2.Width, chart2.Height))
                            {
                                chart2.DrawToBitmap(im, new Rectangle(0, 0, chart2.Width, chart2.Height));

                                im.Save(outputpath + @"\" + count.ToString() + "_" + p.PeptideSeq + "_" + p.Charge.ToString() + ".jpeg");
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
