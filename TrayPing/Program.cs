using System;
using System.Net.NetworkInformation;

namespace TrayPing
{
    class Program {
        private static NotifyIcon trayIcon;
        private static ToolStripMenuItem autolaunchMenuItem;

        static void Main()
        {
            trayIcon = new NotifyIcon
            {
                ContextMenuStrip = new TrayMenu(),
                Visible = true
            };

            Autolaunch.Register();
            string menuText = Autolaunch.IsEnabled() ? "Disable autolaunch" : "Enable autolaunch";
            autolaunchMenuItem = new ToolStripMenuItem(menuText, null, ToggleAutolaunch);
            
            trayIcon.ContextMenuStrip.Items.Add(autolaunchMenuItem);
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            trayIcon.ContextMenuStrip.Items.Add("About", null, ShowAbout);
            trayIcon.ContextMenuStrip.Items.Add("Quit", null, (s, e) => Application.Exit());

            _ = UpdatePingLoop();

            Application.Run();
        }

        static async Task UpdatePingLoop()
        {
            while (true)
            {
                long ping = GetPing("www.google.com");
                CreateTextIcon(ping >= 0 ? ping.ToString() : "Er" );
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        static void CreateTextIcon(string text) 
        {
            Font font = new("TTRounds", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.White);
            Bitmap bitmapText = new(16, 16);
            Graphics graphics = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, font, brush, -4, -2);
           
            hIcon = bitmapText.GetHicon();
            trayIcon.Icon = Icon.FromHandle(hIcon);
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


        static void ToggleAutolaunch(object sender, EventArgs e)
        {
            bool enable = !Autolaunch.IsEnabled();
            Autolaunch.Toggle(enable);
            autolaunchMenuItem.Text = enable ? "Disable autolaunch" : "Enable autolaunch";
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
