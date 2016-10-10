using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroidsMania.Service;
using Microsoft.Xna.Framework.Input;

namespace AsteroidsMania.Core
{
    public class Ship
    {
        public PlayerIndex gamepadIndex { get; set; }

        public Texture2D texture { get; set; }

        public Vector2 position = Vector2.Zero;

        public Vector2 direction = Vector2.Zero;

        public float rotation;

        public Vector2 origin = Vector2.Zero;

        float vitesse = 3f;

        public Ship()
        {
            position = new Vector2();
        }   

    
        public void LoadContent(ContentManager content)
        {

            texture = content.Load<Texture2D>("SpriteSheet/ship.png");
            position.X = 300;
            position.Y = 300;
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }

        public void Update(GameTime gameTime)
        {
            var gamepadState = ServiceHelper.Get<InputManagerService>().GamePad.GetState(this.gamepadIndex);
            var keyboardState = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

            if (gamepadState.ThumbSticks.Left.X > 0.5 || keyboardState.IsKeyDown(Keys.Right))
            {
                rotation +=0.05f;
            }
            if (gamepadState.ThumbSticks.Left.X < -0.5 || keyboardState.IsKeyDown(Keys.Left))
            {
                rotation -=0.05f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) || gamepadState.Triggers.Right > 0)
            {
                //vitesse = gamepadState.Triggers.Right*10;
                vitesse = 5;
                position.X += -(float)Math.Sin((double)rotation)*vitesse;
                position.Y += (float)Math.Cos((double)rotation)*vitesse;

                direction.X = -(float)Math.Sin((double)rotation) * vitesse;
                direction.Y = (float)Math.Cos((double)rotation) * vitesse;
            }
            if(vitesse > 0)
            {
                position.Y += direction.Y;
                position.X += direction.X;
                vitesse-=0.1f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 0.3f,SpriteEffects.None, 0f);
        }
    }
}
