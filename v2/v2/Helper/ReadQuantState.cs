using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace v2.Helper
{
    public class ReadQuantState
    {
        string path = "";

        public ReadQuantState(string path) { this.path = path; }

        public string getLabelingDuration()
        {

            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    /*
                        *** structure of quant state file
                        0 mass_accuracy =  20.0 ppm  // mass accuracy: either in ppm or Da 
                        1 MS1_Type = 1	// data type of MS1, 1 - centroid, 0 - profile  
                        2 protein_score       = 10.0     //minimum protein score
                        3 peptide_score =  10.0 	// minimum peptide score, ion score in Mascot, default is 1
                        4 peptide_expectation = 0.05     // maximum peptide expectation in Mascot
                        5 elutiontimewindow   =   1  // time window  (mins) to search for elution peak. From the time that highest scoring MS2 was triggered
                        6 protein_consistency = 4  // minimum number of experiments for protein consistency
                        7 peptide_consistency = 2   //mininum number of experiments for a peptide consistency
                        8 NParam_RateConst_Fit = 1	// The model for fitting rate constant. Values are 1, and 2
                        9 Labeling_time_unit = Days  // 1 = Days, 0.0416666666666667 = Hours (1/24)
                     */

                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    if (lines.Length > 9)
                    {
                        try
                        {
                            return (lines[9].Split('/')[0]).Split('=')[1].Trim();
                        }
                        catch { return "Labeling Duration"; }
                    }
                    else
                    {
                        return "Labeling Duration";
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("error ==>" + e.Message);
                    MessageBox.Show("Error reading quant.state ==> " + e.Message);
                    return "Labeling Duration";
                }

            }
            else
            {
                Console.WriteLine("***> file not found");
                MessageBox.Show("Error reading quant.state ==> File Not found!!");
                return "Labeling Duration";
            }

        }


    }
}
