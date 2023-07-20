namespace MDC
{
    partial class Meter_Data_Collector_DinRail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Meter_Data_Collector_DinRail));
            this.Stop = new System.Windows.Forms.Button();
            this.btnClientService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listBox_ConnectedClients = new System.Windows.Forms.ListBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.serialPort_cmb = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(44, 42);
            this.Stop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(112, 35);
            this.Stop.TabIndex = 1;
            this.Stop.Text = "Stop Server";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // btnClientService
            // 
            this.btnClientService.Location = new System.Drawing.Point(44, 132);
            this.btnClientService.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClientService.Name = "btnClientService";
            this.btnClientService.Size = new System.Drawing.Size(180, 35);
            this.btnClientService.TabIndex = 2;
            this.btnClientService.Text = "Begin Client Service";
            this.btnClientService.UseVisualStyleBackColor = true;
            this.btnClientService.Click += new System.EventHandler(this.btnClientService_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 140);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Service Started";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(384, 54);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "Server Port";
            // 
            // listBox_ConnectedClients
            // 
            this.listBox_ConnectedClients.FormattingEnabled = true;
            this.listBox_ConnectedClients.ItemHeight = 20;
            this.listBox_ConnectedClients.Location = new System.Drawing.Point(44, 186);
            this.listBox_ConnectedClients.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBox_ConnectedClients.Name = "listBox_ConnectedClients";
            this.listBox_ConnectedClients.ScrollAlwaysVisible = true;
            this.listBox_ConnectedClients.Size = new System.Drawing.Size(1116, 344);
            this.listBox_ConnectedClients.TabIndex = 23;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(996, 108);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(154, 35);
            this.buttonClear.TabIndex = 11;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // serialPort_cmb
            // 
            this.serialPort_cmb.FormattingEnabled = true;
            this.serialPort_cmb.Items.AddRange(new object[] {
            "9001",
            "9002",
            "9003",
            "9004",
            "9005",
            "9006",
            "9007",
            "9008",
            "9009"});
            this.serialPort_cmb.Location = new System.Drawing.Point(499, 46);
            this.serialPort_cmb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.serialPort_cmb.Name = "serialPort_cmb";
            this.serialPort_cmb.Size = new System.Drawing.Size(180, 28);
            this.serialPort_cmb.TabIndex = 26;
            // 
            // Meter_Data_Collector_DinRail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 569);
            this.Controls.Add(this.serialPort_cmb);
            this.Controls.Add(this.listBox_ConnectedClients);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClientService);
            this.Controls.Add(this.Stop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Meter_Data_Collector_DinRail";
            this.Text = "Meter Data Collector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button btnClientService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listBox_ConnectedClients;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.ComboBox serialPort_cmb;
    }
}

