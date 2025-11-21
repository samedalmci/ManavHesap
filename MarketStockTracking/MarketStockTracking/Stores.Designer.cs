namespace MarketStockTracking
{
    partial class Stores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stores));
            dgvMagazalar = new DataGridView();
            btnEkle = new Button();
            txtMagazaAdi = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvMagazalar).BeginInit();
            SuspendLayout();
            // 
            // dgvMagazalar
            // 
            dgvMagazalar.AllowUserToAddRows = false;
            dgvMagazalar.AllowUserToDeleteRows = false;
            dgvMagazalar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMagazalar.Location = new Point(44, 228);
            dgvMagazalar.Name = "dgvMagazalar";
            dgvMagazalar.ReadOnly = true;
            dgvMagazalar.Size = new Size(411, 164);
            dgvMagazalar.TabIndex = 10;
            // 
            // btnEkle
            // 
            btnEkle.Location = new Point(356, 61);
            btnEkle.Name = "btnEkle";
            btnEkle.Size = new Size(99, 32);
            btnEkle.TabIndex = 9;
            btnEkle.Text = "Mağaza Ekle";
            btnEkle.UseVisualStyleBackColor = true;
            btnEkle.Click += btnEkle_Click_1;
            // 
            // txtMagazaAdi
            // 
            txtMagazaAdi.Location = new Point(44, 62);
            txtMagazaAdi.Name = "txtMagazaAdi";
            txtMagazaAdi.Size = new Size(161, 23);
            txtMagazaAdi.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 44);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 11;
            label1.Text = "Mağaza İsmi";
            // 
            // Stores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 450);
            Controls.Add(label1);
            Controls.Add(dgvMagazalar);
            Controls.Add(btnEkle);
            Controls.Add(txtMagazaAdi);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Stores";
            Text = "X Manav Mağazalar";
            Load += Stores_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMagazalar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvMagazalar;
        private Button btnEkle;
        private TextBox txtMagazaAdi;
        private Label label1;
    }
}