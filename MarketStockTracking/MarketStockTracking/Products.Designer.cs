namespace MarketStockTracking
{
    partial class Products
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Products));
            txtUrunAdi = new TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            button1 = new Button();
            dgvUrunler = new DataGridView();
            txtUrunCesidi = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).BeginInit();
            SuspendLayout();
            // 
            // txtUrunAdi
            // 
            txtUrunAdi.Location = new Point(47, 47);
            txtUrunAdi.Name = "txtUrunAdi";
            txtUrunAdi.Size = new Size(161, 23);
            txtUrunAdi.TabIndex = 2;
            txtUrunAdi.TextChanged += txtUrunAdi_TextChanged;
            // 
            // button1
            // 
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(383, 38);
            button1.Name = "button1";
            button1.Size = new Size(108, 38);
            button1.TabIndex = 4;
            button1.Text = "Ürün Kaydet";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dgvUrunler
            // 
            dgvUrunler.BackgroundColor = SystemColors.MenuBar;
            dgvUrunler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUrunler.Location = new Point(47, 199);
            dgvUrunler.Name = "dgvUrunler";
            dgvUrunler.Size = new Size(444, 178);
            dgvUrunler.TabIndex = 6;
            // 
            // txtUrunCesidi
            // 
            txtUrunCesidi.FormattingEnabled = true;
            txtUrunCesidi.Items.AddRange(new object[] { "Meyve", "Sebze" });
            txtUrunCesidi.Location = new Point(237, 47);
            txtUrunCesidi.Name = "txtUrunCesidi";
            txtUrunCesidi.Size = new Size(121, 23);
            txtUrunCesidi.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 25);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 7;
            label1.Text = "Ürün İsmi";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(237, 25);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 8;
            label2.Text = "Ürün Çeşidi";
            // 
            // Products
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(527, 411);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvUrunler);
            Controls.Add(button1);
            Controls.Add(txtUrunCesidi);
            Controls.Add(txtUrunAdi);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Products";
            Text = "X Manav Ürünler";
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtUrunAdi;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button button1;
        private DataGridView dgvUrunler;
        private ComboBox txtUrunCesidi;
        private Label label1;
        private Label label2;
    }
}