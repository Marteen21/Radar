﻿using Radar.Bellona.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.WoWModels.Spells {
    class DKRuneSpell : DoT {
        private DKSpellRuneCost cost;

        public DKRuneSpell(uint i, DKSpellRuneCost c) : base(i) {
            this.cost = c;
        }
        public DKRuneSpell(uint i, ConstController.WindowsVirtualKey kb, DKSpellRuneCost c) : base(i, kb) {
            this.cost = c;
        }
        public override bool ReCast(WoWGlobal wowinfo, WoWUnit unit) {
            if (!unit.HasBuff(this.ID) && !wowinfo.SpellIsPending && wowinfo.HasRunesFor(cost)) {
                this.SendCast();
                return true;
            }
            else {
                return false;
            }
        }
        public bool CastIfHasRunesFor(WoWGlobal wowinfo) {
            if (wowinfo.HasRunesFor(cost)) {
                this.SendCast();
                return true;
            }
            else {
                return false;
            }
        }
        
    }
}
