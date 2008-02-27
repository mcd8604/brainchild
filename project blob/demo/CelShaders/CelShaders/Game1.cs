
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace CelShaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Model blob1;
        Model blob2;
        Model blob3;

        Effect celshader;

		Texture2D text;

        Vector3 blob2Position = Vector3.Zero;
        Vector3 blob1Position = new Vector3( -1.5f, 0.0f, 0.0f);
        Vector3 blob3Position = new Vector3(1.5f, 0.0f, 0.0f);
        Vector3 modelscale = Vector3.One;
        Quaternion modelrotation = Quaternion.Identity;

        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 2.4f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.PreferMultiSampling = true;
            content = new ContentManager(Services);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                blob1 = content.Load<Model>(@"content\models\ball");
                blob2 = content.Load<Model>(@"content\models\ball");
                blob3 = content.Load<Model>(@"content\models\cube");
				text = content.Load<Texture2D>(@"content\textures\test");
                celshader = content.Load<Effect>(@"content\shaders\Cel");
            }


            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the default game to exit on Xbox 360 and Windows
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Vector3 axis = Vector3.Up;
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(modelrotation));
            modelrotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, 0.02f) * modelrotation);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.RenderState.MultiSampleAntiAlias = true;
            this.graphics.GraphicsDevice.PresentationParameters.MultiSampleQuality = 4;
            this.graphics.GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.FourSamples;
            graphics.GraphicsDevice.Clear(Color.DarkOrange);

            //set a bunch of parameters
            Matrix View = Matrix.Invert(Matrix.CreateFromQuaternion(Quaternion.Identity) * Matrix.CreateTranslation(cameraPosition));
            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.0f, 1280.0f / 720.0f, 0.01f, 10.0f);

            if (celshader.Parameters["View"] != null)
                celshader.Parameters["View"].SetValue(View);

            if (celshader.Parameters["Projection"] != null)
                celshader.Parameters["Projection"].SetValue(Projection);

            if (celshader.Parameters["EyePosition"] != null)
                celshader.Parameters["EyePosition"].SetValue(cameraPosition);

            Matrix[] transforms = new Matrix[blob1.Bones.Count];
            blob1.CopyAbsoluteBoneTransformsTo(transforms);

            //Draw the blob1, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in blob1.Meshes)
            {
                Matrix World = Matrix.CreateScale(modelscale) * Matrix.CreateFromQuaternion(modelrotation) * Matrix.CreateTranslation(blob1Position);
                if (celshader.Parameters["World"] != null)
                    celshader.Parameters["World"].SetValue(World);

                if (celshader.Parameters["DiffuseLightColor"] != null)
                    celshader.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.6f, 0.6f, 0.6f, 1.0f));

                if (celshader.Parameters["LightPosition"] != null)
                    celshader.Parameters["LightPosition"].SetValue(new Vector3(5.0f, 600.0f, 600.0f));

                if (celshader.Parameters["LayerOneSharp"] != null)
                    celshader.Parameters["LayerOneSharp"].SetValue(0.6f);

                if (celshader.Parameters["LayerOneRough"] != null)
                    celshader.Parameters["LayerOneRough"].SetValue(0.0f);

                if (celshader.Parameters["LayerOneContrib"] != null)
                    celshader.Parameters["LayerOneContrib"].SetValue(0.05f);

                if (celshader.Parameters["LayerTwoSharp"] != null)
                    celshader.Parameters["LayerTwoSharp"].SetValue(0.85f);

                if (celshader.Parameters["LayerTwoRough"] != null)
                    celshader.Parameters["LayerTwoRough"].SetValue(10.0f);

                if (celshader.Parameters["LayerTwoContrib"] != null)
                    celshader.Parameters["LayerTwoContrib"].SetValue(0.3f);

                if (celshader.Parameters["EdgeOffset"] != null)
                    celshader.Parameters["EdgeOffset"].SetValue(0.016f);

                // Set the index buffer on the device once per mesh
                graphics.GraphicsDevice.Indices = mesh.IndexBuffer;
                // Start the actual effect from the shader returned above
                celshader.Begin();
                // Loop through each pass in the effect like we do elsewhere
                foreach (EffectPass pass in celshader.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    // Each mesh is made of parts (grouped by texture, etc.)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // Change the device settings for each part to be rendered
                        graphics.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                        graphics.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                        // Make sure we use the texture for the current part also
                        graphics.GraphicsDevice.Textures[0] = ((BasicEffect)part.Effect).Texture;
                        // Finally draw the actual triangles on the screen
                        graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                    pass.End();
                }
                celshader.End();
            }

            //Draw the blob2, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in blob2.Meshes)
            {
                Matrix World = Matrix.CreateScale(modelscale) * Matrix.CreateFromQuaternion(modelrotation) * Matrix.CreateTranslation(blob2Position);
                if (celshader.Parameters["World"] != null)
                    celshader.Parameters["World"].SetValue(World);

                if (celshader.Parameters["DiffuseLightColor"] != null)
                    celshader.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.9f, 0.4f, 0.4f, 1.0f));

                if (celshader.Parameters["LightPosition"] != null)
                    celshader.Parameters["LightPosition"].SetValue(new Vector3(5.0f, 200.0f, 300.0f));

                if (celshader.Parameters["LayerOneSharp"] != null)
                    celshader.Parameters["LayerOneSharp"].SetValue(0.1f);

                if (celshader.Parameters["LayerOneRough"] != null)
                    celshader.Parameters["LayerOneRough"].SetValue(0.01f);

                if (celshader.Parameters["LayerOneContrib"] != null)
                    celshader.Parameters["LayerOneContrib"].SetValue(0.1f);
                 //Does nothing layer two contrib is 0
                if (celshader.Parameters["LayerTwoSharp"] != null)
                    celshader.Parameters["LayerTwoSharp"].SetValue(0.8f);
                //Does nothing layer two contrib is 0
                if (celshader.Parameters["LayerTwoRough"] != null)
                    celshader.Parameters["LayerTwoRough"].SetValue(10.0f);
                //layer two contrib is 0
                if (celshader.Parameters["LayerTwoContrib"] != null)
                    celshader.Parameters["LayerTwoContrib"].SetValue(0.6f);

                if (celshader.Parameters["EdgeOffset"] != null)
                    celshader.Parameters["EdgeOffset"].SetValue(0.03f);

                // Set the index buffer on the device once per mesh
                graphics.GraphicsDevice.Indices = mesh.IndexBuffer;
                // Start the actual effect from the shader returned above
                celshader.Begin();
                // Loop through each pass in the effect like we do elsewhere
                foreach (EffectPass pass in celshader.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    // Each mesh is made of parts (grouped by texture, etc.)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // Change the device settings for each part to be rendered
                        graphics.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                        graphics.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                        // Make sure we use the texture for the current part also
                        graphics.GraphicsDevice.Textures[0] = ((BasicEffect)part.Effect).Texture;
                        // Finally draw the actual triangles on the screen
                        graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                    pass.End();
                }
                celshader.End();
            }

            //Draw the blob3, a model can have multiple meshes, so loop
			foreach (ModelMesh mesh in blob3.Meshes)
            {
                Matrix World = Matrix.CreateScale(modelscale) * Matrix.CreateFromQuaternion(modelrotation) * Matrix.CreateTranslation(blob3Position);
                if (celshader.Parameters["World"] != null)
                    celshader.Parameters["World"].SetValue(World);

                if (celshader.Parameters["DiffuseLightColor"] != null)
                    celshader.Parameters["DiffuseLightColor"].SetValue(new Vector4(0.75f, 0.75f, 0.75f, 1.0f));

                if (celshader.Parameters["LightPosition"] != null)
                    celshader.Parameters["LightPosition"].SetValue(new Vector3(1.0f, 600.0f, 600.0f));

                if (celshader.Parameters["LayerOneSharp"] != null)
                    celshader.Parameters["LayerOneSharp"].SetValue(.9f);

                if (celshader.Parameters["LayerOneRough"] != null)
                    celshader.Parameters["LayerOneRough"].SetValue(0.15f);

                if (celshader.Parameters["LayerOneContrib"] != null)
                    celshader.Parameters["LayerOneContrib"].SetValue(0.15f);

                if (celshader.Parameters["LayerTwoSharp"] != null)
                    celshader.Parameters["LayerTwoSharp"].SetValue(0.10f);

                if (celshader.Parameters["LayerTwoRough"] != null)
                    celshader.Parameters["LayerTwoRough"].SetValue(4.0f);

                if (celshader.Parameters["LayerTwoContrib"] != null)
                    celshader.Parameters["LayerTwoContrib"].SetValue(0.8f);

                if (celshader.Parameters["EdgeOffset"] != null)
                    celshader.Parameters["EdgeOffset"].SetValue(0.03f);

                // Set the index buffer on the device once per mesh
                graphics.GraphicsDevice.Indices = mesh.IndexBuffer;
                // Start the actual effect from the shader returned above
                celshader.Begin();
                // Loop through each pass in the effect like we do elsewhere
                foreach (EffectPass pass in celshader.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    // Each mesh is made of parts (grouped by texture, etc.)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
						((BasicEffect)part.Effect).Texture = text;
                        // Change the device settings for each part to be rendered
                        graphics.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                        graphics.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                        // Make sure we use the texture for the current part also
						graphics.GraphicsDevice.Textures[0] = ((BasicEffect)part.Effect).Texture;
                        // Finally draw the actual triangles on the screen
                        graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                    pass.End();
                }
                celshader.End();
            }

            base.Draw(gameTime);
        }
    }
}