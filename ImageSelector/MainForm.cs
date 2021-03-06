﻿using ImageSelector.Core;
using ImageSelector.Infrastructure;
using ImageSelector.ViewModels;
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
        int _zoomFactor = 0;
        Size originalSize = new Size();
        Dictionary<SelectionComponentType, Cursor> _cursors;
        Cursor _activeCursor = Cursors.Default;

        private readonly MainFormViewModel _model;

        public MainForm()
        {
            InitializeComponent();

            _model = new MainFormViewModel(new SelectionManager());
            _model.StateChange += Model_StateChange;

            _cursors = new Dictionary<SelectionComponentType, Cursor>();
            _cursors.Add(SelectionComponentType.BottomEdge, Cursors.SizeNS);
            _cursors.Add(SelectionComponentType.LeftEdge, Cursors.SizeWE);
            _cursors.Add(SelectionComponentType.NECorner, Cursors.SizeNESW);
            _cursors.Add(SelectionComponentType.NWCorner, Cursors.SizeNWSE);
            _cursors.Add(SelectionComponentType.RightEdge, Cursors.SizeWE);
            _cursors.Add(SelectionComponentType.SECorner, Cursors.SizeNWSE);
            _cursors.Add(SelectionComponentType.SWCorner, Cursors.SizeNESW);
            _cursors.Add(SelectionComponentType.TopEdge, Cursors.SizeNS);
            _cursors.Add(SelectionComponentType.Inside, Cursors.SizeAll);            
        }

        private void InitViewer(ImageInfo imageInfo)
        {
            // Load image from file in solution folder.
            _image = imageInfo;
            
            pictureBox.Size = originalSize = _image.Size;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Image = _image.Image;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            
            overlayBox.SizeMode = PictureBoxSizeMode.Zoom;
            overlayBox.Parent = pictureBox;
            overlayBox.BackColor = Color.Transparent;
            overlayBox.Location = pictureBox.Location;
        }

        private void Model_StateChange(object sender, CustomEventArgs<MainFormStatus> e)
        {
            switch (e.Data)
            {
                case MainFormStatus.Idle:
                    Cursor.Current = Cursors.Default;
                    break;
                case MainFormStatus.Selecting:
                    Cursor.Current = Cursors.Default;
                    break;
                case MainFormStatus.ResizingSelection:
                case MainFormStatus.OverEdge:
                case MainFormStatus.OverSelection:
                    Cursor.Current = _cursors[_model.ComponentAtCursorPoint.SelectionComponentType];
                    break;
                default:
                    break;
            }

            _activeCursor = Cursor.Current;

            // Show active selection in the property grid.
            selectionProperties.SelectedObject = _model.ActiveSelection;

            overlayBox.Invalidate();
        }

        private void overlayBox_MouseDown(object sender, MouseEventArgs e)
        {
            // If active selection not null and right button clicked - show context menu.
            if (e.Button == MouseButtons.Right && _model.ActiveSelection != null)
            {
                // Show context menu.
                contextMenuStrip1.Show(Cursor.Position);
            }
            
            // Translate coordinates, if image is zoomed in/out.
            _model.MouseDown(TranslateCursorLocation(), e.Button);
        }


        private void overlayBox_MouseUp(object sender, MouseEventArgs e)
        {            
            _model.MouseUp(TranslateCursorLocation(), e.Button);
        }
        
        private void PictureBox_LocationChanged(object sender, System.EventArgs e)
        {            
            //overlayBox.Location = pictureBox.Location;            
        }

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            overlayBox.Size = pictureBox.Size;
        }

        private void overlayBox_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = _activeCursor;

            _model.MouseMove(TranslateCursorLocation());
            overlayBox.Invalidate();                            
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

            if (_zoomFactor < 9)
            {
                _zoomFactor++;
                pictureBox.Size = new Size((int)(originalSize.Width * (1 + (0.1 * _zoomFactor))), (int)(originalSize.Height * (1 + (0.1 * _zoomFactor))));
            }

            btnZoomOut.Enabled = _zoomFactor > -9;
            btnZoomIn.Enabled = _zoomFactor < 9;

            pictureBox.Location = location;

            pictureBox.ResumeLayout();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            pictureBox.SuspendLayout();

            Point location = pictureBox.Location;

            if (_zoomFactor > -9)
            {
                _zoomFactor--;
                pictureBox.Size = new Size((int)(originalSize.Width * (1 + (0.1 * _zoomFactor))), (int)(originalSize.Height * (1 + (0.1 * _zoomFactor))));
            }

            btnZoomOut.Enabled = _zoomFactor > -9;
            btnZoomIn.Enabled = _zoomFactor < 9;

            pictureBox.Location = location;

            pictureBox.ResumeLayout();
        }

        private void overlayBox_Paint(object sender, PaintEventArgs e)
        {
            // If there is an image and it has a location, 
            // paint it when the Form is repainted.
            base.OnPaint(e);
            foreach (var s in _model.Selections)
            {
                // This has to take into account the fact that image can be resized...
                Rectangle scaledRectagle = ScaleRectangle(s.LocationAndSize);
                Pen pen = Pens.Red;
                if (_model.ActiveSelection != null && _model.ActiveSelection == s)
                {
                    pen = new Pen(Brushes.White, 2.0f);
                }
                e.Graphics.DrawRectangle(pen, scaledRectagle);
            }

            // This is the temp rectangle that is being drawn.
            if(_model.ActiveSelection != null)
                e.Graphics.DrawRectangle(Pens.Red, ScaleRectangle(_model.ActiveSelection.LocationAndSize));
        }

        private Rectangle ScaleRectangle(Rectangle r)
        {
            return new Rectangle(
                                (int)(r.Location.X * (1 + _zoomFactor * (0.1))),
                                (int)(r.Location.Y * (1 + _zoomFactor * (0.1))),
                                (int)(r.Width * (1 + _zoomFactor * (0.1))),
                                (int)(r.Height * (1 + _zoomFactor * (0.1))));
        }

        private Rectangle DescaleRectangle(Rectangle r)
        {
            return new Rectangle(
                                (int)(r.Location.X / (1 + _zoomFactor * (0.1))),
                                (int)(r.Location.Y / (1 + _zoomFactor * (0.1))),
                                (int)(r.Width / (1 + _zoomFactor * (0.1))),
                                (int)(r.Height / (1 + _zoomFactor * (0.1))));
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // Move image up by 5% of its size.
            pictureBox.Top -= pictureBox.Height / 20;
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

        private Point TranslateCursorLocation()
        {
            Point cursorPosition = overlayBox.PointToClient(Cursor.Position);
            Point location = new Point((int)(cursorPosition.X / (1 + _zoomFactor * (0.1))),
                (int)(cursorPosition.Y / (1 + _zoomFactor * (0.1))));
            return location;
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            // Show the tag screen.
            // Collect the text.
            // Cut images from master and save as individual files with naming convention:
            // {Batch_Guid}_{tag}_{seq-no}.jpg
            // Show modal windows to collect tags.
            using (Tags t = new Tags())
            {
                t.Save += Tags_Save;
                t.ShowDialog(this);
            }
        }

        private void Tags_Save(object sender, Infrastructure.CustomEventArgs<string> e)
        {
            _model.ExtractImagesWithTag(e.Data);            
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // Open FileOpenDialog - filter for jpg.
            // On close - delegate to the view model to load the image.
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "JPEG images|*.jpg";
            d.FileOk += OpenFileDialog_FileOk;
            d.ShowDialog();            
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog d = sender as OpenFileDialog;

            if (d == null)
                return;

            InitViewer(_model.LoadImage(d.FileName));
        }

        private void DeleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _model.DeleteActiveSelection();
            
        }
    }
}
