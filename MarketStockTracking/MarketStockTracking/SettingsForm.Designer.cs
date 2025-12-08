namespace MarketStockTracking
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            txtIsletmeAdi = new TextBox();
            txtAdres = new TextBox();
            txtTelefon = new TextBox();
            txtSehir = new TextBox();
            txtKasiyer = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            txtYaziciAdi = new TextBox();
            btnKaydet = new Button();
            label7 = new Label();
            SuspendLayout();
            // 
            // txtIsletmeAdi
            // 
            txtIsletmeAdi.Location = new Point(344, 103);
            txtIsletmeAdi.Name = "txtIsletmeAdi";
            txtIsletmeAdi.Size = new Size(100, 23);
            txtIsletmeAdi.TabIndex = 0;
            // 
            // txtAdres
            // 
            txtAdres.Location = new Point(344, 148);
            txtAdres.Name = "txtAdres";
            txtAdres.Size = new Size(100, 23);
            txtAdres.TabIndex = 1;
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(344, 190);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(100, 23);
            txtTelefon.TabIndex = 2;
            // 
            // txtSehir
            // 
            txtSehir.Location = new Point(344, 234);
            txtSehir.Name = "txtSehir";
            txtSehir.Size = new Size(100, 23);
            txtSehir.TabIndex = 3;
            // 
            // txtKasiyer
            // 
            txtKasiyer.Location = new Point(344, 273);
            txtKasiyer.Name = "txtKasiyer";
            txtKasiyer.Size = new Size(100, 23);
            txtKasiyer.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Location = new Point(61, 106);
            label1.Name = "label1";
            label1.Size = new Size(69, 15);
            label1.TabIndex = 5;
            label1.Text = "İşletme Adı:";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Location = new Point(61, 151);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 6;
            label2.Text = "Adres:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Location = new Point(61, 193);
            label3.Name = "label3";
            label3.Size = new Size(48, 15);
            label3.TabIndex = 7;
            label3.Text = "Telefon:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label4.AutoSize = true;
            label4.Location = new Point(61, 237);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 8;
            label4.Text = "Şehir:";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label5.AutoSize = true;
            label5.Location = new Point(62, 276);
            label5.Name = "label5";
            label5.Size = new Size(47, 15);
            label5.TabIndex = 9;
            label5.Text = "Kasiyer:";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label6.AutoSize = true;
            label6.Location = new Point(61, 316);
            label6.Name = "label6";
            label6.Size = new Size(60, 15);
            label6.TabIndex = 10;
            label6.Text = "Yazıcı Adı:";
            // 
            // txtYaziciAdi
            // 
            txtYaziciAdi.Location = new Point(344, 313);
            txtYaziciAdi.Name = "txtYaziciAdi";
            txtYaziciAdi.Size = new Size(100, 23);
            txtYaziciAdi.TabIndex = 11;
            // 
            // btnKaydet
            // 
            btnKaydet.Location = new Point(194, 363);
            btnKaydet.Name = "btnKaydet";
            btnKaydet.Size = new Size(101, 62);
            btnKaydet.TabIndex = 12;
            btnKaydet.Text = "Kaydet";
            btnKaydet.UseVisualStyleBackColor = true;
            btnKaydet.Click += btnKaydet_Click;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label7.Location = new Point(194, 22);
            label7.Name = "label7";
            label7.Size = new Size(101, 30);
            label7.TabIndex = 13;
            label7.Text = "AYARLAR";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(519, 450);
            Controls.Add(label7);
            Controls.Add(btnKaydet);
            Controls.Add(txtYaziciAdi);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtKasiyer);
            Controls.Add(txtSehir);
            Controls.Add(txtTelefon);
            Controls.Add(txtAdres);
            Controls.Add(txtIsletmeAdi);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(535, 489);
            MinimumSize = new Size(535, 489);
            Name = "SettingsForm";
            Text = "Ayarlar";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIsletmeAdi;
        private TextBox txtAdres;
        private TextBox txtTelefon;
        private TextBox txtSehir;
        private TextBox txtKasiyer;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txtYaziciAdi;
        private Button btnKaydet;
        private Label label7;
    }
}