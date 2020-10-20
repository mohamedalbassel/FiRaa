namespace Egyptian_FLS_code
{
    partial class Form2
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
            this.pieChart1 = new LiveCharts.WinForms.PieChart();
            this.SuspendLayout();
            // 
            // pieChart1
            // 
            this.pieChart1.Location = new System.Drawing.Point(12, 12);
            this.pieChart1.Name = "pieChart1";
            this.pieChart1.Size = new System.Drawing.Size(414, 288);
            this.pieChart1.TabIndex = 1;
            this.pieChart1.Text = "pieChart1";
            this.pieChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.pieChart1_ChildChanged_2);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 312);
            this.Controls.Add(this.pieChart1);
            this.Name = "Form2";
            this.ShowIcon = false;
            this.Text = "Accuracy Chart";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private LiveCharts.WinForms.PieChart pieChart1;
    }
}