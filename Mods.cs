using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fModLauncher
{
    public class Mods
    {
        static string GameDirectory = "";
        static string[] directories = { };
        static string[] backups = { };
        private static string[] clear_array = { };

        public static void RefreshModsList(string path)
        {
            directories = Directory.GetDirectories(path);
        }
        public static string[] GetModList()
        {
            return directories;
        }
        public static int GetModsNumber(bool real)
        {
            if (real)
            {
                int real_number = 0;
                foreach (string path in directories)
                {
                    if (File.Exists(path + @"\config.fml"))
                        real_number += 1;
                }
                return real_number;
            }
            return directories.Length;
        }

        public static string GetModName(string dir)
        {
            string mod_name = "Unembossed Name";
            if (File.Exists(dir + @"\displayname.lua"))
            {
                string raw = File.ReadAllText(dir + @"\displayname.lua");
                int index = raw.IndexOf("English") + 14;
                if (index > 0)
                {
                    raw = raw.Substring(index);
                    mod_name = raw.Substring(0, raw.IndexOf(",") - 1);
                }
                else
                    mod_name = dir.Substring(10);
            }
            else
            {
                mod_name = dir.Substring(10);
            }
            return mod_name;
        }

        private static bool IsInBackupList(string name)
        {
            foreach (string mod_name in backups)
            {
                if (mod_name == name)
                    return true;
                else
                    return false;
            }
            return false;
        }

        private static void CreateBackup(string name)
        {
            if (Directory.Exists(GameDirectory + @"\data\mods\" + name))
            {
                mainWindow.StatusText = "Добавляем мод в массив";
                Array.Resize(ref backups, backups.Length + 1);
                backups[backups.Length - 1] = name;
                if (!Directory.Exists(@"backup\" + name))
                {
                    mainWindow.StatusText = "Создаем backup мода '" + name + "'";
                    Utils.CopyDir(GameDirectory + @"\data\mods\" + name, @"backup\" + name);
                }
            }
            else
            {
                mainWindow.StatusText = "Стандартный мод '" + name + "' не найден!";
                MessageBox.Show("Стандартный мод '" + name + "' не найден!", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async public static void InjectMods(bool use_beep = false)
        {
            await Task.Run(() =>
            {
                int found = mainWindow.GamePath.IndexOf("Forts.exe");
                GameDirectory = mainWindow.GamePath.Substring(0, found - 1);
                foreach (string path in directories)
                {
                    if (File.Exists(path + @"\config.fml"))
                    {
                        string mod_name = File.ReadAllText(path + @"\config.fml");
                        if (IsInBackupList(mod_name) == false)
                        {
                            CreateBackup(mod_name);
                            mainWindow.StatusText = "Внедряем мод '" + mod_name + "'";
                            Directory.Delete(GameDirectory + @"\data\mods\" + mod_name, true);
                            Utils.CopyDir(path, GameDirectory + @"\data\mods\" + mod_name);
                        }
                        else
                        {
                            mainWindow.StatusText = "Невозможно внедрить мод '" + mod_name + "'";
                            MessageBox.Show("Мод '" + mod_name + "' уже внедрен", "fModLoader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                mainWindow.StatusText = "Внедрение прошло успешно!";
                if (use_beep) { Console.Beep(440, 300); }
            });
        }
        async public static void BackupMods()
        {
            await Task.Run(() =>
            {
                foreach (string name in backups)
                {
                    mainWindow.StatusText = "Возвращаем мод '" + name + "'";
                    Directory.Delete(GameDirectory + @"\data\mods\" + name, true);
                    Utils.CopyDir(@"backup\" + name, GameDirectory + @"\data\mods\" + name);
                }
                if (backups.Length > 0) { mainWindow.StatusText = "Выгрузка прошла успешно!"; }
                backups = clear_array;
            });
        }
    }
}
