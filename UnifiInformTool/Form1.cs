using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace UnifiInformTool
{
    public partial class Form1 : Form
    {
        private string settingsPath;
        private string logDirectory;
        private List<SlotConfig> slots;

        public Form1()
        {
            InitializeComponent();

            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "UnifiInformTool",
                "settings.json");

            // Loggfilen sparas i samma mapp som appen
            logDirectory = Application.StartupPath;

            numPort.Value = 22;

            lvDevices.Columns.Clear();
            lvDevices.Columns.Add("IP", 100);
            lvDevices.Columns.Add("Device Type", 120);
            lvDevices.Columns.Add("Info", 200);
            lvDevices.CheckBoxes = true;

            LoadSlots();
            LoadSettingsSafe();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set ComboBox to only allow selection from list
            if (cmbSlots != null)
            {
                cmbSlots.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void txtManualIp_TextChanged(object sender, EventArgs e)
        {
            // Event-handler för txtManualIp TextChanged (kan vara tom)
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            // Event-handler för toolTip1 Popup (kan vara tom)
        }

        private void lvDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Event-handler för lvDevices SelectedIndexChanged (can be empty)
        }

        private void LoadSlots()
        {
            try
            {
                string slotsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "UnifiInformTool",
                    "slots.json");

                if (File.Exists(slotsPath))
                {
                    var json = File.ReadAllText(slotsPath, Encoding.UTF8);
                    slots = JsonSerializer.Deserialize<List<SlotConfig>>(json) ?? new List<SlotConfig>();
                }
                else
                {
                    slots = new List<SlotConfig>();
                }

                // Refresh ComboBox if it exists
                if (cmbSlots != null)
                {
                    cmbSlots.DropDownStyle = ComboBoxStyle.DropDownList; // Only allow selection from list, no typing
                    cmbSlots.Items.Clear();
                    cmbSlots.Items.Add("Select Slot");
                    foreach (var slot in slots)
                    {
                        cmbSlots.Items.Add(slot.Name);
                    }
                    cmbSlots.SelectedIndex = 0;
                }
            }
            catch
            {
                slots = new List<SlotConfig>();
            }
        }

        private void btnEditSlots_Click(object sender, EventArgs e)
        {
            using (var slotEditor = new SlotEditorForm())
            {
                slotEditor.ShowDialog(this);
                LoadSlots(); // Reload slots after editing
            }
        }

        private void btnRunSlot_Click(object sender, EventArgs e)
        {
            if (cmbSlots == null || cmbSlots.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a slot from the dropdown.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedSlot = slots[cmbSlots.SelectedIndex - 1]; // -1 because first item is "-- Select Slot --"

            if (string.IsNullOrWhiteSpace(selectedSlot.Ip) || string.IsNullOrWhiteSpace(selectedSlot.InformUrl))
            {
                MessageBox.Show("Selected slot is missing required information (IP or Inform URL).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedSlot.Username) || string.IsNullOrWhiteSpace(selectedSlot.Password))
            {
                MessageBox.Show("Selected slot is missing SSH credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Run set-inform with slot configuration
            RunSetInformWithSlot(selectedSlot);
        }

        private async void RunSetInformWithSlot(SlotConfig slot)
        {
            string controllerUrl = slot.InformUrl.Trim();

            // Remove "set-inform" from the beginning if user typed it
            if (controllerUrl.StartsWith("set-inform ", StringComparison.OrdinalIgnoreCase))
            {
                controllerUrl = controllerUrl.Substring("set-inform ".Length).Trim();
            }

            string username = slot.Username;
            string password = slot.Password;
            int port = slot.Port;
            string host = slot.Ip;

            txtLog.Clear();
            AppendLog($"Starting set-inform for slot: {slot.Name}");

            var targets = new List<string> { host };

            SetProgressMaximum(targets.Count);

            await Task.Run(() =>
            {
                foreach (var targetHost in targets)
                {
                    try
                    {
                        AppendLog($"[{targetHost}] Connecting via SSH...");
                        using (var client = new SshClient(targetHost, port, username, password))
                        {
                            client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
                            client.Connect();

                            if (!client.IsConnected)
                            {
                                AppendLogError($"[{targetHost}] Could not connect.");
                            }
                            else
                            {
                                // Quote URL to ensure entire URL is sent correctly
                                string quotedUrl = $"\"{controllerUrl}\"";

                                // Try mca-cli-op first (most common method)
                                string cmdText = $"mca-cli-op set-inform {quotedUrl}";

                                // Run set-inform first time
                                AppendLog($"[{targetHost}] Running: {cmdText} (first time)");
                                var cmd = client.CreateCommand(cmdText);
                                string result = cmd.Execute();
                                AppendLog($"Output: {result.Trim()}");

                                // If it failed, try regular set-inform
                                if (result.Contains("command not found") || result.Contains("not found"))
                                {
                                    AppendLogWarning($"[{targetHost}] mca-cli-op not found, trying set-inform directly...");
                                    cmdText = $"set-inform {quotedUrl}";
                                    cmd = client.CreateCommand(cmdText);
                                    result = cmd.Execute();
                                    AppendLog($"Output: {result.Trim()}");
                                }

                                // Wait a bit
                                System.Threading.Thread.Sleep(2000);

                                // Run set-inform second time (often required for UniFi)
                                AppendLog($"[{targetHost}] Running: {cmdText} (second time)");
                                cmd = client.CreateCommand(cmdText);
                                result = cmd.Execute();
                                AppendLog($"Output: {result.Trim()}");

                                // Wait a bit more for device to process
                                System.Threading.Thread.Sleep(2000);

                                // Verify with info command (try both info and mca-cli-op info)
                                AppendLog($"[{targetHost}] Verifying status...");
                                string infoResult = "";

                                // Try mca-cli-op info first
                                var infoCmd = client.CreateCommand("mca-cli-op info");
                                infoResult = infoCmd.Execute();

                                // If it didn't work, try regular info
                                if (string.IsNullOrWhiteSpace(infoResult) || infoResult.Contains("command not found"))
                                {
                                    infoCmd = client.CreateCommand("info");
                                    infoResult = infoCmd.Execute();
                                }

                                AppendLog($"Info output: {infoResult.Trim()}");

                                // Check if controller is set correctly
                                // Remove quotes and do case-insensitive comparison
                                string cleanControllerUrl = controllerUrl.Trim('"', '\'');
                                string infoResultLower = infoResult.ToLower();
                                string cleanControllerUrlLower = cleanControllerUrl.ToLower();

                                // Extract IP and port from URL for extra verification
                                bool urlFound = infoResultLower.Contains(cleanControllerUrlLower);
                                bool hasInformUrl = infoResultLower.Contains("inform") || infoResultLower.Contains("set-inform");
                                bool hasAdopted = infoResultLower.Contains("adopted") || infoResultLower.Contains("connected");

                                // Try to find IP address in info output
                                bool ipFound = false;
                                try
                                {
                                    var uri = new Uri(cleanControllerUrl);
                                    string hostPart = uri.Host + ":" + uri.Port;
                                    ipFound = infoResultLower.Contains(hostPart.ToLower());
                                }
                                catch { }

                                if (urlFound || (hasInformUrl && (hasAdopted || ipFound)))
                                {
                                    AppendLogSuccess($"[{targetHost}] ✓ set-inform verified - device is connected to controller!");
                                }
                                else
                                {
                                    AppendLogWarning($"[{targetHost}] ⚠ set-inform executed, but verification is uncertain. Check manually with 'info'.");
                                }

                                client.Disconnect();
                                AppendLogSuccess($"[{targetHost}] Complete.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLogError($"[{targetHost}] ERROR: {ex.Message}");
                    }
                    finally
                    {
                        IncrementProgress();
                    }
                }
            });

            AppendLogSuccess("set-inform execution complete.");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            using (var helpForm = new HelpForm())
            {
                helpForm.ShowDialog(this);
            }
        }

        #region Inställningar

        private class AppSettings
        {
            public string ControllerUrl { get; set; }
            public string LastSubnetOrIp { get; set; }
            public string SshUsername { get; set; }
            public bool SaveSettings { get; set; }
            public bool EnableFileLog { get; set; }
            public bool UseScanMode { get; set; }
        }

        private void LoadSettingsSafe()
        {
            try
            {
                if (!File.Exists(settingsPath)) return;

                var json = File.ReadAllText(settingsPath, Encoding.UTF8);
                var s = JsonSerializer.Deserialize<AppSettings>(json);
                if (s == null) return;

                chkSaveSettings.Checked = s.SaveSettings;
                chkEnableFileLog.Checked = s.EnableFileLog;

                if (!s.SaveSettings) return;

                txtControllerUrl.Text = s.ControllerUrl ?? "";
                txtSubnet.Text = s.LastSubnetOrIp ?? "";
                txtManualIp.Text = s.LastSubnetOrIp ?? "";
                txtUsername.Text = s.SshUsername ?? "";
            }
            catch
            {
                // Ignorera fel vid läsning
            }
        }

        private void SaveSettingsSafe()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));

                var s = new AppSettings
                {
                    ControllerUrl = txtControllerUrl.Text,
                    LastSubnetOrIp = !string.IsNullOrWhiteSpace(txtSubnet.Text) ? txtSubnet.Text : txtManualIp.Text,
                    SshUsername = txtUsername.Text,
                    SaveSettings = chkSaveSettings.Checked,
                    EnableFileLog = chkEnableFileLog.Checked,
                    UseScanMode = false // Not used anymore, kept for compatibility
                };

                var json = JsonSerializer.Serialize(s, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(settingsPath, json, Encoding.UTF8);
            }
            catch
            {
                // Ignorera fel vid sparning
            }
        }

        #endregion

        #region Hjälpfunktioner logg med färgkodning

        private void AppendLog(string message, Color? color = null)
        {
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AppendLogInternal(line, color)));
                return;
            }

            AppendLogInternal(line, color);
        }

        private void AppendLogInternal(string line, Color? color = null)
        {
            // Om txtLog är RichTextBox, använd färgkodning
            if (txtLog is RichTextBox rtb)
            {
                rtb.SelectionStart = rtb.TextLength;
                rtb.SelectionLength = 0;
                rtb.SelectionColor = color ?? Color.Black;
                rtb.AppendText(line + Environment.NewLine);
                rtb.SelectionColor = Color.Black; // Återställ till svart
                rtb.ScrollToCaret();
            }
            else
            {
                // Fallback om det är vanlig TextBox
                txtLog.AppendText(line + Environment.NewLine);
            }

            if (chkEnableFileLog.Checked)
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                    string logFile = Path.Combine(logDirectory,
                        $"log_{DateTime.Now:yyyy-MM-dd}.txt");
                    File.AppendAllText(logFile, line + Environment.NewLine, Encoding.UTF8);
                }
                catch
                {
                    // Ignorera loggfel
                }
            }
        }

        private void AppendLogSuccess(string message)
        {
            AppendLog(message, Color.Green);
        }

        private void AppendLogError(string message)
        {
            AppendLog(message, Color.Red);
        }

        private void AppendLogWarning(string message)
        {
            AppendLog(message, Color.Orange);
        }

        private void SetProgressMaximum(int max)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => SetProgressMaximum(max)));
                return;
            }
            progressBar.Value = 0;
            progressBar.Maximum = max > 0 ? max : 1;
        }

        private void IncrementProgress()
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(IncrementProgress));
                return;
            }

            if (progressBar.Value < progressBar.Maximum)
                progressBar.Value++;
        }

        #endregion

        #region Nätverksskanning

        private async void button1_Click(object sender, EventArgs e)
        {
            string subnetCidr = txtSubnet.Text.Trim();
            if (string.IsNullOrWhiteSpace(subnetCidr))
            {
                MessageBox.Show("Enter a subnet, e.g. 192.168.1.0/24.");
                return;
            }

            if (!TryParseCidr(subnetCidr, out var baseAddress, out int prefixLength))
            {
                MessageBox.Show("Invalid subnet format. Use e.g. 192.168.1.0/24.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            int port = (int)numPort.Value;
            bool hasCredentials = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);

            txtLog.Clear();
            AppendLog($"Scanning subnet {subnetCidr}...");

            lvDevices.Items.Clear();

            var ipsToScan = GetIpRange(baseAddress, prefixLength);
            SetProgressMaximum(ipsToScan.Count);
            AppendLog($"Number of addresses to test: {ipsToScan.Count}");

            var foundIps = new List<IPAddress>();

            await Task.Run(() =>
            {
                Parallel.ForEach(ipsToScan, ip =>
                {
                    try
                    {
                        using (var ping = new Ping())
                        {
                            var reply = ping.Send(ip, 500);
                            if (reply != null && reply.Status == IPStatus.Success)
                            {
                                if (IsPortOpen(ip, port, 500))
                                {
                                    lock (foundIps)
                                    {
                                        foundIps.Add(ip);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // ignore individual errors
                    }
                    IncrementProgress();
                });
            });

            AppendLog($"Ping/port check complete. Found {foundIps.Count} addresses with port {port} open.");

            var deviceInfo = new Dictionary<string, string>();

            // Only identify device types if SSH credentials are provided
            if (hasCredentials)
            {
                AppendLog("Identifying device types via SSH...");
                SetProgressMaximum(foundIps.Count);

                await Task.Run(() =>
                {
                    Parallel.ForEach(foundIps, ip =>
                    {
                        try
                        {
                            string deviceType = IdentifyDeviceType(ip.ToString(), port, username, password);
                            lock (deviceInfo)
                            {
                                deviceInfo[ip.ToString()] = deviceType;
                            }
                        }
                        catch
                        {
                            // ignore errors
                        }
                        IncrementProgress();
                    });
                });
            }
            else
            {
                AppendLog("SSH credentials not provided. Device types will be shown as 'Unknown'.");
            }

            // Add devices to list
            foreach (var ip in foundIps)
            {
                string ipStr = ip.ToString();
                var item = new ListViewItem(ipStr);

                if (deviceInfo.ContainsKey(ipStr) && !string.IsNullOrWhiteSpace(deviceInfo[ipStr]))
                {
                    item.SubItems.Add(deviceInfo[ipStr]);
                    item.SubItems.Add("UniFi device identified");
                }
                else
                {
                    item.SubItems.Add("Unknown");
                    item.SubItems.Add("SSH port open (possible UniFi device)");
                }

                lvDevices.Items.Add(item);
            }

            AppendLogSuccess("Scan complete.");
        }

        private string IdentifyDeviceType(string host, int port, string username, string password)
        {
            try
            {
                using (var client = new SshClient(host, port, username, password))
                {
                    client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(3);
                    client.Connect();

                    if (!client.IsConnected) return "Connection failed";

                    // Try to get device info
                    var cmd = client.CreateCommand("mca-cli-op info");
                    string result = cmd.Execute();

                    if (string.IsNullOrWhiteSpace(result) || result.Contains("command not found"))
                    {
                        cmd = client.CreateCommand("info");
                        result = cmd.Execute();
                    }

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        // Parse model from info output
                        var lines = result.Split('\n');
                        foreach (var line in lines)
                        {
                            if (line.StartsWith("Model:", StringComparison.OrdinalIgnoreCase))
                            {
                                string model = line.Substring("Model:".Length).Trim();
                                client.Disconnect();
                                return model;
                            }
                        }
                    }

                    client.Disconnect();
                    return "UniFi device (model unknown)";
                }
            }
            catch
            {
                return "SSH failed";
            }
        }

        private bool TryParseCidr(string cidr, out IPAddress baseAddress, out int prefixLength)
        {
            baseAddress = null;
            prefixLength = 0;

            var parts = cidr.Split('/');
            if (parts.Length != 2) return false;
            if (!IPAddress.TryParse(parts[0], out baseAddress)) return false;
            if (!int.TryParse(parts[1], out prefixLength)) return false;
            if (prefixLength < 0 || prefixLength > 32) return false;
            return true;
        }

        private List<IPAddress> GetIpRange(IPAddress baseAddress, int prefixLength)
        {
            var ips = new List<IPAddress>();

            uint baseAddrInt = IpToUint(baseAddress);
            int hostBits = 32 - prefixLength;
            uint hostCount = (uint)(1 << hostBits);
            uint network = baseAddrInt & (~(hostCount - 1));

            for (uint i = 1; i < hostCount - 1; i++)
            {
                uint ipInt = network + i;
                ips.Add(UintToIp(ipInt));
            }
            return ips;
        }

        private uint IpToUint(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private IPAddress UintToIp(uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return new IPAddress(bytes);
        }

        private bool IsPortOpen(IPAddress ip, int port, int timeoutMs)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(ip, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeoutMs);
                    if (!success) return false;
                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Testa SSH-anslutning

        private async void btnTestSsh_Click(object sender, EventArgs e)
        {
            string host = "";

            // Check if subnet is filled (scan mode)
            if (!string.IsNullOrWhiteSpace(txtSubnet.Text.Trim()) && lvDevices.Items.Count > 0)
            {
                if (lvDevices.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a device from the list first.");
                    return;
                }
                host = lvDevices.SelectedItems[0].Text;
            }
            else
            {
                host = txtManualIp.Text.Trim();
                if (string.IsNullOrWhiteSpace(host))
                {
                    MessageBox.Show("Enter IP/hostname or scan network first.");
                    return;
                }
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            int port = (int)numPort.Value;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Enter SSH username and password.");
                return;
            }

            AppendLog($"Testing SSH connection to {host}...");

            await Task.Run(() =>
            {
                try
                {
                    using (var client = new SshClient(host, port, username, password))
                    {
                        client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
                        client.Connect();

                        if (client.IsConnected)
                        {
                            // Test running a simple command
                            var cmd = client.CreateCommand("echo 'SSH OK'");
                            string result = cmd.Execute();
                            AppendLogSuccess($"[{host}] SSH connection successful!");
                            AppendLog($"Output: {result.Trim()}");
                            client.Disconnect();
                        }
                        else
                        {
                            AppendLogError($"[{host}] Could not connect via SSH.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendLogError($"[{host}] SSH error: {ex.Message}");
                }
            });
        }

        #endregion

        #region SSH set-inform med verifiering

        private async void button2_Click(object sender, EventArgs e)
        {
            string controllerUrl = txtControllerUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(controllerUrl))
            {
                MessageBox.Show("Enter controller URL.");
                return;
            }

            // Remove "set-inform" from the beginning if user typed it
            if (controllerUrl.StartsWith("set-inform ", StringComparison.OrdinalIgnoreCase))
            {
                controllerUrl = controllerUrl.Substring("set-inform ".Length).Trim();
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            int port = (int)numPort.Value;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Enter SSH username and password.");
                return;
            }

            txtLog.Clear();
            AppendLog("Starting set-inform...");

            SaveSettingsSafe();

            var targets = new List<string>();

            // Check if subnet is filled and devices are selected (scan mode)
            if (!string.IsNullOrWhiteSpace(txtSubnet.Text.Trim()) && lvDevices.CheckedItems.Count > 0)
            {
                foreach (ListViewItem item in lvDevices.CheckedItems)
                {
                    targets.Add(item.Text);
                }
            }
            else if (!string.IsNullOrWhiteSpace(txtManualIp.Text.Trim()))
            {
                // Manual IP mode
                string ip = txtManualIp.Text.Trim();
                targets.Add(ip);
            }
            else
            {
                MessageBox.Show("Either scan network and select devices, or enter manual IP/hostname.");
                return;
            }

            SetProgressMaximum(targets.Count);

            await Task.Run(() =>
            {
                foreach (var host in targets)
                {
                    try
                    {
                        AppendLog($"[{host}] Connecting via SSH...");
                        using (var client = new SshClient(host, port, username, password))
                        {
                            client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
                            client.Connect();

                            if (!client.IsConnected)
                            {
                                AppendLogError($"[{host}] Could not connect.");
                            }
                            else
                            {
                                // Quote URL to ensure entire URL is sent correctly
                                string quotedUrl = $"\"{controllerUrl}\"";

                                // Try mca-cli-op first (most common method)
                                string cmdText = $"mca-cli-op set-inform {quotedUrl}";

                                // Run set-inform first time
                                AppendLog($"[{host}] Running: {cmdText} (first time)");
                                var cmd = client.CreateCommand(cmdText);
                                string result = cmd.Execute();
                                AppendLog($"Output: {result.Trim()}");

                                // If it failed, try regular set-inform
                                if (result.Contains("command not found") || result.Contains("not found"))
                                {
                                    AppendLogWarning($"[{host}] mca-cli-op not found, trying set-inform directly...");
                                    cmdText = $"set-inform {quotedUrl}";
                                    cmd = client.CreateCommand(cmdText);
                                    result = cmd.Execute();
                                    AppendLog($"Output: {result.Trim()}");
                                }

                                // Wait a bit
                                System.Threading.Thread.Sleep(2000);

                                // Run set-inform second time (often required for UniFi)
                                AppendLog($"[{host}] Running: {cmdText} (second time)");
                                cmd = client.CreateCommand(cmdText);
                                result = cmd.Execute();
                                AppendLog($"Output: {result.Trim()}");

                                // Wait a bit more for device to process
                                System.Threading.Thread.Sleep(2000);

                                // Verify with info command (try both info and mca-cli-op info)
                                AppendLog($"[{host}] Verifying status...");
                                string infoResult = "";

                                // Try mca-cli-op info first
                                var infoCmd = client.CreateCommand("mca-cli-op info");
                                infoResult = infoCmd.Execute();

                                // If it didn't work, try regular info
                                if (string.IsNullOrWhiteSpace(infoResult) || infoResult.Contains("command not found"))
                                {
                                    infoCmd = client.CreateCommand("info");
                                    infoResult = infoCmd.Execute();
                                }

                                AppendLog($"Info output: {infoResult.Trim()}");

                                // Check if controller is set correctly
                                // Remove quotes and do case-insensitive comparison
                                string cleanControllerUrl = controllerUrl.Trim('"', '\'');
                                string infoResultLower = infoResult.ToLower();
                                string cleanControllerUrlLower = cleanControllerUrl.ToLower();

                                // Extract IP and port from URL for extra verification
                                bool urlFound = infoResultLower.Contains(cleanControllerUrlLower);
                                bool hasInformUrl = infoResultLower.Contains("inform") || infoResultLower.Contains("set-inform");
                                bool hasAdopted = infoResultLower.Contains("adopted") || infoResultLower.Contains("connected");

                                // Try to find IP address in info output
                                bool ipFound = false;
                                try
                                {
                                    var uri = new Uri(cleanControllerUrl);
                                    string hostPart = uri.Host + ":" + uri.Port;
                                    ipFound = infoResultLower.Contains(hostPart.ToLower());
                                }
                                catch { }

                                if (urlFound || (hasInformUrl && (hasAdopted || ipFound)))
                                {
                                    AppendLogSuccess($"[{host}] ✓ set-inform verified - device is connected to controller!");
                                }
                                else
                                {
                                    AppendLogWarning($"[{host}] ⚠ set-inform executed, but verification is uncertain. Check manually with 'info'.");
                                }

                                client.Disconnect();
                                AppendLogSuccess($"[{host}] Complete.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLogError($"[{host}] ERROR: {ex.Message}");
                    }
                    finally
                    {
                        IncrementProgress();
                    }
                }
            });

            AppendLogSuccess("set-inform execution complete.");
        }

        #endregion
    }
}
