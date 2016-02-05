using Radar.Bellona.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.WoWModels.Spells {
    class SpellWithCooldown : Spell{
        private System.Threading.Timer cooldownTimer;
        private int cooldown;
        private bool isitReady;
        public SpellWithCooldown(uint i, ConstController.WindowsVirtualKey kb, int cooldown) : base(i, kb) {
            this.cooldown = cooldown;
        }
        
    }
}
