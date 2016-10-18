using AsteroidsMania.Scenes;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
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

        Body body;
        TailleEnum taille { get; set; }
        float rayon { get; set; }
        Texture2D texture { get; set; }
        

        public Asteroid()
        {
            
        }

        public void Initialize(TailleEnum taille)
        {
            Id = NextAsteroidUniqueId;
            NextAsteroidUniqueId++;
            this.taille = taille;
            switch (taille)
            {
                case TailleEnum.PETIT:
                    Health = 50;
                    break;
                case TailleEnum.MOYEN:
                    Health = 100;
                    break;
                case TailleEnum.GRAND:
                    Health = 150;
                    break;
                default:
                    Health = 150;
                    break;
            }
        }

        public void LoadContent(World world, ContentManager content, Vector2 positionEnJeu)
        {
            body = BodyFactory.CreateCircle(world, rayon, 1);
            body.BodyType = BodyType.Dynamic;
            body.Position = positionEnJeu;

            CircleShape circleShape = new CircleShape(rayon, 1f);
            circleShape.Position = positionEnJeu;
            
            body.CreateFixture(circleShape);
        }






        public void Update()
        {

        }
    
        public void DrawAtBodyPosition()
        {

        }


        public void Draw()
        {

        }
    }
}
