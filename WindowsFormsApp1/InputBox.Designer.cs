namespace WindowsFormsApp1
{
    partial class InputBox
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
            this.acrylicLabel1 = new AcrylicUI.Controls.AcrylicLabel();
            this.acrylicButton1 = new AcrylicUI.Controls.AcrylicButton();
            this.acrylicButton2 = new AcrylicUI.Controls.AcrylicButton();
            this.acrylicTextBox1 = new AcrylicUI.Controls.AcrylicTextBox();
            this.SuspendLayout();
            // 
            // acrylicLabel1
            // 
            this.acrylicLabel1.AutoSize = true;
            this.acrylicLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.acrylicLabel1.Location = new System.Drawing.Point(22, 19);
            this.acrylicLabel1.Name = "acrylicLabel1";
            this.acrylicLabel1.Size = new System.Drawing.Size(37, 15);
            this.acrylicLabel1.TabIndex = 0;
            this.acrylicLabel1.Text = "描述";
            // 
            // acrylicButton1
            // 
            this.acrylicButton1.Default = false;
            this.acrylicButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.acrylicButton1.Image = null;
            this.acrylicButton1.ImagePadding = 6;
            this.acrylicButton1.Location = new System.Drawing.Point(477, 19);
            this.acrylicButton1.Name = "acrylicButton1";
            this.acrylicButton1.Padding = new System.Windows.Forms.Padding(5);
            this.acrylicButton1.Size = new System.Drawing.Size(100, 30);
            this.acrylicButton1.TabIndex = 1;
            this.acrylicButton1.Text = "确定";
            this.acrylicButton1.UseVisualStyleBackColor = false;
            this.acrylicButton1.Click += new System.EventHandler(this.acrylicButton1_Click);
            // 
            // acrylicButton2
            // 
            this.acrylicButton2.Default = false;
            this.acrylicButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.acrylicButton2.Image = null;
            this.acrylicButton2.ImagePadding = 7;
            this.acrylicButton2.Location = new System.Drawing.Point(477, 70);
            this.acrylicButton2.Name = "acrylicButton2";
            this.acrylicButton2.Padding = new System.Windows.Forms.Padding(5);
            this.acrylicButton2.Size = new System.Drawing.Size(100, 30);
            this.acrylicButton2.TabIndex = 2;
            this.acrylicButton2.Text = "取消";
            this.acrylicButton2.UseVisualStyleBackColor = false;
            this.acrylicButton2.Click += new System.EventHandler(this.acrylicButton2_Click);
            // 
            // acrylicTextBox1
            // 
            this.acrylicTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.acrylicTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.acrylicTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.acrylicTextBox1.Location = new System.Drawing.Point(25, 124);
            this.acrylicTextBox1.Name = "acrylicTextBox1";
            this.acrylicTextBox1.Size = new System.Drawing.Size(552, 25);
            this.acrylicTextBox1.TabIndex = 3;
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(589, 174);
            this.Controls.Add(this.acrylicTextBox1);
            this.Controls.Add(this.acrylicButton2);
            this.Controls.Add(this.acrylicButton1);
            this.Controls.Add(this.acrylicLabel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "InputBox";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AcrylicUI.Controls.AcrylicLabel acrylicLabel1;
        private AcrylicUI.Controls.AcrylicButton acrylicButton1;
        private AcrylicUI.Controls.AcrylicButton acrylicButton2;
        private AcrylicUI.Controls.AcrylicTextBox acrylicTextBox1;
    }
}