using System;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace TrayPing
{
    class Program {
        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        private static extern bool ShouldSystemUseDarkMode();

        private static NotifyIcon trayIcon;

        static void Main()
        {
            trayIcon = new NotifyIcon
            {
                ContextMenuStrip = new TrayMenu(),
            };
            
            trayIcon.ContextMenuStrip.Items.Add("Preferences", null, (s, e) => new PreferencesForm().Show());
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            trayIcon.ContextMenuStrip.Items.Add("About", null, ShowAbout);
            trayIcon.ContextMenuStrip.Items.Add("Quit", null, (s, e) => Application.Exit());

            Autolaunch.Register();

            _ = UpdatePingLoop();

            bool createdNew;
            using(var mutex = new System.Threading.Mutex(true, "TrayPing", out createdNew))
            {
                if (createdNew)
                {
                    trayIcon.Visible = true;
                    Application.Run();
                }
                else
                {
                    MessageBox.Show(
                        "There is already an instance running",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        static void CreateTextIcon(string text) 
        {
            Font font = new("TTRounds", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(ShouldSystemUseDarkMode() ? Color.White : Color.Black);
            Bitmap bitmapText = new(16, 16);
            Graphics graphics = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, font, brush, -4, -2);
           
            hIcon = bitmapText.GetHicon();
            trayIcon.Icon = Icon.FromHandle(hIcon);
        }

        static async Task UpdatePingLoop()
        {
            while (true)
            {
                long ping = GetPing(Properties.Settings.Default.TargetHost);
                CreateTextIcon(ping >= 0 ? ping.ToString() : "Er" );
                await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.PingFrequency));
            }
        }


        static long GetPing(string host)
        {
            try
            {
                Ping ping = new();
                PingReply reply = ping.Send(host);
                if (reply.Status == IPStatus.Success)
                {
                    return reply.RoundtripTime;
                }
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        static void ShowAbout(object sender, EventArgs e)
        {
            MessageBox.Show(
                "TrayPing v.1.0\nPing status tray app",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    } 
}
