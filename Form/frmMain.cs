using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Skybound.Gecko;       //Skybound.Gecko.dll,拖到引用中去,然后也拖到工具箱中去(可以新建一个GeckoFX工具箱选项卡)

/// <summary>
/// http://www.m5home.com/blog/archives/2015/07/87.html
/// </summary>
namespace JingsuPlatform
{
    public partial class frmMain : Form
    {
        string m_LoadState = "-";

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        public ComboBox resolution { get; set; }
        public Button captureBtn { get; set; }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern System.IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("gdi32.dll")]
        private static extern int BitBlt(
            IntPtr hdcDest,     // handle to destination DC (device context)
            int nXDest,         // x-coord of destination upper-left corner
            int nYDest,         // y-coord of destination upper-left corner
            int nWidth,         // width of destination rectangle
            int nHeight,        // height of destination rectangle
            IntPtr hdcSrc,      // handle to source DC
            int nXSrc,          // x-coordinate of source upper-left corner
            int nYSrc,          // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
        );

        public frmMain()
        {
            InitializeComponent();
            Xpcom.Initialize(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"xulrunner");
            //初始化xulrunner目录,必须的

            captureBtn = new Button();
            captureBtn.Text = "截图";
            captureBtn.Left = 0;
            captureBtn.Top = 0;
            captureBtn.Width = 150;
            captureBtn.Height = 25;
            captureBtn.Click += CaptureBtn_Click;
            captureBtn.MouseUp += CaptureBtn_MouseUp;
            this.Controls.Add(captureBtn);
            this.Controls.SetChildIndex(captureBtn, 0);

            resolution = new ComboBox();
            resolution.Items.Add("1366x768");
            resolution.Items.Add("1920x1080");
            resolution.Items.Add("2560x1440");
            resolution.Items.Add("3840x2160");
            resolution.Items.Add("7680x4320");
            resolution.Items.Add("128x72");
            resolution.Items.Add("自定义...");
            resolution.Left = 150;
            resolution.Top = 0;
            resolution.Width = 100;
            resolution.Height = 30;
            resolution.DropDownStyle = ComboBoxStyle.DropDownList;
            resolution.SelectedIndex = 0;
            resolution.SelectedIndexChanged += Resolution_SelectedIndexChanged;
            this.Controls.Add(resolution);
            this.Controls.SetChildIndex(resolution, 0);

        }

        private void CaptureBtn_MouseUp(object sender, MouseEventArgs e)
        {
            return;
        }

        private void invokeCapture()
        {
            try
            {
                Bitmap b = GetWindow(FindWindow(null, "改口 小花仙登录器 V0.1 BY 鄙人 贴吧/B站：wujiuqier Github：No5972"));
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "PNG图片 (*.png)|*.png";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    b.Save(s.FileName);
                    MessageBox.Show(s.FileName + "保存成功。文件大小：" + decimal.Round(
                        decimal.Divide(decimal.Parse(File.ReadAllBytes(s.FileName).Length.ToString()), decimal.Parse("1048576"))
                        ,2) + "MB", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show("截图过程中出现了未知错误：" + e.Message);
                return;
            }

            
        }

        private void CaptureBtn_Click(object sender, EventArgs e)
        {
            invokeCapture();
        }

        private void Resolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (resolution.SelectedItem)
            {
                case "128x72":
                    resizeWindow(128, 72);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "1366x768":
                    resizeWindow(1366, 768);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "1920x1080":
                    resizeWindow(1920, 1080);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "2560x1440":
                    resizeWindow(2560, 1440);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "3840x2160":
                    resizeWindow(3840, 2160);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "7680x4320":
                    resizeWindow(7680, 4320);
                    // browser.ExecuteScriptAsync("document.getElementsByTagName('embed')[0].Zoom(100);");
                    break;
                case "自定义...":
                    CustomResolution resolution = new CustomResolution(this.Width, this.Height);
                    if (resolution.ShowDialog() == DialogResult.OK)
                    {
                        resizeWindow(resolution.thisWidth, resolution.thisHeight);
                        resolution.Dispose();
                    }
                    break;
                default: break;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate("http://hua.61.com/Client.swf?platform=winform&timestamp="  + DateTime.Now.ToString());
        }

        private void cmdGo_Click(object sender, EventArgs e)
        {
            // geckoWebBrowser1.Navigate(textBox1.Text);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                // cmdGo.PerformClick();
            }
        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, EventArgs e)
        {
            // this.Text = "加载完成。";
            // timer1.Enabled = false;
        }

        private void geckoWebBrowser1_Navigating(object sender, GeckoNavigatingEventArgs e)
        {
            // timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (m_LoadState)
            {
                case "-":
                    m_LoadState = "\\";
                    break;
                case "\\":
                    m_LoadState = "/";
                    break;
                case "/":
                    m_LoadState = "-";
                    break;
            }

            this.Text = "正在加载。。。。" + m_LoadState;
        }

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct POINTAPI
        {
            public int x;
            public int y;
        }

        public struct MINMAXINFO
        {
            public POINTAPI ptReserved;
            public POINTAPI ptMaxSize;
            public POINTAPI ptMaxPosition;
            public POINTAPI ptMinTrackSize;
            public POINTAPI ptMaxTrackSize;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_GETMINMAXINFO = 0x24;
            if (m.Msg == WM_GETMINMAXINFO)
            {
                MINMAXINFO mmi = (MINMAXINFO)m.GetLParam(typeof(MINMAXINFO));
                mmi.ptMinTrackSize.x = this.Size.Width;
                mmi.ptMinTrackSize.y = this.Size.Height;
                Marshal.StructureToPtr(mmi, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        private void resizeWindow(int width, int height)
        {
            const int SWP_NOSENDCHANGING = 0x0400;

            Process[] processes = Process.GetProcesses(".");
            foreach (var process in processes)
            {
                var handle = process.MainWindowHandle;
                var form = Control.FromHandle(handle);

                if (form == null) continue;

                RECT windowRect = new RECT();
                GetWindowRect(this.Handle, ref windowRect);
                SetWindowPos(handle, 0, this.Left, this.Top, width, height, SWP_NOSENDCHANGING);
            }
        }

        public static Bitmap GetWindow(IntPtr hWnd)
        {
            IntPtr hscrdc = GetWindowDC(hWnd);
            Control control = Control.FromHandle(hWnd);
            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, control.Width, control.Height);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            DeleteDC(hscrdc);//删除用过的对象
            DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
       string lpszDriver,        // driver name驱动名
       string lpszDevice,        // device name设备名
       string lpszOutput,        // not used; should be NULL
       IntPtr lpInitData  // optional printer data
       );
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(
         IntPtr hdcDest, // handle to destination DC目标设备的句柄
         int nXDest,  // x-coord of destination upper-left corner目标对象的左上角的X坐标
         int nYDest,  // y-coord of destination upper-left corner目标对象的左上角的Y坐标
         int nWidth,  // width of destination rectangle目标对象的矩形宽度
         int nHeight, // height of destination rectangle目标对象的矩形长度
         IntPtr hdcSrc,  // handle to source DC源设备的句柄
         int nXSrc,   // x-coordinate of source upper-left corner源对象的左上角的X坐标
         int nYSrc,   // y-coordinate of source upper-left corner源对象的左上角的Y坐标
         UInt32 dwRop  // raster operation code光栅的操作值
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,        // handle to DC
         int nWidth,     // width of bitmap, in pixels
         int nHeight     // height of bitmap, in pixels
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
         IntPtr hdc,          // handle to DC
         IntPtr hgdiobj   // handle to object
         );

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
         IntPtr hdc          // handle to DC
         );

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,               // Window to copy,Handle to the window that will be copied. 
         IntPtr hdcBlt,             // HDC to print into,Handle to the device context. 
         UInt32 nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values. 
         );

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );

    }
}
