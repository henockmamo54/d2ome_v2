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
