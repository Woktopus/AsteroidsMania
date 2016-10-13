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

namespace AsteroidsMania.Scenes
{
    public class PhysicsScene : Scene
    {
        public const string wintext = "CONGRATULATIONS !";

        public World world;

        protected DebugViewXNA debugView;

        Ship ship;
        

        public PhysicsScene()
        {
            world = null;
            ship = new Ship();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graph)
        {
            base.LoadContent(content, graph);
            
            Settings.UseFPECollisionCategories = true;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);

            if (world == null)
                world = new World(Vector2.Zero);
            else
                world.Clear();
            
            world.Gravity = Vector2.Zero;
            

            if (debugView == null)
            {
                debugView = new DebugViewXNA(world);

                debugView.LoadContent(graph, content);
            }

            ship.LoadContent(content, PlayerIndex.One, graph, world);
        }

        public override void Update(GameTime gameTime, Game game)
        {
            base.Update(gameTime, game);
            //ici gamepad.1.connected   passe à true chez moi 
            
            ship.Update(gameTime);
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / PhysicsUtils.FPS)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            ship.Draw(spriteBatch);
        }
    }
}
