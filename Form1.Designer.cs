using fModLauncher.Properties;
using System.IO;
using System.Windows.Forms;

namespace fModLauncher
{
    partial class mainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExtractAddictives(string file, byte[] data)
        {
            string temp_path = Path.GetTempPath();
            if (!File.Exists(temp_path + file))
                File.WriteAllBytes(temp_path + file, data);
        }

        public static string GamePath = "";
        private void InitializeAddictives()
        {
            ExtractAddictives("7za.dll", Resources._7za_dll);
            ExtractAddictives("7za.exe", Resources._7za_exe);
            ExtractAddictives("7zxa.dll", Resources._7zxa_dll);

            Utils.InitializeRegistry();
            if (Utils.IsProcessRunning(GameProcess))
                GamePath = Utils.GetGamePath(GameProcess);
            else
                GamePath = Utils.GetRegistryPath();
            if (!Directory.Exists("resources"))
            {
                Directory.CreateDirectory("resources");
            }
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainWindow));
            this.InformationLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.InjectButton = new System.Windows.Forms.Button();
            this.GameLaunchTimer = new System.Windows.Forms.Timer(this.components);
            this.AutoHideCheckbox = new System.Windows.Forms.CheckBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.by_xoireitwekt = new System.Windows.Forms.Timer(this.components);
            this.TrollTimer = new System.Windows.Forms.Timer(this.components);
            this.isGameLoaded = new System.Windows.Forms.Timer(this.components);
            this.AutoInjectCheckbox = new System.Windows.Forms.CheckBox();
            this.TabSelector = new System.Windows.Forms.TabControl();
            this.ModsLibrary = new System.Windows.Forms.TabPage();
            this.ModsLibraryList = new System.Windows.Forms.ListBox();
            this.AllowedMods = new System.Windows.Forms.TabPage();
            this.AllowedModsList = new System.Windows.Forms.ListBox();
            this.TabSelector.SuspendLayout();
            this.ModsLibrary.SuspendLayout();
            this.AllowedMods.SuspendLayout();
            this.SuspendLayout();
            // 
            // InformationLabel
            // 
            this.InformationLabel.AutoSize = true;
            this.InformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InformationLabel.Location = new System.Drawing.Point(9, 234);
            this.InformationLabel.Name = "InformationLabel";
            this.InformationLabel.Size = new System.Drawing.Size(217, 15);
            this.InformationLabel.TabIndex = 0;
            this.InformationLabel.Text = "Доступно/загружено модов: 0/0";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusLabel.Location = new System.Drawing.Point(9, 258);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(316, 15);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Здесь будет отображаться текущее действие";
            // 
            // InjectButton
            // 
            this.InjectButton.Location = new System.Drawing.Point(12, 310);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(152, 39);
            this.InjectButton.TabIndex = 2;
            this.InjectButton.Text = "Запустить игру";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // GameLaunchTimer
            // 
            this.GameLaunchTimer.Tick += new System.EventHandler(this.GameLaunching);
            // 
            // AutoHideCheckbox
            // 
            this.AutoHideCheckbox.AutoSize = true;
            this.AutoHideCheckbox.Location = new System.Drawing.Point(12, 287);
            this.AutoHideCheckbox.Name = "AutoHideCheckbox";
            this.AutoHideCheckbox.Size = new System.Drawing.Size(103, 17);
            this.AutoHideCheckbox.TabIndex = 3;
            this.AutoHideCheckbox.Text = "Скрывать окно";
            this.AutoHideCheckbox.UseVisualStyleBackColor = true;
            this.AutoHideCheckbox.Click += new System.EventHandler(this.AutoHideChange);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(173, 310);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(152, 39);
            this.RefreshButton.TabIndex = 4;
            this.RefreshButton.Text = "Обновить список";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // by_xoireitwekt
            // 
            this.by_xoireitwekt.Enabled = true;
            this.by_xoireitwekt.Tick += new System.EventHandler(this.by_xoireitwekt_Tick);
            // 
            // TrollTimer
            // 
            this.TrollTimer.Enabled = true;
            this.TrollTimer.Interval = 5000;
            this.TrollTimer.Tick += new System.EventHandler(this.TrollTimer_Tick);
            // 
            // isGameLoaded
            // 
            this.isGameLoaded.Interval = 1000;
            this.isGameLoaded.Tick += new System.EventHandler(this.isGameLoaded_Tick);
            // 
            // AutoInjectCheckbox
            // 
            this.AutoInjectCheckbox.AutoSize = true;
            this.AutoInjectCheckbox.Location = new System.Drawing.Point(121, 287);
            this.AutoInjectCheckbox.Name = "AutoInjectCheckbox";
            this.AutoInjectCheckbox.Size = new System.Drawing.Size(204, 17);
            this.AutoInjectCheckbox.TabIndex = 5;
            this.AutoInjectCheckbox.Text = "Автоматическое внедрение (BETA)\r\n";
            this.AutoInjectCheckbox.UseVisualStyleBackColor = true;
            this.AutoInjectCheckbox.Click += new System.EventHandler(this.AutoInjectChange);
            // 
            // TabSelector
            // 
            this.TabSelector.Controls.Add(this.ModsLibrary);
            this.TabSelector.Controls.Add(this.AllowedMods);
            this.TabSelector.Location = new System.Drawing.Point(12, 12);
            this.TabSelector.Name = "TabSelector";
            this.TabSelector.SelectedIndex = 0;
            this.TabSelector.Size = new System.Drawing.Size(412, 210);
            this.TabSelector.TabIndex = 6;
            // 
            // ModsLibrary
            // 
            this.ModsLibrary.Controls.Add(this.ModsLibraryList);
            this.ModsLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.ModsLibrary.Location = new System.Drawing.Point(4, 22);
            this.ModsLibrary.Name = "ModsLibrary";
            this.ModsLibrary.Padding = new System.Windows.Forms.Padding(3);
            this.ModsLibrary.Size = new System.Drawing.Size(404, 184);
            this.ModsLibrary.TabIndex = 0;
            this.ModsLibrary.Text = "Моя библиотека";
            this.ModsLibrary.UseVisualStyleBackColor = true;
            // 
            // ModsLibraryList
            // 
            this.ModsLibraryList.FormattingEnabled = true;
            this.ModsLibraryList.ItemHeight = 15;
            this.ModsLibraryList.Location = new System.Drawing.Point(0, 0);
            this.ModsLibraryList.Name = "ModsLibraryList";
            this.ModsLibraryList.Size = new System.Drawing.Size(403, 184);
            this.ModsLibraryList.TabIndex = 0;
            // 
            // AllowedMods
            // 
            this.AllowedMods.Controls.Add(this.AllowedModsList);
            this.AllowedMods.Location = new System.Drawing.Point(4, 22);
            this.AllowedMods.Name = "AllowedMods";
            this.AllowedMods.Padding = new System.Windows.Forms.Padding(3);
            this.AllowedMods.Size = new System.Drawing.Size(404, 184);
            this.AllowedMods.TabIndex = 1;
            this.AllowedMods.Text = "Доступные моды";
            this.AllowedMods.UseVisualStyleBackColor = true;
            // 
            // AllowedModsList
            // 
            this.AllowedModsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.AllowedModsList.FormattingEnabled = true;
            this.AllowedModsList.ItemHeight = 15;
            this.AllowedModsList.Location = new System.Drawing.Point(0, 0);
            this.AllowedModsList.Name = "AllowedModsList";
            this.AllowedModsList.Size = new System.Drawing.Size(403, 184);
            this.AllowedModsList.TabIndex = 1;
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 361);
            this.Controls.Add(this.TabSelector);
            this.Controls.Add(this.AutoInjectCheckbox);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.AutoHideCheckbox);
            this.Controls.Add(this.InjectButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.InformationLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fModLoader";
            this.Load += new System.EventHandler(this.RefreshInformation);
            this.TabSelector.ResumeLayout(false);
            this.ModsLibrary.ResumeLayout(false);
            this.AllowedMods.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InformationLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button InjectButton;
        private Timer GameLaunchTimer;
        private CheckBox AutoHideCheckbox;
        private Button RefreshButton;
        private Timer by_xoireitwekt;
        private Timer TrollTimer;
        private Timer isGameLoaded;
        private CheckBox AutoInjectCheckbox;
        private TabControl TabSelector;
        private TabPage ModsLibrary;
        private TabPage AllowedMods;
        private ListBox ModsLibraryList;
        private ListBox AllowedModsList;
    }
}

