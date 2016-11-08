using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Media;
using System.Media;
using System.IO;
using AsteroidsMania.Utils;
using AsteroidsMania.Core;
using AsteroidsMania.Service;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace AsteroidsMania.Scenes
{
    public class PhysicsScene : Scene
    {
        public World world;

        protected DebugViewXNA debugView;
        protected Matrix projection;



        Asteroid asteroid;

        Texture2D textureShip, textureAsteroid;

        List<Ship> shipList;


        public PhysicsScene()
        {
            world = null;
            shipList = new List<Ship>();
            shipList.Add(new Ship());
            asteroid = new Asteroid();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graph)
        {
            base.LoadContent(content, graph);

            textureShip = content.Load<Texture2D>("Ship/Vaisseau_1.png");
            textureAsteroid = content.Load<Texture2D>("Asteroid/Asteroide_1.png");

            Settings.UseFPECollisionCategories = true;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);

            if (world == null)
                world = new World(new Vector2(0, 0));
            else
                world.Clear();
            
            world.Gravity = new Vector2(0, 0);



            world.ContactManager.BeginContact += onBeginContact;/*
            world.ContactManager.EndContact += onEndContact;
            world.ContactManager.PreSolve += onPreSolve;
            world.ContactManager.PostSolve += onPostSolve;*/

            if (debugView == null)
            {
                debugView = new DebugViewXNA(world);

                debugView.LoadContent(graph, content);
            }

            projection = Matrix.CreateOrthographicOffCenter(
                0f, ConvertUnits.ToSimUnits(graph.Viewport.Width),
                ConvertUnits.ToSimUnits(graph.Viewport.Height), 0f,
                0f, 1f
            );

            shipList.ElementAt(0).LoadContent(content, PlayerIndex.One, graph, world);

            asteroid.LoadContent(world, content, graph, new Vector2(), TailleEnum.GRAND, new Vector2(), 25f);


        }

        bool onBeginContact(Contact contact)
        {
            BeginContactForShip(contact);
            

            Fixture fa = contact.FixtureA;
            Fixture fb = contact.FixtureB;

            return true;
        }

        private void BeginContactForShip(Contact contact)
        {
            Fixture fa = contact.FixtureA;
            Fixture fb = contact.FixtureB;

            if ((int)fa.UserData == 50)
            {
                if ((int)fb.UserData == 100)
                {
                    shipList.ElementAt(0).body.Position = Vector2.Zero;
                }
            }
            if ((int)fa.UserData == 100)
            {
                if ((int)fb.UserData == 50)
                {
                    shipList.ElementAt(0).body.Position = Vector2.Zero;
                }
            }
        }

        public override void Update(GameTime gameTime, Game game)
        {
            base.Update(gameTime, game);
            //ici gamepad.1.connected   passe à true chez moi 

            getInput();

            shipList.ElementAt(0).Update(gameTime);
            asteroid.Update(gameTime);
            //ship2.Update(gameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void getInput()
        {
            var keyboardState = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();
            var gamepadState = ServiceHelper.Get<InputManagerService>().GamePad.GetState(PlayerIndex.One);

            if ( keyboardState.IsKeyDown(Keys.Left))
            {
                shipList.ElementAt(0).isRotating = -1;
            }
            if ( keyboardState.IsKeyDown(Keys.Right))
            {
                shipList.ElementAt(0).isRotating = 1;
            }
            if(keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                shipList.ElementAt(0).isRotating = 0;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                shipList.ElementAt(0).isGoingForward = true;
            }if (keyboardState.IsKeyUp(Keys.Space))
            {
                shipList.ElementAt(0).isGoingForward = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            shipList.ElementAt(0).Draw(spriteBatch);
            asteroid.Draw(spriteBatch);
            //ship2.Draw(spriteBatch);

            debugView.RenderDebugData(ref projection);
        }
    }
}
