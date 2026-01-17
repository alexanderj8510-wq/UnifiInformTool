using System;
using System.Drawing;
using System.Windows.Forms;

namespace UnifiInformTool
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Help - UniFi Inform Tool";
            this.Size = new Size(700, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // TabControl for different help sections
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(670, 490);
            this.Controls.Add(tabControl);

            // Tab 1: Using Slots
            TabPage tabSlots = new TabPage("Using Slots");
            RichTextBox rtbSlots = new RichTextBox();
            rtbSlots.Dock = DockStyle.Fill;
            rtbSlots.ReadOnly = true;
            rtbSlots.Font = new Font("Segoe UI", 9);
            rtbSlots.Text = @"METHOD 1: USING SLOTS (RECOMMENDED FOR REPEATED USE)

1. EDIT SLOTS:
   • Click 'Edit Slots' button
   • Fill in the following information:
     - Name: A descriptive name for this slot (e.g., 'Office AP1')
     - Username: SSH username
     - Password: SSH password
     - IP/Hostname: Device IP address or hostname
     - Port: SSH port (default: 22)
     - Inform URL: Controller URL (e.g., http://192.168.1.2:8080/inform)
   • Click 'Save Slot' to save the configuration
   • You can create multiple slots for different devices/environments
   • Click 'Delete Slot' to remove unwanted slots

2. RUN SLOT:
   • Select a slot from the dropdown menu
   • Click 'Run Slot' button
   • The set-inform command will execute automatically with saved settings
   • No need to fill in any fields manually!

BENEFITS:
   • Save time for frequently configured devices
   • Keep credentials organized and secure
   • Quick switching between different devices/environments";
            tabSlots.Controls.Add(rtbSlots);
            tabControl.TabPages.Add(tabSlots);

            // Tab 2: Manual Configuration
            TabPage tabManual = new TabPage("Manual Configuration");
            RichTextBox rtbManual = new RichTextBox();
            rtbManual.Dock = DockStyle.Fill;
            rtbManual.ReadOnly = true;
            rtbManual.Font = new Font("Segoe UI", 9);
            rtbManual.Text = @"METHOD 2: MANUAL CONFIGURATION

1. CONTROLLER URL:
   • Enter your UniFi controller URL
   • Example: http://192.168.1.2:8080/inform
   • Do NOT include 'set-inform' - just the URL

2. NETWORK SCANNING:
   • Enter subnet in 'Subnet' field (e.g., 192.168.1.0/24)
   • Click 'Scan' button
   • Devices with open SSH port will appear in the list
   • Device types will be identified if SSH credentials are provided
   • Check the boxes next to devices you want to configure
   • Note: Scanning works without SSH credentials, but device types will show as 'Unknown'

3. MANUAL IP:
   • Enter IP address or hostname in 'Manual IP' field
   • No need to scan - just enter the IP directly
   • Works immediately without network scanning

4. SSH CREDENTIALS:
   • Enter SSH username and password
   • Default port is 22 (change if needed)
   • Required for: Test SSH, Run set-inform, and device type identification
   • Optional for network scanning (but recommended for device identification)

5. TEST SSH:
   • Click 'Test SSH' to verify SSH connection
   • Useful to check credentials before running set-inform
   • Works with scanned devices or manual IP

6. RUN SET-INFORM:
   • For scanned devices: Check boxes next to devices, then click 'Run set-inform'
   • For manual IP: Enter IP, then click 'Run set-inform'
   • The command will be executed twice automatically (required for UniFi)
   • Status will be verified automatically";
            tabManual.Controls.Add(rtbManual);
            tabControl.TabPages.Add(tabManual);

            // Tab 3: Settings & Tips
            TabPage tabSettings = new TabPage("Settings & Tips");
            RichTextBox rtbSettings = new RichTextBox();
            rtbSettings.Dock = DockStyle.Fill;
            rtbSettings.ReadOnly = true;
            rtbSettings.Font = new Font("Segoe UI", 9);
            rtbSettings.Text = @"SETTINGS & LOGGING

SAVE LAST SETTINGS:
   • Check this box to automatically save your current settings
   • Settings are restored when you restart the application
   • Includes: Controller URL, Subnet/IP, Username, and preferences

WRITE LOG TO FILE:
   • Check this box to save logs to a file
   • Log files are saved in the same directory as the application
   • Filename format: log_YYYY-MM-DD.txt
   • Useful for troubleshooting and keeping records

LOG FILE LOCATION:
   • Log files are saved in the application directory
   • Each day gets its own log file
   • Easy to find and review later

═══════════════════════════════════════════════════════
TIPS & TRICKS
═══════════════════════════════════════════════════════

• Use Slots for devices you configure frequently - saves time!
• Network scanning works without SSH credentials, but you won't see device types
• The set-inform command runs twice automatically (UniFi requirement)
• Check the log output for detailed status and troubleshooting
• Color coding in log:
  - Green = Success
  - Red = Error
  - Orange = Warning

TROUBLESHOOTING:
• If set-inform fails, check SSH credentials with 'Test SSH'
• Verify controller URL is correct and accessible
• Make sure device is on the same network
• Check firewall settings if connection fails";
            tabSettings.Controls.Add(rtbSettings);
            tabControl.TabPages.Add(tabSettings);

            // Close button
            Button btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 30);
            btnClose.Location = new Point(580, 510);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }
    }
}
