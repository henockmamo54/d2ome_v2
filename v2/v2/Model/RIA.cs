using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Model
{
    public class RIA
    {
        public string experimentName { get; set; }
        public List<string> experimentNames { get; set; }
        public string peptideSeq { get; set; }
        public double? charge { get; set; }
        public int time { get; set; }
        public double? I0 { get; set; }
        public double? RIA_value { get; set; }
    }
}
