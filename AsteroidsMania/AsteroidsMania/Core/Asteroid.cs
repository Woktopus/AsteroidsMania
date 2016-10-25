using AsteroidsMania.Scenes;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsMania.Core
{
    public class Asteroid
    {
        public static long NextAsteroidUniqueId = 0;

        public long Id { get; set; }
        public int Health { get; set; }
        public float Vitesse { get; set; }

        GraphicsDevice graph;

        Body body;
        TailleEnum taille { get; set; }
        Texture2D texture { get; set; }
        public Vector2 position = Vector2.Zero;


        public Asteroid()
        {
            position = new Vector2();
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(World world, ContentManager content, GraphicsDevice graph, Vector2 positionEnJeu, TailleEnum taille, Vector2 Transform, float rotation)
        {
            Id = NextAsteroidUniqueId;
            NextAsteroidUniqueId++;
            this.taille = taille;
            switch (taille)
            {
                case TailleEnum.PETIT:
                    Health = 50;
                    Vitesse = 12f;
                    break;
                case TailleEnum.MOYEN:
                    Health = 100;
                    Vitesse = 7f;
                    break;
                case TailleEnum.GRAND:
                    Health = 150;
                    Vitesse = 5f;

                    break;
                default:
                    Health = 150;
                    break;
            }
            this.texture = content.Load<Texture2D>("Asteroid/Asteroide_1.png");
            this.graph = graph;
            position.X = 200;
            position.Y = 200;
            body = CreatePolygonFromTexture(texture, world, 1, ConvertUnits.ToSimUnits(position), 0.1f);
            body.BodyType = BodyType.Dynamic;
            body.Rotation = rotation;
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

            Vector2 centroid = vertices.GetCentroid();
            vertices.Translate(ref centroid);
            //basketOrigin = -centroid;
            return BodyFactory.CreateCompoundPolygon(world, vertexList, density, position);
        }

        public void Update(GameTime gametime)
        {
            body.LinearVelocity = new Vector2(-(float)Math.Sin((double)body.Rotation) * Vitesse, (float)Math.Cos((double)body.Rotation) * Vitesse);
            body.AngularVelocity = 0;
            if (ConvertUnits.ToDisplayUnits(body.Position.Y) > graph.Viewport.Height)
            {
                body.SetTransform(new Vector2(body.Position.X, 0), body.Rotation);
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
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
        }
    }
}
