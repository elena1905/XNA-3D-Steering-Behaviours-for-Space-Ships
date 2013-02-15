using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Steering
{
    class Sphere:Entity
    {
        float radius;

        public Sphere(float radius)
        {
            this.radius = radius;
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public override void LoadContent()
        {
            model = XNAGame.Instance().Content.Load<Model>("sphere");
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            worldTransform = Matrix.CreateScale(radius) * Matrix.CreateTranslation(pos);
            // Draw the mesh
            if (model != null)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = worldTransform;
                        effect.Projection = XNAGame.Instance().Camera.getProjection();
                        effect.View = XNAGame.Instance().Camera.getView();
                    }
                    mesh.Draw();
                }
            }
        }

        public bool closestRayIntersects(Ray ray, Vector3 point, ref Vector3 intersection)
        {
            // Calculate p0-c call it v

            Vector3 v = ray.pos - pos;
            Vector3 p0 = Vector3.Zero, p1 = Vector3.Zero;

            // Now calculate a, b and c
            float a, b, c;

            /*
             *  a = u.u
                b = 2u(p0 – pc)
                c = (p0 – c).(p0 – c) - r2
            */
            a = Vector3.Dot(ray.look, ray.look);
            b = 2.0f * Vector3.Dot(v, ray.look);
            c = Vector3.Dot(v, v) - (radius * radius);

            // Calculate the discriminant
            float discriminant = (b * b) - (4.0f * a * c);

            // Test for imaginary number
            // If discriminant > 0, calculate values for t0 and t1
            // Substitute into the ray equation and calculate the 2 intersection points
            // Push the interesctions into the vector intersections
            if (discriminant >= 0.0f)
            {

                discriminant = (float) Math.Sqrt(discriminant);

                float t0 = (-b + discriminant) / (2.0f * a);
                float t1 = (-b - discriminant) / (2.0f * a);

                p0 = ray.pos + (ray.look * t0);
                p1 = ray.pos + (ray.look * t1);

                if ((point - p0).Length() < (point - p1).Length())
                {
                    intersection = p0;
                }
                else
                {
                    intersection = p1;
                }
                return true;
            }
            return false;
        }
    }
}
