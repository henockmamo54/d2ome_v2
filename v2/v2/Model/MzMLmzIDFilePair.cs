using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class MzMLmzIDFilePair
    {
        public string MzML_FileName { get; set; }
        public string MzID_FileName { get; set; }
        public double Time { get; set; }
        public double BWE { get; set; }
        public MzMLmzIDFilePair() { }
        public MzMLmzIDFilePair(double time, string mzml, string mzid, double bwe)
        {
            this.Time = time;
            this.MzML_FileName = mzml;
            this.MzID_FileName = mzid;
            this.BWE = bwe;
        }
    }
}
