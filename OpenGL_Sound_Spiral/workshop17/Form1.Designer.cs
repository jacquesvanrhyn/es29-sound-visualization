namespace workshop17
{
    partial class Form1
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
            this.glControl1 = new OpenTK.GLControl();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(548, 488);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.AutoSize = true;
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.nextButton.FlatAppearance.BorderSize = 0;
            this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextButton.ForeColor = System.Drawing.Color.Transparent;
            this.nextButton.Location = new System.Drawing.Point(516, 0);
            this.nextButton.Margin = new System.Windows.Forms.Padding(2);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(32, 505);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = ">";
            this.nextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.nextButton.UseVisualStyleBackColor = false;
            // 
            // prevButton
            // 
            this.prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.prevButton.AutoSize = true;
            this.prevButton.BackColor = System.Drawing.Color.Transparent;
            this.prevButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.prevButton.FlatAppearance.BorderSize = 0;
            this.prevButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevButton.ForeColor = System.Drawing.Color.Transparent;
            this.prevButton.Location = new System.Drawing.Point(0, 0);
            this.prevButton.Margin = new System.Windows.Forms.Padding(2);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(32, 505);
            this.prevButton.TabIndex = 2;
            this.prevButton.Text = "<";
            this.prevButton.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 488);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.glControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
    }
}

