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
        private static int[,] tileData;
        private Dictionary<(int x, int y), int> tileTextureIndices;
        private Random random;
        public Map()
        {
            mapData = File.ReadAllLines("Content/Misc/Map.txt");
            width = mapData[0].Length;
            height = mapData.Length;
            tileData = new int[width, height];
            tileTextureIndices = new Dictionary<(int x, int y), int>();
            random = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tileData[x, y] = mapData[y][x] - '0'; // Subtract '0' to convert from ASCII to integer

                    if (tileData[x, y] == 1)
                    {
                        // Check if the tile already has a stored texture index
                        if (!tileTextureIndices.ContainsKey((x, y)))
                        {
                            // If not, generate a random texture index
                            int randomTextureIndex = random.Next(3);
                            tileTextureIndices[(x, y)] = randomTextureIndex;
                        }
                    }

                    if (tileData[x, y] == 2)
                    {
                        if (!tileTextureIndices.ContainsKey((x, y)))
                        {
                            int randomTextureIndex = random.Next(2);
                            tileTextureIndices[(x, y)] = randomTextureIndex;
                        }
                    }
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
                    Vector2 origin = new Vector2(x * 25, y * 25);
                    Vector2 neworigin = new Vector2(origin.X + (int)10, origin.Y + 30);
                    if (GameData.TextureCoords.TryGetValue(tileValue, out textureRectangle))
                    {
                        if (tileValue == 1)
                        {
                            int textureIndex = tileTextureIndices[(x, y)]; // Get the stored texture index
                            textureRectangle = GetRandomTextureRectangle(textureIndex, tileValue);
                            _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle(x * 25, y * 25, 25, 25), textureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Game1.GetDepth(neworigin, _graphics));
                        }
                        if (tileValue == 2)
                        {
                            int textureIndex = tileTextureIndices[(x, y)];
                            textureRectangle = GetRandomTextureRectangle(textureIndex, tileValue);
                            _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle(x * 25, y * 25, 35, 35), textureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Game1.GetDepth(neworigin, _graphics));

                        }
                    }
                }
            }
        }

        private Rectangle GetRandomTextureRectangle(int textureIndex, int tileValue)
        {
            if(tileValue == 1)
            {
                Rectangle[] possibleTextures = new Rectangle[]
                {
                    new Rectangle(0, 25, 25, 25),
                    new Rectangle(25, 25, 25, 25),
                    new Rectangle(50, 25, 25, 25)
                };
                return possibleTextures[textureIndex];
            }

            if (tileValue == 2)
            {
                Rectangle[] possibleTextures = new Rectangle[]
                {
                    new Rectangle(75, 25, 25, 25),
                    new Rectangle(0, 50, 25, 25)
                };
                return possibleTextures[textureIndex];
            }

            return new Rectangle(0, 0, 25, 25);
        }
    }
}