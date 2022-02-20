using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace v2.Helper
{
    public class CustomTooltip: System.Windows.Forms.ToolTip
    {
        public CustomTooltip() {
            this.BackColor = Color.White;
            ForeColor = Color.Black;
            OwnerDraw = true;
            Draw += cu_Draw;
        }

        private void cu_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
            e.Graphics.DrawLine(Pens.LimeGreen, 0, e.Bounds.Height - 4, e.Bounds.Width, e.Bounds.Height - 4);
        }
    }
}
