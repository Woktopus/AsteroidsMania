﻿using Microsoft.Xna.Framework;
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
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Common.Decomposition;

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

        float vitesse = 10f;

        GraphicsDevice graph;

        ContentManager localContent;

        public Body body;


        


        public Ship()
        {
            position = new Vector2();
        }   

    
        public void LoadContent(ContentManager content, PlayerIndex index, GraphicsDevice vi, World world)
        {
            graph = vi;


            switch (index)
            {
                case PlayerIndex.One:
                    position.X = 100;
                    position.Y = 300;
                    break;
                case PlayerIndex.Two:
                    position.X = 100;
                    position.Y = 100;
                    break;

            }

            texture = content.Load<Texture2D>("Ship/Vaisseau_1.png");

            body = CreatePolygonFromTexture(texture, world, 1, ConvertUnits.ToSimUnits(position), 0.1f);
            body.BodyType = BodyType.Dynamic;

            if(index == PlayerIndex.One)
            {
                body.BodyType = BodyType.Static;
                body.FixedRotation = true;
            }

        }


        public Body CreatePolygonFromTexture(Texture2D tex, World world, float density, Vector2 position, float scale, TriangulationAlgorithm algorithm = TriangulationAlgorithm.Bayazit)
        {
            uint[] texData = new uint[tex.Width * tex.Height];
            tex.GetData<uint>(texData);

            Vertices vertices = TextureConverter.DetectVertices(texData, tex.Width);
            List<Vertices> vertexList = Triangulate.ConvexPartition(vertices, algorithm);

            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(scale));
            foreach (Vertices vert in vertexList)
                vert.Scale(ref vertScale);

            Vector2 centroid = -vertices.GetCentroid();
            vertices.Translate(ref centroid);
            //basketOrigin = -centroid;

            return BodyFactory.CreateCompoundPolygon(world, vertexList, density, position);
        }



        public void Update(GameTime gameTime)
        {
            var keyboardState = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();
            var gamepadState = ServiceHelper.Get<InputManagerService>().GamePad.GetState(this.gamepadIndex);

            if (gamepadState.ThumbSticks.Left.X > 0.5 || keyboardState.IsKeyDown(Keys.Left))
            {
                body.Rotation += 0.05f;
                body.AngularVelocity = 0;
            }
            if (gamepadState.ThumbSticks.Left.X < -0.5 || keyboardState.IsKeyDown(Keys.Right))
            {
                body.Rotation -= 0.05f;
                body.AngularVelocity = 0;
            }
            if (keyboardState.IsKeyDown(Keys.Space) || gamepadState.Triggers.Right > 0)
            {
                body.LinearVelocity = new Vector2(-(float)Math.Sin((double)body.Rotation)*vitesse, (float)Math.Cos((double)body.Rotation)*vitesse);
            }
            Console.WriteLine(body.WorldCenter);
            
            if (ConvertUnits.ToDisplayUnits(body.Position.Y) > graph.Viewport.Height)
            {
                body.SetTransform(new Vector2(body.Position.X,0),body.Rotation);
            }
            if (body.Position.Y < 0)
            {
                body.SetTransform(new Vector2(body.Position.X, ConvertUnits.ToSimUnits(graph.Viewport.Height)), body.Rotation);
            }
            if (ConvertUnits.ToDisplayUnits(body.Position.X) > graph.Viewport.Width)
            {
                body.SetTransform(new Vector2(0, body.Position.Y), body.Rotation);

            }
            if (body.Position.X < 0)
            {
                position.X = graph.Viewport.Width;
                body.SetTransform(new Vector2(ConvertUnits.ToSimUnits(graph.Viewport.Width), body.Position.Y), body.Rotation);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            Vector2 position =ConvertUnits.ToDisplayUnits( body.Position);
            spriteBatch.Draw(texture, position, null, Color.White, body.Rotation, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
        }
    }
}
