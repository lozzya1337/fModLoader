using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace fModLauncher
{
    public partial class mainWindow : Form
    {
#if DEBUG
        public static string BuildType = "development build";
#else
        public static string BuildType = "release";
#endif
        public static string BuildVersion = "0.18.2";

        public static string StatusText = "Здесь будет отображаться текущее действие";
        private string DirectoryName = "resources";
        private string GameProcess = "Forts";
        private bool IsMinimized = false;
        private int TicksDelay = 0;
        private bool GameLoaded = false;

        private string mod_name = "";
        private string[] ModsList = { };
        private string[] ModsData = { };
        private string[] FileIDS = { };
        private string[] clear_array = { };

        public mainWindow()
        {
            InitializeAddictives();
            InitializeComponent();
            Downloader.GetModList();
            Mods.RefreshModsList(DirectoryName);
        }

        private void RefreshInformation(object s, EventArgs e)
        {
            InformationLabel.Text = "Доступно/загружено модов: " + Mods.GetModsNumber(false) + "/" + Mods.GetModsNumber(true);
            AutoHideCheckbox.Checked = Utils.GetAutoHide();
            AutoInjectCheckbox.Checked = Utils.GetAutoInject();
            Text = "fModLoader " + BuildVersion + BuildType;

            AllowedModsList.Items.Clear();
            Utils.AddListItem(AllowedModsList, "", "Обновляем список...", false);
            // parsing modsData list
            ModsList = clear_array; ModsData = clear_array; FileIDS = clear_array;
            foreach (string mod_data in Downloader.modsData)
            {
                int data_s = mod_data.IndexOf(", data=");
                string preview = mod_data.Substring(10, data_s - 11);
                int id_s = mod_data.IndexOf(", id="); string data = mod_data.Substring(data_s + 8, id_s - 1 - (data_s + 8));
                string dw_id = mod_data.Substring(id_s + 6, mod_data.Length - 2 - (id_s + 6));
                Array.Resize(ref ModsList, ModsList.Length + 1);
                ModsList[ModsList.Length - 1] = preview;
                Array.Resize(ref ModsData, ModsData.Length + 1);
                ModsData[ModsData.Length - 1] = data;
                Array.Resize(ref FileIDS, FileIDS.Length + 1);
                FileIDS[FileIDS.Length - 1] = dw_id;
            }
            // populating allowed mods
            if (ModsList.Length > 0) { AllowedModsList.Items.Clear(); }
            foreach (string mod_name in ModsList)
            {
                Utils.AddListItem(AllowedModsList, "", mod_name, false);
            }

            // populating library list
            ModsLibraryList.Items.Clear();
            Utils.AddListItem(ModsLibraryList, "", "Здесь пока ничего нет :(", false);
            string[] mods = Mods.GetModList();
            if (mods.Length > 0) { ModsLibraryList.Items.Clear(); }
            foreach (string mod in mods)
            {
                Utils.AddListItem(ModsLibraryList, mod, Mods.GetModName(mod), true);
            }
        }

        private void AutoHideChange(object sender, EventArgs e) => Utils.SetAutoHide(AutoHideCheckbox.Checked);
        private void AutoInjectChange(object sender, EventArgs e) => Utils.SetAutoInject(AutoInjectCheckbox.Checked);

        private void GameLaunching(object s, EventArgs e)
        {
            if (!Utils.IsProcessRunning(GameProcess))
            {
                TicksDelay = 0;
                GameLoaded = false;
                if (IsMinimized)
                    Visible = true; /*WindowState = FormWindowState.Normal;*/ StatusText = "Открываемся"; IsMinimized = false;
                Mods.BackupMods();
                InjectButton.Enabled = true; GameLaunchTimer.Enabled = false;
            }
            else
            {
                TicksDelay += 1;
                if (AutoHideCheckbox.Checked)
                {
                    if (!IsMinimized)
                        Visible = false; /*WindowState = FormWindowState.Minimized;*/ StatusText = "Закрываемся"; IsMinimized = true;
                }
                if (TicksDelay >= Utils.GetDelayValue() && isGameLoaded.Enabled == false && GameLoaded == false && AutoInjectCheckbox.Checked)
                {
                    StatusText = "Ждем загрузку игры...";
                    isGameLoaded.Enabled = true;
                    //MessageBox.Show("Происходит автовнедрение\nПросьба не сворачивать окно с игрой!", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Utils.IsProcessRunning(GameProcess))
            {
                DialogResult Inject = MessageBox.Show("Вы уверены что хотите внедрить модификации?\nЕсли игра не загрузилась до конца, последующий запуск\nигры будет не возможен!", "fModLoader", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Inject == DialogResult.Yes)
                {
                    GameLoaded = true;
                    Mods.InjectMods(); GameLaunchTimer.Enabled = true;
                    InjectButton.Enabled = false;
                    StatusText = "Внедрение...";
                }
            }
            else
            {
                if (File.Exists(GamePath))
                {
                    Process.Start(GamePath);
                    StatusText = "Создан процесс игры";
                    GameLaunchTimer.Enabled = true;
                    if (AutoInjectCheckbox.Checked)
                        InjectButton.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Похоже вы запустили fModLoader впервые!\nПриложению не удается найти путь к игре.\nДействуйте вручную, следющий раз вы сможете запустить игру.", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (TabSelector.SelectedIndex == 0) { Mods.RefreshModsList(DirectoryName); RefreshInformation(sender, e); StatusText = "Информация о файлах обновлена"; }
            else
            {
                if (!Downloader.isDownloading)
                {
                    if (AllowedModsList.SelectedIndex >= 0)
                    {
                        mod_name = ModsList[AllowedModsList.SelectedIndex];
                        if (FileIDS[AllowedModsList.SelectedIndex] != "nan")
                            Downloader.DownloadFileFromGoogleDriveAsync(ModsData[AllowedModsList.SelectedIndex], FileIDS[AllowedModsList.SelectedIndex]);
                        else
                            MessageBox.Show("Данный мод не найден!\nВозможно он был удален с сервера.", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("Выберите мод из списка", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Дождитесь загрузки предыдущего файла.", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void by_xoireitwekt_Tick(object sender, EventArgs e)
        {
            StatusLabel.Text = StatusText;

            if (Utils.IsProcessRunning(GameProcess))
                InjectButton.Text = "Внедрить модификации";
            else
                InjectButton.Text = "Запустить игру";

            if (TabSelector.SelectedIndex == 0)
            { RefreshButton.Text = "Обновить список"; RefreshButton.Enabled = true; }
            else
            {
                if (Downloader.result) { Downloader.result = false; Mods.RefreshModsList(DirectoryName); RefreshInformation(sender, e); }
                if (Downloader.isDownloading)
                { RefreshButton.Text = mod_name + $" {Downloader.Percentage}%"; RefreshButton.Enabled = true; }
                else
                {
                    if (AllowedModsList.SelectedIndex >= 0)
                    {
                        if (!File.Exists("resources/" + ModsData[AllowedModsList.SelectedIndex] + "/config.fml"))
                        { RefreshButton.Text = "Установить выбранный мод"; RefreshButton.Enabled = true; }
                        else
                        { RefreshButton.Text = "Данный мод уже установлен"; RefreshButton.Enabled = false; }
                    }
                    else
                    { RefreshButton.Text = "Мод не выбран"; RefreshButton.Enabled = false; }
                }
            }
        }

        private bool isNerd = false;
        private void TrollTimer_Tick(object sender, EventArgs e)
        {
            isNerd = !isNerd;
            if (isNerd)
                Text = "fModLoader " + BuildVersion + BuildType + " 🤓";
            else
                Text = "fModLoader " + BuildVersion + BuildType;
        }

        private void isGameLoaded_Tick(object sender, EventArgs e)
        {
            Point cursor = new Point(10, 10);
            var color = Utils.GetColorAt(cursor);

            if (!(color.R == color.G && color.G == color.B && color.R == color.B))
            {
                //MessageBox.Show($"color is: r{color.R} g{color.G} b{color.B}");
                StatusText = "Игра загрузилась, внедряемся в неё полностью...";
                GameLoaded = true;
                Mods.InjectMods(true);
                isGameLoaded.Enabled = false;
            }
        }
    }
}
