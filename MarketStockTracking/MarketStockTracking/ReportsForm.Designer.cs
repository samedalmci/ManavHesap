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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsForm));
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            btnAdet = new Button();
            btnBorc = new Button();
            btnMaliyet = new Button();
            dgvReport = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            btnExcelExport = new Button();
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
            btnAdet.ForeColor = Color.DarkGreen;
            btnAdet.Location = new Point(583, 9);
            btnAdet.Name = "btnAdet";
            btnAdet.Size = new Size(75, 23);
            btnAdet.TabIndex = 2;
            btnAdet.Text = "Adet";
            btnAdet.UseVisualStyleBackColor = true;
            // 
            // btnBorc
            // 
            btnBorc.ForeColor = Color.DarkGreen;
            btnBorc.Location = new Point(583, 52);
            btnBorc.Name = "btnBorc";
            btnBorc.Size = new Size(75, 23);
            btnBorc.TabIndex = 3;
            btnBorc.Text = "Borç";
            btnBorc.UseVisualStyleBackColor = true;
            // 
            // btnMaliyet
            // 
            btnMaliyet.ForeColor = Color.DarkGreen;
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
            dgvReport.BackgroundColor = SystemColors.MenuBar;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReport.Location = new Point(42, 154);
            dgvReport.Name = "dgvReport";
            dgvReport.Size = new Size(627, 347);
            dgvReport.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(107, 30);
            label1.Name = "label1";
            label1.Size = new Size(85, 15);
            label1.TabIndex = 6;
            label1.Text = "Başlangıç Tarih";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(324, 30);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Bitiş Tarih";
            // 
            // btnExcelExport
            // 
            btnExcelExport.ForeColor = Color.DarkGreen;
            btnExcelExport.Location = new Point(42, 116);
            btnExcelExport.Name = "btnExcelExport";
            btnExcelExport.Size = new Size(120, 32);
            btnExcelExport.TabIndex = 8;
            btnExcelExport.Text = "Excel Olarak İndir";
            btnExcelExport.UseVisualStyleBackColor = true;
            btnExcelExport.Click += btnExcelExport_Click;
            // 
            // ReportsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(754, 552);
            Controls.Add(btnExcelExport);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvReport);
            Controls.Add(btnMaliyet);
            Controls.Add(btnBorc);
            Controls.Add(btnAdet);
            Controls.Add(dtpEnd);
            Controls.Add(dtpStart);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ReportsForm";
            Text = "Yeşillikçi Mustafa Rapor";
            ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Button btnAdet;
        private Button btnBorc;
        private Button btnMaliyet;
        private DataGridView dgvReport;
        private Label label1;
        private Label label2;
        private Button btnExcelExport;
    }
}