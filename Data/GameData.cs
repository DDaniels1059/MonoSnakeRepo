using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoSnake.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoSnake.Data
{
    public class GameData
    {
        public static SpriteFont GameFont;
        public static List<Apple> Apples;
        public static Dictionary<int, Rectangle> TextureMap;
        public static Texture2D TextureAtlas;
        public static Dictionary<string, Rectangle> TextureCoordinates;



        public static void LoadData(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Misc/TextureAtlas");
            TextureCoordinates = new Dictionary<string, Rectangle>
            {                                  //X   Y   W    H
                ["Player"] =       new Rectangle(0,  0,  25, 25),
                ["snakeTail"] =    new Rectangle(25, 0,  25, 25),
                ["snakeBody"] =    new Rectangle(50, 0,  25, 25),
                ["snakeHead"] =    new Rectangle(75, 0,  25, 25),
                ["cactus"] =       new Rectangle(0,  25, 25, 25),
                ["cactusflower"] = new Rectangle(25, 25, 25, 25),
                ["cactussingle"] = new Rectangle(50, 25, 25, 25),
                ["Grass1"] =       new Rectangle(75, 25, 25, 25),
                ["Grass2"] =       new Rectangle(0,  50, 25, 25)
            };

            TextureMap = new Dictionary<int, Rectangle>
            {
                { 1, TextureCoordinates["cactus"] },
                { 2, TextureCoordinates["cactusflower"] },
                { 3, TextureCoordinates["cactussingle"] },
                { 4, TextureCoordinates["Grass1"] },
                { 5, TextureCoordinates["Grass2"] }
            };

            Apples = new List<Apple>();
            GameFont = content.Load<SpriteFont>("Misc/gameFont");
        }
    }
}
