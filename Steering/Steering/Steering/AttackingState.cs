using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class AttackingState:State
    {
        float timeShot = 0.25f;
        public AttackingState(Entity entity):base(entity)
        {
        }

        public override void Enter()
        {
            AIFighter fighter = (AIFighter)Entity;
            fighter.SteeringBehaviours.turnOffAll();
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.offset_pursuit);
            fighter.offset = new Vector3(0, 0, 5);
            fighter.Leader = XNAGame.Instance().Leader;
        }

        public override void Exit()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float range = 30.0f;
            timeShot += timeDelta;
            float fov = MathHelper.PiOver4;
            // Can I see the leader?
            Fighter leader = XNAGame.Instance().Leader;
            if ((leader.pos - Entity.pos).Length() > range)
            {
                // Is the leader inside my FOV
                AIFighter fighter = (AIFighter)Entity;
                fighter.SwicthState(new IdleState(fighter));
            }
            else
            {
                float angle;
                Vector3 toEnemy = (leader.pos - Entity.pos);
                toEnemy.Normalize();
                angle = (float) Math.Acos(Vector3.Dot(toEnemy, Entity.look));
                if (angle < fov)
                {
                    if (timeShot > 0.25f)
                    {
                        Lazer lazer = new Lazer();
                        lazer.pos = Entity.pos;
                        lazer.look = Entity.look;
                        XNAGame.Instance().Children.Add(lazer);
                        timeShot = 0.0f;
                    }
                }
            }
        }

    }
}
