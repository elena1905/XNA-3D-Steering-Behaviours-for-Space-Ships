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
    public abstract class Entity
    {
        private bool alive = true;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        private bool solid = false;
        public bool Solid
        {
            get { return solid; }
            set { solid = value; }
        }

        BoundingSphere boundingSphere;

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
            set { boundingSphere = value; }
        }

        public Model model = null;
        public Vector3 pos = Vector3.Zero;
        
        public Vector3 velocity = Vector3.Zero;
        public Quaternion quaternion;
       
        static Random randomize = new Random();
        
        public Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);
        public Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 look = new Vector3(0, 0, -1);
        public Vector3 basis = new Vector3(0, 0, -1);
        public Vector3 globalUp = new Vector3(0, 0, 1);


        public float scale;

        
        float mass = 1.0f;

        public Matrix worldTransform = new Matrix();

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        public Vector3 force = Vector3.Zero;
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void UnloadContent();

        public void yaw(float angle)
        {
            Matrix T = Matrix.CreateRotationY(angle);
            right = Vector3.Transform(right, T);
            look = Vector3.Transform(look, T);
        }

        public void pitch(float angle)
        {
            Matrix T = Matrix.CreateFromAxisAngle(right, angle);
            //_up = Vector3.Transform(_up, T);
            look = Vector3.Transform(look, T);
        }

        public void walk(float timeDelta)
        {
            float speed = 5.0f;
            pos += look * timeDelta * speed ;

        }

        public void strafe(float timeDelta)
        {
            float speed = 5.0f;
            pos += right * timeDelta * speed;
        }

        public float getYaw()
        {
            Vector3 localLook = look;
            localLook.Y = basis.Y;
            localLook.Normalize();
            SteeringBehaviours.checkNaN(ref localLook, Vector3.Forward);
            float angle = (float)Math.Acos(Vector3.Dot(basis, localLook));

            if (look.X > 0)
            {
                angle = (MathHelper.Pi * 2.0f) - angle;
            }
            return angle;

        }

        public float getPitch()
        {
            if (look.Y == basis.Y)
            {
                return 0;
            }
            Vector3 localBasis = new Vector3(look.X, 0, look.Z);
            localBasis.Normalize();
            float dot = Vector3.Dot(localBasis, look);
            float angle = (float)Math.Acos(dot);
            if (float.IsNaN(angle))
            {
                return 0.0f;
            }
            if (look.Y < 0)
            {
                angle = (MathHelper.Pi * 2.0f) - angle;
            }
            
            return angle;
        }
    }
}
