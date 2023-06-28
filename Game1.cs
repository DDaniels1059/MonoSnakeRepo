using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoSnake.Data;
using MonoSnake.Items;
using MonoSnake.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MonoSnake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FpsMonitor FPSM = new FpsMonitor();
        private KeyboardState kStateOld = Keyboard.GetState();
        private MouseState mStateOld = Mouse.GetState();
        private Rectangle _screenBounds;
        private Vector2 lastPosition;

        private Button windowButton = new Button();
        private Button debugButton = new Button();

        private Snake player;
        private Generator gen;
        private Map map;
        private Camera camera;

        private Texture2D debugTexture;
        private Texture2D rect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        } 

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            //_graphics.PreferMultiSampling = true;
            //this.IsFixedTimeStep = true;
            //this._graphics.SynchronizeWithVerticalRetrace = false;
            //this.TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / 60L));
            Window.AllowUserResizing = false;
            Window.IsBorderless = false;
            Window.Title = "Mono-Snake V0.1";
            this.camera = new Camera(_graphics.GraphicsDevice);
            this.camera.Zoom = 2f;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameData.LoadData(Content);

            debugTexture = Content.Load<Texture2D>("Debug");

            rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.DarkSlateGray });


            map = new Map();
            player = new Snake();
            gen = new Generator();


            windowButton.CreateButton(GameData.TextureMap["FullScreen"], GameData.TextureMap["Windowed"], true, 2f);
            windowButton.buttonPress += WindowPress;

            debugButton.CreateButton(GameData.TextureMap["Debug"], GameData.TextureMap["Debug"], true, 2f);
            debugButton.buttonPress += DebugPress;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                KeyboardState kState = Keyboard.GetState();
                MouseState mState = Mouse.GetState();
                _screenBounds = new Rectangle(50, 50, (Map.width * 25) - 100, (Map.height * 25) - 80);

                lastPosition = player.headPosition;
                player.Update(gameTime, kState);
                gen.generatorUpdate(dt);

                //if (kState.IsKeyDown(Keys.E) && kStateOld.IsKeyUp(Keys.E))
                //{
                //    player.addSegment();
                //}

                if (!player.headCollider.Intersects(_screenBounds))
                {
                    player.headPosition = lastPosition;
                }

                foreach (Apple apple in GameData.Apples)
                {
                    if (player.headCollider.Intersects(apple.bounds))
                    {
                        player.addSegment();
                        apple.Collided = true;
                    }
                }


                windowButton.Update(mState, mStateOld, dt, new Vector2(_graphics.PreferredBackBufferWidth - 50, (_graphics.PreferredBackBufferHeight / 2) / 25));
                debugButton.Update(mState, mStateOld, dt, new Vector2(windowButton.location.X - 50, (_graphics.PreferredBackBufferHeight / 2) / 25));

                mStateOld = mState;
                kStateOld = kState;

                GameData.Apples.RemoveAll(i => i.Collided);
                FPSM.Update();

                this.camera.Position = player.headPosition;
                this.camera.Update(gameTime);
                base.Update(gameTime);
            }           
        }

        public static float GetDepth(Vector2 origin, GraphicsDeviceManager _graphics)
        {
            float depth = origin.Y / _graphics.PreferredBackBufferHeight;
            depth = depth * 0.01f; // multiply the depth by a small value
            return depth;
        } 

        public void WindowPress()
        {
            if (windowButton.GetToggleState())
            {
                //GameData.isFullscreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Window.IsBorderless = true;
                _graphics.ApplyChanges();
            }
            else
            {
                //GameData.isFullscreen = false;
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
                Window.IsBorderless = false;
                _graphics.ApplyChanges();
            }
        }

        public void DebugPress()
        {
            if (GameData.isDebug)
            {
                GameData.isDebug = false;
            }
            else
            {
                GameData.isDebug = true;
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            Color color = gameTime.IsRunningSlowly ? Color.Red : Color.CornflowerBlue;

            GraphicsDevice.Clear(Color.DarkKhaki);
            Viewport viewport = GraphicsDevice.Viewport;

            //Dynamic Display
            _spriteBatch.Begin(this.camera, SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            player.Draw(_spriteBatch, _graphics); 
            map.Draw(_spriteBatch, _graphics);


            foreach (Apple apple in GameData.Apples)
            {
                apple.Draw(_spriteBatch);
            }
            
            _spriteBatch.End();


            //Static Display
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if(GameData.isDebug)
            {
                FPSM.Draw(_spriteBatch, GameData.GameFont, new Vector2(50, 20), Color.White);
                _spriteBatch.DrawString(GameData.GameFont, "X : " + player.headCollider.X, new Vector2(50, 50), Color.Black);
                _spriteBatch.DrawString(GameData.GameFont, "Y : " + player.headCollider.Y, new Vector2(50, 80), Color.Black);
                _spriteBatch.DrawString(GameData.GameFont, "Apples : " + GameData.Apples.Count, new Vector2(50, 110), Color.Black);
            }

            foreach(Button button in GameData.ButtonList)
            {
                button.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class FpsMonitor
    {
        public float Value { get; private set; }
        public TimeSpan Sample { get; set; }
        private Stopwatch sw;
        private int Frames;
        public FpsMonitor()
        {
            this.Sample = TimeSpan.FromSeconds(1);
            this.Value = 0;
            this.Frames = 0;
            this.sw = Stopwatch.StartNew();
        }
        public void Update()
        {
            if (sw.Elapsed > Sample)
            {
                this.Value = (float)(Frames / sw.Elapsed.TotalSeconds);
                this.sw.Reset();
                this.sw.Start();
                this.Frames = 0;
            }
        }
        public void Draw(SpriteBatch SpriteBatch, SpriteFont Font, Vector2 Location, Color Color)
        {
            this.Frames++;
            SpriteBatch.DrawString(Font, "FPS: " + this.Value.ToString(), Location, Color);
        }
    }
}
