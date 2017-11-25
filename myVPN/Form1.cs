using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace myVPN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string FolderPath => string.Concat(Directory.GetCurrentDirectory(),
            "\\VPN");

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var sb = new StringBuilder();
            sb.AppendLine("[VPN]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Device=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVICE=vpn");
            sb.AppendLine("PhoneNumber=" + txtHost.Text);

            File.WriteAllText(FolderPath + "\\VpnConnection.pbk", sb.ToString());

            sb = new StringBuilder();
            sb.AppendLine("rasdial \"VPN\" " + txtUsrname.Text + " " + txtPassword.Text + " /phonebook:\"" + FolderPath +
                          "\\VpnConnection.pbk\"");

            File.WriteAllText(FolderPath + "\\VpnConnection.bat", sb.ToString());

            var newProcess = new Process
            {
                StartInfo =
                {
                    FileName = FolderPath + "\\VpnConnection.bat",
                    WindowStyle = ProcessWindowStyle.Normal
                }
            };

            newProcess.Start();
            newProcess.WaitForExit();
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            File.WriteAllText(FolderPath + "\\VpnDisconnect.bat", "rasdial /d");

            var newProcess = new Process
            {
                StartInfo =
                {
                    FileName = FolderPath + "\\VpnDisconnect.bat",
                    WindowStyle = ProcessWindowStyle.Normal
                }
            };

            newProcess.Start();
            newProcess.WaitForExit();
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }
    }
}