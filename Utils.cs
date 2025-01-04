using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace fModLauncher
{
    public static class Utils
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        static private string RegistryKey = "fModLoader";
        static RegistryKey Software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
        static bool AutoInject = false;
        static bool AutoHide = false;
        static int InjectDelay = 100; // * 100 (ms)

        static string tokey(string id) => RegistryKey + "_" + id;

        public static void InitializeRegistry()
        {
            try
            {
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey);
                AutoInject = Convert.ToBoolean((int)GeneralKey.GetValue(tokey("AutoInject")));
                AutoHide = Convert.ToBoolean((int)GeneralKey.GetValue(tokey("AutoHide")));
                //InjectDelay = (int)GeneralKey.GetValue(tokey("AutoInjectDelay"));
                GeneralKey.Close();
            }
            catch
            {
                Software.CreateSubKey(RegistryKey);
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey, true);
                GeneralKey.SetValue(tokey("AutoInject"), false, RegistryValueKind.DWord);
                GeneralKey.SetValue(tokey("AutoHide"), false, RegistryValueKind.DWord);
                //GeneralKey.SetValue(tokey("AutoInjectDelay"), 0, RegistryValueKind.DWord);
                GeneralKey.Close();
            }
        }

        public static int GetDelayValue() { return InjectDelay; }
        /*public static void SetDelayValue(int delay)
        {
            InjectDelay = delay;
            try
            {
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey, true);
                GeneralKey.SetValue(tokey("AutoInjectDelay"), delay, RegistryValueKind.DWord); GeneralKey.Close();
            }
            catch { }
        }*/
        public static bool GetAutoInject() { return AutoInject; }
        public static void SetAutoInject(bool state)
        {
            AutoInject = state;
            try
            {
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey, true);
                GeneralKey.SetValue(tokey("AutoInject"), state, RegistryValueKind.DWord); GeneralKey.Close();
            }
            catch { }
        }
        public static bool GetAutoHide() { return AutoHide; }
        public static void SetAutoHide(bool state)
        {
            AutoHide = state;
            try
            {
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey, true);
                GeneralKey.SetValue(tokey("AutoHide"), state, RegistryValueKind.DWord); GeneralKey.Close();
            }
            catch { }
        }

        public static bool IsProcessRunning(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length > 0)
                return true;
            return false;
        }

        public static string GetGamePath(string game)
        {
            Process[] processes = Process.GetProcessesByName(game);
            if (processes.Length > 0)
            {
                string GamePath = processes[0].MainModule.FileName;
                try
                {
                    RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey, true);
                    GeneralKey.SetValue(tokey("GamePath"), GamePath); GeneralKey.Close();
                }
                catch { }
                return GamePath;
            }
            return "";
        }
        public static string GetRegistryPath()
        {
            try
            {
                RegistryKey GeneralKey = Software.OpenSubKey(RegistryKey);
                string GamePath = (string)GeneralKey.GetValue(tokey("GamePath")); GeneralKey.Close();
                return GamePath;
            }
            catch { return ""; }
        }

        public static void CopyDir(string from, string to)
        {
            Directory.CreateDirectory(to);
            foreach (string s1 in Directory.GetFiles(from))
            {
                string s2 = to + @"\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(from))
            {
                CopyDir(s, to + @"\" + Path.GetFileName(s));
            }
        }

        public static void AddListItem(ListBox list_box, string dir, string name, bool check)
        {
            if (!File.Exists(dir + @"\config.fml") & check)
                name = "* " + name + "(не найден конфигурационный файл)";
            list_box.Items.Add(name);
        }

        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public static Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
    }
}
