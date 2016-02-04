using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Radar {
    public class RadarUnit {
        private Color drawColor;
        private Vector2 position;
        private float rotation;
        private uint baseaddr;
        public Color DrawColor {
            get {
                return drawColor;
            }

            set {
                drawColor = value;
            }
        }

        public Vector2 Position {
            get {
                return position;
            }

            set {
                position = value;
            }
        }

        public float Rotation {
            get {
                return rotation;
            }

            set {
                rotation = value;
            }
        }

        public RadarUnit(Vector2 pos, Color color) {
            this.DrawColor = color;
            this.Position = pos;
            this.Rotation = 0f;
        }
        public RadarUnit(Vector2 pos, WoWClass wclass) {
            this.DrawColor = RadarUnit.GetColorFromWoWClass(wclass);
            this.Position = pos;
            this.Rotation = 0f;
        }
        public RadarUnit(ulong guid) {
            
        }

        
        public static Color GetColorFromWoWClass(WoWClass wclass) {
            switch (wclass) {
                case WoWClass.None:
                    return Color.Black;
                case WoWClass.DeathKnight:
                    return new Color(0.77f, 0.12f, 0.23f);
                case WoWClass.Druid:
                    return new Color(1.00f, 0.49f, 0.04f);
                case WoWClass.Hunter:
                    return new Color(0.67f, 0.83f, 0.45f);
                case WoWClass.Mage:
                    return new Color(0.41f, 0.80f, 0.94f);
                case WoWClass.Paladin:
                    return new Color(0.96f, 0.55f, 0.73f);
                case WoWClass.Priest:
                    return Color.White;
                case WoWClass.Rogue:
                    return new Color(1.00f, 0.96f, 0.41f);
                case WoWClass.Shaman:
                    return new Color(0.00f, 0.44f, 0.87f);
                case WoWClass.Warlock:
                    return new Color(0.58f, 0.51f, 0.79f);
                case WoWClass.Warrior:
                    return new Color(0.78f, 0.61f, 0.43f);
                default:
                    return Color.Black;
            }
        }
    }
}
