using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class Peptide
    {
        public string PeptideSeq { get; set; }
        public bool? UniqueToProtein { get; set; }
        public double? Rateconst { get; set; }
        public double? RSquare { get; set; }
        public double? Charge { get; set; }
        public double? SeqMass { get; set; }
        public double? Exchangeable_Hydrogens { get; set; }
        public double? IsotopeDeviation { get; set; }
        public double? M0 { get; set; }
        public double? M1 { get; set; }
        public double? M2 { get; set; }
        public double? M3 { get; set; }
        public double? M4 { get; set; }
        public double? M5 { get; set; }
        public double? Total_Labeling { get; set; }
        public double? RMSE_value { get; set; }
        public int Order { get; set; }
        public double? Abundance { get; set; }
        public double? NDP { get; set; }
        public double? std_k { get; set; }

    }
}
