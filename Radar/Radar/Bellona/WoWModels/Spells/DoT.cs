using Radar.Bellona.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.WoWModels.Spells {
    public class DoT : Spell {
        public DoT(uint i) : base(i) {

        }
        public DoT(uint i, ConstController.WindowsVirtualKey kb) : base(i, kb) {

        }
        public virtual bool ReCast(WoWGlobal wowinfo, WoWUnit unit) {
            if (!unit.HasBuff(this.ID) /*&& !wowinfo.SpellIsPending*/) {
                this.SendCast();
                return true;
            }
            else {
                return false;
            }
        }
    }
}
