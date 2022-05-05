using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace v2.Helper
{
    public class ReadFilesInfo_txt
    {
        public string path = @"";
        public List<int> experimentTimes = new List<int>(); //contains unique time values
        public List<int> experimentTimes_all = new List<int>(); //contains All time values for each experiment
        public List<string> experimentIDs = new List<string>();
        public List<FileContent> filecontents = new List<FileContent>();

        public ReadFilesInfo_txt(string path)
        {
            this.path = path;
            experimentTimes = new List<int>();
            experimentIDs = new List<string>();
            filecontents = new List<FileContent>();
        }
        public void readFile()
        {
            readFile(this.path);
        }
        public void readFile(string path)
        {
            //Extracts the information from files.txt

            //check if the file exists
            //string path = this.path;

            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    foreach (string line in lines)
                    {
                        // remove all extra spaces from the text file.
                        // the assumption is that the text file is one space separted to indicate each column
                        var temp = line.Trim();
                        temp = Regex.Replace(temp, @"\s+", " ");
                        var rowvalues = temp.Split(' ');

                        FileContent fc = new FileContent(int.Parse(rowvalues[0]), rowvalues[1], rowvalues[2], double.Parse(rowvalues[3]), "");


                        // get the time
                        experimentTimes_all.Add(fc.time);

                        // extract experiment id
                        var temp_id = fc.mzid_path.Trim().Split('\\');
                        var e_id = temp_id[temp_id.Length - 1].Replace(".mzid", string.Empty).Replace(".mzID", string.Empty); 
                        experimentIDs.Add(e_id);

                        fc.experimentID=e_id;
                        filecontents.Add(fc);
                    }

                    //extract distict time list 
                    experimentTimes = experimentTimes_all.Distinct().ToList(); 

                }
                catch (Exception e)
                {
                    Console.WriteLine("error ==>" + e.Message);
                    MessageBox.Show("Error reading files.txt ==> " + e.Message);
                }

            }
            else
            {
                Console.WriteLine("***> file not found");
                MessageBox.Show("Error reading files.txt ==> File Not found!!");
            }

        }

        public struct FileContent
        {
            public int time;
            public string mzML_path;
            public string mzid_path;
            public double BWE;
            public string experimentID;

            public FileContent(int time, string mzml, string mzid, double val, string experimentID)
            {
                this.time = time;
                this.mzML_path = mzml;
                this.mzid_path = mzid;
                this.BWE = val;
                this.experimentID = experimentID;
            }
        }
    }
}
