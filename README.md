# UniFi Inform Tool

A Windows desktop application that simplifies the process of setting the inform URL on UniFi devices when adopting them to a new controller. This tool helps resolve devices stuck in "adopting" state by connecting via SSH and executing the `set-inform` command.

## SmartScreen Warning

When downloading and running the executable for the first time, Windows SmartScreen may show a warning that the app is from an "Unknown publisher". This is normal for unsigned applications.

**To run the app:**
1. Click "More info"
2. Click "Run anyway"

The app is safe to use - the source code is available for review on GitHub.

## Features

- **Slot Management**: Save and manage multiple device configurations for quick reuse
- **Network Scanning**: Automatically scan your network to discover UniFi devices with SSH port open
- **Device Type Identification**: Automatically identify device types (e.g., UAP-AC-Mesh) via SSH
- **Manual IP Configuration**: Directly enter IP addresses or hostnames without scanning
- **SSH Connection Testing**: Verify SSH credentials before running set-inform
- **Automatic Verification**: Automatically runs set-inform twice and verifies the result (required by UniFi)
- **Color-Coded Logging**: Visual status indicators (Green = Success, Red = Error, Orange = Warning)
- **Settings Persistence**: Save your configuration for next use
- **File Logging**: Optional log file creation for troubleshooting
- **Comprehensive Help**: Built-in help system with categorized information

## Purpose

When adopting UniFi devices to a new controller, devices sometimes get stuck in the "adopting" state. This application simplifies the process by:

1. Automatically connecting to devices via SSH
2. Executing the `set-inform` command with the correct controller URL
3. Running the command twice (as required by UniFi devices)
4. Verifying that the device is properly connected to the controller

## Requirements

- Windows 10/11
- .NET Framework 4.8 or later (usually pre-installed on Windows 10/11)
- SSH access to UniFi devices
- UniFi devices with SSH enabled

## Building from Source

1. Open the project in Visual Studio (Community edition is free)
2. Ensure you have the following NuGet packages installed:
   - `SSH.NET` (Renci.SshNet)
   - `System.Text.Json`
3. Build the solution in Release mode
4. The executable will be in `bin\Release\UnifiInformTool.exe`

## Usage

### Method 1: Using Slots (Recommended)

1. Click **"Edit Slots"** to create saved configurations
2. Fill in device information (Name, Username, Password, IP, Port, Inform URL)
3. Click **"Save Slot"**
4. Select a slot from the dropdown menu
5. Click **"Run Slot"** to execute set-inform with saved settings

### Method 2: Manual Configuration

1. Enter the **Controller URL** (e.g., `http://192.168.1.2:8080/inform`)
2. Choose one of two options:
   - **Network Scanning**: Enter subnet (e.g., `192.168.1.0/24`) and click "Scan"
   - **Manual IP**: Enter device IP address or hostname directly
3. Enter **SSH credentials** (Username, Password, Port)
4. Optionally click **"Test SSH"** to verify connection
5. Select devices (if scanned) or use manual IP
6. Click **"Run set-inform"** to configure devices

## Technical Details

- **Framework**: .NET Framework 4.8 (Windows Forms)
- **SSH Library**: SSH.NET (Renci.SshNet)
- **Configuration Storage**: JSON files in `%AppData%\UnifiInformTool\`
- **Log Files**: Saved in application directory (if enabled)
- **Executable Size**: ~5-15 MB (without .NET runtime)

## File Structure

```
UnifiInformTool/
├── Form1.cs                    # Main application form
├── SlotEditorForm.cs           # Slot configuration form
├── HelpForm.cs                 # Help and documentation form
└── README.md                   # This file
```

## Configuration Files

- **Settings**: `%AppData%\UnifiInformTool\settings.json`
- **Slots**: `%AppData%\UnifiInformTool\slots.json`
- **Logs**: `{ApplicationDirectory}\log_YYYY-MM-DD.txt` (if enabled)

## Features in Detail

### Network Scanning
- Scans specified subnet (CIDR notation, e.g., `192.168.1.0/24`)
- Identifies devices with open SSH port (default: 22)
- Optionally identifies device types via SSH (requires credentials)
- Displays results in a list with checkboxes for multi-selection

### Slot System
- Save frequently used device configurations
- Quick switching between different devices/environments
- Stores: Name, Username, Password, IP, Port, Inform URL
- Easy management with edit and delete options

### SSH Operations
- Secure SSH connections to UniFi devices
- Supports custom SSH ports
- Connection testing before set-inform execution
- Automatic retry and error handling

### Verification
- Automatically verifies set-inform success using `info` command
- Checks for controller URL in device output
- Color-coded status indicators in log

## Troubleshooting

**Device not found during scan:**
- Verify SSH port is open (default: 22)
- Check firewall settings
- Ensure device is on the same network

**SSH connection fails:**
- Verify username and password are correct
- Check if SSH is enabled on the device
- Verify port number is correct

**set-inform doesn't work:**
- Test SSH connection first using "Test SSH" button
- Verify controller URL is correct and accessible
- Check device network connectivity
- Review log output for detailed error messages

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Disclaimer

This tool is not officially affiliated with Ubiquiti Inc. or UniFi. Use at your own risk.

## Screenshots

*Screenshots would go here - feel free to add them!*

---

**Note**: The application requires .NET Framework 4.8 or later, which is typically pre-installed on Windows 10/11. If you encounter runtime errors, you may need to install the .NET Desktop Runtime from Microsoft's website.
