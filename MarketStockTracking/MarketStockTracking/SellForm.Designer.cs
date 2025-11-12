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
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).BeginInit();
            SuspendLayout();
            // 
            // txtUrunAdi
            // 
            txtUrunAdi.FormattingEnabled = true;
            txtUrunAdi.Location = new Point(43, 61);
            txtUrunAdi.Name = "txtUrunAdi";
            txtUrunAdi.Size = new Size(121, 23);
            txtUrunAdi.TabIndex = 2;
            // 
            // txtMagza
            // 
            txtMagza.FormattingEnabled = true;
            txtMagza.Location = new Point(43, 127);
            txtMagza.Name = "txtMagza";
            txtMagza.Size = new Size(121, 23);
            txtMagza.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 31);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 4;
            label1.Text = "Ürün İsmi";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 97);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 5;
            label2.Text = "Mağza";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(332, 105);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 6;
            label3.Text = "Alış Fiyatı";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(332, 31);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 7;
            label4.Text = "Satış Fiyatı";
            // 
            // txtAdet
            // 
            txtAdet.Location = new Point(204, 97);
            txtAdet.Name = "txtAdet";
            txtAdet.Size = new Size(100, 23);
            txtAdet.TabIndex = 8;
            // 
            // txtNet
            // 
            txtNet.Location = new Point(332, 61);
            txtNet.Name = "txtNet";
            txtNet.Size = new Size(100, 23);
            txtNet.TabIndex = 9;
            txtNet.TextChanged += txtNet_TextChanged;
            // 
            // txtBrut
            // 
            txtBrut.Location = new Point(332, 127);
            txtBrut.Name = "txtBrut";
            txtBrut.Size = new Size(100, 23);
            txtBrut.TabIndex = 10;
            txtBrut.TextChanged += txtBrut_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(204, 70);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 11;
            label5.Text = "Adet";
            // 
            // txtKarZarar
            // 
            txtKarZarar.AutoSize = true;
            txtKarZarar.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtKarZarar.Location = new Point(479, 82);
            txtKarZarar.Name = "txtKarZarar";
            txtKarZarar.Size = new Size(73, 37);
            txtKarZarar.TabIndex = 13;
            txtKarZarar.Text = "Adet";
            // 
            // button1
            // 
            button1.Location = new Point(721, 48);
            button1.Name = "button1";
            button1.Size = new Size(112, 102);
            button1.TabIndex = 14;
            button1.Text = "KAYDET";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(479, 61);
            label9.Name = "label9";
            label9.Size = new Size(62, 15);
            label9.TabIndex = 16;
            label9.Text = "Kar - Zarar";
            label9.Click += label9_Click;
            // 
            // txtPesin
            // 
            txtPesin.Location = new Point(587, 61);
            txtPesin.Name = "txtPesin";
            txtPesin.Size = new Size(100, 23);
            txtPesin.TabIndex = 17;
            // 
            // txtBorc
            // 
            txtBorc.Location = new Point(587, 127);
            txtBorc.Name = "txtBorc";
            txtBorc.Size = new Size(100, 23);
            txtBorc.TabIndex = 18;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(587, 31);
            label10.Name = "label10";
            label10.Size = new Size(35, 15);
            label10.TabIndex = 19;
            label10.Text = "Peşin";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(587, 105);
            label11.Name = "label11";
            label11.Size = new Size(31, 15);
            label11.TabIndex = 20;
            label11.Text = "Borç";
            // 
            // dgvUrunler
            // 
            dgvUrunler.AllowUserToAddRows = false;
            dgvUrunler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUrunler.Location = new Point(43, 258);
            dgvUrunler.Name = "dgvUrunler";
            dgvUrunler.ReadOnly = true;
            dgvUrunler.Size = new Size(790, 230);
            dgvUrunler.TabIndex = 21;
            // 
            // SellForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 518);
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
            Name = "SellForm";
            Text = "SellForm";
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
    }
}