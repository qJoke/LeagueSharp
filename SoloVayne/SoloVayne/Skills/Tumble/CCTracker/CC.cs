using System;
using LeagueSharp;

namespace SoloVayne.Skills.Tumble.CCTracker
{
    class CC
    {
        public CC(string ChampName, SpellSlot Slot, float Range, CCRange RangeType, CCType Type)
        {
            this.ChampName = ChampName;
            this.Slot = Slot;
            this.Range = Range;
            this.RangeType = RangeType;
            this.Type = Type;
        }

        public CC(string ChampName, SpellSlot Slot, float Range, float AOERadius, CCRange RangeType, CCType Type)
        {
            this.ChampName = ChampName;
            this.Slot = Slot;
            this.Range = Range;
            this.AOERadius = AOERadius;
            this.RangeType = RangeType;
            this.Type = Type;
        }

        public CC(string ChampName, SpellSlot Slot, float Range, CCRange RangeType, CCType Type, ConditionDelegate d)
        {
            this.ChampName = ChampName;
            this.Slot = Slot;
            this.Range = Range;
            this.RangeType = RangeType;
            this.Type = Type;
            this.NecessaryCondition = d;
        }

        public CC(string ChampName, SpellSlot Slot, float Range, float AOERadius, CCRange RangeType, CCType Type, ConditionDelegate d)
        {
            this.ChampName = ChampName;
            this.Slot = Slot;
            this.Range = Range;
            this.AOERadius = AOERadius;
            this.RangeType = RangeType;
            this.Type = Type;
            this.NecessaryCondition = d;
        }

        public string ChampName { get; set; }

        public SpellSlot Slot { get; set; }

        public float Range { get; set; }

        public float AOERadius { get; set; }

        public CCRange RangeType { get; set; }

        public CCType Type { get; set; }

        public ConditionDelegate NecessaryCondition { get; set; }

        public delegate bool ConditionDelegate();

        public float GetDangerValue()
        {
            //Some Calculations here
            return 0f;
        }

    }

    enum CCRange
    {
        Ranged, Melee
    }

    enum CCType
    {
        AOE, Targetted, AOEFromChamp
    }
}
