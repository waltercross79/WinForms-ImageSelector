namespace ImageSelector
{
    partial class EventViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bsEvents = new System.Windows.Forms.BindingSource(this.components);
            this.eventNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLeftCornerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OverlayLeftCorner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mouseCoordinatesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageSizeString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OverlaySizeString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewPortWidthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewPortHeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eventNameDataGridViewTextBoxColumn,
            this.imageLeftCornerDataGridViewTextBoxColumn,
            this.OverlayLeftCorner,
            this.mouseCoordinatesDataGridViewTextBoxColumn,
            this.ImageSizeString,
            this.OverlaySizeString,
            this.viewPortWidthDataGridViewTextBoxColumn,
            this.viewPortHeightDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bsEvents;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(662, 283);
            this.dataGridView1.TabIndex = 0;
            // 
            // bsEvents
            // 
            this.bsEvents.DataSource = typeof(ImageSelector.Infrastructure.ImageAndMouseState);
            // 
            // eventNameDataGridViewTextBoxColumn
            // 
            this.eventNameDataGridViewTextBoxColumn.DataPropertyName = "EventName";
            this.eventNameDataGridViewTextBoxColumn.HeaderText = "EventName";
            this.eventNameDataGridViewTextBoxColumn.Name = "eventNameDataGridViewTextBoxColumn";
            // 
            // imageLeftCornerDataGridViewTextBoxColumn
            // 
            this.imageLeftCornerDataGridViewTextBoxColumn.DataPropertyName = "ImageLeftCorner";
            this.imageLeftCornerDataGridViewTextBoxColumn.HeaderText = "ImageLeftCorner";
            this.imageLeftCornerDataGridViewTextBoxColumn.Name = "imageLeftCornerDataGridViewTextBoxColumn";
            // 
            // OverlayLeftCorner
            // 
            this.OverlayLeftCorner.DataPropertyName = "OverlayLeftCorner";
            this.OverlayLeftCorner.HeaderText = "OverlayLeftCorner";
            this.OverlayLeftCorner.Name = "OverlayLeftCorner";
            // 
            // mouseCoordinatesDataGridViewTextBoxColumn
            // 
            this.mouseCoordinatesDataGridViewTextBoxColumn.DataPropertyName = "MouseCoordinates";
            this.mouseCoordinatesDataGridViewTextBoxColumn.HeaderText = "MouseCoordinates";
            this.mouseCoordinatesDataGridViewTextBoxColumn.Name = "mouseCoordinatesDataGridViewTextBoxColumn";
            // 
            // ImageSizeString
            // 
            this.ImageSizeString.DataPropertyName = "ImageSizeString";
            this.ImageSizeString.HeaderText = "ImageSizeString";
            this.ImageSizeString.Name = "ImageSizeString";
            this.ImageSizeString.ReadOnly = true;
            // 
            // OverlaySizeString
            // 
            this.OverlaySizeString.DataPropertyName = "OverlaySizeString";
            this.OverlaySizeString.HeaderText = "OverlaySizeString";
            this.OverlaySizeString.Name = "OverlaySizeString";
            this.OverlaySizeString.ReadOnly = true;
            // 
            // viewPortWidthDataGridViewTextBoxColumn
            // 
            this.viewPortWidthDataGridViewTextBoxColumn.DataPropertyName = "ViewPortWidth";
            this.viewPortWidthDataGridViewTextBoxColumn.HeaderText = "ViewPortWidth";
            this.viewPortWidthDataGridViewTextBoxColumn.Name = "viewPortWidthDataGridViewTextBoxColumn";
            // 
            // viewPortHeightDataGridViewTextBoxColumn
            // 
            this.viewPortHeightDataGridViewTextBoxColumn.DataPropertyName = "ViewPortHeight";
            this.viewPortHeightDataGridViewTextBoxColumn.HeaderText = "ViewPortHeight";
            this.viewPortHeightDataGridViewTextBoxColumn.Name = "viewPortHeightDataGridViewTextBoxColumn";
            // 
            // EventViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "EventViewer";
            this.Size = new System.Drawing.Size(662, 283);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEvents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsEvents;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLeftCornerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OverlayLeftCorner;
        private System.Windows.Forms.DataGridViewTextBoxColumn mouseCoordinatesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageSizeString;
        private System.Windows.Forms.DataGridViewTextBoxColumn OverlaySizeString;
        private System.Windows.Forms.DataGridViewTextBoxColumn viewPortWidthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn viewPortHeightDataGridViewTextBoxColumn;
    }
}
