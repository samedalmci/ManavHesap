namespace MarketStockTracking
{
    partial class ReportsForm
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
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            btnAdet = new Button();
            btnBorc = new Button();
            btnMaliyet = new Button();
            dgvReport = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
            SuspendLayout();
            // 
            // dtpStart
            // 
            dtpStart.Location = new Point(107, 48);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(109, 23);
            dtpStart.TabIndex = 0;
            dtpStart.Value = new DateTime(2025, 11, 19, 0, 0, 0, 0);
            // 
            // dtpEnd
            // 
            dtpEnd.Location = new Point(324, 48);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(109, 23);
            dtpEnd.TabIndex = 1;
            // 
            // btnAdet
            // 
            btnAdet.Location = new Point(583, 9);
            btnAdet.Name = "btnAdet";
            btnAdet.Size = new Size(75, 23);
            btnAdet.TabIndex = 2;
            btnAdet.Text = "Adet";
            btnAdet.UseVisualStyleBackColor = true;
            // 
            // btnBorc
            // 
            btnBorc.Location = new Point(583, 52);
            btnBorc.Name = "btnBorc";
            btnBorc.Size = new Size(75, 23);
            btnBorc.TabIndex = 3;
            btnBorc.Text = "Borç";
            btnBorc.UseVisualStyleBackColor = true;
            // 
            // btnMaliyet
            // 
            btnMaliyet.Location = new Point(583, 98);
            btnMaliyet.Name = "btnMaliyet";
            btnMaliyet.Size = new Size(75, 23);
            btnMaliyet.TabIndex = 4;
            btnMaliyet.Text = "Maliyet";
            btnMaliyet.UseVisualStyleBackColor = true;
            // 
            // dgvReport
            // 
            dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReport.Location = new Point(42, 154);
            dgvReport.Name = "dgvReport";
            dgvReport.Size = new Size(627, 347);
            dgvReport.TabIndex = 5;
            // 
            // ReportsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(754, 552);
            Controls.Add(dgvReport);
            Controls.Add(btnMaliyet);
            Controls.Add(btnBorc);
            Controls.Add(btnAdet);
            Controls.Add(dtpEnd);
            Controls.Add(dtpStart);
            Name = "ReportsForm";
            Text = "Reports";
            ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Button btnAdet;
        private Button btnBorc;
        private Button btnMaliyet;
        private DataGridView dgvReport;
    }
}