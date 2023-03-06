using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Paint
{
    public abstract class Figure
    {
        public float penSize = 1f;
        private int? _x1 = null, _x2 = null, _y1 = null, _y2 = null;
        public int X => _x1 != null && _x2 != null ? Math.Min(_x1 ?? 0, _x2 ?? 0) : -1;
        public int Y => _y1 != null && _y2 != null ? Math.Min(_y1 ?? 0, _y2 ?? 0) : -1;
        public int Width => _x1 != null && _x2 != null ? Math.Abs((_x1 ?? 0) - (_x2 ?? 0)) : 1;
        public int Height => _y1 != null && _y2 != null ? Math.Abs((_y1 ?? 0) - (_y2 ?? 0)) : 1;
        public Color Color { get; set; }

        public Rectangle Rectangle => new Rectangle(X, Y, Width, Height);

        public abstract void Paint(Graphics g);

        public void AddPoint(Point p)
        {
            if (_x1 == null)
            {
                _x1 = p.X;
                _y1 = p.Y;
            }
            else
            {
                if (p.X == _x1 && p.Y == _y1)
                {
                    _x2 = null;
                    _y2 = null;
                }
                else
                {
                    _x2 = p.X;
                    _y2 = p.Y;
                }
            }
        }
    }
}
