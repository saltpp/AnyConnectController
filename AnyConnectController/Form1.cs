using System;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;   // for Process
using System.Threading; // for Thread
using System.Runtime.InteropServices;   // for Win32 API
using System.Security.Cryptography; // for ProtectedData

namespace AnyConnectController
{
    public partial class Form1 : Form
    {
        // for Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        // Delegate for EnumWindows callback function
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, [Out] char[] lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out] char[] lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MINIMIZE = 6;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        private IntPtr hWndMain = IntPtr.Zero;
        private IntPtr hWndAlreadyLaunched = IntPtr.Zero;


        public Form1()
        {
            InitializeComponent();

            // Change the window style to prevent resizing the window
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;

            // Add handler
            this.FormClosing += Form1_FormClosing;
            this.Load += Form1_Load;
            buttonConnect.Click += ButtonConnect_Click;
            buttonDisconnect.Click += ButtonDisconnect_Click;

            // Load password
            textBoxPassword.Text = DecryptPassword(ConfigurationManager.AppSettings[Constants.KEY_PASSWORD]);

            // Check if this app is already launched
            hWndAlreadyLaunched = FindWindow(null, "AnyConnectController");
            Debug.WriteLine("hWndAlreadyLaunched = {0}", hWndAlreadyLaunched);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Get the window handle
            hWndMain = this.Handle;
            Debug.WriteLine("Form1_Load " + hWndMain);

            // Set the caption text
            this.Text = "AnyConnectController Ver." + Application.ProductVersion;

            // Load window location
            int.TryParse(ConfigurationManager.AppSettings[Constants.KEY_WINDOW_POS_X], out int windowPosX);
            int.TryParse(ConfigurationManager.AppSettings[Constants.KEY_WINDOW_POS_Y], out int windowPosY);
            Debug.WriteLine("x, y = {0}, {1}", windowPosX, windowPosY);
            if (windowPosX < 0) windowPosX = 0;
            if (windowPosY < 0) windowPosY = 0;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width - this.Width;
            if (windowPosX > screenWidth) windowPosX = screenWidth;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height - this.Height;
            if (windowPosY > screenHeight) windowPosY = screenHeight;
            SetWindowPos(hWndMain, IntPtr.Zero, windowPosX, windowPosY, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW | SWP_NOZORDER);

            Boolean.TryParse(ConfigurationManager.AppSettings[Constants.KEY_ALWAYS_ON_TOP], out bool alwaysOnTop);
            checkBoxOnTop.Checked = !alwaysOnTop;   // To call handler
            checkBoxOnTop.Checked = alwaysOnTop;

            // Close this instance if this app is already launched
            if (hWndAlreadyLaunched != IntPtr.Zero)
            {
                SetForegroundWindow(hWndAlreadyLaunched);
                this.Close();
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save password and window location to App.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Password
            config.AppSettings.Settings[Constants.KEY_PASSWORD].Value = EncryptPassword(textBoxPassword.Text);

            // Window location
            config.AppSettings.Settings[Constants.KEY_WINDOW_POS_X].Value = this.Location.X.ToString();
            config.AppSettings.Settings[Constants.KEY_WINDOW_POS_Y].Value = this.Location.Y.ToString();

            // Always on top
            config.AppSettings.Settings[Constants.KEY_ALWAYS_ON_TOP].Value = checkBoxOnTop.Checked.ToString();

            config.Save();
        }


        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            Thread threadConenct = new Thread(ThreadConnect);
            threadConenct.Start();
        }


        private void ButtonDisconnect_Click(object sender, EventArgs e)
        {
            Thread threadDisconenct = new Thread(ThreadDisconnect);
            threadDisconenct.Start();
        }


        private void LinkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //リンク先に移動したことにする
            linkLabelGitHub.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://github.com/saltpp/AnyConnectController/blob/main/README.ja.md");
        }


        private void CheckBoxOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnTop.Checked)
            {
                Debug.WriteLine("checkBoxOnTop checked");
                SetWindowPos(hWndMain, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
            }
            else
            {
                Debug.WriteLine("checkBoxOnTop unchecked");
                SetWindowPos(hWndMain, new IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
            }
        }


        private string EncryptPassword(string password)
        {
            string strResult = "";
            try
            {
                byte[] encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), null, DataProtectionScope.CurrentUser);
                strResult = Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred: " + ex.Message);
            }

            return strResult;
        }


        private string DecryptPassword(string encryptedPassword)
        {
            string strResult = "";
            try
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedPassword);
                strResult = Encoding.UTF8.GetString(ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred: " + ex.Message);
            }

            return strResult;
        }


        private bool LaunchAnyConnect()
        {
            try
            {
                Debug.WriteLine("Launch AnyConnect");
                Process.Start(Constants.FULL_PATH_ANY_CONNECT);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred: " + ex.Message);
                return false;
            }

            return true;
        }


        // returns hWnd if found, IntPtr.Zero if not found
        private IntPtr FindAnyConnect()
        {
            int n = 100;    // to wait for launching
            IntPtr hWnd = IntPtr.Zero;
            while (--n > 0 && hWnd == IntPtr.Zero)
            {
                hWnd = FindWindow("#32770", "Cisco AnyConnect Secure Mobility Client");
                Debug.WriteLine("hWnd = " + hWnd.ToString("X"));
                Thread.Sleep(100);     // 100msec
            }

            return hWnd;
        }

        
        // returns hWnd if found, IntPtr.Zero if not found
        private IntPtr FindChildDialog(IntPtr hWndParent)
        {
            int n = 10;
            IntPtr hWnd = IntPtr.Zero;
            while (--n > 0 && hWnd == IntPtr.Zero)
            {
                hWnd = FindWindowEx(hWndParent, IntPtr.Zero, "#32770", "");
                Debug.WriteLine("hWnd ChildDialog = " + hWnd.ToString("X"));
                Thread.Sleep(100);     // 100msec
            }

            return hWnd;
        }


        // returns hWnd if found, IntPtr.Zero if not found
        private IntPtr FindChildButton(IntPtr hWndParent, string caption)
        {
            int n = 10;
            IntPtr hWnd = IntPtr.Zero;
            while (--n > 0 && hWnd == IntPtr.Zero)
            {
                hWnd = FindWindowEx(hWndParent, IntPtr.Zero, "Button", caption);
                Debug.WriteLine("hWnd Child Button = " + hWnd.ToString("X"));
                Thread.Sleep(100);     // 100msec
            }

            return hWnd;
        }


        private IntPtr FindWindowByClassAndCaptionForwardMatch(string lpClassName, string lpCaptionName)
        {
            IntPtr hWndResult = IntPtr.Zero;

            EnumWindows(
                (hWnd, lParam) => 
                {
                    // Check the class name and caption of the window
                    char[] className = new char[256];
                    char[] caption = new char[256];
                    GetClassName(hWnd, className, className.Length);
                    GetWindowText(hWnd, caption, caption.Length);

                    if (new string(className).Contains(lpClassName) && new string(caption).StartsWith(lpCaptionName))
                    {
                        hWndResult = hWnd;
                        return false; // Stop enumeration after finding the first matching window
                    }

                    return true; // Continue enumeration
                },
                IntPtr.Zero);


            return hWndResult;
        }


        private IntPtr FindEditBoxPassword(IntPtr hWndParent)
        {
            Debug.WriteLine("FindEditBoxPassword() hWndParent = " + hWndParent.ToString("X"));

            IntPtr hWndResult = IntPtr.Zero;
            int count = 0;

            EnumChildWindows(
                hWndParent,
                (hWnd, lParam) => 
                {
                    // Check the class name and caption of the child window
                    char[] className = new char[256];
                    char[] caption = new char[256];
                    GetClassName(hWnd, className, className.Length);
                    GetWindowText(hWnd, caption, caption.Length);
                    Debug.WriteLine("hWnd = " + hWnd.ToString("X"));
                    //Debug.WriteLine("classname = " + new string(className) + "\n");
                    //Debug.WriteLine("caption = [" + new string(caption) + "]\n");
                    //Debug.WriteLine("\n");
                    //Debug.WriteLine("caption.length = {0}", new string(caption).Length);
                    //Debug.WriteLine("caption[0] = {0} {1}", caption[0], (int) caption[0]);
                    //Debug.WriteLine("caption[1] = {0} {1}", caption[1], (int) caption[1]);
                    //Debug.WriteLine("caption[2] = {0} {1}", caption[2], (int) caption[2]);
                    //Debug.WriteLine("caption[3] = {0} {1}", caption[3], (int) caption[3]);
                    //Debug.WriteLine("caption[4] = {0} {1}", caption[4], (int) caption[4]);


                    // There are 2 Edit windows and both have no caption text...
                    if (new string(className).Contains("Edit") && ++count == 2)
                    {
                        hWndResult = hWnd;
                        return false; // Stop enumeration after finding the first matching child window
                    }

                    return true; // Continue enumeration
                },
                IntPtr.Zero);

            return hWndResult;
        }


        // Thread func for connection
        private void ThreadConnect()
        {
            // Launch AnyConnect
            if (!LaunchAnyConnect())
            {
                return;
            }


            // Find AnyConnect
            IntPtr hWnd = FindAnyConnect();
            if (hWnd == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find AnyConnect !");
#endif
                return;
            }


            // Find child dialog
            IntPtr hWndChildDialog = FindChildDialog(hWnd);
            if (hWndChildDialog == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find child dialog");
#endif
                return;
            }


            // Find connect button
            IntPtr hWndConnectButton = FindChildButton(hWndChildDialog, "Connect");
            if (hWndConnectButton == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find connect button");
#endif

                // Minimize window
                ShowWindow(hWnd, SW_MINIMIZE);

                return;
            }


            // Push connect button
            int n = 10;
            while (--n > 0 && !IsWindowEnabled(hWndConnectButton))  // Wait if the button is disabled
            {
                Thread.Sleep(1000); // 1sec
            }
            SendMessage(hWndConnectButton, 0x00F5 /* BM_CLICK */, IntPtr.Zero, IntPtr.Zero);


            // Find popup dialog
            n = 200; // 100;
            IntPtr hWndDialogPassword = IntPtr.Zero;
            while (--n > 0 && hWndDialogPassword == IntPtr.Zero)
            {
                hWndDialogPassword = FindWindowByClassAndCaptionForwardMatch("#32770", "Cisco AnyConnect |");
                Debug.WriteLine("hWndDialogPassword = " + hWndDialogPassword.ToString("X"));
                Thread.Sleep(100);     // 100msec
            }
            if (hWndDialogPassword == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find dialog for password");
#endif
                return;

            }


            // Find password box
            n = 10;
            IntPtr hWndEditBoxPassword = IntPtr.Zero;
            while (--n > 0 && hWndEditBoxPassword == IntPtr.Zero)
            {
                hWndEditBoxPassword = FindEditBoxPassword(hWndDialogPassword);
                Debug.WriteLine("hWndEditBoxPassword = " + hWndEditBoxPassword.ToString("X"));
                Thread.Sleep(100);     // 100msec
            }
            if (hWndEditBoxPassword == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find edit box for password");
#endif
                return;

            }


            // Send password
            SendMessage(hWndEditBoxPassword, 0x000C /* WM_SETTEXT */, 0, textBoxPassword.Text);


            // Find ok button
            IntPtr hWndButtonOk = FindChildButton(hWndDialogPassword, "OK");
            if (hWndButtonOk == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find ok button");
#endif
                return;

            }

            // Push ok button
            SendMessage(hWndButtonOk, 0x00F5 /* BM_CLICK */, IntPtr.Zero, IntPtr.Zero);
        }


        // Thread func for disconnection
        private void ThreadDisconnect()
        {
            // Launch AnyConnect
            if (!LaunchAnyConnect())
            {
                return;
            }


            // Find AnyConnect
            IntPtr hWnd = FindAnyConnect();
            if (hWnd == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find AnyConnect !");
#endif
                return;
            }


            // Find child dialog
            IntPtr hWndChildDialog = FindChildDialog(hWnd);
            if (hWndChildDialog == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find child dialog");
#endif
                return;
            }


            // Find disconenct button
            IntPtr hWndDisconnectButton = FindChildButton(hWndChildDialog, "Disconnect");
            if (hWndDisconnectButton == IntPtr.Zero)
            {
#if DEBUG
                MessageBox.Show("Couldn't find disconnect button");
#endif

                // Minimize window
                ShowWindow(hWnd, SW_MINIMIZE);

                return;
            }


            // Push disconnect button
            SendMessage(hWndDisconnectButton, 0x00F5 /* BM_CLICK */, IntPtr.Zero, IntPtr.Zero);


            // Minimize window
            ShowWindow(hWnd, SW_MINIMIZE);
        }
    }

}
