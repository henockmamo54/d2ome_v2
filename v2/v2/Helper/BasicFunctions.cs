using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v2.Helper
{
    public class BasicFunctions
    {
        public static double computeRIA(double i0, double i1, double i2, double i3, double i4, double i5)
        {
            return i0 / (i0 + i1 + i2 + i3 + i4 + i5);
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
    }
}
