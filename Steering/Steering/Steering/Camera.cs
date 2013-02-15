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
    public class Camera : Entity
    {
        
        public Matrix projection;
        public Matrix view;
        private KeyboardState keyboardState;
        private MouseState mouseState;

        public override void Draw(GameTime gameTime)
        {
            SpriteFont spriteFont = XNAGame.Instance().SpriteFont;
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Pos: " + pos.X + " " + pos.Y + " " + pos.Z, new Vector2(500, 10), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Look: " + look.X + " " + look.Y + " " + look.Z, new Vector2(500, 30), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Right: " + right.X + " " + right.Y + " " + right.Z, new Vector2(500, 50), Color.White);
            XNAGame.Instance().SpriteBatch.DrawString(spriteFont, "Up: " + up.X + " " + up.Y + " " + up.Z, new Vector2(500, 70), Color.White);
        }

        public override void LoadContent()
        {
        }
        public override void UnloadContent()
        {
        }
        public Camera()
        {
            pos = new Vector3(0.0f, 30.0f, 50.0f);
            look = new Vector3(0.0f, 0.0f, -1.0f);
        }

        public override void Update(GameTime gameTime)
        {

            float timeDelta = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            int midX = GraphicsDeviceManager.DefaultBackBufferHeight / 2;
            int midY = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
            
            int deltaX = mouseX - midX;
            int deltaY = mouseY - midY;

            yaw(-(float)deltaX / 100.0f);
            pitch(-(float)deltaY / 100.0f);
            Mouse.SetPosition(midX, midY);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector3 newTargetPos= pos + (look * 100.0f);
                //newTargetPos.Y = 8;
                XNAGame.Instance().Leader.targetPos = newTargetPos;
                
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                Vector3 newTargetPos = pos;
                XNAGame.Instance().Leader.targetPos = newTargetPos;

            }
            

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                timeDelta *= 20.0f;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                walk(timeDelta);   
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                walk(-timeDelta);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                strafe(-timeDelta);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                strafe(timeDelta);
            }          
            view = Matrix.CreateLookAt(pos, pos + look, up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), XNAGame.Instance().GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            
        }

        public Matrix getProjection()
        {
            return projection;
        }

        public Matrix getView()
        {
            return view;
        }

        
    }
}
