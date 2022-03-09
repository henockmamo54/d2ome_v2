using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get
{
    public class Program
    {
        public void Main(string[] args)
        {

        }

        public static void GetBestPath(float[,] inputdata)
        {
            int rows = inputdata.GetLength(0);
            int columns = inputdata.GetLength(1);
            List<List<float>> best_path = new List<List<float>>();
            for (int i = 0; i < rows; i++)
            {
                var temp_path_i = new List<float>();
                //temp_path_i.Add(inputdata[i, 0]);
                temp_path_i.Add(0);

                for (int j = 1; j < columns; j++)
                {
                    // check montonosity. if the next data point is greater than the current value, choose it as the next path
                    if (inputdata[i, j] > inputdata[i, j - 1])
                    {
                        temp_path_i.Add(inputdata[i, j]);
                    }
                    else
                    {
                        var min_value = 1E6;
                        float next_value = (float)1E6;
                        for (int k = 1; k < rows; k++)
                        {
                            if (inputdata[k, j] > inputdata[i, j - 1] && (inputdata[k, j] - inputdata[i, j - 1] < min_value))
                            {
                                min_value = inputdata[k, j] - inputdata[i, j - 1];
                                next_value = inputdata[k, j];
                            }
                            else if (inputdata[k, j] < inputdata[i, j - 1] && min_value < 0 && (inputdata[k, j] - inputdata[i, j - 1] > min_value))
                            {
                                min_value = inputdata[k, j] - inputdata[i, j - 1];
                                next_value = inputdata[k, j];
                            }
                        }
                        if (min_value != 1E6) temp_path_i.Add(next_value);
                        else temp_path_i.Add(temp_path_i[temp_path_i.Count() - 1]);

                    }
                }
                best_path.Add(temp_path_i);
            }
        }
    }
}
