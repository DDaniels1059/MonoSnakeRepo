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
        public static List<Button> ButtonList;
        public static List<Apple> Apples;

        public static Dictionary<int, Rectangle> TextureCoords;
        public static Dictionary<string, Rectangle> TextureMap;

        public static Texture2D TextureAtlas;
        public static SpriteFont GameFont;

        public static bool isFullScreen = false;
        public static bool isDebug = false;

        public static void LoadData(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Misc/TextureAtlas");
            TextureMap = new Dictionary<string, Rectangle>
            {                                  //X   Y   W    H
                //Player
                ["Player"] =       new Rectangle(0,  0,  25, 25),
                ["snakeTail"] =    new Rectangle(25, 0,  25, 25),
                ["snakeBody"] =    new Rectangle(50, 0,  25, 25),
                ["snakeHead"] =    new Rectangle(75, 0,  25, 25),

                //Plants
                ["cactus"] =       new Rectangle(0,  25, 25, 25),
                ["cactusflower"] = new Rectangle(25, 25, 25, 25),
                ["cactussingle"] = new Rectangle(50, 25, 25, 25),
                ["Grass1"] =       new Rectangle(75, 25, 25, 25),
                ["Grass2"] =       new Rectangle(0,  50, 25, 25),

                //ICONS
                ["FullScreen"] =   new Rectangle(25, 50, 25, 25),
                ["Windowed"] =     new Rectangle(50, 50, 25, 25),
                ["Debug"] =        new Rectangle(75, 50, 25, 25)

            };

            TextureCoords = new Dictionary<int, Rectangle>
            {
                { 1, TextureMap["cactus"] },
                { 2, TextureMap["cactusflower"] },
                { 3, TextureMap["cactussingle"] },
                { 4, TextureMap["Grass1"] },
                { 5, TextureMap["Grass2"] }
            };

            Apples = new List<Apple>();
            ButtonList = new List<Button>();
            GameFont = content.Load<SpriteFont>("Misc/gameFont");
        }
    }
}
