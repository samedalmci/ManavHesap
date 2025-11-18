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
            txtMagza = new ComboBox();
            dgvBorclar = new DataGridView();
            button1 = new Button();
            txtOdenenMiktar = new TextBox();
            txtKalanBorc = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvBorclar).BeginInit();
            SuspendLayout();
            // 
            // txtMagza
            // 
            txtMagza.FormattingEnabled = true;
            txtMagza.Location = new Point(57, 49);
            txtMagza.Name = "txtMagza";
            txtMagza.Size = new Size(121, 23);
            txtMagza.TabIndex = 0;
            // 
            // dgvBorclar
            // 
            dgvBorclar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorclar.Location = new Point(57, 138);
            dgvBorclar.Name = "dgvBorclar";
            dgvBorclar.Size = new Size(420, 224);
            dgvBorclar.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(501, 181);
            button1.Name = "button1";
            button1.Size = new Size(102, 91);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtOdenenMiktar
            // 
            txtOdenenMiktar.Location = new Point(286, 49);
            txtOdenenMiktar.Name = "txtOdenenMiktar";
            txtOdenenMiktar.Size = new Size(100, 23);
            txtOdenenMiktar.TabIndex = 3;
            // 
            // txtKalanBorc
            // 
            txtKalanBorc.Location = new Point(501, 49);
            txtKalanBorc.Name = "txtKalanBorc";
            txtKalanBorc.Size = new Size(100, 23);
            txtKalanBorc.TabIndex = 4;
            // 
            // DebtCancellation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 407);
            Controls.Add(txtKalanBorc);
            Controls.Add(txtOdenenMiktar);
            Controls.Add(button1);
            Controls.Add(dgvBorclar);
            Controls.Add(txtMagza);
            Name = "DebtCancellation";
            Text = "DebtCancellation";
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
    }
}