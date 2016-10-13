using AsteroidsMania.Scenes;
using AsteroidsMania.Service;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidsMania
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PhysicsScene ph;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ServiceHelper.Game = this;
            Components.Add(new InputManagerService(this));
            ph = new PhysicsScene();
        }

        protected override void Initialize()
        {
            base.Initialize();
            graphics.IsFullScreen = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ph.LoadContent(Content,GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            ph.Update(gameTime,this);
        }

       protected override void Draw(GameTime gameTime)
       {
            GraphicsDevice.Clear(Color.Black);
            

            base.Draw(gameTime);

            spriteBatch.Begin();
            ph.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
