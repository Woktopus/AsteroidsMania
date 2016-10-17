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
using AsteroidsMania.Scene;

namespace AsteroidsMania.Scenes
{
    public class PhysicsScene : Scene
    {
        public World world;

        protected DebugViewXNA debugView;
        protected Matrix projection;
        Camera camera;

        Ship ship;
        

        public PhysicsScene()
        {
            world = null;
            camera = new Camera();  
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

            camera.viewportWidth = graph.Viewport.Width;
            camera.viewportHeight = graph.Viewport.Height;
            camera.zoom = 1f;


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
            Matrix cameraMatrix = camera.DebugMatrix;
            debugView.RenderDebugData(ref projection, ref cameraMatrix);
        }
    }
}
