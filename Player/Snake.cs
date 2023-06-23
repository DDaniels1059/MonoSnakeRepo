using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoSnake.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoSnake.Player
{
    internal class Snake
    {
        private List<SnakeSegment> Body = new List<SnakeSegment>();
        private Rectangle headRectangle;
        private Vector2 spriteOrigin;
        private Vector2 headDrawPosition;
        private int speed = 300;
        private Direction direction;
        public Vector2 headPosition;
        public Rectangle headCollider;

        //Segment Variables
        Vector2 bodyPos;
        Vector2 newBodyOrigin;
        Vector2 previousSegmentPosition;
        Vector2 forwardSegmentPosition;
        float segmentRotation;
        enum Direction
        {
            None, Up, Down, Left, Right,
        }
        public Snake()
        {
            headRectangle = GameData.TextureCoordinates["snakeHead"];
            headPosition = new Vector2(150, 300);
            headCollider = new Rectangle((int)headPosition.X, (int)headPosition.Y, headRectangle.Width, headRectangle.Height);

            spriteOrigin = new Vector2(headRectangle.Width / 2f, headRectangle.Height / 2f);

            direction = Direction.Right;

            addSegment();
            addSegment();
        }

        public void Update(GameTime gameTime, KeyboardState kState)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.W))
            {
                //headPosition.Y -= speed * dt;

                direction = Direction.Up;
            }
            else if (kState.IsKeyDown(Keys.S))
            {
                //headPosition.Y += speed * dt;

                direction = Direction.Down;
            }
            else if (kState.IsKeyDown(Keys.A))
            {
                //headPosition.X -= speed * dt;

                direction = Direction.Left;
            }
            else if (kState.IsKeyDown(Keys.D))
            {
                //headPosition.X += speed * dt;

                direction = Direction.Right;
            }


            if (direction == Direction.Up)
            {
                headPosition.Y -= speed * dt;
            }
            else if (direction == Direction.Down)
            {
                headPosition.Y += speed * dt;
            }
            else if (direction == Direction.Left)
            {
                headPosition.X -= speed * dt;
            }
            else if (direction == Direction.Right)
            {
                headPosition.X += speed * dt;
            }

            // Update Head Collider Before Collision Check
            headCollider.Location = new Point((int)headPosition.X, (int)headPosition.Y);

            // Only update segments' positions when the snake is moving
            foreach (SnakeSegment segment in Body)
            {
                if (Body.Count > 0)
                {
                    segment.isTail = false;
                    SnakeSegment lastSegment = Body.Last();
                    lastSegment.isTail = true;
                }

                if (segment == Body[0]) // Check if it's the first segment
                {
                    // Use headPosition as the base position.
                    Vector2 norm = headPosition - segment.position;
                    float distance = Vector2.Distance(headPosition, segment.position);
                    if (distance > 20)
                    {
                        Vector2 orientation = Vector2.Normalize(norm);
                        float k = 35; // you can change this, larger numbers make the segments faster/stiffer, smaller numbers (closer to zero) make it slower/stretchier
                        segment.position -= orientation * (25 - distance) * k * dt;
                    }
                }
                else
                {
                    // Use the position of the segment in front as the base position
                    SnakeSegment previousSegment = Body[Body.IndexOf(segment) - 1];
                    Vector2 norm = previousSegment.position - segment.position;
                    float distance = Vector2.Distance(previousSegment.position, segment.position);
                    if (distance > 20)
                    {
                        Vector2 orientation = Vector2.Normalize(norm);
                        float k = 45; // you can change this, larger numbers make the segments faster/stiffer, smaller numbers (closer to zero) make it slower/stretchier
                        segment.position -= orientation * (25 - distance) * k * dt; // (55 - distance) is the Offset/Overshoot correction
                    }
                }

                segment.collider.Location = new Point((int)segment.position.X + 7, (int)segment.position.Y + 7);
            }

            segmentRotation = setRotation(direction); // Initialize the segment rotation
            foreach (SnakeSegment bodypart in Body)
            {
                bodyPos = bodypart.position + spriteOrigin;
                bodypart.newOrigin = new Vector2(bodyPos.X - 8, bodyPos.Y + 9);

                if (bodypart.isTail)
                {
                    previousSegmentPosition = Body[Body.IndexOf(bodypart) - 1].position;
                    segmentRotation = CalculateSegmentRotationRelativeToPrevious(bodypart.position, previousSegmentPosition);
                    //bodypart.rotation = segmentRotation;

                }
                else
                {
                    if (bodypart == Body[0])
                    {
                        forwardSegmentPosition = Body[Body.IndexOf(bodypart) + 1].position;
                        segmentRotation = CalculateSegmentRotation(bodypart.position, forwardSegmentPosition);
                    }
                    else
                    {
                        previousSegmentPosition = Body[Body.IndexOf(bodypart) - 1].position;
                        segmentRotation = CalculateSegmentRotationRelativeToPrevious(bodypart.position, previousSegmentPosition);
                    }
                }

                bodypart.rotation = segmentRotation;

            }

            headDrawPosition = headPosition + spriteOrigin;

            ////Check Collision
            //for (int i = 1; i < Body.Count; i++)
            //{
            //    if (Body[i].collider.Intersects(headCollider))
            //    {
            //        // Handle collision logic here
            //    }
            //}  
        }

        public void Draw(SpriteBatch _spritebatch, GraphicsDeviceManager _graphics)
        {     
            Vector2 newOrigin = new Vector2(headDrawPosition.X - 8, headDrawPosition.Y + 9);
            _spritebatch.Draw(GameData.TextureAtlas, new Vector2((int)headDrawPosition.X, (int)headDrawPosition.Y), headRectangle, Color.White, setRotation(direction), spriteOrigin, 1f, SpriteEffects.None, Game1.GetDepth(newOrigin, _graphics));
            //_spritebatch.Draw(GameData.Textures["Player"], newOrigin, null, Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);

            foreach (SnakeSegment bodypart in Body)
            {
                if (bodypart.isTail)
                {
                    _spritebatch.Draw(GameData.TextureAtlas, new Vector2((int)(bodypart.position + spriteOrigin).X, (int)(bodypart.position + spriteOrigin).Y), GameData.TextureCoordinates["snakeTail"], Color.White, bodypart.rotation, spriteOrigin, 1f, SpriteEffects.None, Game1.GetDepth(bodypart.newOrigin, _graphics));
                    //_spritebatch.Draw(GameData.Textures["Player"], newBodyOrigin, null, Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                }
                else
                {
                    _spritebatch.Draw(GameData.TextureAtlas, new Vector2((int)(bodypart.position + spriteOrigin).X, (int)(bodypart.position + spriteOrigin).Y), GameData.TextureCoordinates["snakeBody"], Color.White, bodypart.rotation, spriteOrigin, 1f, SpriteEffects.None, Game1.GetDepth(bodypart.newOrigin, _graphics));
                    //_spritebatch.Draw(GameData.Textures["Player"], newBodyOrigin, null, Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
                }
            }
        }

        public void addSegment()
        {
            SnakeSegment bodypart = new SnakeSegment();

            if (Body.Count > 0)
            {
                SnakeSegment lastpart = Body.LastOrDefault();
                bodypart.position = lastpart.position;
                bodypart.collider.Location = new Point((int)lastpart.position.X + 7, (int)lastpart.position.Y + 7);

            }
            else
            {
                bodypart.position = headPosition;
                bodypart.collider.Location = new Point((int)headPosition.X + 7, (int)headPosition.Y + 7);
            }

            Body.Add(bodypart);
        }

        private float CalculateSegmentRotation(Vector2 position, Vector2 targetPosition)
        {
            Vector2 directionToTarget = Vector2.Normalize(targetPosition - position);
            return (float)Math.Atan2(directionToTarget.Y, directionToTarget.X);
        }

        private float CalculateSegmentRotationRelativeToPrevious(Vector2 currentPosition, Vector2 previousPosition)
        {
            Vector2 directionToPreviousSegment = Vector2.Normalize(previousPosition - currentPosition);
            return (float)Math.Atan2(directionToPreviousSegment.Y, directionToPreviousSegment.X);
        }

        private float setRotation(Direction directon)
        {
            float rotation = 0f;
            switch (direction)
            {
                case Direction.Up:
                    rotation = MathHelper.ToRadians(-90f);
                    break;
                case Direction.Down:
                    rotation = MathHelper.ToRadians(90f);
                    break;
                case Direction.Left:
                    rotation = MathHelper.ToRadians(180f);
                    break;
                case Direction.Right:
                    rotation = 0f;
                    break;
                default:
                    break;
            }

            return rotation;
        }
    }
}
