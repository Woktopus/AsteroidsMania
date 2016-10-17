using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroidsMania.Service;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using AsteroidsMania.Scenes;
using FarseerPhysics;

namespace AsteroidsMania.Core
{
    public class Ship
    {
        public PlayerIndex gamepadIndex { get; set; }

        public Texture2D texture { get; set; }

        //public Vector2 position = Vector2.Zero;

        public Vector2 direction = Vector2.Zero;

        public float rotation;

        public Vector2 origin = Vector2.Zero;

        float vitesse = 3f;

        GraphicsDevice graph;

        ContentManager localContent;

        //test from web farseer physics tutorial;s
        Body body;

        public const float UNIT_TO_PIXEL = 100.0f;
        public const float PIXEL_TO_UNIT = 1 / UNIT_TO_PIXEL;


        public Vector2 position;
        public Vector2 DisplayPosition
        {
            get { return body.Position * UNIT_TO_PIXEL; }
            set { body.Position = value * PIXEL_TO_UNIT;  }
        }

        private Vector2 size;
        public Vector2 Size
        {
            get { return size * UNIT_TO_PIXEL; }
            set { size = value * PIXEL_TO_UNIT; }
        }


        public Ship()
        {
            position = new Vector2();
        }   

    
        public void LoadContent(ContentManager content, PlayerIndex index, GraphicsDevice vi, World world)
        {

            localContent = content;
            graph = vi;
            texture = content.Load<Texture2D>("Ship/Vaisseau_1.png");

            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            body = BodyFactory.CreateRectangle(world, texture.Width, texture.Height, 1);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;

            
            switch (index)
            {
                case PlayerIndex.One:
                    body.Position = new Vector2(100, 100);
                    
                    break;
                case PlayerIndex.Two:
                    body.Position = new Vector2(1000, 100);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }

       

        public void Update(GameTime gameTime)
        {
            var gamepadState = ServiceHelper.Get<InputManagerService>().GamePad.GetState(this.gamepadIndex);
            
            var gamepadStateBefore = ServiceHelper.Get<InputManagerService>().GamePad.GetPrevState(this.gamepadIndex);

            var keyboardState = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

            if (gamepadState.ThumbSticks.Left.X > 0.5 || keyboardState.IsKeyDown(Keys.Right))
            {
                rotation +=0.05f;
            }
            if (gamepadState.ThumbSticks.Left.X < -0.5 || keyboardState.IsKeyDown(Keys.Left))
            {
                 rotation -= 0.05f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) || gamepadState.Triggers.Right > 0)
            {
                //vitesse = gamepadState.Triggers.Right*10;
                vitesse = 5;

                position = body.Position;
                position.X += -(float)Math.Sin((double)rotation) * vitesse;
                position.Y += (float)Math.Cos((double)rotation) * vitesse;
                body.Position = position;
                
                direction.X = -(float)Math.Sin((double)rotation) * vitesse;
                direction.Y = (float)Math.Cos((double)rotation) * vitesse;
            }

            // System.Console.WriteLine(view);
            position = body.Position;
            if (position.Y > graph.Viewport.Height)
            {
                position.Y = 0;
            }
            if (position.Y <0)
            {
                position.Y = graph.Viewport.Height;
            }
            if (position.X > graph.Viewport.Width)
            {
                position.X = 0;
            }
            if (position.X <0)
            {
                position.X = graph.Viewport.Width;
            }
            body.Rotation = rotation;
            body.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, /*DisplayPosition*/ body.Position , null, Color.White, body.Rotation, origin, 0.1f,SpriteEffects.None, 0f);
        }

        public Vector2 GetDrawPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position);
        }
    }
}
