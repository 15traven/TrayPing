using System.Net.NetworkInformation;
using System.Windows.Forms.PropertyGridInternal;

namespace TrayPing
{
    class Program {
        private static NotifyIcon trayIcon;

        static void Main()
        {
            trayIcon = new NotifyIcon
            {
                Visible = true,

                ContextMenuStrip = new ContextMenuStrip()
            };
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
            Font font = new Font("TTRounds", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
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
    } 
}