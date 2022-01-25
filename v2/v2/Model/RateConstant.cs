using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class RateConstant
    { 
        public string PeptideSeq { get; set; }
        public double? RateConstant_value { get; set; }
        public double? Correlations { get; set; }
        public double? RootMeanRSS { get; set; }
        public double? AbsoluteIsotopeError { get; set; }

        public int order { get; set; }

    }
}
