using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoSnake.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoSnake.Player
{
    public class SnakeSegment
    {
        public Vector2 position;
        public Rectangle textureRectangle; // Updated field to store the texture rectangle from the sprite atlas
        public Rectangle collider;
        public bool isTail = false;
        public bool isHead = false;
        public float rotation;
        public Vector2 newOrigin = Vector2.Zero;
        public SnakeSegment()
        {
            textureRectangle = GameData.TextureMap["Player"];
            collider = new Rectangle((int)position.X + 7, (int)position.Y + 7, textureRectangle.Width - 15, textureRectangle.Height - 15);
            rotation = 0f; // Initialize the rotation to 0
        }
    }
}
