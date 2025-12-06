namespace MarketStockTracking
{
    partial class SellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SellForm));
            txtUrunAdi = new ComboBox();
            txtMagza = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtAdet = new TextBox();
            txtNet = new TextBox();
            txtBrut = new TextBox();
            label5 = new Label();
            txtKarZarar = new Label();
            button1 = new Button();
            label9 = new Label();
            txtPesin = new TextBox();
            txtBorc = new TextBox();
            label10 = new Label();
            label11 = new Label();
            dgvUrunler = new DataGridView();
            btnExportExcel = new Button();
            btnSatisGecmisi = new Button();
            btnGuncelUrun = new Button();
            btnSatış = new Button();
            button2 = new Button();
            btnSil = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).BeginInit();
            SuspendLayout();
            // 
            // txtUrunAdi
            // 
            txtUrunAdi.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtUrunAdi.FormattingEnabled = true;
            txtUrunAdi.Location = new Point(141, 61);
            txtUrunAdi.Name = "txtUrunAdi";
            txtUrunAdi.Size = new Size(121, 23);
            txtUrunAdi.TabIndex = 2;
            // 
            // txtMagza
            // 
            txtMagza.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtMagza.FormattingEnabled = true;
            txtMagza.Location = new Point(141, 127);
            txtMagza.Name = "txtMagza";
            txtMagza.Size = new Size(121, 23);
            txtMagza.TabIndex = 3;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Location = new Point(141, 31);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 4;
            label1.Text = "Ürün İsmi";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Location = new Point(141, 98);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 5;
            label2.Text = "Mağza";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Location = new Point(302, 105);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 6;
            label3.Text = "Alış Fiyatı";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label4.AutoSize = true;
            label4.Location = new Point(302, 31);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 7;
            label4.Text = "Satış Fiyatı";
            // 
            // txtAdet
            // 
            txtAdet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtAdet.Location = new Point(434, 89);
            txtAdet.Name = "txtAdet";
            txtAdet.Size = new Size(100, 23);
            txtAdet.TabIndex = 8;
            txtAdet.TextChanged += txtAdet_TextChanged;
            // 
            // txtNet
            // 
            txtNet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtNet.Location = new Point(302, 61);
            txtNet.Name = "txtNet";
            txtNet.Size = new Size(100, 23);
            txtNet.TabIndex = 9;
            txtNet.TextChanged += txtNet_TextChanged;
            // 
            // txtBrut
            // 
            txtBrut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtBrut.Location = new Point(302, 127);
            txtBrut.Name = "txtBrut";
            txtBrut.Size = new Size(100, 23);
            txtBrut.TabIndex = 10;
            txtBrut.TextChanged += txtBrut_TextChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label5.AutoSize = true;
            label5.Location = new Point(434, 61);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 11;
            label5.Text = "Adet";
            // 
            // txtKarZarar
            // 
            txtKarZarar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtKarZarar.AutoSize = true;
            txtKarZarar.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtKarZarar.Location = new Point(302, 194);
            txtKarZarar.Name = "txtKarZarar";
            txtKarZarar.Size = new Size(73, 37);
            txtKarZarar.TabIndex = 13;
            txtKarZarar.Text = "Adet";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top;
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(785, 39);
            button1.Name = "button1";
            button1.Size = new Size(112, 45);
            button1.TabIndex = 14;
            button1.Text = "Ürün Ekle";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label9.AutoSize = true;
            label9.Location = new Point(302, 172);
            label9.Name = "label9";
            label9.Size = new Size(62, 15);
            label9.TabIndex = 16;
            label9.Text = "Kar - Zarar";
            // 
            // txtPesin
            // 
            txtPesin.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtPesin.Location = new Point(580, 61);
            txtPesin.Name = "txtPesin";
            txtPesin.Size = new Size(100, 23);
            txtPesin.TabIndex = 17;
            txtPesin.TextChanged += txtPesin_TextChanged;
            // 
            // txtBorc
            // 
            txtBorc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            txtBorc.Location = new Point(580, 127);
            txtBorc.Name = "txtBorc";
            txtBorc.Size = new Size(100, 23);
            txtBorc.TabIndex = 18;
            txtBorc.TextChanged += txtBorc_TextChanged;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label10.AutoSize = true;
            label10.ForeColor = Color.Black;
            label10.Location = new Point(580, 31);
            label10.Name = "label10";
            label10.Size = new Size(35, 15);
            label10.TabIndex = 19;
            label10.Text = "Peşin";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label11.AutoSize = true;
            label11.Location = new Point(580, 105);
            label11.Name = "label11";
            label11.Size = new Size(31, 15);
            label11.TabIndex = 20;
            label11.Text = "Borç";
            // 
            // dgvUrunler
            // 
            dgvUrunler.AllowUserToAddRows = false;
            dgvUrunler.AllowUserToDeleteRows = false;
            dgvUrunler.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUrunler.BackgroundColor = SystemColors.MenuBar;
            dgvUrunler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUrunler.Location = new Point(43, 245);
            dgvUrunler.Name = "dgvUrunler";
            dgvUrunler.ReadOnly = true;
            dgvUrunler.Size = new Size(899, 349);
            dgvUrunler.TabIndex = 21;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Top;
            btnExportExcel.ForeColor = Color.DarkGreen;
            btnExportExcel.Location = new Point(785, 195);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(112, 36);
            btnExportExcel.TabIndex = 22;
            btnExportExcel.Text = "Excel Olarak İndir";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnSatisGecmisi
            // 
            btnSatisGecmisi.ForeColor = Color.DarkGreen;
            btnSatisGecmisi.Location = new Point(152, 208);
            btnSatisGecmisi.Name = "btnSatisGecmisi";
            btnSatisGecmisi.Size = new Size(110, 31);
            btnSatisGecmisi.TabIndex = 23;
            btnSatisGecmisi.Text = "Satış Geçmişi";
            btnSatisGecmisi.UseVisualStyleBackColor = true;
            btnSatisGecmisi.Click += btnSatisGecmisi_Click;
            // 
            // btnGuncelUrun
            // 
            btnGuncelUrun.ForeColor = Color.DarkGreen;
            btnGuncelUrun.Location = new Point(45, 208);
            btnGuncelUrun.Name = "btnGuncelUrun";
            btnGuncelUrun.Size = new Size(101, 31);
            btnGuncelUrun.TabIndex = 24;
            btnGuncelUrun.Text = "Güncel Ürünler";
            btnGuncelUrun.UseVisualStyleBackColor = true;
            btnGuncelUrun.Click += btnGuncelUrun_Click;
            // 
            // btnSatış
            // 
            btnSatış.Anchor = AnchorStyles.Top;
            btnSatış.ForeColor = Color.DarkGreen;
            btnSatış.Location = new Point(785, 105);
            btnSatış.Name = "btnSatış";
            btnSatış.Size = new Size(112, 45);
            btnSatış.TabIndex = 25;
            btnSatış.Text = "Satış ";
            btnSatış.UseVisualStyleBackColor = true;
            btnSatış.Click += btnSatış_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.Font = new Font("Segoe MDL2 Assets", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(686, 127);
            button2.Name = "button2";
            button2.Size = new Size(46, 22);
            button2.TabIndex = 26;
            button2.Text = "^";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSil
            // 
            btnSil.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSil.ForeColor = Color.Red;
            btnSil.Location = new Point(3, 245);
            btnSil.Name = "btnSil";
            btnSil.Size = new Size(34, 41);
            btnSil.TabIndex = 27;
            btnSil.Text = "X";
            btnSil.UseVisualStyleBackColor = true;
            btnSil.Click += btnSil_Click;
            // 
            // SellForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(984, 661);
            Controls.Add(btnSil);
            Controls.Add(button2);
            Controls.Add(btnSatış);
            Controls.Add(btnGuncelUrun);
            Controls.Add(btnSatisGecmisi);
            Controls.Add(btnExportExcel);
            Controls.Add(dgvUrunler);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(txtBorc);
            Controls.Add(txtPesin);
            Controls.Add(label9);
            Controls.Add(button1);
            Controls.Add(txtKarZarar);
            Controls.Add(label5);
            Controls.Add(txtBrut);
            Controls.Add(txtNet);
            Controls.Add(txtAdet);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtMagza);
            Controls.Add(txtUrunAdi);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1000, 700);
            Name = "SellForm";
            Text = "Yeşillikçi Mustafa Satış";
            Load += SellForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox txtUrunAdi;
        private ComboBox txtMagza;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtAdet;
        private TextBox txtNet;
        private TextBox txtBrut;
        private Label label5;
        private Label txtKarZarar;
        private Button button1;
        private Label label9;
        private TextBox txtPesin;
        private TextBox txtBorc;
        private Label label10;
        private Label label11;
        private DataGridView dgvUrunler;
        private Button btnExportExcel;
        private Button btnSatisGecmisi;
        private Button btnGuncelUrun;
        private Button btnSatış;
        private Button button2;
        private Button btnSil;
    }
}