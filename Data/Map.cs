using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoSnake.Data
{
    internal class Map
    {
        public static string[] mapData;
        public static int width;
        public static int height;
        private int[,] tileData;
        private Random random;
        public Map()
        {
            mapData = File.ReadAllLines("Content/Misc/map.txt");
            width = mapData[0].Length;
            height = mapData.Length;
            tileData = new int[width, height];
            random = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tileData[x, y] = mapData[y][x] - '0'; // Subtract '0' to convert from ASCII to integer
                }
            }            
        }

        public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int tileValue = tileData[x, y];
                    Rectangle textureRectangle;
                    Vector2 origin = new Vector2(x * 25, y * 25); // Set the origin to the center of the texture
                    Vector2 neworigin = new Vector2(origin.X + (int)10, origin.Y + 30);
                    if (GameData.TextureCoords.TryGetValue(tileValue, out textureRectangle))
                    {
                        if (tileValue == 4 || tileValue == 5)
                        {   
                            _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle(x * 25, y * 25, 35, 35), textureRectangle, Color.White, -0f, Vector2.Zero, SpriteEffects.None, Game1.GetDepth(neworigin, _graphics));
                            //_spriteBatch.DrawString(GameData.GameFont, "Pos: " + Game1.GetDepth(neworigin, _graphics), new Vector2(neworigin.X - 20, neworigin.Y + 10), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);
                            //_spriteBatch.Draw(GameData.Textures["Player"], neworigin, null, Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 1f);
                        }
                        else
                        {
                            _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle(x * 25, y * 25, 25, 25), textureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                        }
                    }
                }
            }
        }
    }
}