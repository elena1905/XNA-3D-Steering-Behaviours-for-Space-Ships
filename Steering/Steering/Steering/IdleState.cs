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
    class IdleState:State
    {
        static Vector3 initialPos = Vector3.Zero;
        public IdleState(Entity entity):base(entity)
        {
        }

        public override void Enter()
        {
            AIFighter fighter = (AIFighter) Entity;
            if (initialPos == Vector3.Zero)
            {
                initialPos = fighter.pos;
            }
            fighter.Path.Waypoints.Add(initialPos);
            fighter.Path.Waypoints.Add(initialPos + new Vector3(-50, 0, -80));
            fighter.Path.Waypoints.Add(initialPos + new Vector3(0, 0, -160));
            fighter.Path.Waypoints.Add(initialPos + new Vector3(50, 0, -80));
            fighter.Path.Looped = true;
            fighter.Path.DrawPath = true;
            fighter.SteeringBehaviours.turnOffAll();
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.follow_path);
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
        }
        public override void Exit()
        {            
            AIFighter fighter = (AIFighter) Entity;
            fighter.Path.Waypoints.Clear();
            fighter.Path.DrawPath = false;

        }

        public override void Update(GameTime gameTime)
        {
            float range = 30.0f;           
            // Can I see the leader?
            Fighter leader = XNAGame.Instance().Leader;
            if ((leader.pos - Entity.pos).Length() < range)
            {
                // Is the leader inside my FOV
                AIFighter fighter = (AIFighter)Entity;
                fighter.SwicthState(new AttackingState(fighter));
            }
        }
    }
}
