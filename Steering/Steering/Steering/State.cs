using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Steering
{
    abstract class State
    {
        Entity entity;

        public State(Entity entity)
        {
            this.entity = entity;
        }

        public Entity Entity
        {
            get { return entity; }
            set { entity = value; }
        }
        public abstract void Enter();
        public abstract void Exit();

        public abstract void Update(GameTime gameTime);
    }
}
