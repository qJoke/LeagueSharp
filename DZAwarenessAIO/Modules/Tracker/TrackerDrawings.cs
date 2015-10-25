using System;
using System.Collections.Generic;
using System.Linq;
using DZAwarenessAIO.Properties;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace DZAwarenessAIO.Modules.Tracker
{
    /// <summary>
    /// The Tracker Drawings Class
    /// </summary>
    class TrackerDrawings
    {
        /// <summary>
        /// The last tick the OnUpdate event delegate was called
        /// </summary>
        private static float LastTick;

        /// <summary>
        /// The players' wrappers list
        /// </summary>
        private static List<PlayerWrapper> playersWrapper = new List<PlayerWrapper>();

        /// <summary>
        /// The tracker's wrappers list
        /// </summary>
        private static List<TrackerWrapper> TrackerWrappers = new List<TrackerWrapper>();


        /// <summary>
        /// Called when module loads.
        /// </summary>
        public static void OnLoad()
        {
            LoadList();
            LoadSprites();
            Game.OnUpdate += OnUpdate;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_DomainUnload;
        }

        /// <summary>
        /// Handles the DomainUnload event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            /**Remove the sprites*/
            try
            {
                foreach (var w in TrackerWrappers)
                {
                    w.Hud.Remove();

                    w.Summoner1.Remove();
                    w.Summoner2.Remove();

                    w.SummonerSpell1Rectangle.Remove();
                    w.SummonerSpell2Rectangle.Remove();
                    w.SummonerSpell1Text.Remove();
                    w.SummonerSpell2Text.Remove();

                    w.Spell1Rectangle.Remove();
                    w.Spell1Text.Remove();

                    w.Spell2Rectangle.Remove();
                    w.Spell2Text.Remove();

                    w.Spell3Rectangle.Remove();
                    w.Spell3Text.Remove();

                    w.Spell4Rectangle.Remove();
                    w.Spell4Text.Remove();
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddToLog(new LogItem("Tracker_Drawings", ex, LogSeverity.Severe));
            }
        }

        /// <summary>
        /// Raises the <see cref="Game.OnUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnUpdate(EventArgs args)
        {
            if (Environment.TickCount - LastTick < 250)
            {
                return;
            }
            LastTick = Environment.TickCount;
            try
            {
                foreach (var w in TrackerWrappers)
                {
                    var hero = w.Hero;

                    if ((hero.IsAlly || hero.IsMe) && !TrackerVariables.TrackAllies)
                    {
                        continue;
                    }

                    if (hero.IsEnemy && !TrackerVariables.TrackEnemies)
                    {
                        continue;
                    }

                    foreach (var summonerSpell in TrackerVariables.Summoners)
                    {
                        var spellInstance = hero.Spellbook.GetSpell(summonerSpell);
                        var spellCooldown = spellInstance.CooldownExpires - Game.Time;
                        var widthDef = 13;
                        var widthCd = spellCooldown > 0 ? widthDef * (spellCooldown / spellInstance.Cooldown) : 0;

                        switch (summonerSpell)
                        {
                            case SpellSlot.Summoner1:
                                if (w.SummonerSpell1Rectangle != null)
                                {
                                    w.SummonerSpell1Rectangle.Visible = (int)widthCd != 0;
                                    w.SummonerSpell1Rectangle.Width = (int) widthCd;
                                }
                                break;
                            case SpellSlot.Summoner2:
                                if (w.SummonerSpell2Rectangle != null)
                                {
                                    w.SummonerSpell2Rectangle.Visible = (int)widthCd != 0;
                                    w.SummonerSpell2Rectangle.Width = (int) widthCd;
                                }
                                break;
                        }
                    }

                    foreach (var spell in TrackerVariables.SpellSlots)
                    {
                        var spellInstance = hero.Spellbook.GetSpell(spell);
                        var spellCooldown = spellInstance.CooldownExpires - Game.Time;
                        var widthDef = 23;
                        var widthCd = spellCooldown > 0 ? widthDef * (spellCooldown / spellInstance.Cooldown) : 0;
                        switch (spell)
                        {
                            case SpellSlot.Q:
                                if (w.Spell1Rectangle != null)
                                {
                                    w.Spell1Rectangle.Visible = (int)widthCd != 0;
                                    w.Spell1Rectangle.Width = (int)widthCd;
                                }
                                break;
                            case SpellSlot.W:
                                if (w.Spell2Rectangle != null)
                                {
                                    w.Spell2Rectangle.Visible = (int)widthCd != 0;
                                    w.Spell2Rectangle.Width = (int)widthCd;
                                }
                                break;
                            case SpellSlot.E:
                                if (w.Spell3Rectangle != null)
                                {
                                    w.Spell3Rectangle.Visible = (int)widthCd != 0;
                                    w.Spell3Rectangle.Width = (int)widthCd;
                                }
                                break;
                            case SpellSlot.R:
                                if (w.Spell4Rectangle != null)
                                {
                                    w.Spell4Rectangle.Visible = (int)widthCd != 0;
                                    w.Spell4Rectangle.Width = (int)widthCd;
                                }
                                break;
                        }
                    }

                    
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Tracker_Drawings", e));
            }
        }

        /// <summary>
        /// Gets the xp per level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        private static float GetXPPerLevel(int level)
        {
            return 80 + 100 * (level);
        }

        /// <summary>
        /// Gets the cumulative xp.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        private static float GetCumulativeXP(float level)
        {
            
            var exp = -180f;
            for (var i = 1; i <= level; i++)
            {
                exp += GetXPPerLevel(i);
            }

            return exp;
        }

        /// <summary>
        /// Loads the sprites.
        /// </summary>
        private static void LoadSprites()
        {
            foreach (var player in ObjectManager.Get<Obj_AI_Hero>().Where(h => !h.IsMe))
            {
                try
                {
                    var player_Ex = player;
                    var Summoner1Bitmap =
                        TrackerVariables.summonerSpells[player_Ex.Spellbook.GetSpell(SpellSlot.Summoner1).Name];
                    var Summoner2Bitmap =
                        TrackerVariables.summonerSpells[player_Ex.Spellbook.GetSpell(SpellSlot.Summoner2).Name];
                    Render.Sprite SummonerSpell1 = new Render.Sprite(Resources.empty, new Vector2());
                    Render.Sprite SummonerSpell2 = new Render.Sprite(Resources.empty, new Vector2());
                    var member = new TrackerWrapper();
                    var scale = 0.94f;
                    var offset = 8 * scale;
                    var offsetX = 0;

                    var Hudsprite = new Render.Sprite(TrackerVariables.TrackerHud, new Vector2(0, 0))
                    {
                        PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X - 14 * scale, player_Ex.HPBarPosition.Y + offset + 6 * scale),
                        VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                        Scale = new Vector2(1.0f, 1.0f)
                    };

                    Hudsprite.Add(0);

                    #region Summoner Spells
                    if (Summoner1Bitmap != null)
                    {
                        SummonerSpell1 = new Render.Sprite(Summoner1Bitmap, new Vector2(0, 0))
                        {
                            PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X - 8 * scale, player_Ex.HPBarPosition.Y + offset + 8 * scale),
                            VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                            Scale = new Vector2(1.0f, 1.0f)

                        };
                        SummonerSpell1.Add(0);
                        member.Summoner1 = SummonerSpell1;

                        var Summoner1Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X - 7, (int)player_Ex.HPBarPosition.Y + 8, 13, 13, new ColorBGRA(0, 0, 0, 175))
                        {
                            VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                            PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X - 7 * scale, (int)player_Ex.HPBarPosition.Y + offset + 8 * scale),
                        };
                        Summoner1Rectangle.Add(0);

                        member.SummonerSpell1Rectangle = Summoner1Rectangle;

                        var spellCooldown = player_Ex.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires - Game.Time;

                        var Summoner1Text = new Render.Text((int)player_Ex.HPBarPosition.X - 31, (int)player_Ex.HPBarPosition.Y + 6, ((int)spellCooldown).ToString(), 14, new ColorBGRA(255, 255, 255, 255))
                        {
                            TextUpdate = () => ((int)(player_Ex.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires - Game.Time) > 0 ? 
                                ((int)(player_Ex.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires - Game.Time)).ToString() 
                                : string.Empty),
                            PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X - 31 * scale, (int)player_Ex.HPBarPosition.Y + offset + 6 * scale),
                            VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                        };

                        Summoner1Text.Add(0);

                        member.SummonerSpell1Text = Summoner1Text;

                    }

                    if (Summoner2Bitmap != null)
                    {
                        SummonerSpell2 = new Render.Sprite(Summoner2Bitmap, new Vector2(0, 0))
                        {
                            PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X - 8 * scale, player_Ex.HPBarPosition.Y + offset + 25 * scale),
                            VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                            Scale = new Vector2(1.0f, 1.0f)
                        };
                        SummonerSpell2.Add(0);
                        member.Summoner2 = SummonerSpell2;

                        var Summoner2Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X - 7, (int)player_Ex.HPBarPosition.Y + 26, 13, 13, new ColorBGRA(0, 0, 0, 175))
                        {
                            VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                            PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X - 7 * scale, (int)player_Ex.HPBarPosition.Y + offset + 26 * scale),
                        };
                        Summoner2Rectangle.Add(0);

                        var spellCooldown = player_Ex.Spellbook.GetSpell(SpellSlot.Summoner2).CooldownExpires - Game.Time;

                        var Summoner2Text = new Render.Text((int)player_Ex.HPBarPosition.X - 31, (int)player_Ex.HPBarPosition.Y + 24, ((int)spellCooldown).ToString(), 14, new ColorBGRA(255, 255, 255, 255))
                        {
                            TextUpdate = () => ((int)(player_Ex.Spellbook.GetSpell(SpellSlot.Summoner2).CooldownExpires - Game.Time) > 0 ?
                                ((int)(player_Ex.Spellbook.GetSpell(SpellSlot.Summoner2).CooldownExpires - Game.Time)).ToString()
                                : string.Empty),
                            PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X - 31 * scale, (int)player_Ex.HPBarPosition.Y + offset + 24 * scale),
                            VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(SpellSlot.Summoner2).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                        };
                        Summoner2Text.Add(0);

                        member.SummonerSpell2Text = Summoner2Text;
                        member.SummonerSpell2Rectangle = Summoner2Rectangle;
                    }
                    #endregion

                    #region Normal Spells

                    #region Spell1
                    var slot1 = SpellSlot.Q;

                    var Spell1Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X + 13, (int)player_Ex.HPBarPosition.Y + 28, 23, 6, new ColorBGRA(255, 0, 0, 255))
                    {
                        VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                        PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X + offsetX + 13.2f * scale, (int)player_Ex.HPBarPosition.Y + offset + 30 * scale),
                        Width = 23,
                        Height = 1,
                        Color = new ColorBGRA(255, 0, 0, 255)
                    };

                    member.Spell1Rectangle = Spell1Rectangle;
                    Spell1Rectangle.Add(0);

                    var Spell1Text = new Render.Text((int)player_Ex.HPBarPosition.X + 16, (int)player_Ex.HPBarPosition.Y + 32, string.Empty, 14, new ColorBGRA(255, 255, 255, 255))
                    {
                        TextUpdate = () => ((player_Ex.Spellbook.GetSpell(slot1).CooldownExpires - Game.Time) > 0 ?
                            (Truncate((player_Ex.Spellbook.GetSpell(slot1).CooldownExpires - Game.Time)))
                            : string.Empty),
                        PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X + offsetX + 16 * scale, (int)player_Ex.HPBarPosition.Y + offset + 32 * scale),
                        VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(slot1).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                    };

                    member.Spell1Text = Spell1Text;
                    Spell1Text.Add(0);
                    #endregion

                    #region Spell2
                    var slot2 = SpellSlot.W;

                    var Spell2Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X + 41, (int)player_Ex.HPBarPosition.Y + 28, 23, 6, new ColorBGRA(255, 0, 0, 255))
                    {
                        VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                        PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X + offsetX + 41f * scale, (int)player_Ex.HPBarPosition.Y + offset + 30 * scale),
                        Width = 23,
                        Height = 1,
                        Color = new ColorBGRA(255, 0, 0, 255)
                    };

                    member.Spell2Rectangle = Spell2Rectangle;
                    Spell2Rectangle.Add(0);

                    var Spell2Text = new Render.Text((int)player_Ex.HPBarPosition.X + 44, (int)player_Ex.HPBarPosition.Y + 32, string.Empty, 14, new ColorBGRA(255, 255, 255, 255))
                    {
                        TextUpdate = () => ((player_Ex.Spellbook.GetSpell(slot2).CooldownExpires - Game.Time) > 0 ?
                            (Truncate((player_Ex.Spellbook.GetSpell(slot2).CooldownExpires - Game.Time)))
                            : string.Empty),
                        PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X + offsetX + 43.8f * scale, (int)player_Ex.HPBarPosition.Y + offset + 32 * scale),
                        VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(slot2).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                    };

                    member.Spell2Text = Spell2Text;
                    Spell2Text.Add(0);
                    #endregion

                    #region Spell3
                    var slot3 = SpellSlot.E;

                    var Spell3Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X + 41, (int)player_Ex.HPBarPosition.Y + 28, 23, 6, new ColorBGRA(255, 0, 0, 255))
                    {
                        VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                        PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X + offsetX + 69f * scale, (int)player_Ex.HPBarPosition.Y + offset + 30 * scale),
                        Width = 23,
                        Height = 1,
                        Color = new ColorBGRA(255, 0, 0, 255)
                    };

                    member.Spell3Rectangle = Spell3Rectangle;
                    Spell3Rectangle.Add(0);

                    var Spell3Text = new Render.Text((int)player_Ex.HPBarPosition.X + 44, (int)player_Ex.HPBarPosition.Y + 32, string.Empty, 14, new ColorBGRA(255, 255, 255, 255))
                    {
                        TextUpdate = () => ((player_Ex.Spellbook.GetSpell(slot3).CooldownExpires - Game.Time) > 0 ?
                            (Truncate((player_Ex.Spellbook.GetSpell(slot3).CooldownExpires - Game.Time)))
                            : string.Empty),
                        PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X + offsetX + 73.8f * scale, (int)player_Ex.HPBarPosition.Y + offset + 32 * scale),
                        VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(slot3).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                    };

                    member.Spell3Text = Spell3Text;
                    Spell3Text.Add(0);
                    #endregion

                    #region Spell4
                    var slot4 = SpellSlot.R;

                    var Spell4Rectangle = new Render.Rectangle((int)player_Ex.HPBarPosition.X + 41, (int)player_Ex.HPBarPosition.Y + 28, 23, 6, new ColorBGRA(255, 0, 0, 255))
                    {
                        VisibleCondition = sender => player_Ex.IsHPBarRendered && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies)),
                        PositionUpdate = () => new Vector2(player_Ex.HPBarPosition.X + offsetX + 96f * scale, (int)player_Ex.HPBarPosition.Y + offset + 30 * scale),
                        Width = 23,
                        Height = 1,
                        Color = new ColorBGRA(255, 0, 0, 255)
                    };

                    member.Spell4Rectangle = Spell4Rectangle;
                    Spell4Rectangle.Add(0);

                    var Spell4Text = new Render.Text((int)player_Ex.HPBarPosition.X + 44, (int)player_Ex.HPBarPosition.Y + 32, string.Empty, 14, new ColorBGRA(255, 255, 255, 255))
                    {
                        TextUpdate = () => ((player_Ex.Spellbook.GetSpell(slot4).CooldownExpires - Game.Time) > 0 ?
                            (Truncate((player_Ex.Spellbook.GetSpell(slot4).CooldownExpires - Game.Time)))
                            : string.Empty),
                        PositionUpdate = () => new Vector2((int)player_Ex.HPBarPosition.X + offsetX + 101f * scale, (int)player_Ex.HPBarPosition.Y + offset + 32 * scale),
                        VisibleCondition = sender => MenuExtensions.GetItemValue<bool>("dz191.dza.tracker.track.cd") && player_Ex.IsHPBarRendered && (player_Ex.Spellbook.GetSpell(slot4).CooldownExpires - Game.Time) > 0 && (((player_Ex.IsAlly || player_Ex.IsMe) && TrackerVariables.TrackAllies) || (player_Ex.IsEnemy && TrackerVariables.TrackEnemies))
                    };

                    member.Spell4Text = Spell4Text;
                    Spell4Text.Add(0);
                    #endregion

                    #endregion

                    member.Hero = player_Ex;
                    member.Hud = Hudsprite;
                    member.Summoner1 = SummonerSpell1;
                    member.Summoner2 = SummonerSpell2;

                    TrackerWrappers.Add(member);
                }
                catch (Exception e)
                {
                    LogHelper.AddToLog(new LogItem("Tracker_Drawings", e, LogSeverity.Severe));
                }
                
            }
        }

        /// <summary>
        /// Truncates the specified string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        private static String Truncate(string s)
        {
            return s.Length > 1 ? s.Remove(1, (s.Length - 1)).Replace(",", ":") : s;
        }

        private static String Truncate(float s)
        {
            var s2 = Math.Ceiling(s).ToString();
            return s2;
            return s2.Length > 2 ? s2.Remove(2, (s2.Length - 2)).Replace(",", ":") : s2;
        }

        /// <summary>
        /// Loads the playersWrapper list.
        /// </summary>
        private static void LoadList()
        {
            try
            {
                foreach (var player in HeroManager.AllHeroes)
                {
                    var member = new PlayerWrapper
                    {
                        Hero = player,
                        Summoners =
                            new Tuple<string, string>(
                                player.Spellbook.GetSpell(SpellSlot.Summoner1).Name,
                                player.Spellbook.GetSpell(SpellSlot.Summoner2).Name)
                    };

                    playersWrapper.Add(member);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Tracker_Drawings", e, LogSeverity.Severe));
            }
            
        }

    }

    /// <summary>
    /// The TrackerWrapper class
    /// </summary>
    internal class TrackerWrapper
    {
        public Obj_AI_Hero Hero { get; set; }

        public Render.Sprite Hud { get; set; }

        public Render.Sprite Summoner1 { get; set; }

        public Render.Sprite Summoner2 { get; set; }

        public Render.Rectangle Spell1Rectangle { get; set; }

        public Render.Rectangle Spell2Rectangle { get; set; }

        public Render.Rectangle Spell3Rectangle { get; set; }

        public Render.Rectangle Spell4Rectangle { get; set; }

        public Render.Rectangle SummonerSpell1Rectangle { get; set; }

        public Render.Rectangle SummonerSpell2Rectangle { get; set; }

        public Render.Text SummonerSpell1Text { get; set; }

        public Render.Text SummonerSpell2Text { get; set; }

        public Render.Text Spell1Text { get; set; }

        public Render.Text Spell2Text { get; set; }

        public Render.Text Spell3Text { get; set; }

        public Render.Text Spell4Text { get; set; }

    }

    /// <summary>
    /// The Player Wrapper Class
    /// </summary>
    internal class PlayerWrapper
    {
        public Obj_AI_Hero Hero { get; set; }

        public Tuple<string, string> Summoners { get; set; }
    }
}
