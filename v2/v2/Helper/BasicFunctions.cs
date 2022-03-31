using LBFGS_Library_Call;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public static unsafe void computation(List<double> chart_TimeCourseI0Isotope, List<int> chart_TimeCourseDates,
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


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


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
    }
}
