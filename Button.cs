using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoSnake.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoSnake
{
    public class Button
    {
        private Rectangle defaultSprite;
        private Rectangle pressedSprite;
        private Rectangle bounds;
        private bool isPressed = false;
        private bool isToggle = false;

        private bool toggled = false;
        private float timer = 100;
        private float scale = 1f;

        public delegate void onPress();
        public onPress buttonPress;
        public Vector2 location;

        //Default Press Action
        private void DefaultPress()
        {
            //Debug.WriteLine("Button Pressed: ADD New Function");

            if(isToggle)
            {
                if(toggled == true)
                {
                    toggled = false;
                }
                else
                {
                    toggled = true;
                }
            }
        }

        //Creates Button, Adds to List, Sets Default Button Press
        //No Manual Width Or Height, Redundant since I want Textures To be a set size
        //I Can Just Scale Them Up From That Size If Needed
        public void CreateButton(Rectangle defaultSprite, Rectangle pressedSprite, bool isToggle, float scale)
        {
            this.isToggle = isToggle;
            this.defaultSprite = defaultSprite;
            this.pressedSprite = pressedSprite;
            this.scale = scale;
            buttonPress = DefaultPress;
            GameData.ButtonList.Add(this);
        }

        public bool GetToggleState()
        {
            return toggled;
        }

        //Update Each Button In Main Call By Using ForEach Loop
        public void Update(MouseState mState, MouseState mStateOld, float DeltaTime, Vector2 location)
        {
            this.location = location;
            bounds = new Rectangle((int)location.X, (int)location.Y, defaultSprite.Width, defaultSprite.Height);

            Vector2 mousePosScreen = new Vector2(mState.X, mState.Y);

            if (bounds.Contains(mousePosScreen.X, mousePosScreen.Y) && mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
            {
                isPressed = true;

                //If Button press != Null //buttonPress?.Invoke();
                if (buttonPress != null)
                {
                    buttonPress.Invoke();
                }
            }

            if (isPressed)
            {
                timer -= 400 * DeltaTime;

                if (timer <= 0)
                {
                    isPressed = false;
                    timer = 100;
                    //Debug.WriteLine("Button Released");
                }
            }
        }

        //Draw Each Button In Main Call By Using ForEach Loop
        public void Draw(SpriteBatch _spriteBatch)
        {
            if (!isToggle)
            {
                if (isPressed)
                {
                    int scaledWidth = (int)(pressedSprite.Width * scale);
                    int scaledHeight = (int)(pressedSprite.Height * scale);
                    Vector2 offset = new Vector2((defaultSprite.Width - scaledWidth) / 2, (defaultSprite.Height - scaledHeight) / 2);

                    //_spriteBatch.Draw(GameData.TextureAtlas, new Vector2((int)location.X, (int)location.Y) + offset, new Rectangle(pressedSprite.X, pressedSprite.Y, scaledWidth, scaledHeight), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                    _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle((int)location.X + (int)offset.X, (int)location.Y + (int)offset.Y, scaledWidth, scaledHeight), new Rectangle(pressedSprite.X, pressedSprite.Y, 25, 25), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                }
                else
                {
                    int scaledWidth = (int)(defaultSprite.Width * scale);
                    int scaledHeight = (int)(defaultSprite.Height * scale);
                    Vector2 offset = new Vector2((defaultSprite.Width - scaledWidth) / 2, (defaultSprite.Height - scaledHeight) / 2);

                    //_spriteBatch.Draw(GameData.TextureAtlas, new Vector2((int)location.X, (int)location.Y) + offset, new Rectangle(defaultSprite.X, defaultSprite.Y, scaledWidth, scaledHeight), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                    _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle((int)location.X + (int)offset.X, (int)location.Y + (int)offset.Y, scaledWidth, scaledHeight), new Rectangle(defaultSprite.X, defaultSprite.Y, 25, 25), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                }
            }
            else
            {
                if (toggled)
                {
                    // Calculate the scaled dimensions
                    int scaledWidth = (int)(pressedSprite.Width * scale);
                    int scaledHeight = (int)(pressedSprite.Height * scale);

                    // Calculate the offset to center the scaled rectangle
                    Vector2 offset = new Vector2((pressedSprite.Width - scaledWidth) / 2, (pressedSprite.Height - scaledHeight) / 2);

                    // Draw the scaled rectangle with the centering offset
                    _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle((int)location.X + (int)offset.X, (int)location.Y + (int)offset.Y, scaledWidth, scaledHeight), new Rectangle(pressedSprite.X, pressedSprite.Y, 25, 25), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

                }
                else
                {
                    int scaledWidth = (int)(defaultSprite.Width * scale);
                    int scaledHeight = (int)(defaultSprite.Height * scale);
                    Vector2 offset = new Vector2((defaultSprite.Width - scaledWidth) / 2, (defaultSprite.Height - scaledHeight) / 2);
                    _spriteBatch.Draw(GameData.TextureAtlas, new Rectangle((int)location.X + (int)offset.X, (int)location.Y + (int)offset.Y, scaledWidth, scaledHeight), new Rectangle(defaultSprite.X, defaultSprite.Y, 25, 25), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                }
            }    
        }
    }
}
