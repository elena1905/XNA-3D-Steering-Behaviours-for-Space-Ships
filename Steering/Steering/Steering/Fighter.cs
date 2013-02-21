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
   
    public class Fighter:Entity
    {
        public Vector3 targetPos = Vector3.Zero;
        private Fighter target = null;
        public Vector3 offset;
        private Path path = new Path();
        string modelName;

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        internal Path Path
        {
            get { return path; }
            set { path = value; }
        }

        
        public Fighter Target
        {
            get { return target; }
            set { target = value; }
        }
        private Fighter leader = null;

        public Fighter Leader
        {
            get { return leader; }
            set { leader = value; }
        }

        SteeringBehaviours steeringBehaviours;
        public float maxSpeed = 150.0f;
        bool drawAxis;
        List<Vector3> feelers = new List<Vector3>();

        // The acceleration is smoothed
        Vector3 acceleration;

        public List<Vector3> Feelers
        {
            get { return feelers; }
            set { feelers = value; }
        }

        public bool DrawAxis
        {
            get { return drawAxis; }
            set { drawAxis = value; }
        }

        bool drawFeelers;

        float roll = 0.0f;

        public bool DrawFeelers
        {
            get { return drawFeelers; }
            set { drawFeelers = value; }
        }

        internal SteeringBehaviours SteeringBehaviours
        {
            get { return steeringBehaviours; }
            set { steeringBehaviours = value; }
        }

        public Fighter()
        {
            worldTransform = Matrix.Identity;
            pos = new Vector3(0, 10, 0);
            look = new Vector3(0, 0, -1);
            right = new Vector3(1, 0, 0);
            up = new Vector3(0, 1, 0);
            globalUp = new Vector3(0, 1, 0);
            steeringBehaviours = new SteeringBehaviours(this);
            drawAxis = false;
            Solid = true;
            modelName = "fighter";
        }

        public override void LoadContent() 
        {            
            model = XNAGame.Instance().Content.Load<Model>(modelName);
            worldTransform = Matrix.CreateWorld(pos, look, up);

            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere = BoundingSphere.CreateMerged(BoundingSphere, mesh.BoundingSphere);
            }

        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            float smoothRate;
            steeringBehaviours.timeDelta = timeDelta;
            force = steeringBehaviours.calculate();

            Vector3 newAcceleration = force / Mass;

            if (timeDelta > 0)
            {
                smoothRate = Utilities.Clip(9 * timeDelta, 0.15f, 0.4f) / 2.0f;
                Utilities.BlendIntoAccumulator(smoothRate, newAcceleration, ref acceleration);
            }

            velocity += acceleration * timeDelta;
            float speed = velocity.Length();
            if (speed > maxSpeed)
            {

                velocity.Normalize();
                velocity *= maxSpeed;
            }
            pos += velocity * timeDelta;
            SteeringBehaviours.checkNaN(force);

            // the length of this global-upward-pointing vector controls the vehicle's
            // tendency to right itself as it is rolled over from turning acceleration
            Vector3 globalUp = new Vector3(0, 0.2f, 0);
            // acceleration points toward the center of local path curvature, the
            // length determines how much the vehicle will roll while turning
            Vector3 accelUp = acceleration * 0.05f;
            // combined banking, sum of UP due to turning and global UP
            Vector3 bankUp = accelUp + globalUp;
            // blend bankUp into vehicle's UP basis vector
            smoothRate = timeDelta * 3;
            Vector3 tempUp = up;
            Utilities.BlendIntoAccumulator(smoothRate, bankUp, ref tempUp);
            up = tempUp;
            up.Normalize();

            if (speed > 0.0001f)
            {
                look = velocity;
                look.Normalize();
                if (look.Equals(right))
                {
                    right = Vector3.Right;
                }
                else
                {
                    right = Vector3.Cross(look, up);

                    right.Normalize();

                    SteeringBehaviours.checkNaN(ref right, Vector3.Right);
                    up = Vector3.Cross(right, look);
                    up.Normalize();
                    SteeringBehaviours.checkNaN(ref up, Vector3.Up);
                }
                velocity *= 0.99f;
            }
            
            if (look != basis)
            {
                
                float angle = (float)Math.Acos(Vector3.Dot(basis, look));                
                Vector3 axis = Vector3.Cross(basis, look);

                quaternion = Quaternion.CreateFromAxisAngle(axis, angle);
                quaternion.Normalize();
                
                worldTransform.Up = up;
                worldTransform.Forward = look;
                worldTransform.Right = right;
                worldTransform = Matrix.CreateWorld(pos, look, up);
                checkNan(worldTransform);
            }
            else
            {
                worldTransform = Matrix.CreateTranslation(pos);
            }
            drawAxis = false;
        }

        private void checkNan(Matrix worldTransform)
        {
            if (float.IsNaN(worldTransform.M21))
            {
                System.Console.WriteLine("NAN!!");
            }

        }
        public override void Draw(GameTime gameTime)
        {
            /*
            SpriteFont spriteFont = XNAGame.Instance().SpriteFont;
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Pos: " + pos.X + " " + pos.Y + " " + pos.Z, new Vector2(10, 10), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Look: " + look.X + " " + look.Y + " " + look.Z, new Vector2(10, 30), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Right: " + right.X + " " + right.Y + " " + right.Z, new Vector2(10, 50), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Up: " + up.X + " " + up.Y + " " + up.Z, new Vector2(10, 70), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Roll: " + roll, new Vector2(10, 110), Color.White);
            */
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

            if (drawAxis)
            {
                Line.DrawLine(pos, pos + (look * 10), Color.White);
                Line.DrawLine(pos, pos + (up * 10), Color.Red);
                Line.DrawLine(pos, pos + (right * 10), Color.Blue);                                
            }

            if (drawFeelers)
            {
                foreach (Vector3 feeler in feelers)
                {
                    Line.DrawLine(pos, feeler, Color.Chartreuse);                    
                }
            }
        }
    }
}
