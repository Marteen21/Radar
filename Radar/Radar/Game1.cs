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
using Radar.Bellona.WoWModels;
using Magic;
using Radar.Bellona.MemoryReading;

namespace Radar {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        ExtendedSpriteBatch spriteBatch;
        public Texture2D UnitTexture;
        public Texture2D SpellTexture;
        private MouseState mouse = new MouseState();
        private Vector2 startPoint = new Vector2();
        private Rectangle selectionBox = new Rectangle();
        private List<RadarPlayer> unitsToDraw = new List<RadarPlayer>();
        private List<RadarPlayer> spellsToDraw = new List<RadarPlayer>();

        private BlackMagic wow;
        private Vector2 PlayerPos;
        private static WoWGlobal clientInfo;
        private bool selecting;
        private bool drawthisshit;
        private TimeSpan bela = TimeSpan.Zero;
        private TimeSpan gyula = TimeSpan.Zero;

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
            if (!Initializer.ConnectToGame(out wow, Program.PROCESS_WINDOW_TITLE)) {
                throw new Exception("No compatible Client");
            }
            clientInfo = new WoWGlobal(wow);
            base.Initialize();
            Vector3 beluka = new GameObject(wow, clientInfo.PlayerGUID).Unit.Position;
            PlayerPos = new Vector2(beluka.X, beluka.Y);

            Bellona.EveryoneGetinHere.RefreshNearbyGameObjects(PlayerPos, wow, 500);

            Bellona.EveryoneGetinHere.RefreshNewGameObjects(PlayerPos, wow);

            System.Diagnostics.Debug.WriteLine("LOL");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            UnitTexture = Content.Load<Texture2D>("arrow");
            SpellTexture = Content.Load<Texture2D>("run");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }
            SelectionBoxRefresh();
            if ((gameTime.TotalGameTime - this.gyula) > TimeSpan.FromMilliseconds(750)) {
                this.gyula = gameTime.TotalGameTime;
                //Bellona.EveryoneGetinHere.RefreshNearbyGameObjects(PlayerPos, wow, 50);
                Bellona.EveryoneGetinHere.RefreshNewGameObjects(PlayerPos, wow);
            }
            if ((gameTime.TotalGameTime - this.bela) > TimeSpan.FromMilliseconds(125)) {
                this.bela = gameTime.TotalGameTime;
                WoWRaid wr = new WoWRaid(wow);
                Vector3 beluka = new GameObject(wow, clientInfo.PlayerGUID).Unit.Position;
                PlayerPos = new Vector2(beluka.X, beluka.Y);
                Console.WriteLine(Bellona.EveryoneGetinHere.NearbyGameObjects.Count + " " + Bellona.EveryoneGetinHere.newGameObjects.Count);
                unitsToDraw.Clear();
                foreach (GameObject go in Bellona.EveryoneGetinHere.NearbyGameObjects) {
                    go.Unit.RefreshForRadar(wow, go);
                    unitsToDraw.Add(new RadarPlayer(go));
                }
                spellsToDraw.Clear();
                foreach (GameObject go in Bellona.EveryoneGetinHere.newGameObjects) {
                    spellsToDraw.Add(new RadarPlayer(go,0));
                }
            }
            base.Update(gameTime);
        }

        private void SelectionBoxRefresh() {
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
            spriteBatch.DrawPlayers(unitsToDraw, UnitTexture, PlayerPos);
            spriteBatch.DrawPlayers(spellsToDraw, SpellTexture, PlayerPos);
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
