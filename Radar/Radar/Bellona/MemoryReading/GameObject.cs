﻿using Magic;
using Radar.Bellona.WoWModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.MemoryReading {
    public class GameObject {
        private UIntPtr baseAddress;
        private UInt64 guid;
        private UIntPtr descriptorArrayAddress;
        private UIntPtr buffBigArrayAddress;
        private UIntPtr buffSmallArrayAddress;
        private UIntPtr movementArrayAddress;
        private WoWUnit unit;
        #region properties
        public UIntPtr BaseAddress {
            get {
                return baseAddress;
            }

            set {
                baseAddress = value;
            }
        }

        public ulong GUID {
            get {
                return guid;
            }

            set {
                guid = value;
            }
        }

        public UIntPtr DescriptorArrayAddress {
            get {
                return descriptorArrayAddress;
            }

            set {
                descriptorArrayAddress = value;
            }
        }

        public UIntPtr BuffBigArrayAddress {
            get {
                return buffBigArrayAddress;
            }

            set {
                buffBigArrayAddress = value;
            }
        }

        public UIntPtr BuffSmallArrayAddress {
            get {
                return buffSmallArrayAddress;
            }

            set {
                buffSmallArrayAddress = value;
            }
        }

        internal WoWUnit Unit {
            get {
                return unit;
            }

            set {
                unit = value;
            }
        }

        public UIntPtr MovementArrayAddress {
            get {
                return movementArrayAddress;
            }

            set {
                movementArrayAddress = value;
            }
        }
        #endregion
        #region constructors
        public GameObject(GameObject other) {
            this.BaseAddress = other.BaseAddress;
            this.GUID = other.GUID;
            this.DescriptorArrayAddress = other.DescriptorArrayAddress;
            this.BuffBigArrayAddress = other.BuffBigArrayAddress;
            this.BuffSmallArrayAddress = other.BuffSmallArrayAddress;
            this.Unit = new WoWUnit();
        }
        public GameObject(BlackMagic w, UIntPtr baddr) {
            try {
                this.BaseAddress = baddr;
                this.GUID = w.ReadUInt64((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalGUID);
                this.descriptorArrayAddress = (UIntPtr)w.ReadUInt((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalDescriptorArray) + 0x10;
                this.BuffBigArrayAddress = (UIntPtr)w.ReadUInt((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalBuffBigArray) + 0x4;
                this.BuffSmallArrayAddress = (UIntPtr)((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalBuffSmallArray);
                this.MovementArrayAddress = (UIntPtr)w.ReadUInt((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalMovementArray);
                this.Unit = new WoWUnit();
            }
            catch {
                //Program.WowPrinter.Print(ConstStrings.GameObjectConstructorError);
                this.Unit = new WoWUnit();
            }
        }
        public GameObject(BlackMagic w, UInt64 guid) {
            try {
                this.GUID = guid;
                if (this.GUID != 0) {
                    GameObject TempObject = new GameObject(Initializer.FirstObject);
                    while ((uint)TempObject.BaseAddress != 0) {
                        if (TempObject.GUID == this.GUID) {
                            this.BaseAddress = TempObject.BaseAddress;
                            this.GUID = TempObject.GUID;
                            this.DescriptorArrayAddress = TempObject.DescriptorArrayAddress;
                            this.BuffBigArrayAddress = TempObject.BuffBigArrayAddress;
                            this.BuffSmallArrayAddress = TempObject.BuffSmallArrayAddress;
                            this.MovementArrayAddress = (UIntPtr)w.ReadUInt((uint)this.BaseAddress + (uint)ConstOffsets.ObjectManager.LocalMovementArray);
                            this.RefreshUnit(w);
                            return;
                        }
                        else {
                            TempObject = new GameObject(w, (UIntPtr)w.ReadUInt(((uint)TempObject.BaseAddress + (uint)ConstOffsets.ObjectManager.NextObject)));
                        }
                    }
                    //Program.WowPrinter.Print(new Error(String.Format("Couldnt Find Gameobject with GUID 0x{0:X16}", guid)));
                }
                this.Unit = new WoWUnit();

            }
            catch {
                this.Unit = new WoWUnit();
                //Program.WowPrinter.Print(ConstStrings.GameObjectConstructorError);
            }
        }
        #endregion
        public void RefreshUnit(BlackMagic w) {
            if (this.BaseAddress != (UIntPtr)0) {
                this.Unit = new WoWUnit(w, this);
            }
        }
        public static bool HPMin(ref GameObject minimum, GameObject next) {
            if (next.GUID == 0 || minimum.unit.GetHealthPercent() < next.Unit.GetHealthPercent()) {
                return false;
            }
            else{
                minimum = next;
                return true;
            }
        }
    }
}
