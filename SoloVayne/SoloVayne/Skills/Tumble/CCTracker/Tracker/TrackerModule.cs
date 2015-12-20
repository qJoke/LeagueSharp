using LeagueSharp;

namespace SoloVayne.Skills.Tumble.CCTracker.Tracker
{
    class TrackerModule
    {
        private Obj_AI_Hero Champ;

        public TrackerModule(Obj_AI_Hero champion)
        {
            Champ = champion;
        }

        public float GetSpellCooldown(SpellSlot slot)
        {
            if (Champ == null)
            {
                return -1;
            }

            var spellInstance = GetChampion().Spellbook.GetSpell(slot);
            var spellCooldown = spellInstance.CooldownExpires - Game.Time;
            if (spellInstance.Level == 0)
            {
                return -1;
            }

            return spellCooldown > 0 ? spellCooldown : 0f;
        }

        public Obj_AI_Hero GetChampion()
        {
            return Champ;
        }

        public string GetChampionName()
        {
            return Champ.ChampionName;
        }
    }
}
