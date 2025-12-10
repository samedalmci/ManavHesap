namespace MarketStockTracking
{
    partial class DebtCancellation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebtCancellation));
            txtMagza = new ComboBox();
            dgvBorclar = new DataGridView();
            button1 = new Button();
            txtOdenenMiktar = new TextBox();
            txtKalanBorc = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnExportExcel = new Button();
            button2 = new Button();
            lblSecimBilgi = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBorclar).BeginInit();
            SuspendLayout();
            // 
            // txtMagza
            // 
            txtMagza.Anchor = AnchorStyles.Top;
            txtMagza.FormattingEnabled = true;
            txtMagza.Location = new Point(102, 49);
            txtMagza.Name = "txtMagza";
            txtMagza.Size = new Size(121, 23);
            txtMagza.TabIndex = 0;
            // 
            // dgvBorclar
            // 
            dgvBorclar.AllowUserToAddRows = false;
            dgvBorclar.AllowUserToDeleteRows = false;
            dgvBorclar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBorclar.BackgroundColor = SystemColors.MenuBar;
            dgvBorclar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorclar.Location = new Point(56, 149);
            dgvBorclar.Name = "dgvBorclar";
            dgvBorclar.ReadOnly = true;
            dgvBorclar.Size = new Size(624, 324);
            dgvBorclar.TabIndex = 1;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top;
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(545, 112);
            button1.Name = "button1";
            button1.Size = new Size(135, 31);
            button1.TabIndex = 2;
            button1.Text = "Borcu Öde";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtOdenenMiktar
            // 
            txtOdenenMiktar.Anchor = AnchorStyles.Top;
            txtOdenenMiktar.Location = new Point(331, 49);
            txtOdenenMiktar.Name = "txtOdenenMiktar";
            txtOdenenMiktar.Size = new Size(122, 23);
            txtOdenenMiktar.TabIndex = 3;
            // 
            // txtKalanBorc
            // 
            txtKalanBorc.Anchor = AnchorStyles.Top;
            txtKalanBorc.Location = new Point(580, 50);
            txtKalanBorc.Name = "txtKalanBorc";
            txtKalanBorc.Size = new Size(100, 23);
            txtKalanBorc.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(102, 27);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 5;
            label1.Text = "Mağaza İsmi";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(331, 27);
            label2.Name = "label2";
            label2.Size = new Size(101, 15);
            label2.TabIndex = 6;
            label2.Text = "Ödenecek Peşinat";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(580, 28);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 7;
            label3.Text = "Kalan Borç";
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Top;
            btnExportExcel.ForeColor = Color.DarkGreen;
            btnExportExcel.Location = new Point(46, 112);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(117, 31);
            btnExportExcel.TabIndex = 8;
            btnExportExcel.Text = "Excel Olarak İndir";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.Font = new Font("Segoe MDL2 Assets", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(496, 49);
            button2.Name = "button2";
            button2.Size = new Size(46, 22);
            button2.TabIndex = 27;
            button2.Text = "<";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lblSecimBilgi
            // 
            lblSecimBilgi.AutoSize = true;
            lblSecimBilgi.Location = new Point(196, 112);
            lblSecimBilgi.Name = "lblSecimBilgi";
            lblSecimBilgi.Size = new Size(72, 15);
            lblSecimBilgi.TabIndex = 28;
            lblSecimBilgi.Text = "X Adet Seçili";
            // 
            // DebtCancellation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(739, 512);
            Controls.Add(lblSecimBilgi);
            Controls.Add(button2);
            Controls.Add(btnExportExcel);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtKalanBorc);
            Controls.Add(txtOdenenMiktar);
            Controls.Add(button1);
            Controls.Add(dgvBorclar);
            Controls.Add(txtMagza);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(664, 450);
            Name = "DebtCancellation";
            Text = "Yeşillikçi Mustafa Borç Ödeme";
            ((System.ComponentModel.ISupportInitialize)dgvBorclar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox txtMagza;
        private DataGridView dgvBorclar;
        private Button button1;
        private TextBox txtOdenenMiktar;
        private TextBox txtKalanBorc;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnExportExcel;
        private Button button2;
        private Label lblSecimBilgi;
    }
}