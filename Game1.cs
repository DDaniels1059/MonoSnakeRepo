using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoSnake.Data;
using MonoSnake.Items;
using MonoSnake.Player;
using System;
using System.Collections.Generic;

namespace MonoSnake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kStateOld = Keyboard.GetState();
        private Rectangle _screenBounds;
        private Snake player;
        private Vector2 lastPosition;
        private Generator gen;
        private Map map;
        private Camera camera;


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
            _graphics.PreferMultiSampling = true;
            _graphics.SynchronizeWithVerticalRetrace = false; //Vsync
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
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

            map = new Map();
            player = new Snake();
            gen = new Generator();
            _screenBounds = new Rectangle(50, 50, (Map.width * 25) - 100, (Map.height * 25) - 80);

            //GameData.apples.Add(new Apple(300, 200));
            //GameData.apples.Add(new Apple(100, 400));
            //GameData.apples.Add(new Apple(700, 100));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastPosition = player.headPosition;

            KeyboardState kState = Keyboard.GetState();
            player.Update(gameTime, kState);
            gen.generatorUpdate(dt);

            if (kState.IsKeyDown(Keys.E) && kStateOld.IsKeyUp(Keys.E))
            {
                player.addSegment();
            }

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

            kStateOld = kState;
            GameData.Apples.RemoveAll(i => i.Collided);
            this.camera.Position = player.headPosition;
            this.camera.Update(gameTime);
            base.Update(gameTime);
        }

        public static float GetDepth(Vector2 origin, GraphicsDeviceManager _graphics)
        {
            float depth = origin.Y / _graphics.PreferredBackBufferHeight;
            depth = depth * 0.01f; // multiply the depth by a small value
            return depth;
        } 

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.DarkKhaki);
            Viewport viewport = GraphicsDevice.Viewport;

            //Dynamic Display
            _spriteBatch.Begin(this.camera, SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, null);


            player.Draw(_spriteBatch, _graphics);
            map.Draw(_spriteBatch, _graphics);


            foreach (Apple apple in GameData.Apples)
            {
                apple.Draw(_spriteBatch);
            }
            
            _spriteBatch.End();


            //Static Display
            _spriteBatch.Begin();
                _spriteBatch.DrawString(GameData.GameFont, "X : " + player.headCollider.X, new Vector2(50, 50), Color.Black);
                _spriteBatch.DrawString(GameData.GameFont, "Y : " + player.headCollider.Y, new Vector2(50, 80), Color.Black);
                _spriteBatch.DrawString(GameData.GameFont, "Apples : " + GameData.Apples.Count, new Vector2(50, 110), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
