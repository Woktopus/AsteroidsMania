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

namespace AsteroidsMania.Scenes
{
    public class PhysicsScene : Scene
    {
        public const string wintext = "CONGRATULATIONS !";

        public World world;

        protected DebugViewXNA debugView;
        protected Matrix projection;

        Ship ship;
        Ship ship2;

        public PhysicsScene()
        {
            world = null;
            ship = new Ship();
            ship2 = new Ship();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graph)
        {
            base.LoadContent(content, graph);
            
            Settings.UseFPECollisionCategories = true;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);

            if (world == null)
            {
                world = new World(Vector2.Zero);
            }
            else
            {
                world.Clear();
            }

            // register
            world.ContactManager.BeginContact += onBeginContact;
            world.ContactManager.EndContact += onEndContact;
            world.ContactManager.PreSolve += onPreSolve;
            world.ContactManager.PostSolve += onPostSolve;

            world.Gravity = Vector2.Zero;
            // NOTE: you should probably unregister on destructor or wherever is relevant...

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

            //instance objects with body
            ship.LoadContent(content, PlayerIndex.One, graph, world);
            //ship2.LoadContent(content,PlayerIndex.Two, graph, world);


        }

        bool onBeginContact(Contact contact)
        {
            BeginContactForProjectile(contact);
            BeginContactForCollectibleItem(contact);
            BeginContactForPlayer(contact);
            BeginContactForSpike(contact);
            BeginContactForDagger(contact);
            BeginContactForEnemy(contact);
            BeginContactForBoss(contact);

            Fixture fa = contact.FixtureA;
            Fixture fb = contact.FixtureB;

            if ((int)fa.UserData == 9000 && ((int)fb.UserData == 0 || (int)fb.UserData == 1))
            {
                return false;
            }

            if ((int)fb.UserData == 9000 && ((int)fa.UserData == 0 || (int)fb.UserData == 1))
            {
                return false;
            }

            return true;
        }

        void onEndContact(Contact contact)
        {
            EndContactForPlayer(contact);

            EndContactForEnemy(contact);
        }

        //here
        public void BeginContactForBoss(Contact contact)
        {
            
        }
        public void BeginContactForDagger(Contact contact)
        {
            
        }


        private void BeginContactForSpike(Contact contact)
        {
            
        }

        private void BeginContactForProjectile(Contact contact)
        {
            

        }

        private void BeginContactForCollectibleItem(Contact contact)
        {
            

        }

        private void BeginContactForPlayer(Contact contact)
        {
            
        }

        private void EndContactForPlayer(Contact contact)
        {
            
        }

        private void BeginContactForEnemy(Contact contact)
        {
            
        }

        private void EndContactForEnemy(Contact contact)
        {
            
        }

        void onPreSolve(Contact contact, ref Manifold oldManifold)
        {
            // ...
        }
        void onPostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
            // ...
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, Game game)
        {
            base.Update(gameTime, game);

            ship.Update(gameTime);
            //ship2.Update(gameTime);
            // variable time step but never less then 30 Hz
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / PhysicsUtils.FPS)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.TranslationMatrix);

            ship.Draw(spriteBatch);
            //ship2.Draw(spriteBatch);
            debugView.RenderDebugData(ref projection);
        }
    }
}
