using Microsoft.Xna.Framework;
using Radar.Bellona.MemoryReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radar {
    public class RadarPlayer : RadarUnit {
        private bool selected = false;
        public RadarPlayer(Vector2 pos, Color color) : base (pos, color) {

        }
        public RadarPlayer(GameObject go) : base(new Vector2(go.Unit.Position.X,go.Unit.Position.Y), (float)go.Unit.Rotation, go.Unit.WowClass) {
            selected = true;
        }
        public RadarPlayer(GameObject go, int rot) : base(new Vector2(go.Unit.Position.X, go.Unit.Position.Y), (float)rot, go.Unit.WowClass) {
            selected = true;
        }
    }
}
