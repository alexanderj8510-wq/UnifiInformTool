namespace UnifiInformTool
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
            this.components = new System.ComponentModel.Container();
            this.txtControllerUrl = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtManualIp = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.Ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSetInform = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.chkSaveSettings = new System.Windows.Forms.CheckBox();
            this.chkEnableFileLog = new System.Windows.Forms.CheckBox();
            this.txtSubnet = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnTestSsh = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.cmbSlots = new System.Windows.Forms.ComboBox();
            this.btnEditSlots = new System.Windows.Forms.Button();
            this.btnRunSlot = new System.Windows.Forms.Button();
            this.lblVerticalLine = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtControllerUrl
            // 
            this.txtControllerUrl.AccessibleName = "txtControllerUrl";
            this.txtControllerUrl.Location = new System.Drawing.Point(12, 243);
            this.txtControllerUrl.Name = "txtControllerUrl";
            this.txtControllerUrl.Size = new System.Drawing.Size(204, 20);
            this.txtControllerUrl.TabIndex = 6;
            this.txtControllerUrl.Text = "http://192.168.1.2:8080/inform";
            // 
            // btnScan
            // 
            this.btnScan.AccessibleName = "btnScan";
            this.btnScan.Location = new System.Drawing.Point(249, 267);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 8;
            this.btnScan.Text = "Scan";
            this.toolTip1.SetToolTip(this.btnScan, "Scan for available devices");
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtManualIp
            // 
            this.txtManualIp.AccessibleName = "txtManualIp";
            this.txtManualIp.Location = new System.Drawing.Point(12, 129);
            this.txtManualIp.Name = "txtManualIp";
            this.txtManualIp.Size = new System.Drawing.Size(136, 20);
            this.txtManualIp.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtManualIp, "Manually enter device IP");
            this.txtManualIp.TextChanged += new System.EventHandler(this.txtManualIp_TextChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.AccessibleName = "txtUsername";
            this.txtUsername.Location = new System.Drawing.Point(12, 51);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(136, 20);
            this.txtUsername.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtUsername, "SSH-username");
            // 
            // txtPassword
            // 
            this.txtPassword.AccessibleName = "txtPassword";
            this.txtPassword.Location = new System.Drawing.Point(12, 90);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(136, 20);
            this.txtPassword.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtPassword, "SSH-password");
            // 
            // numPort
            // 
            this.numPort.AccessibleName = "numPort";
            this.numPort.Location = new System.Drawing.Point(12, 165);
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(136, 20);
            this.numPort.TabIndex = 4;
            this.toolTip1.SetToolTip(this.numPort, "Default SSH port is 22");
            this.numPort.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // lvDevices
            // 
            this.lvDevices.AccessibleName = "lvDevices";
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ip,
            this.Info});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.HideSelection = false;
            this.lvDevices.Location = new System.Drawing.Point(154, 51);
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(348, 180);
            this.lvDevices.TabIndex = 12;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            this.lvDevices.SelectedIndexChanged += new System.EventHandler(this.lvDevices_SelectedIndexChanged);
            // 
            // Ip
            // 
            this.Ip.Width = 79;
            // 
            // btnSetInform
            // 
            this.btnSetInform.AccessibleName = "btnSetInform";
            this.btnSetInform.Location = new System.Drawing.Point(249, 243);
            this.btnSetInform.Name = "btnSetInform";
            this.btnSetInform.Size = new System.Drawing.Size(75, 23);
            this.btnSetInform.TabIndex = 7;
            this.btnSetInform.Text = "Run";
            this.toolTip1.SetToolTip(this.btnSetInform, "Run command on selected devices");
            this.btnSetInform.UseVisualStyleBackColor = true;
            this.btnSetInform.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar
            // 
            this.progressBar.AccessibleName = "progressBar";
            this.progressBar.Location = new System.Drawing.Point(12, 269);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(204, 23);
            this.progressBar.TabIndex = 11;
            // 
            // chkSaveSettings
            // 
            this.chkSaveSettings.AccessibleName = "chkSaveSettings";
            this.chkSaveSettings.AutoSize = true;
            this.chkSaveSettings.Location = new System.Drawing.Point(12, 298);
            this.chkSaveSettings.Name = "chkSaveSettings";
            this.chkSaveSettings.Size = new System.Drawing.Size(118, 17);
            this.chkSaveSettings.TabIndex = 13;
            this.chkSaveSettings.Text = "Save latest settings";
            this.chkSaveSettings.UseVisualStyleBackColor = true;
            // 
            // chkEnableFileLog
            // 
            this.chkEnableFileLog.AccessibleName = "chkEnableFileLog";
            this.chkEnableFileLog.AutoSize = true;
            this.chkEnableFileLog.Location = new System.Drawing.Point(136, 298);
            this.chkEnableFileLog.Name = "chkEnableFileLog";
            this.chkEnableFileLog.Size = new System.Drawing.Size(80, 17);
            this.chkEnableFileLog.TabIndex = 14;
            this.chkEnableFileLog.Text = "Write to log";
            this.chkEnableFileLog.UseVisualStyleBackColor = true;
            // 
            // txtSubnet
            // 
            this.txtSubnet.Location = new System.Drawing.Point(12, 204);
            this.txtSubnet.Name = "txtSubnet";
            this.txtSubnet.Size = new System.Drawing.Size(136, 20);
            this.txtSubnet.TabIndex = 5;
            this.txtSubnet.Text = "192.168.1.0/24";
            this.toolTip1.SetToolTip(this.txtSubnet, "Subnet to scan");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "UniFi Inform-tool";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "IP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Inform-URL";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Subnet";
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(11, 325);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(491, 98);
            this.txtLog.TabIndex = 15;
            this.txtLog.Text = "";
            // 
            // btnTestSsh
            // 
            this.btnTestSsh.Location = new System.Drawing.Point(249, 292);
            this.btnTestSsh.Name = "btnTestSsh";
            this.btnTestSsh.Size = new System.Drawing.Size(75, 23);
            this.btnTestSsh.TabIndex = 9;
            this.btnTestSsh.Text = "Test SSH";
            this.btnTestSsh.UseVisualStyleBackColor = true;
            this.btnTestSsh.Click += new System.EventHandler(this.btnTestSsh_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(427, 292);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 23;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // cmbSlots
            // 
            this.cmbSlots.FormattingEnabled = true;
            this.cmbSlots.Location = new System.Drawing.Point(338, 243);
            this.cmbSlots.Name = "cmbSlots";
            this.cmbSlots.Size = new System.Drawing.Size(75, 21);
            this.cmbSlots.TabIndex = 24;
            // 
            // btnEditSlots
            // 
            this.btnEditSlots.Location = new System.Drawing.Point(338, 293);
            this.btnEditSlots.Name = "btnEditSlots";
            this.btnEditSlots.Size = new System.Drawing.Size(75, 23);
            this.btnEditSlots.TabIndex = 25;
            this.btnEditSlots.Text = "Edit slots";
            this.btnEditSlots.UseVisualStyleBackColor = true;
            this.btnEditSlots.Click += new System.EventHandler(this.btnEditSlots_Click);
            // 
            // btnRunSlot
            // 
            this.btnRunSlot.Location = new System.Drawing.Point(338, 267);
            this.btnRunSlot.Name = "btnRunSlot";
            this.btnRunSlot.Size = new System.Drawing.Size(75, 23);
            this.btnRunSlot.TabIndex = 26;
            this.btnRunSlot.Text = "Run slot";
            this.btnRunSlot.UseVisualStyleBackColor = true;
            this.btnRunSlot.Click += new System.EventHandler(this.btnRunSlot_Click);
            // 
            // lblVerticalLine
            // 
            this.lblVerticalLine.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblVerticalLine.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblVerticalLine.Location = new System.Drawing.Point(330, 243);
            this.lblVerticalLine.Name = "lblVerticalLine";
            this.lblVerticalLine.Size = new System.Drawing.Size(2, 73);
            this.lblVerticalLine.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label8.Location = new System.Drawing.Point(419, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 73);
            this.label8.TabIndex = 27;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 435);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblVerticalLine);
            this.Controls.Add(this.btnRunSlot);
            this.Controls.Add(this.btnEditSlots);
            this.Controls.Add(this.cmbSlots);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnTestSsh);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSubnet);
            this.Controls.Add(this.chkEnableFileLog);
            this.Controls.Add(this.chkSaveSettings);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSetInform);
            this.Controls.Add(this.lvDevices);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtManualIp);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtControllerUrl);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Unifi inform-tool 1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtControllerUrl;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox txtManualIp;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader Ip;
        private System.Windows.Forms.ColumnHeader Info;
        private System.Windows.Forms.Button btnSetInform;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox chkSaveSettings;
        private System.Windows.Forms.CheckBox chkEnableFileLog;
        private System.Windows.Forms.TextBox txtSubnet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnTestSsh;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ComboBox cmbSlots;
        private System.Windows.Forms.Button btnEditSlots;
        private System.Windows.Forms.Button btnRunSlot;
        private System.Windows.Forms.Label lblVerticalLine;
        private System.Windows.Forms.Label label8;
    }
}

