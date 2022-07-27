using LBFGS_Library_Call;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace v2.Helper
{
    public static class BasicFunctions
    {
        public static double computeRIA(double i0, double i1, double i2, double i3, double i4, double i5)
        {
            return i0 / (i0 + i1 + i2 + i3 + i4 + i5);
        }

        public static void NormalizeDataPoints(List<double> datapoints, double M0, double BWE, List<double> bweList, double NEH)
        {

            ////select proper io
            //double I0 = M0 / 100;
            //var zeroTimePoint = datapoints[0];
            //if (!double.IsNaN((double)zeroTimePoint))
            //{
            //    if (Math.Abs((double)zeroTimePoint - I0) < 0.1) { I0 = (double)zeroTimePoint; }
            //}

            //var IO_asymptote = I0 * (1 - (BWE / (1 - Helper.Constants.ph)) * NEH);

            //// normalize each time point
            //foreach (var d in datapoints)
            //{
            //    var BWE_t = filecontents.Where(x => x.time == d.Time).FirstOrDefault().BWE;
            //    var IO_t_asymptote = I0 * (1 - (BWE_t / (1 - Helper.Constants.ph)) * p.Exchangeable_Hydrogens);

            //    // compute the new value 
            //    var I0_t = (d.Time != 0) ? IO_asymptote + (d.RIA_value - IO_t_asymptote) / (I0 - IO_t_asymptote) * (I0 - IO_asymptote) : d.RIA_value;

            //    //update the value
            //    d.RIA_value = I0_t;
            //    normalizedRIAvalues.Add(d);
            //}
        }

        public static unsafe double computeRateConstant(List<double> chart_TimeCourseI0Isotope, List<int> chart_TimeCourseDates,
            float M0, double pw, double neh)
        {

            try
            {
                var scale = 60;

                float[] TimeCourseDates = chart_TimeCourseDates.Select(x => (float)(x)).ToArray();
                float[] TimeCourseI0Isotope = chart_TimeCourseI0Isotope.Select(x => (float)(x)).ToArray();


                var current_peptide_M0 = M0 / 100;
                var experiment_peptide_I0 = chart_TimeCourseI0Isotope[0];

                double selected_Io = 0;
                if (!double.IsNaN(experiment_peptide_I0))
                    selected_Io = (double)experiment_peptide_I0;
                else
                    selected_Io = (double)current_peptide_M0;

                if ((!double.IsNaN(experiment_peptide_I0)) && Math.Abs((double)current_peptide_M0 - (double)experiment_peptide_I0) > 0.1)
                    selected_Io = (double)current_peptide_M0;

                var I0_AtAsymptote = selected_Io * Math.Pow((1 - (pw / 1 - Helper.Constants.ph)), neh);
                float rkd, rks, fx1;


                float[] new_TimeCourseDates = TimeCourseDates.Select(x => x / scale).ToArray();
                fixed (float* ptr_TimeCourseDates = new_TimeCourseDates)
                fixed (float* ptr_TimeCourseI0Isotope = TimeCourseI0Isotope)
                {
                    LBFGS lbfgs = new LBFGS(ptr_TimeCourseDates, TimeCourseDates.Count(), 1, "One_Compartment_exponential");
                    lbfgs.InitializeTime();
                    var nret = lbfgs.Optimize(ptr_TimeCourseI0Isotope, (float)I0_AtAsymptote, (float)(selected_Io - I0_AtAsymptote), &rkd, &rks, &fx1);
                    double fDegradationConstant = Math.Exp(lbfgs.fParams[0]) / scale;

                    Console.WriteLine(" new rate constant " + nret.ToString() + "=======>" + fDegradationConstant.ToString());
                    if (nret == 0)
                        return fDegradationConstant;
                    else return double.NaN;

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return double.NaN;
        }

        public static double computeRate(List<int> TimeCourseDates, List<double> TimeCourseI0Isotope, double nature_Io,
           double pw, double neh)
        {
            double ph = 1.5574E-4;
            float rkd, rks, fx1;

            double previous_teta = 0;
            double teta = Math.Log(1E-5);
            double fit_error = 1;


            var current_peptide_M0 = nature_Io / 100;
            var experiment_peptide_I0 = TimeCourseI0Isotope[0];

            double selected_Io = 0;
            if (!double.IsNaN(experiment_peptide_I0))
                selected_Io = (double)experiment_peptide_I0;
            else
                selected_Io = (double)current_peptide_M0;

            if ((!double.IsNaN(experiment_peptide_I0)) && Math.Abs((double)current_peptide_M0 - (double)experiment_peptide_I0) > 0.1)
                selected_Io = (double)current_peptide_M0;

            var I0_AtAsymptote = selected_Io * Math.Pow((1 - (pw / 1 - Helper.Constants.ph)), neh);



            while (Math.Abs(teta - previous_teta) > 1E-8)
            {
                float k = (float)Math.Exp(teta);

                // compute derivatives for each time point
                List<double> y = new List<double>();
                foreach (var t in TimeCourseDates)
                {
                    double temp = (double)((nature_Io - I0_AtAsymptote) * Math.Exp(-k * t) * (-t) * k);
                    y.Add(temp);
                }

                //comute del y
                List<double> del_y = new List<double>();
                for (int i = 0; i < TimeCourseI0Isotope.Count; i++)
                {
                    var fit_value = I0_AtAsymptote + (nature_Io - I0_AtAsymptote) * Math.Exp(-k * TimeCourseDates[i]);
                    del_y.Add((double)(TimeCourseI0Isotope[i] - fit_value));
                }

                // compute yT*y
                double val_1 = y.Select(x => x * x).ToList().Sum();

                //compute yT*del_y
                double val_2 = 0;
                for (int i = 0; i < del_y.Count; i++)
                {
                    val_2 = val_2 + (del_y[i] * y[i]);
                }

                double del_teta = val_2 / val_1;
                previous_teta = teta;
                teta = teta + 0.001 * del_teta;



            }

            return Math.Exp(teta);

        }

        public static double computeRsquared(List<double> experimentalValue, List<double> fitvalue)
        {
            var mean_exp = experimentalValue.Where(x => !double.IsNaN(x)).Average();
            double rss = 0;
            double ss = 0;
            double rsquared = double.NaN;

            for (int i = 0; i < experimentalValue.Count; i++)
            {
                if (!double.IsNaN(experimentalValue[i]))
                {
                    ss = ss + Math.Pow((double)(experimentalValue[i] - mean_exp), 2);
                    rss = rss + Math.Pow((double)(experimentalValue[i] - fitvalue[i]), 2);
                }
            }

            //if (r.Rateconst > 0.0006) RSquare = 1 - (rss / ss);
            //else RSquare = 1 - (diff);

            if (ss != 0)
                rsquared = 1 - (rss / ss);

            return rsquared;
        }
        public static double getMedian(List<double> sourceNumbers)
        {
            //Framework 2.0 version of this method. there is an easier way in F4        
            if (sourceNumbers == null || sourceNumbers.Count == 0)
                return double.NaN;
            //throw new System.Exception("Median of empty array not defined.");

            //make sure the list is sorted, but use a new array
            double[] sortedPNumbers = sourceNumbers.OrderBy(x => x).ToArray();

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;
            return median;
        }
        public static double getStandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        public static string formatdoubletothreedecimalplace(double n)
        {
            var tempval = "";
            if (n > 0.09999999) tempval = String.Format("{0:0.###}", n);
            else if (n > 0.00999999) tempval = String.Format("{0:0.####}", n);
            else if (n > 0.00099999) tempval = String.Format("{0:0.#####}", n);
            else if (n > 0.00009999) tempval = String.Format("{0:0.######}", n);
            else tempval = String.Format("{0:0.######}", n);

            return tempval;
        }
        public static List<List<double>> GetBestPath(float[,] inputdata)
        {
            int rows = inputdata.GetLength(0);
            int columns = inputdata.GetLength(1);
            List<List<double>> best_path = new List<List<double>>();
            for (int i = 0; i < rows; i++)
            {


                var temp_path_i = new List<double>();
                temp_path_i.Add(inputdata[i, 0]);
                //temp_path_i.Add(0);

                for (int j = 1; j < columns; j++)
                {
                    if (inputdata[i, j] == 0)
                    {
                        temp_path_i.Add(double.NaN);
                        continue;
                    }

                    var previous_dataPoint = temp_path_i[temp_path_i.Count - 1];
                    int counter = 1;
                    while (double.IsNaN(previous_dataPoint))
                    {
                        previous_dataPoint = temp_path_i[temp_path_i.Count - 1 - counter];
                        counter++;
                    }



                    // check montonosity. if the next data point is greater than the current value, choose it as the next path
                    if (inputdata[i, j] < 1.2 && inputdata[i, j] > 0 && inputdata[i, j] > previous_dataPoint)
                    {
                        temp_path_i.Add(inputdata[i, j]);
                    }
                    else
                    {
                        var min_value = 1E6;
                        float next_value = (float)1E6;

                        for (int k = 0; k < rows; k++)
                        {
                            if (inputdata[k, j] < 1.2 && inputdata[k, j] > 0 && inputdata[k, j] >= previous_dataPoint && ((inputdata[k, j] - previous_dataPoint) <= min_value))
                            {
                                min_value = inputdata[k, j] - previous_dataPoint;
                                next_value = inputdata[k, j];
                            }

                        }

                        if (min_value == 1E6)
                        {
                            for (int k = 0; k < rows; k++)
                            {
                                if (inputdata[k, j] < 1.2 && inputdata[k, j] > 0 && Math.Abs(inputdata[k, j] - previous_dataPoint) <= min_value)
                                {
                                    min_value = Math.Abs(inputdata[k, j] - previous_dataPoint);
                                    next_value = inputdata[k, j];
                                }
                            }

                        }

                        if (min_value == 1E6)
                            temp_path_i.Add(double.NaN);
                        else
                            temp_path_i.Add(next_value);


                    }
                }
                best_path.Add(temp_path_i);
            }
            return best_path;
        }

        public static double RMSE(List<double> selected_points, List<double> theoretical_points)
        {
            double rss = 0;
            //selected_points, theoretical_RIA
            for (int i = 0; i < selected_points.Count(); i++)
            {
                if (!double.IsNaN((double)(selected_points[i])))
                {
                    rss = rss + Math.Pow((double)(selected_points[i] - theoretical_points[i]), 2);
                }
            }
            var rmse = Math.Sqrt(rss / selected_points.Count());
            return rmse;
        }

        public static double sigma(List<double> selected_points, List<double> theoretical_points, Model.Peptide currentPeptide,
            double bwe, List<int> experimentTime)
        {
            double rss = 0;
            double dn = 0;
            double IO_assymptot = (double)(currentPeptide.M0 / 100 * Math.Pow((1 - (bwe / (1 - Constants.ph))), (double)currentPeptide.Exchangeable_Hydrogens));
            var meanval_ria = selected_points.Where(x => !double.IsNaN((double)x)).Average(x => x);

            for (int i = 0; i < selected_points.Count(); i++)
            {
                if (!double.IsNaN((double)selected_points[i]))
                {
                    var computedRIAValue = theoretical_points[i];
                    rss = rss + Math.Pow((double)(selected_points[i] - computedRIAValue), 2);
                    dn = dn + Math.Pow(experimentTime[i], 2) * Math.Pow((double)(computedRIAValue - IO_assymptot), 2);
                }
            }

            var var_k = dn == 0 ? double.NaN : (rss / selected_points.Where(x => !double.IsNaN((double)x)).Count()) / (dn);
            var Sigma = Math.Sqrt(var_k);

            return Sigma;

        }

        public static void CreateCSV<T>(List<T> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

    }
}
