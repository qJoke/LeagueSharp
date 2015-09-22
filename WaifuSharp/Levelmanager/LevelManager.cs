using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaifuSharp.Enums;

namespace WaifuSharp.Levelmanager
{
    class LevelManager
    {
        public delegate void OnWaifuLevelChange(int previousLevel, int currentLevel);

        public static int baseExp = 200;

        public static float Step = 1.4f;

        public static int KillExp = 50;

        public static int MultiKillAward = 25;
        
        public static void Onload()
        {
            
        }

        private static void LoadMenu()
        {
            
        }

        private static void RaiseWaifuEXP(ResourcePriority Kill)
        {
            var currentWaifu = WaifuSelector.WaifuSelector.GetCurrentWaifu();
            if (currentWaifu != null)
            {
                var currentExperience = currentWaifu.CurrentExp;
                var currentLevel = currentWaifu.CurrentLevel;
                var killNumbers = (int) Kill;
                var killExp = KillExp * killNumbers + MultiKillAward * killNumbers;
                var nextLevelExp = baseExp * currentLevel * Step;

                if (currentExperience + killExp >= nextLevelExp)
                {
                    var remainingExp = currentWaifu.CurrentLevel + KillExp - nextLevelExp;
                    currentWaifu.CurrentExp = (int) remainingExp;
                    currentWaifu.CurrentLevel += 1;
                }
                else
                {
                    currentWaifu.CurrentExp = (int)killExp;
                }
            }
        }

        public static void SaveWaifuLevel()
        {
            //TODO Save level and EXP
        }
    }
}
