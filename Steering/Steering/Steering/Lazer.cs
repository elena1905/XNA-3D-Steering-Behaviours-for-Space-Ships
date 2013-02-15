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
    class Lazer:Entity
    {

        public override void LoadContent()
        {
        }
        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float width = XNAGame.Instance().Ground.width;
            float height = XNAGame.Instance().Ground.height;
            float speed = 5.0f;
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((pos.X < - (width / 2)) || (pos.X > width / 2) || (pos.Z < - (height / 2)) || (pos.Z > height / 2) || (pos.Y < 0) || (pos.Y > 100))
            {
                Alive = false;
            }
            pos += look * speed;
        }

        public override void Draw(GameTime gameTime)
        {
            Line.DrawLine(pos, pos + look * 10, Color.Red);
        }

    }
}
