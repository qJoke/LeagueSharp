using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using WaifuSharp.WaifuHelper;

namespace WaifuSharp.WaifuSelector
{
    class WaifuSelector
    {
        private static string LeagueSharpAppData
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LS" + Environment.UserName.GetHashCode().ToString("X"));
            }
        }

        public static String AssemblyDir
        {
            get { return Path.Combine(LeagueSharpAppData, "WaifuSharp"); }
        }

        public static String WaifusDir
        {
            get { return Path.Combine(AssemblyDir, "WaifusDir"); }
        }

        public static List<Waifu> Waifus = new List<Waifu>();
 
        public static void OnLoad()
        {
            CheckAndCreateDirectories();
            LoadMenu();
        }

        #region Waifus Loading
        private static void LoadWaifus()
        {
            foreach (var directory in Directory.GetDirectories(WaifusDir))
            {
                var currentWaifu = new Waifu
                {
                    Name = directory
                };
                string[] content = Directory.GetFiles(directory);
                foreach (var file in content)
                {
                    LoadContentToWaifu(file, currentWaifu);
                }
            }
        }

        private static void LoadContentToWaifu(String FilePath, Waifu currentWaifu)
        {
            var array = FilePath.Split(Path.DirectorySeparatorChar);
            var fileName = array.Last().ToLower();

            if (fileName.Contains("onkill"))
            {
                OnKillLoad(currentWaifu);
            }

            if (fileName.Contains("ondeath"))
            {
                OnDeathLoad(currentWaifu);
            }
        }

        public static void OnKillLoad(Waifu currentWaifu)
        {
            
        }

        public static void OnDeathLoad(Waifu currentWaifu)
        {

        }
        #endregion

        #region Menu Creation
        private static void LoadMenu()
        {
            
        }
        #endregion

        #region Utility Methods
        private static void CheckAndCreateDirectories()
        {
            if (!Directory.Exists(AssemblyDir))
            {
                Directory.CreateDirectory(AssemblyDir);
            }

            if (!Directory.Exists(WaifusDir))
            {
                Directory.CreateDirectory(WaifusDir);
            }
        }
        #endregion

    }
}
