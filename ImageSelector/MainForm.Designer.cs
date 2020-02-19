namespace ImageSelector
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelPicture = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.imageProperties = new System.Windows.Forms.Panel();
            this.cbxSelections = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.selectionProperties = new System.Windows.Forms.PropertyGrid();
            this.overlayBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.imageProperties.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelPicture);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageProperties);
            this.splitContainer1.Size = new System.Drawing.Size(1154, 699);
            this.splitContainer1.SplitterDistance = 657;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelPicture
            // 
            this.panelPicture.AutoScroll = true;
            this.panelPicture.Controls.Add(this.pictureBox);
            this.panelPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPicture.Location = new System.Drawing.Point(0, 0);
            this.panelPicture.Name = "panelPicture";
            this.panelPicture.Size = new System.Drawing.Size(657, 699);
            this.panelPicture.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox.Location = new System.Drawing.Point(47, 124);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1, 1);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.LocationChanged += new System.EventHandler(this.PictureBox_LocationChanged);
            this.pictureBox.SizeChanged += new System.EventHandler(this.pictureBox_SizeChanged);
            // 
            // imageProperties
            // 
            this.imageProperties.Controls.Add(this.cbxSelections);
            this.imageProperties.Controls.Add(this.panel1);
            this.imageProperties.Controls.Add(this.selectionProperties);
            this.imageProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageProperties.Location = new System.Drawing.Point(0, 0);
            this.imageProperties.Name = "imageProperties";
            this.imageProperties.Size = new System.Drawing.Size(487, 699);
            this.imageProperties.TabIndex = 0;
            // 
            // cbxSelections
            // 
            this.cbxSelections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSelections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSelections.FormattingEnabled = true;
            this.cbxSelections.Location = new System.Drawing.Point(14, 12);
            this.cbxSelections.Name = "cbxSelections";
            this.cbxSelections.Size = new System.Drawing.Size(454, 21);
            this.cbxSelections.TabIndex = 3;
            this.cbxSelections.SelectedIndexChanged += new System.EventHandler(this.cbxSelections_SelectedIndexChanged);
            this.cbxSelections.DataSourceChanged += new System.EventHandler(this.cbxSelections_DataSourceChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRight);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnLeft);
            this.panel1.Controls.Add(this.btnZoomOut);
            this.panel1.Controls.Add(this.btnZoomIn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 470);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(487, 229);
            this.panel1.TabIndex = 1;
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(210, 82);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(75, 53);
            this.btnRight.TabIndex = 5;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(129, 141);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 53);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseDown);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(129, 23);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 53);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(46, 82);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(75, 53);
            this.btnLeft.TabIndex = 2;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseDown);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(129, 112);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(75, 23);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(129, 82);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(75, 23);
            this.btnZoomIn.TabIndex = 0;
            this.btnZoomIn.Text = "Zoom In";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // selectionProperties
            // 
            this.selectionProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectionProperties.CanShowVisualStyleGlyphs = false;
            this.selectionProperties.Location = new System.Drawing.Point(14, 39);
            this.selectionProperties.Name = "selectionProperties";
            this.selectionProperties.Size = new System.Drawing.Size(456, 379);
            this.selectionProperties.TabIndex = 2;
            // 
            // overlayBox
            // 
            this.overlayBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.overlayBox.Location = new System.Drawing.Point(-53, 0);
            this.overlayBox.Name = "overlayBox";
            this.overlayBox.Size = new System.Drawing.Size(1, 1);
            this.overlayBox.TabIndex = 0;
            this.overlayBox.TabStop = false;
            this.overlayBox.Paint += new System.Windows.Forms.PaintEventHandler(this.overlayBox_Paint);
            this.overlayBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.overlayBox_MouseDown);
            this.overlayBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.overlayBox_MouseMove);
            this.overlayBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.overlayBox_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 699);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(1170, 738);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelPicture.ResumeLayout(false);
            this.panelPicture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.imageProperties.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.overlayBox)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel imageProperties;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox overlayBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Panel panelPicture;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.ComboBox cbxSelections;
        private System.Windows.Forms.PropertyGrid selectionProperties;
    }
}

