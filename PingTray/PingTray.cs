using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PingTray
{
    public partial class PingTrayForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr window, int index, int value);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr window, int index);


// ReSharper disable InconsistentNaming
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOOLWINDOW = 0x00000080;
// ReSharper restore InconsistentNaming

        NotifyIcon _notifyIcon;
        List<String> _pingHistory;
        String _pingHistoryString;
        long _lastReplyTime;
        Thread _doPingThread;
        String _pingAdresString;
        Color _textColor = Color.LightGreen;

        private void GetIcon(NotifyIcon notiIcon, string text)
        {
            var bitmap = new Bitmap(32, 32);

            int fontsize;
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
            var drawFont = new Font("Calibri", fontsize, FontStyle.Bold);
            var drawBrush = new SolidBrush(_textColor);

            var graphics = Graphics.FromImage(bitmap);

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

        private void ParseArgs(IEnumerable<string> args)
        {
            String currentArgType = "";
            foreach (String arg in args)
            {
                switch (currentArgType)
                {
                    case "ip":
                        _pingAdresString = arg;
                        currentArgType = "";
                        break;
                    case "color":
                        Color textColorFromArg = Color.FromName(arg);
                        if (textColorFromArg.IsKnownColor)
                            _textColor = textColorFromArg;
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
        
        public PingTrayForm(IEnumerable<string> args)
        {
            ParseArgs(args);
            InitializeComponent();                        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing || MessageBox.Show("Точно закрыть?", "Выход", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                _doPingThread.Abort();
                _notifyIcon.Visible = false;    
            }
            else
            {
                e.Cancel = true;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.MouseClick += notifyIcon_MouseClick;

            _notifyIcon.Visible = true;

            if(_pingAdresString == null)
                _pingAdresString = "ya.ru";
            pingAdres.Text = _pingAdresString;            

            _pingHistory = new List<String>();

            _doPingThread = new Thread(DoPingLoop);
            _doPingThread.Start();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            int windowStyle = GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);

        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{       
                //_notifyIcon.ShowBalloonTip(10000);
            //}

            //if (e.Button == MouseButtons.Left)
            //{
                if (this.WindowState == FormWindowState.Minimized)
                {                    
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                    int windowStyle = GetWindowLong(Handle, GWL_EXSTYLE);
                    SetWindowLong(Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
                }

            //}
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                int windowStyle = GetWindowLong(Handle, GWL_EXSTYLE);
                SetWindowLong(Handle, GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
            }
        }

        private void DoPing()
        {
            if (_pingAdresString == "")
                return;

            var ping = new Ping();

            long replyTime = 0;

            String replyString = "";

            try
            {
                PingReply reply = ping.Send(_pingAdresString);

                if (reply.Status == IPStatus.Success)
                    replyTime = reply.RoundtripTime;

                replyString = "Время ответа: " + Convert.ToString(replyTime);

                if (replyTime == 0)
                    replyString += "(" + reply.Status.ToString() + ")";
            
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    replyString = ex.InnerException.Message;
            }

            
            _lastReplyTime = replyTime;

            
            _pingHistory.Add(replyString);

            if (_pingHistory.Count > 20)
            {
                _pingHistory.RemoveAt(0);
            }

            _pingHistoryString = "";

            foreach (var pingHistoryElement in _pingHistory)
            {
                if (_pingHistoryString != "")
                    _pingHistoryString += Environment.NewLine;

                _pingHistoryString += pingHistoryElement;

            }
        }

        private void DoPingLoop()
        {
            while (true)
            {
                DoPing();
                Thread.Sleep(500);
            }
// ReSharper disable once FunctionNeverReturns
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {       
            //_notifyIcon.BalloonTipText = _pingHistoryString;            
            GetIcon(_notifyIcon, Convert.ToString(_lastReplyTime));
            tbPingHistory.Text = _pingHistoryString;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _pingAdresString = pingAdres.Text;
            pingAdres.ForeColor = SystemColors.WindowText;
            btnApply.Visible = false;
        }

        private void pingAdres_TextChanged(object sender, EventArgs e)
        {
            if (pingAdres.Text != _pingAdresString)
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
