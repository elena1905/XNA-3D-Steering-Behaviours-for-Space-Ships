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
    class SkySphere:Entity
    {
        Model SkySphereModel;
        Effect SkySphereEffect;

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void UnloadContent()
        {
            
        }

        public override void LoadContent()
        {

            SkySphereModel = XNAGame.Instance().Content.Load<Model>("SkySphere/SphereHighPoly");
            TextureCube SkySphereTexture = XNAGame.Instance().Content.Load<TextureCube>("SkySphere/SkySphereTexture");
            SkySphereEffect = XNAGame.Instance().Content.Load<Effect>("SkySphere");
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(XNAGame.Instance().Camera.view);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(XNAGame.Instance().Camera.projection);
            SkySphereEffect.Parameters["SkyboxTexture"].SetValue(SkySphereTexture);

            SkySphereEffect = XNAGame.Instance().Content.Load<Effect>("SkySphere");

            foreach (ModelMesh mesh in SkySphereModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = SkySphereEffect;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(XNAGame.Instance().Camera.view);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(XNAGame.Instance().Camera.projection);

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            XNAGame.Instance().GraphicsDevice.DepthStencilState = dss;
            foreach (ModelMesh mesh in SkySphereModel.Meshes)
            {

                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            XNAGame.Instance().GraphicsDevice.DepthStencilState = dss;
        }
    }
}
