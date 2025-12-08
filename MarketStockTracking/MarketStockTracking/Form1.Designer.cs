namespace MarketStockTracking
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            buttonSatis = new Button();
            buttonUrun = new Button();
            button2 = new Button();
            button1 = new Button();
            button3 = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // buttonSatis
            // 
            buttonSatis.Location = new Point(225, 328);
            buttonSatis.Name = "buttonSatis";
            buttonSatis.Size = new Size(75, 24);
            buttonSatis.TabIndex = 0;
            buttonSatis.Text = "Satış";
            buttonSatis.UseVisualStyleBackColor = true;
            buttonSatis.Click += buttonSatis_Click;
            // 
            // buttonUrun
            // 
            buttonUrun.Location = new Point(225, 357);
            buttonUrun.Name = "buttonUrun";
            buttonUrun.Size = new Size(75, 24);
            buttonUrun.TabIndex = 1;
            buttonUrun.Text = "Ürün";
            buttonUrun.UseVisualStyleBackColor = true;
            buttonUrun.Click += buttonUrun_Click;
            // 
            // button2
            // 
            button2.Location = new Point(225, 386);
            button2.Name = "button2";
            button2.Size = new Size(75, 24);
            button2.TabIndex = 2;
            button2.Text = "Marketler";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(225, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 24);
            button1.TabIndex = 3;
            button1.Text = "Rapor";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(225, 444);
            button3.Name = "button3";
            button3.Size = new Size(75, 24);
            button3.TabIndex = 4;
            button3.Text = "Borç";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(12, 203);
            label1.Name = "label1";
            label1.Size = new Size(514, 86);
            label1.TabIndex = 5;
            label1.Text = "Yeşillikçi Mustafa";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(190, 38);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(164, 162);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // button4
            // 
            button4.Location = new Point(433, 12);
            button4.Name = "button4";
            button4.Size = new Size(92, 31);
            button4.TabIndex = 7;
            button4.Text = "Ayarlar";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(537, 537);
            Controls.Add(button4);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(buttonUrun);
            Controls.Add(buttonSatis);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(553, 576);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Yeşillikçi Mustafa Uygulaması";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSatis;
        private Button buttonUrun;
        private Button button2;
        private Button button1;
        private Button button3;
        private Label label1;
        private PictureBox pictureBox1;
        private Button button4;
    }
}
