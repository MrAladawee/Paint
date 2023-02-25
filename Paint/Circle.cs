using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Paint
{
    public class Circle : Figure
    {
        public override void Paint(Graphics g)
        {
            try
            {
                var p = new Pen(Color);
                g.DrawEllipse(p, Rectangle);
            }
            catch
            {

            }

        }
    }
}
