using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Radar {
    public class ExtendedSpriteBatch : SpriteBatch {
        /// <summary>
        /// The texture used when drawing rectangles, lines and other 
        /// primitives. This is a 1x1 white texture created at runtime.
        /// </summary>
        public Texture2D WhiteTexture { get; protected set; }

        public ExtendedSpriteBatch(GraphicsDevice graphicsDevice)
            : base(graphicsDevice) {
            this.WhiteTexture = new Texture2D(this.GraphicsDevice, 1, 1);
            this.WhiteTexture.SetData(new Color[] { Color.White });
        }

        /// <summary>
        /// Draw a line between the two supplied points.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="end">End point.</param>
        /// <param name="color">The draw color.</param>
        public void DrawLine(Vector2 start, Vector2 end, Color color) {
            float length = (end - start).Length();
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            this.Draw(this.WhiteTexture, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The draw color.</param>
        public void DrawRectangle(Rectangle rectangle, Color color) {
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            this.Draw(this.WhiteTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);

        }

        /// <summary>
        /// Fill a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to fill.</param>
        /// <param name="color">The fill color.</param>
        public void FillRectangle(Rectangle rectangle, Color color) {
            this.Draw(this.WhiteTexture, rectangle, color);
        }
        public void DrawUnit(RadarUnit nUnit, Texture2D nTexture) {
            this.Draw(nTexture, nUnit.Position, null, nUnit.DrawColor, (float)(nUnit.Rotation-Math.PI / 2), new Vector2(nTexture.Width / 2, nTexture.Height / 2), 0.08f, SpriteEffects.None, 0f);
        }
        public void DrawPlayer(RadarPlayer nUnit, Texture2D nTexture, Vector2 nPlayerPos) {
            
            this.Draw(nTexture, new Vector2(-2*(nUnit.Position.Y - nPlayerPos.Y) + this.GraphicsDevice.Viewport.Width / 2, -2*(nUnit.Position.X - nPlayerPos.X) + this.GraphicsDevice.Viewport.Height / 2), null, nUnit.DrawColor, (float)(-nUnit.Rotation - Math.PI / 2), new Vector2(nTexture.Width / 2, nTexture.Height / 2), 0.08f, SpriteEffects.None, 0f);
        }
        public void DrawUnits(List<RadarUnit> nUnitlist, Texture2D nTexture) {
            foreach (RadarUnit u in nUnitlist) {
                this.DrawUnit(u, nTexture);
            }
        }
        public void DrawPlayers(List<RadarPlayer> nUnitlist, Texture2D nTexture, Vector2 nPlayerPos) {
            foreach (RadarPlayer u in nUnitlist) {
                this.DrawPlayer(u, nTexture, nPlayerPos);
            }
        }
    }
}
