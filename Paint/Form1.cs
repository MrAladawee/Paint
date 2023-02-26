using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Paint
{
    public partial class Form1 : Form
    {
        private List<Figure> fig = new List<Figure>();
        private Painter pan;
        public Form1()
        {
            InitializeComponent();

            pan = new Painter(fig);
            // Set default form size and to draw on bitmap

            this.Width = 1089;
            this.Height = 728;
            //this.Width = 950;
            //this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;

        }

        // Initialization default options
        //

        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        Pen erase = new Pen(Color.White, 10);

        ColorDialog cd = new ColorDialog();
        Color new_color;

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if ((index == 3 || index == 4) && e.Button == MouseButtons.Left)
            {
                Figure? gf = null;

                if (index == 3)
                {
                    gf = new Rectan();

                }
                else if (index == 4)
                {
                    gf = new Circle();
                }

                if (gf != null)
                {
                    gf.AddPoint(e.Location);
                    gf.Color = new_color;
                    fig.Add(gf);
                }
            }
            // If user click on the pic canvas then set the paint bool value = true
            // and assign the click to pY

            paint = true;
            py = e.Location;
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {

            // Method for drawing if paint is true and index == N

            if (paint)
            {
                // Pencil

                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    py = px;
                }

                // Erase

                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(erase, px, py);
                    py = px;
                }

                // Circle or Rectangle

                if ((index == 3 || index == 4) && e.Button == MouseButtons.Left)
                {
                    fig[^1].AddPoint(e.Location);
                    pic.Refresh();
                }

                // Next u need to resolve problem with drawing ellipse, rectangle, line with indexs.
                // Use "Click" event on button and change index to select drawing mode.
            }

            pic.Refresh();

        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
           // If mouse is up => paint = false
           paint = false;
        }

        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            pan.Paint(e.Graphics);

            if (paint)
            {
                // Here u need to do method to draw the selected index
                // and display the current drawing positions
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

            // Method for clearing

            g.Clear(Color.White);
            fig.Clear();
            pic.Image = bm;
            index = 0;
        }

        private void btn_color_Click(object sender, EventArgs e)
        {
            // If color btn is pressed then open
            // color dialogbox and set the selected color
            // to the new_color, pen color and pic_color

            cd.ShowDialog();
            new_color = cd.Color;
            pic_color.BackColor = cd.Color;
            p.Color = cd.Color;
        }

        static Point set_point(PictureBox pb, Point pt)
        {

            // Method to set and return color palette image point/

            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            new_color = pic_color.BackColor;
            p.Color = pic_color.BackColor;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, pic.Width, pic.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Image Save Sucessfully.");
            }
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private void btn_rect_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        // Method to validate pixel old color before filling the shape to the new color
        private void validate(Bitmap bm, Stack<Point>sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        // Creating FloodFill function using validate method
        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point>pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x,y,new_clr);
            if (old_color == new_clr) return;

            // This method will get the old pixel color
            // and fill new Color from the clicked point till the stack count > 0
            // else if the old color is equal to new color then return

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X>0 && pt.Y>0 && pt.X<bm.Width-1 && pt.Y<bm.Height-1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }
        }

        int index;

    }
}
