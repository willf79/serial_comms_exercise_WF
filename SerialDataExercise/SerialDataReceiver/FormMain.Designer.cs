namespace SerialDataReceiver
{
    partial class FormMain
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
            this.buttonReceive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTransmit
            // 
            this.buttonReceive.Location = new System.Drawing.Point(12, 12);
            this.buttonReceive.Name = "buttonTransmit";
            this.buttonReceive.Size = new System.Drawing.Size(205, 22);
            this.buttonReceive.TabIndex = 0;
            this.buttonReceive.Text = "Receive";
            this.buttonReceive.UseVisualStyleBackColor = true;
            this.buttonReceive.Click += new System.EventHandler(this.buttonReceive_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 46);
            this.Controls.Add(this.buttonReceive);
            this.Name = "FormMain";
            this.Text = "Receiver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonReceive;
    }
}

