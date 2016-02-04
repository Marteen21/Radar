using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radar {
    class Player : RadarUnit {
        private bool selected = false;
        public Player(Vector2 pos, Color color) : base (pos, color) {
            this.DrawColor = color;
            this.Position = pos;
            this.Rotation = 0f;
        }
    }
}
