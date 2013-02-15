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
    class Path:Entity
    {

        public override void LoadContent()
        {                        

        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (draw)
            {
                for (int i = 1; i < waypoints.Count(); i++)
                {
                    Line.DrawLine(waypoints[i - 1], waypoints[i], Color.Cyan);
                }
                if (looped)
                {
                    Line.DrawLine(waypoints[0], waypoints[waypoints.Count() - 1], Color.Cyan);
                }
            }
        }
        private List<Vector3> waypoints = new List<Vector3>();
        private int next = 0;
        bool draw;

        public bool DrawPath
        {
            get { return draw; }
            set 
            { 
                draw = value;
                if (value == true)
                {
                    XNAGame.Instance().Children.Add(this);
                }
                else
                {
                    XNAGame.Instance().Children.Remove(this);
                }
            }
        }

        public Vector3 NextWaypoint()
        {
            return waypoints[next];
        }

        public bool IsLast()
        {
            return (next == waypoints.Count() - 1);
        }

        public void AdvanceToNext()
        {
            if (looped)
            {                
                next = (next + 1) % waypoints.Count();
                System.Console.WriteLine(next);
            }
            else
            {
                if (next != waypoints.Count() - 1)
                {
                    next = next + 1;
                }
            }
        }

        public int Next
        {
            get { return next; }
            set { next = value; }
        }

        public List<Vector3> Waypoints
        {
            get { return waypoints; }
            set { waypoints = value; }
        }

        private bool looped; 
    

        public bool Looped
        {
          get { return looped; }
          set { looped = value; }
        }
    }
}
