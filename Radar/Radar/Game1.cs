using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Radar {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        ExtendedSpriteBatch spriteBatch;
        public Texture2D UnitTexture;
        private Rectangle TitleSafe;
        private MouseState mouse = new MouseState();
        private Vector2 pos = new Vector2();
        private Vector2 startPoint = new Vector2();
        private Rectangle selectionBox = new Rectangle();
        private List<RadarUnit> unitsToDraw = new List<RadarUnit>();
        private bool selecting;
        private bool drawthisshit;
        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            unitsToDraw.Add(new RadarUnit(new Vector2(200, 300), WoWClass.Mage));
            unitsToDraw.Add(new RadarUnit(new Vector2(500, 300), WoWClass.Rogue));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            UnitTexture = Content.Load<Texture2D>("arrow");
            //TitleSafe = GetTitleSafeArea(.8f);
            //pos.X = GraphicsDevice.Viewport.Width / 2;
            //pos.Y = GraphicsDevice.Viewport.Height / 2;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed) {
                if (selecting) {
                    selectionBox.Width = (int)Math.Abs(mouse.X - startPoint.X);
                    selectionBox.Height = (int)Math.Abs(mouse.Y - startPoint.Y);
                    //Set the start point to the top left, which is the minimum X and minimum Y
                    //Choose between the start point and the current mouse position
                    selectionBox.X = (int)Math.Min(startPoint.X, mouse.X);
                    selectionBox.Y = (int)Math.Min(startPoint.Y, mouse.Y);
                    drawthisshit = true;
                }
                else {
                    selecting = true;
                    startPoint.X = mouse.X;
                    startPoint.Y = mouse.Y;
                    selectionBox.X = mouse.X;
                    selectionBox.Y = mouse.Y;
                }
            }
            else {
                drawthisshit = false;
                selecting = false;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.FillRectangle(new Rectangle(mouse.X, mouse.Y, 5, 5), Color.Blue);
            // TODO: Add your drawing code here
            if (drawthisshit) {
                spriteBatch.DrawRectangle(selectionBox, Color.Red);
            }
            spriteBatch.DrawUnits(unitsToDraw, UnitTexture);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected Rectangle GetTitleSafeArea(float percent) {
            Rectangle retval = new Rectangle(
                graphics.GraphicsDevice.Viewport.X,
                graphics.GraphicsDevice.Viewport.Y,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
#if XBOX
            // Find Title Safe area of Xbox 360.
            float border = (1 - percent) / 2;
            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);
            return retval;            
#else
            return retval;
#endif
        }
    }
}
