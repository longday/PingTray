using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication1
{
    public partial class PingTrayForm : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        NotifyIcon notifyIcon;
        List<String> pingHistory;
        String pingHistoryString;
        long lastReplyTime;
        Thread doPingThread;
        String pingAdresString;
        Color textColor = Color.LightGreen;

        private void GetIcon(NotifyIcon notiIcon, string text)
        {
            Bitmap bitmap = new Bitmap(32, 32);

            int fontsize = 0;
            switch (text.Length)
            {
                case 0:
                case 1:
                case 2:
                    fontsize = 16;
                    break;
                case 3:
                    fontsize = 15;
                    break;
                default:
                    fontsize = 16;
                    break;
            }
            System.Drawing.Font drawFont = new System.Drawing.Font("Calibri", fontsize, FontStyle.Bold);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(textColor);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString(text, drawFont, drawBrush, 1, 2);

            Icon createdIcon = Icon.FromHandle(bitmap.GetHicon());

            notiIcon.Icon = createdIcon;

            DestroyIcon(createdIcon.Handle);
     
            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();
        }

        private void ParseArgs(String[] args)
        {
            String currentArgType = "";
            foreach (String arg in args)
            {
                switch (currentArgType)
                {
                    case "ip":
                        pingAdresString = arg;
                        currentArgType = "";
                        break;
                    case "color":
                        Color textColorFromArg = Color.FromName(arg);
                        if (textColorFromArg.IsKnownColor)
                            textColor = textColorFromArg;
                        currentArgType = "";
                        break;
                }

                switch (arg)
                {
                    case "/ip":
                        currentArgType = "ip";
                        break;
                    case "/color":
                        currentArgType = "color";
                        break;
                }
            }
        }
        
        public PingTrayForm(String []args)
        {
            ParseArgs(args);
            InitializeComponent();                        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            doPingThread.Abort();
            notifyIcon.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);

            notifyIcon.Visible = true;

            if(pingAdresString == null)
                pingAdresString = "ya.ru";
            pingAdres.Text = pingAdresString;            

            pingHistory = new List<String>();

            doPingThread = new Thread(DoPingLoop);
            doPingThread.Start();         
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {       
                notifyIcon.ShowBalloonTip(10000);
            };

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {                    
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                }

            };

            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
        }

        private void DoPing()
        {
            if (pingAdresString == "")
                return;

            Ping ping = new Ping();

            long replyTime = 0;
            String errorString = "";

            try
            {
                PingReply reply = ping.Send(pingAdresString);

                replyTime = reply.RoundtripTime;
            }
            catch (Exception ex)
            {
                errorString = ex.InnerException.Message;
            }

            String ReplyString = Convert.ToString(replyTime);

            lastReplyTime = replyTime;

            if (replyTime == 0)
                ReplyString += "(" + errorString + ")";

            pingHistory.Add(ReplyString);

            if (pingHistory.Count > 20)
            {
                pingHistory.RemoveAt(0);
            }

            pingHistoryString = "";

            foreach (String pingHistoryElement in pingHistory)
            {
                if (pingHistoryString != "")
                    pingHistoryString += Environment.NewLine;

                pingHistoryString += pingHistoryElement;

            }
        }

        private void DoPingLoop()
        {
            while (true)
            {
                DoPing();
                Thread.Sleep(500);
            }
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {       
            notifyIcon.BalloonTipText = pingHistoryString;            
            GetIcon(notifyIcon, Convert.ToString(lastReplyTime));        
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            pingAdresString = pingAdres.Text;
            pingAdres.ForeColor = SystemColors.WindowText;
            btnApply.Visible = false;
        }

        private void pingAdres_TextChanged(object sender, EventArgs e)
        {
            if (pingAdres.Text != pingAdresString)
            {
                pingAdres.ForeColor = Color.Gray;
                btnApply.Visible = true;
            }
            else
            {
                pingAdres.ForeColor = SystemColors.WindowText;
                btnApply.Visible = false;
            }

        }
    }
}
