using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class ExperimentRecord
    {
        public string ExperimentName { get; set; }
        public string PeptideSeq { get; set; }
        public double? Charge { get; set; }
        public double SpecMass { get; set; }
        public double? IonScore { get; set; }
        public double? Expectn { get; set; }
        public double? Error { get; set; }
        public double? Scan { get; set; }
        public double? I0 { get; set; }
        public double? I1 { get; set; }
        public double? I2 { get; set; }
        public double? I3 { get; set; }
        public double? I4 { get; set; }
        public double? I5 { get; set; }
        public double? Start_Elution { get; set; }
        public double? End_Elution { get; set; }
        public double? I0_Peak_Width { get; set; }
        public double? Total_Labeling { get; set; }
        public double? Net_Labeling { get; set; }
        public double? Deuteriumenrichment { get; set; } //pX(t)
        public double? ExperimentTime { get; set; }
        public double? I0_t { get; set; }

    }
}
