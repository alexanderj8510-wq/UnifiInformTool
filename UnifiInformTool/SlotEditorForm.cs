using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace UnifiInformTool
{
    public partial class SlotEditorForm : Form
    {
        private List<SlotConfig> slots;
        private string slotsPath;
        private ListBox lbSlots;
        private TextBox txtSlotName;
        private TextBox txtSlotUsername;
        private TextBox txtSlotPassword;
        private TextBox txtSlotIp;
        private NumericUpDown numSlotPort;
        private TextBox txtSlotInformUrl;
        private Button btnSaveSlot;
        private Button btnDeleteSlot;
        private Button btnClose;

        public SlotEditorForm()
        {
            slotsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "UnifiInformTool",
                "slots.json");
            LoadSlots();
            SetupUI();
        }

        private void SetupUI()
        {
            // Set form properties
            this.Text = "Edit Slots";
            this.Size = new Size(600, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create UI elements programmatically
            // ListBox for slots
            lbSlots = new ListBox();
            lbSlots.Location = new Point(10, 10);
            lbSlots.Size = new Size(200, 350);
            lbSlots.SelectedIndexChanged += LbSlots_SelectedIndexChanged;
            this.Controls.Add(lbSlots);

            // Labels and controls
            Label lblName = new Label();
            lblName.Text = "Name:";
            lblName.Location = new Point(220, 10);
            lblName.Size = new Size(100, 20);
            this.Controls.Add(lblName);

            txtSlotName = new TextBox();
            txtSlotName.Location = new Point(220, 30);
            txtSlotName.Size = new Size(350, 20);
            this.Controls.Add(txtSlotName);

            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(220, 60);
            lblUsername.Size = new Size(100, 20);
            this.Controls.Add(lblUsername);

            txtSlotUsername = new TextBox();
            txtSlotUsername.Location = new Point(220, 80);
            txtSlotUsername.Size = new Size(350, 20);
            this.Controls.Add(txtSlotUsername);

            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(220, 110);
            lblPassword.Size = new Size(100, 20);
            this.Controls.Add(lblPassword);

            txtSlotPassword = new TextBox();
            txtSlotPassword.Location = new Point(220, 130);
            txtSlotPassword.Size = new Size(350, 20);
            txtSlotPassword.PasswordChar = '*';
            this.Controls.Add(txtSlotPassword);

            Label lblIp = new Label();
            lblIp.Text = "IP/Hostname:";
            lblIp.Location = new Point(220, 160);
            lblIp.Size = new Size(100, 20);
            this.Controls.Add(lblIp);

            txtSlotIp = new TextBox();
            txtSlotIp.Location = new Point(220, 180);
            txtSlotIp.Size = new Size(350, 20);
            this.Controls.Add(txtSlotIp);

            Label lblPort = new Label();
            lblPort.Text = "Port:";
            lblPort.Location = new Point(220, 210);
            lblPort.Size = new Size(100, 20);
            this.Controls.Add(lblPort);

            numSlotPort = new NumericUpDown();
            numSlotPort.Location = new Point(220, 230);
            numSlotPort.Size = new Size(100, 20);
            numSlotPort.Minimum = 1;
            numSlotPort.Maximum = 65535;
            numSlotPort.Value = 22;
            this.Controls.Add(numSlotPort);

            Label lblInformUrl = new Label();
            lblInformUrl.Text = "Inform URL:";
            lblInformUrl.Location = new Point(220, 260);
            lblInformUrl.Size = new Size(100, 20);
            this.Controls.Add(lblInformUrl);

            txtSlotInformUrl = new TextBox();
            txtSlotInformUrl.Location = new Point(220, 280);
            txtSlotInformUrl.Size = new Size(350, 20);
            this.Controls.Add(txtSlotInformUrl);

            // Buttons
            btnSaveSlot = new Button();
            btnSaveSlot.Text = "Save Slot";
            btnSaveSlot.Location = new Point(220, 320);
            btnSaveSlot.Size = new Size(100, 30);
            btnSaveSlot.Click += BtnSaveSlot_Click;
            this.Controls.Add(btnSaveSlot);

            btnDeleteSlot = new Button();
            btnDeleteSlot.Text = "Delete Slot";
            btnDeleteSlot.Location = new Point(330, 320);
            btnDeleteSlot.Size = new Size(100, 30);
            btnDeleteSlot.Click += BtnDeleteSlot_Click;
            this.Controls.Add(btnDeleteSlot);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new Point(470, 320);
            btnClose.Size = new Size(100, 30);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Add new slot button
            Button btnNewSlot = new Button();
            btnNewSlot.Text = "New Slot";
            btnNewSlot.Location = new Point(10, 370);
            btnNewSlot.Size = new Size(200, 30);
            btnNewSlot.Click += BtnNewSlot_Click;
            this.Controls.Add(btnNewSlot);

            RefreshSlotList();
        }

        private void RefreshSlotList()
        {
            lbSlots.Items.Clear();
            foreach (var slot in slots)
            {
                lbSlots.Items.Add(slot.Name);
            }
        }

        private void LbSlots_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSlots.SelectedIndex >= 0 && lbSlots.SelectedIndex < slots.Count)
            {
                var slot = slots[lbSlots.SelectedIndex];
                txtSlotName.Text = slot.Name;
                txtSlotUsername.Text = slot.Username;
                txtSlotPassword.Text = slot.Password;
                txtSlotIp.Text = slot.Ip;
                numSlotPort.Value = slot.Port;
                txtSlotInformUrl.Text = slot.InformUrl;
            }
        }

        private void BtnNewSlot_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtSlotName.Focus();
        }

        private void BtnSaveSlot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSlotName.Text))
            {
                MessageBox.Show("Please enter a name for the slot.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSlotIp.Text) || string.IsNullOrWhiteSpace(txtSlotInformUrl.Text))
            {
                MessageBox.Show("Please enter IP and Inform URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var slot = new SlotConfig
            {
                Name = txtSlotName.Text.Trim(),
                Username = txtSlotUsername.Text.Trim(),
                Password = txtSlotPassword.Text,
                Ip = txtSlotIp.Text.Trim(),
                Port = (int)numSlotPort.Value,
                InformUrl = txtSlotInformUrl.Text.Trim()
            };

            // Check if slot with this name already exists
            int existingIndex = slots.FindIndex(s => s.Name.Equals(slot.Name, StringComparison.OrdinalIgnoreCase));
            if (existingIndex >= 0 && (lbSlots.SelectedIndex < 0 || existingIndex != lbSlots.SelectedIndex))
            {
                var result = MessageBox.Show($"A slot named '{slot.Name}' already exists. Overwrite?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
                slots[existingIndex] = slot;
            }
            else if (lbSlots.SelectedIndex >= 0)
            {
                // Update existing slot
                slots[lbSlots.SelectedIndex] = slot;
            }
            else
            {
                // Add new slot
                slots.Add(slot);
            }

            SaveSlots();
            RefreshSlotList();

            // Select the saved slot
            int index = slots.FindIndex(s => s.Name == slot.Name);
            if (index >= 0)
                lbSlots.SelectedIndex = index;

            MessageBox.Show("Slot saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteSlot_Click(object sender, EventArgs e)
        {
            if (lbSlots.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a slot to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{slots[lbSlots.SelectedIndex].Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                slots.RemoveAt(lbSlots.SelectedIndex);
                SaveSlots();
                RefreshSlotList();
                ClearFields();
                MessageBox.Show("Slot deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearFields()
        {
            txtSlotName.Text = "";
            txtSlotUsername.Text = "";
            txtSlotPassword.Text = "";
            txtSlotIp.Text = "";
            numSlotPort.Value = 22;
            txtSlotInformUrl.Text = "";
            lbSlots.SelectedIndex = -1;
        }

        private void LoadSlots()
        {
            try
            {
                if (File.Exists(slotsPath))
                {
                    var json = File.ReadAllText(slotsPath, Encoding.UTF8);
                    slots = JsonSerializer.Deserialize<List<SlotConfig>>(json) ?? new List<SlotConfig>();
                }
                else
                {
                    slots = new List<SlotConfig>();
                }
            }
            catch
            {
                slots = new List<SlotConfig>();
            }
        }

        private void SaveSlots()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(slotsPath));
                var json = JsonSerializer.Serialize(slots, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(slotsPath, json, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Failed to save slots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<SlotConfig> GetSlots()
        {
            return slots;
        }
    }

    public class SlotConfig
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string InformUrl { get; set; }
    }
}
