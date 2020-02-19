using ImageSelector.Core;
using ImageSelector.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSelector
{
    public partial class MainForm : Form
    {
        ImageInfo _image;
        ImageInfo _seeThrough;
        bool _drawing = false;
        Point _actionStart = new Point(0,0);
        int zoomFactor = 0;
        Size originalSize = new Size();
        List<Selection> _selections;
        Selection _activeSelection = null;
        Dictionary<SelectionComponentType, Cursor> _cursors;

        public MainForm()
        {
            InitializeComponent();

            _selections = new List<Selection>();
            _cursors = new Dictionary<SelectionComponentType, Cursor>();
            _cursors.Add(SelectionComponentType.BottomEdge, Cursors.SizeNS);
            _cursors.Add(SelectionComponentType.LeftEdge, Cursors.SizeWE);
            _cursors.Add(SelectionComponentType.NECorner, Cursors.SizeNESW);
            _cursors.Add(SelectionComponentType.NWCorner, Cursors.SizeNWSE);
            _cursors.Add(SelectionComponentType.RightEdge, Cursors.SizeWE);
            _cursors.Add(SelectionComponentType.SECorner, Cursors.SizeNWSE);
            _cursors.Add(SelectionComponentType.SWCorner, Cursors.SizeNESW);
            _cursors.Add(SelectionComponentType.TopEdge, Cursors.SizeNS);

            // Load image from file in solution folder.
            _image = new ImageInfo(".\\Images\\sample1.jpg");
            _seeThrough = new ImageInfo(".\\Images\\GreenBox1px.png");

            // Display image in form.
            pictureBox.Size = originalSize = _image.Size;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Image = _image.Image;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Hook up events in viewer.
            overlayBox.SizeMode = PictureBoxSizeMode.Zoom;
            overlayBox.Parent = pictureBox;
            overlayBox.BackColor = Color.Transparent;
            overlayBox.Location = pictureBox.Location;

            // add rectangle the size of overlaybox... for debug only
            //_selections.Add(new Rectangle(overlayBox.Location, overlayBox.Size));
            
            cbxSelections.DisplayMember = "Description";
        }

        private void overlayBox_MouseDown(object sender, MouseEventArgs e)
        {
            // set starting point for a rectangle.
            if (e.Button == MouseButtons.Left)
                _drawing = true;
            
            _actionStart = overlayBox.PointToClient(Cursor.Position);
        }

        private void overlayBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _drawing)
            {
                _drawing = false;
                var actionEnd = overlayBox.PointToClient(Cursor.Position);
                var r = CreateRectangleFromPoints(_actionStart, actionEnd);
                overlayBox.CreateGraphics().DrawRectangle(Pens.Red, r);

                if (r.Width > 0 & r.Height > 0)
                {
                    Selection s = null;
                    if (zoomFactor == 0)
                        s = new Selection (r, 0);
                    else
                        s = new Selection(DescaleRectangle(r), 0);

                    _selections.Add(s);
                    cbxSelections.Items.Add(s);
                }
            }

            _temp = null;
        }

        private Rectangle CreateRectangleFromPoints(Point p1, Point p2)
        {
            var result = new Rectangle();

            Point viewerLocation = overlayBox.Location;

            Point topLeft = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            Point bottomRight = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));

            result.Location = topLeft;
            result.Size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

            return result;
        }

        //private void btnExtract_Click(object sender, EventArgs e)
        //{            
        //    // Show modal windows to collect tags.
        //    using (Tags t = new Tags())
        //    {
        //        t.Save += Tags_Save;
        //        t.ShowDialog(this);
        //    }            
        //}

        //private void Tags_Save(object sender, Infrastructure.CustomEventArgs<string> e)
        //{
        //    // Clone section of the original image in given location/rectangle.
        //    Bitmap selected = _image.Image.Clone(_selection, _image.Image.PixelFormat);

        //    // Save copy of the section to a new file.            
        //    selected.Save(".\\Images\\selection.jpg", ImageFormat.Jpeg);
        //}

        private void PictureBox_LocationChanged(object sender, System.EventArgs e)
        {            
            //overlayBox.Location = pictureBox.Location;            
        }

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            overlayBox.Size = pictureBox.Size;
            //overlayBox.Location = pictureBox.Location;
        }

        private Nullable<Rectangle> _temp;
        private void overlayBox_MouseMove(object sender, MouseEventArgs e)
        {
            var actionEnd = overlayBox.PointToClient(Cursor.Position);

            if (_drawing)
            {
                overlayBox.Invalidate();
                
                _temp = CreateRectangleFromPoints(_actionStart, actionEnd);
            } 
            else
            {
                foreach (var s in _selections)
                {
                    var target = s.GetSelectionAtPoint(actionEnd, 2);
                    if(target != null)
                        Cursor.Current = _cursors[target.SelectionComponentType];
                }                
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                // Maximized!
                if (pictureBox.Top > 0)
                    pictureBox.Top = 0;
                if (pictureBox.Left > 0)
                    pictureBox.Left = 0;
            }
        }        

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            pictureBox.SuspendLayout();

            Point location = pictureBox.Location;

            if (zoomFactor < 9)
            {
                zoomFactor++;
                pictureBox.Size = new Size((int)(originalSize.Width * (1 + (0.1 * zoomFactor))), (int)(originalSize.Height * (1 + (0.1 * zoomFactor))));
            }

            btnZoomOut.Enabled = zoomFactor > -9;
            btnZoomIn.Enabled = zoomFactor < 9;

            pictureBox.Location = location;

            pictureBox.ResumeLayout();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            pictureBox.SuspendLayout();

            Point location = pictureBox.Location;

            if (zoomFactor > -9)
            {
                zoomFactor--;
                pictureBox.Size = new Size((int)(originalSize.Width * (1 + (0.1 * zoomFactor))), (int)(originalSize.Height * (1 + (0.1 * zoomFactor))));
            }

            btnZoomOut.Enabled = zoomFactor > -9;
            btnZoomIn.Enabled = zoomFactor < 9;

            pictureBox.Location = location;

            pictureBox.ResumeLayout();
        }

        private void overlayBox_Paint(object sender, PaintEventArgs e)
        {
            // If there is an image and it has a location, 
            // paint it when the Form is repainted.
            base.OnPaint(e);
            foreach (var s in _selections)
            {
                // This has to take into account the fact that image can be resized...
                Rectangle scaledRectagle = ScaleRectangle(s.LocationAndSize);
                Pen pen = Pens.Red;
                if (_activeSelection != null && _activeSelection == s)
                {
                    pen = new Pen(Brushes.White, 2.0f);
                }
                e.Graphics.DrawRectangle(pen, scaledRectagle);
            }

            // This is the temp rectangle that is being drawn.
            if (_temp != null)
                e.Graphics.DrawRectangle(Pens.Red, _temp.Value);
        }

        private Rectangle ScaleRectangle(Rectangle r)
        {
            return new Rectangle(
                                (int)(r.Location.X * (1 + zoomFactor * (0.1))),
                                (int)(r.Location.Y * (1 + zoomFactor * (0.1))),
                                (int)(r.Width * (1 + zoomFactor * (0.1))),
                                (int)(r.Height * (1 + zoomFactor * (0.1))));
        }

        private Rectangle DescaleRectangle(Rectangle r)
        {
            return new Rectangle(
                                (int)(r.Location.X / (1 + zoomFactor * (0.1))),
                                (int)(r.Location.Y / (1 + zoomFactor * (0.1))),
                                (int)(r.Width / (1 + zoomFactor * (0.1))),
                                (int)(r.Height / (1 + zoomFactor * (0.1))));
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // Move image up by 5% of its size.
            pictureBox.Top -= pictureBox.Height / 20;
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            // Move image left by 5% of its size.
            pictureBox.SuspendLayout();
            pictureBox.Left -= pictureBox.Width / 20;
            pictureBox.ResumeLayout();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // Move image right by 5% of its size.
            pictureBox.Left += pictureBox.Width / 20;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            // Move image down by 5% of its size.
            pictureBox.Top += pictureBox.Height / 20;
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            // Move image right by 10% of its size.
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void cbxSelections_DataSourceChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Data source changed");
        }

        private void cbxSelections_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionProperties.SelectedObject = cbxSelections.SelectedItem;
            _activeSelection = cbxSelections.SelectedItem as Selection;
            overlayBox.Invalidate();
        }
    }
}
