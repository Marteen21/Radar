﻿using Magic;
using Radar.Bellona.MemoryReading;
using Radar.Bellona.WoWModels.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.WoWModels {

    public class WoWGlobal {
        private UInt64 playerGUID;
        private UInt64 targetGUID;
        private UInt64 focusGUID;
        private byte comboPoints;
        private byte runes;
        private bool spellIsPending;
        private MarkerGUIDs markers;
        #region properties
        public ulong PlayerGUID {
            get {
                return playerGUID;
            }

            set {
                playerGUID = value;
            }
        }

        public ulong TargetGUID {
            get {
                return targetGUID;
            }

            set {
                targetGUID = value;
            }
        }

        public ulong FocusGUID {
            get {
                return focusGUID;
            }

            set {
                focusGUID = value;
            }
        }

        public byte ComboPoints {
            get {
                return comboPoints;
            }

            set {
                comboPoints = value;
            }
        }

        public bool SpellIsPending {
            get {
                return spellIsPending;
            }

            set {
                spellIsPending = value;
            }
        }

        public MarkerGUIDs Markers {
            get {
                return markers;
            }

            set {
                markers = value;
            }
        }
        #endregion
        public WoWGlobal(BlackMagic w) {
            this.Refresh(w);
        }
        public void Refresh(BlackMagic w) {
            try {
                this.PlayerGUID = w.ReadUInt64((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.PlayerGUID);
                this.TargetGUID = w.ReadUInt64((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.CurrentTargetGUID);
                this.FocusGUID = w.ReadUInt64((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.FocusTargetGUID);
                this.ComboPoints = w.ReadByte((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.ComboPoints);
                this.SpellIsPending = !(w.ReadByte((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.SpellIsPending) == 0);
                this.runes = w.ReadByte((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.Runes);
                this.Markers = new MarkerGUIDs(w.ReadBytes((uint)w.MainModule.BaseAddress + (uint)ConstOffsets.Globals.Markers,64));
            }
            catch {

            }
        }
        public bool HasRunesFor(DKSpellRuneCost dkspc) {
            foreach (Rune r in dkspc.Costs) {
                if (!HasRune(r)) {
                    return false;
                }
            }
            return true;
        }
        private bool HasRune(Rune r) {
            switch (r.Type) {
                case RuneType.Blood:
                    if (r.Cost <= GetBloodRunes()) {
                        return true;
                    }
                    else {
                        return false;
                    }
                case RuneType.Frost:
                    if (r.Cost <= GetFrostRunes()) {
                        return true;
                    }
                    else {
                        return false;
                    }
                case RuneType.Unholy:
                    if (r.Cost <= GetUnholyRunes()) {
                        return true;
                    }
                    else {
                        return false;
                    }
            }
            return false;
        }
        public int GetBloodRunes() {
            int temp = this.runes & 0x03;
            return Convert.ToString(temp, 2).ToCharArray().Count(c => c == '1');

        }
        public int GetFrostRunes() {
            int temp = this.runes & 0x30;
            return Convert.ToString(temp, 2).ToCharArray().Count(c => c == '1');
        }
        public int GetUnholyRunes() {
            int temp = this.runes & 0x0C;
            return Convert.ToString(temp, 2).ToCharArray().Count(c => c == '1');
        }
    }

}
