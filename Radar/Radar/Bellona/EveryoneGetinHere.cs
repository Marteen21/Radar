using Magic;
using Microsoft.Xna.Framework;
using Radar.Bellona.MemoryReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radar.Bellona {
    class EveryoneGetinHere {
        public static List<GameObject> NearbyGameObjects = new List<GameObject>();
        public static List<GameObject> newGameObjects = new List<GameObject>();

        public static void GetEveryObject(ref List<RadarPlayer> players, Vector2 playerpos, BlackMagic w) {
            players.Clear();
            GameObject TempObject = new GameObject(Initializer.FirstObject);
            int i = 0;
            while ((uint)TempObject.BaseAddress != 0 && i < 300) {
                i++;
                TempObject.Unit = new WoWModels.WoWUnit();
                TempObject.Unit.RefreshForRadar(w, TempObject);
                if (Vector3.Distance(TempObject.Unit.Position, new Vector3(playerpos, TempObject.Unit.Position.Z)) < 50) {
                    players.Add(new RadarPlayer(TempObject));
                }
                try {
                    TempObject = new GameObject(w, (UIntPtr)w.ReadUInt(((uint)TempObject.BaseAddress + (uint)ConstOffsets.ObjectManager.NextObject)));
                }
                catch {
                    return;
                }
            }

        }
        public static void RefreshNearbyGameObjects(Vector2 playerpos, BlackMagic w, int threshhold) {
            GameObject TempObject = new GameObject(Initializer.FirstObject);
            int i = 0;
            NearbyGameObjects.Clear();
            while ((uint)TempObject.BaseAddress != 0) {
                i++;
                TempObject.Unit = new WoWModels.WoWUnit();
                TempObject.Unit.RefreshForRadar(w, TempObject);
                //if (Vector3.Distance(TempObject.Unit.Position, new Vector3(playerpos, TempObject.Unit.Position.Z)) < threshhold) {
                NearbyGameObjects.Add(TempObject);
                //}
                try {
                    TempObject = new GameObject(w, (UIntPtr)w.ReadUInt(((uint)TempObject.BaseAddress + (uint)ConstOffsets.ObjectManager.NextObject)));
                }
                catch {
                    break;
                }

            }

        }
        public static void RefreshNewGameObjects(Vector2 playerpos, BlackMagic w) {
            GameObject TempObject = new GameObject(Initializer.FirstObject);
            int i = 0;
            newGameObjects.Clear();
            while ((uint)TempObject.BaseAddress != 0 && i < 500) {
                i++;
                TempObject.Unit = new WoWModels.WoWUnit();
                TempObject.Unit.RefreshForRadar(w, TempObject);
                bool shit = false;
                foreach(GameObject go in NearbyGameObjects) {
                    if (go.GUID == TempObject.GUID) {
                        shit = true;
                    }
                }
                if (!shit) {
                    newGameObjects.Add(TempObject);
                }
                try {
                    TempObject = new GameObject(w, (UIntPtr)w.ReadUInt(((uint)TempObject.BaseAddress + (uint)ConstOffsets.ObjectManager.NextObject)));
                }
                catch {
                    break;
                }

            }
            foreach(GameObject ngo in newGameObjects) {
                ngo.Unit.Position = new Vector3(w.ReadFloat((uint)newGameObjects[0].BaseAddress + 0x110), w.ReadFloat((uint)newGameObjects[0].BaseAddress + 0x114), 104);
            }
        }

    }
}
