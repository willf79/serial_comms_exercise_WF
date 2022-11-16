namespace SerialDataTransmitter
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
            this.buttonTransmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTransmit
            // 
            this.buttonTransmit.Location = new System.Drawing.Point(12, 12);
            this.buttonTransmit.Name = "buttonTransmit";
            this.buttonTransmit.Size = new System.Drawing.Size(205, 22);
            this.buttonTransmit.TabIndex = 0;
            this.buttonTransmit.Text = "Transmit";
            this.buttonTransmit.UseVisualStyleBackColor = true;
            this.buttonTransmit.Click += new System.EventHandler(this.buttonTransmit_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 46);
            this.Controls.Add(this.buttonTransmit);
            this.Name = "FormMain";
            this.Text = "Transmitter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTransmit;
    }
}

