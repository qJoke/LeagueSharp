using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Permissions;
using LeagueSharp;
using LeagueSharp.Common;
using WaifuSharp.Enums;
using WaifuSharp.WaifuHelper;

namespace WaifuSharp
{
    class WaifuSharp
    {
        public static Menu Menu { get; set; }

        public static float LastEventTick;

        private static ResourcePriority KillPriority = ResourcePriority.SingleKill;

        private static SoundPlayer sPlayer = new SoundPlayer();

        private static float Kills = 0;

        private static float Assists = 0;

        private static int delay = 125;

        public static void OnLoad()
        {
            Game.OnNotify += Game_OnNotify;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnBuffUpdateCount += Obj_AI_Base_OnBuffUpdateCount;
            Obj_AI_Base.OnBuffAdd += Obj_AI_Base_OnBuffAdd;
            //Obj_AI_Base.OnDamage += Obj_AI_Base_OnDamage;
        }

       



        static void Game_OnUpdate(EventArgs args)
        {
           // Kills = ObjectManager.Player.Score;
        }

        static void Obj_AI_Base_OnBuffUpdateCount(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateCountEventArgs args)
        {
            if (sender.IsMe && args.Buff.Name == "s5test_dragonslayerbuff")
            {
                Utility.DelayAction.Add(
                    250, () =>
                    {
                        Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.DragonKill);
                        LastEventTick = Game.Time;
                        Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                    });
            }
        }

        static void Obj_AI_Base_OnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            if (sender.IsMe && (args.Buff.DisplayName.ToLower().Contains("hand of baron") || args.Buff.Name.ToLower().Contains("baron") || args.Buff.Name.ToLower().Contains("worm")))
            {
                Utility.DelayAction.Add(
                    250, () =>
                    {
                        Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.BaronKill);
                        LastEventTick = Game.Time;
                        Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                    });
            }
        }


        private static void Game_OnNotify(GameNotifyEventArgs args)
        {
            
                switch (args.EventId)
                {
                    case GameEventId.OnChampionKill:
                        Utility.DelayAction.Add(250, () =>
                        {
                            if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsEnemy && ObjectManager.Player.ChampionsKilled > Kills)
                            {
                                Kills = ObjectManager.Player.ChampionsKilled;
                                Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.SingleKill);
                                LastEventTick = Game.Time;
                                Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                            }
                            
                            if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe)
                            {
                                Levelmanager.LevelManager.DecreaseWaifuExp();
                                Utility.DelayAction.Add(delay, ShowOnDeathWaifu);
                            }
                        });
                        
                        break;
                    case GameEventId.OnChampionDoubleKill:
                        Utility.DelayAction.Add(
                            250, () =>
                            {
                                if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsEnemy &&
                                    ObjectManager.Player.ChampionsKilled > Kills)
                                {
                                    Kills = ObjectManager.Player.ChampionsKilled;
                                    Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.DoubleKill);
                                    LastEventTick = Game.Time;
                                    Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                                }
                            });
                        break;
                    case GameEventId.OnChampionTripleKill:
                        Utility.DelayAction.Add(
                            250, () =>
                            {
                                if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsEnemy &&
                                    ObjectManager.Player.ChampionsKilled > Kills)
                                {
                                    Kills = ObjectManager.Player.ChampionsKilled;
                                    Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.TripleKill);
                                    LastEventTick = Game.Time;
                                    Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                                }
                            });
                        break;
                    case GameEventId.OnChampionQuadraKill:
                        Utility.DelayAction.Add(
                            250, () =>
                            {
                                if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsEnemy &&
                                    ObjectManager.Player.ChampionsKilled > Kills)
                                {
                                    Kills = ObjectManager.Player.ChampionsKilled;
                                    Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.QuadraKill);
                                    LastEventTick = Game.Time;
                                    Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                                }
                            });
                        break;

                    case GameEventId.OnChampionPentaKill:
                        Utility.DelayAction.Add(
                            250, () =>
                            {
                                if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsEnemy &&
                                    ObjectManager.Player.ChampionsKilled > Kills)
                                {
                                    Kills = ObjectManager.Player.ChampionsKilled;
                                    Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.PentaKill);
                                    LastEventTick = Game.Time;
                                    Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                                }
                            });
                        break;
                    case GameEventId.OnDeathAssist:
                        Utility.DelayAction.Add(
                            250, () =>
                            {
                                if (ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(args.NetworkId).IsMe &&
                                    ObjectManager.Player.Assists > Assists)
                                {
                                    Assists = ObjectManager.Player.Assists;
                                    Levelmanager.LevelManager.RaiseWaifuEXP(ResourcePriority.SingleKill, true);
                                    LastEventTick = Game.Time;
                                    Utility.DelayAction.Add(delay, ShowOnKillWaifu);
                                }
                            });
                        break;
                }
        }

        [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
        private static void ShowOnKillWaifu()
        {
            if (WaifuSelector.WaifuSelector.CurrentSprite != null)
            {
                WaifuSelector.WaifuSelector.CurrentSprite.Visible = false;
                WaifuSelector.WaifuSelector.CurrentSprite.Remove();
            }
            sPlayer.Stop();

            var waifu = GetCurrentWaifu();
            if (waifu != null)
            {
                var soundList =
                        GetCurrentWaifu()
                            .OnKillSounds.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                            .ToArray();
                if (soundList.Any())
                {
                    var currentSound = soundList[new Random().Next(0, soundList.Count())];

                    if (currentSound != null)
                    {
                        sPlayer.Stream = new MemoryStream(currentSound.SoundStream, true);
                        sPlayer.Load();
                        if (sPlayer.IsLoadCompleted)
                        {
                            sPlayer.Play();
                        }
                        sPlayer.Dispose();
                    }
                }


                var spriteList = GetCurrentWaifu()
                            .OnKillPics.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                            .ToArray();
                if (spriteList.Any())
                {
                    var sprite = spriteList[new Random().Next(0, spriteList.Count())];
                    if (sprite != null)
                    {
                        WaifuSelector.WaifuSelector.InitKillSprite(sprite);
                    }
                }

            }
        }

         [SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
         private static void ShowOnDeathWaifu()
         {
             if (WaifuSelector.WaifuSelector.CurrentSprite != null)
             {
                 WaifuSelector.WaifuSelector.CurrentSprite.Visible = false;
                 WaifuSelector.WaifuSelector.CurrentSprite.Remove();
             }
             sPlayer.Stop();

             var waifu = GetCurrentWaifu();
             if (waifu != null)
             {
                 var soundList =
                         GetCurrentWaifu()
                             .OnDeathSounds.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                             .ToArray();
                 if (soundList.Any())
                 {
                     var currentSound = soundList[new Random().Next(0, soundList.Count())];

                     if (currentSound != null)
                     {
                         sPlayer.Stream = new MemoryStream(currentSound.SoundStream, true);
                         sPlayer.Load();
                         if (sPlayer.IsLoadCompleted)
                         {
                             sPlayer.Play();
                         }
                         sPlayer.Dispose();
                     }
                 }


                 var spriteList = GetCurrentWaifu()
                             .OnDeathPics.Where(m => m.MinWaifuLevel <= GetCurrentWaifu().CurrentLevel)
                             .ToArray();
                 if (spriteList.Any())
                 {
                     var sprite = spriteList[new Random().Next(0, spriteList.Count())];
                     if (sprite != null)
                     {
                         WaifuSelector.WaifuSelector.InitDeathSprite(sprite);
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
