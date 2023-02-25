using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Painter
    {
        private List<Figure> figures;
        
        public Painter(List<Figure> f)
        {
            figures = f;
        }
        
        public void Paint(Graphics g)
        {
            foreach (var r in figures)
            {
                r.Paint(g);
            }
        }
    }
}
