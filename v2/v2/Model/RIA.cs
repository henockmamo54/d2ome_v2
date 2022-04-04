using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class RIA
    {
        public string ExperimentName { get; set; }
        public List<string> ExperimentNames { get; set; }
        public string PeptideSeq { get; set; }
        public double? Charge { get; set; }
        public int Time { get; set; }
        public double? I0 { get; set; }
        public double? IonScore { get; set; }
        public double? RIA_value { get; set; }
        public double? I0_t_fromA1A0 { get; set; }
        public double? I0_t_fromA2A0 { get; set; }
        public double? I0_t_fromA2A1 { get; set; }
        //public double? I0_t_fromA1A0_pxt { get; set; }
        //public double? I0_t_fromA2A0_pxt { get; set; }
        //public double? I0_t_fromA2A1_pxt { get; set; }

    }
}
