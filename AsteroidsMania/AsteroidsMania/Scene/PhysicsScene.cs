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

namespace AsteroidsMania.Scenes
{
    public class PhysicsScene : Scene
    {
        public World world;

        protected DebugViewXNA debugView;
        protected Matrix projection;


        Ship ship;

        Asteroid asteroid;

        Texture2D textureShip, textureAsteroid;



        public PhysicsScene()
        {
            world = null;
            ship = new Ship();
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

            ship.LoadContent(content, PlayerIndex.One, graph, world);

            asteroid.LoadContent(world, content, graph, new Vector2(), TailleEnum.GRAND, new Vector2(), 25f);
        }

        public override void Update(GameTime gameTime, Game game)
        {
            base.Update(gameTime, game);
            //ici gamepad.1.connected   passe à true chez moi 
            
            ship.Update(gameTime);
            //ship2.Update(gameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ship.Draw(spriteBatch);
            //ship2.Draw(spriteBatch);

            debugView.RenderDebugData(ref projection);
        }
    }
}
