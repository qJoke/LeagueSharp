using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX.Multimedia;
using WaifuSharp.Enums;
using WaifuSharp.ResourceClasses;
using WaifuSharp.WaifuHelper;

namespace WaifuSharp
{
    class WaifuSharp
    {
        public static Menu Menu { get; set; }

        public static float LastEventTick;

        private static ResourcePriority KillPriority = ResourcePriority.SingleKill;

        private static SoundPlayer sPlayer = new SoundPlayer();
        public static void OnLoad()
        {
            Game.OnNotify += Game_OnNotify;
        }

        private static void Game_OnNotify(GameNotifyEventArgs args)
        {
            
                switch (args.EventId)
                {
                    case GameEventId.OnChampionKill:
                        if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                        {
                            Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.SingleKill);
                            LastEventTick = Game.Time;
                            ShowOnKillWaifu();
                        }
                        break;
                    case GameEventId.OnChampionDoubleKill:
                        if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                        {
                        Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.DoubleKill);
                        LastEventTick = Game.Time;
                        ShowOnKillWaifu();
                        }
                        break;
                    case GameEventId.OnChampionTripleKill:
                        if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                        {
                            Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.TripleKill);
                            LastEventTick = Game.Time;
                            ShowOnKillWaifu();
                        }
                        break;
                    case GameEventId.OnChampionQuadraKill:
                        if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                        {
                            Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.QuadraKill);
                            LastEventTick = Game.Time;
                            ShowOnKillWaifu();
                        }
                        break;

                    case GameEventId.OnChampionPentaKill:
                        if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                        {
                            Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.PentaKill);
                            LastEventTick = Game.Time;
                            ShowOnKillWaifu();
                        }
                        break;
            }
        }
    

        private static void ShowOnKillWaifu()
        {
            if (WaifuSelector.WaifuSelector.CurrentSprite != null)
            {
                WaifuSelector.WaifuSelector.CurrentSprite.Visible = false;
                WaifuSelector.WaifuSelector.CurrentSprite.Remove();
            }
            sPlayer.Stop();

            var currentWaifu = GetCurrentWaifu();
            if (currentWaifu != null)
            {
                var spriteList = GetCurrentWaifu()
                        .OnKillPics.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                        .ToArray();
                var currentSprite = spriteList[new Random().Next(0, spriteList.Count())];
                var soundList =
                    GetCurrentWaifu()
                        .OnKillSounds.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                        .ToArray();
                var currentSound = soundList[new Random().Next(0, GetCurrentWaifu().OnKillSounds.Count())];

                WaifuSelector.WaifuSelector.InitKillSprite(currentSprite);
                if (currentSound != null)
                {
                    var sSound = currentSound;
                    sPlayer.Stream = new MemoryStream(currentSound.SoundStream, true);
                    sPlayer.Load();
                    if (sPlayer.IsLoadCompleted)
                    {
                        sPlayer.PlaySync();
                    }
                    sPlayer.Dispose();

                    var s = GetCurrentWaifu()
                        .OnKillSounds.FirstOrDefault(m => m.SoundStream == sSound.SoundStream);

                    if (s != null)
                    {
                        s.SoundStream = sSound.SoundStream;

                    }

                }
            }
        }

        private static Waifu GetCurrentWaifu()
        {
            var menuItem = Menu.Item("waifusharp.options.waifus");

            if (menuItem != null)
            {
                return GetWaifuByName(menuItem.GetValue<StringList>().SelectedValue);
            }

            return WaifuSelector.WaifuSelector.Waifus.FirstOrDefault();
        }

        private static Waifu GetWaifuByName(String name)
        {
            return WaifuSelector.WaifuSelector.Waifus.FirstOrDefault(w => w.Name.ToLower() == name.ToLower());
        }
    }
}
