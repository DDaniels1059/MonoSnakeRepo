using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoSnake.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoSnake.Items
{
    public class Apple
    {
        public Rectangle bounds;
        public Rectangle textureRectangle; // Updated field to store the texture rectangle from the sprite atlas
        public bool Collided = false;

        public Apple(int X, int Y)
        {
            textureRectangle = GameData.TextureMap["Player"]; // Retrieve the texture rectangle for "Player" from the sprite atlas
            bounds = new Rectangle(X, Y, textureRectangle.Width, textureRectangle.Height);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(GameData.TextureAtlas, bounds, textureRectangle, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
