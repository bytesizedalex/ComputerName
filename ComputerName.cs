using System.Windows;
using System.Net;
using System.Linq;

namespace PCName
{
    public partial class MainWindow : Window
    {
        IPAddress[] addresses;
        string internetConnectionStatus = "Failed";
        string internalConnectionStatus = "Failed"; /* This variable is used for the internal company resource URL */
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StatusIndicator_Initialized(object sender, System.EventArgs e)
        {

            StatusIndicator.Fill = System.Windows.Media.Brushes.Red;

            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://www.google.co.uk"))
                {
                    StatusIndicator.Fill = System.Windows.Media.Brushes.Orange;
                    internetConnectionStatus = "Successful";
                }
            }
            catch { }
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://mytesturl.mydomain.com/")) /* Use an internal company URL resource to test domain connectivity */
                {
                    StatusIndicator.Fill = System.Windows.Media.Brushes.Green;
                    internalConnectionStatus = "Successful";
                }
            }
            catch { }
        }

        private void MachineName_Initialized(object sender, System.EventArgs e)
        {
            MachineName.Text = (System.Environment.MachineName);
        }

        private void Domain_Initialized(object sender, System.EventArgs e)
        {
            Domain.Text = (System.Environment.UserDomainName);
        }

        private void IPs_Initialized(object sender, System.EventArgs e)
        {
            addresses = Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();

            foreach (object address in addresses)
            {
                IPs.Items.Add(address);
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string releaseId = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString();
            string copiedDetails = "Hostname: " + MachineName.Text + "\r\n" + "Domain Name: " + Domain.Text + "\r\n" + "Internet Connection Status: " + internetConnectionStatus + "\r\n" + "Internal Domain Connection Status: " + internalConnectionStatus + "\r\n" + "Windows 10 Release: " + releaseId + "\r\n";

            foreach (var ipAddress in addresses)
            {
                copiedDetails += "IP Address: " + ipAddress.ToString() + "\r\n";
            }

            try
            {
                Clipboard.SetText(copiedDetails);
            }
            catch { }
        }
    }
}
