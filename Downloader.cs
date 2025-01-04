using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fModLauncher
{
    public static class Downloader
    {
        public static bool isDownloading = false;
        public static string[] modsData = { };
        public static bool result = false;
        public static int Percentage = 0;
        private static string current_file = "";

        async public static void GetModList()
        {
            await Task.Run(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1YJCeL7WkJ3Bvozk93RX78C_lhcSOJS-D"), "modlist.txt");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleteCall);
                void DownloadCompleteCall(object sender, AsyncCompletedEventArgs e) { modsData = File.ReadAllLines("modlist.txt"); result = true; }
            });
        }

        async public static void DownloadFileFromGoogleDriveAsync(string file_name, string file_id)
        {
            await Task.Run(() =>
            {
                isDownloading = true; mainWindow.StatusText = "Скачиваем файлы..."; Percentage = 0;
                WebClient webClient = new WebClient();
                webClient.DownloadFileAsync(new Uri($"https://drive.google.com/uc?export=download&id={file_id}"), $"{Path.GetTempPath()}{file_name}.tmp");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(UpdateProgressBar);
                void UpdateProgressBar(object sender, DownloadProgressChangedEventArgs e)
                {
                    Percentage = e.ProgressPercentage;
                    if (Percentage > 0)
                        Percentage -= 1;
                }
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleteCall);
                void DownloadCompleteCall(object sender, AsyncCompletedEventArgs e) { AddToUnpack(file_name); }
            });
        }
        async static Task UnpackTask()
        {
            await Task.Run(() =>
            {
                string zip_path = Path.GetTempPath() + "7za.exe";
                try
                {
                    ProcessStartInfo process = new ProcessStartInfo();
                    process.WindowStyle = ProcessWindowStyle.Hidden;
                    process.FileName = zip_path;
                    process.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", $"{Path.GetTempPath()}{current_file}.tmp", "resources");
                    Process x = Process.Start(process);
                    x.WaitForExit();
                }
                catch
                {
                    MessageBox.Show("Неудалось распаковать архив.", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
        async static void AddToUnpack(string file_name)
        {
            await Task.Run(() =>
            {
                current_file = file_name; mainWindow.StatusText = "Распаковываем архив";
                var task = Task.Run(UnpackTask); task.Wait();
                Percentage = 100;
                File.Delete($"{Path.GetTempPath()}{file_name}.tmp");
                isDownloading = false; result = true; mainWindow.StatusText = "Мод успешно загружен и добавлен в библиотеку";
            });

        }
    }
}
