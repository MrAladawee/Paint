using System.Drawing.Imaging;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // Set default form size and to draw on bitmap

            this.Width = 950;
            this.Height = 700;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;

        }

        // Initialization default options

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
            Graphics g = e.Graphics;

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

        // Othet methods (fill)
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

        int index;
    }
}