using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace Shader
{
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private struct myownvertexformat
		{
			private Vector3 Position;
			private Vector2 TexCoord;
			private Vector3 Normal;

			public myownvertexformat(Vector3 Position, Vector2 TexCoord, Vector3 Normal)
			{
				this.Position = Position;
				this.TexCoord = TexCoord;
				this.Normal = Normal;
			}

			public static VertexElement[] Elements =
             {
                 new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                 new VertexElement(0, sizeof(float)*3, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0),
                 new VertexElement(0, sizeof(float)*5, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0),
             };
			public static int SizeInBytes = sizeof(float) * (3 + 2 + 3);
		}

		GraphicsDeviceManager graphics;
		ContentManager content;
		GraphicsDevice device;
		Effect effect;
		Vector3 CameraPos;
		Matrix viewMatrix;
		Matrix projectionMatrix;
		VertexBuffer vb;
		Texture2D StreetTexture;
		Model LamppostModel;
		Model CarModel;
		Texture2D[] CarTextures;
		Texture2D[] LamppostTextures;
		Vector4 LightPos;
		float LightPower;
		Matrix lightViewProjectionMatrix;
		RenderTarget2D renderTarget;
		Texture2D texturedRenderedTo;
		Texture2D CarLight;
		Vector4[] LamppostPos;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);

			if (GraphicsAdapter.DefaultAdapter.GetCapabilities(DeviceType.Hardware).MaxPixelShaderProfile < ShaderProfile.PS_2_0)

				graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(SetToReference);
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		private void SetUpVertices()
		{
			myownvertexformat[] vertices = new myownvertexformat[18];

			vertices[0] = new myownvertexformat(new Vector3(-20, -10, 0), new Vector2(-0.25f, 25.0f), new Vector3(0, 0, 1));
			vertices[1] = new myownvertexformat(new Vector3(-20, 100, 0), new Vector2(-0.25f, 0.0f), new Vector3(0, 0, 1));
			vertices[2] = new myownvertexformat(new Vector3(2, -10, 0), new Vector2(0.25f, 25.0f), new Vector3(0, 0, 1));
			vertices[3] = new myownvertexformat(new Vector3(2, 100, 0), new Vector2(0.25f, 0.0f), new Vector3(0, 0, 1));
			vertices[4] = new myownvertexformat(new Vector3(2, -10, 0), new Vector2(0.25f, 25.0f), new Vector3(-1, 0, 0));
			vertices[5] = new myownvertexformat(new Vector3(-2, 100, 0), new Vector2(0.25f, 0.0f), new Vector3(-1, 0, 0));
			vertices[6] = new myownvertexformat(new Vector3(2, -10, 1), new Vector2(0.375f, 25.0f), new Vector3(-1, 0, 0));
			vertices[7] = new myownvertexformat(new Vector3(2, 100, 1), new Vector2(0.375f, 0.0f), new Vector3(-1, 0, 0));
			vertices[8] = new myownvertexformat(new Vector3(2, -10, 1), new Vector2(0.375f, 25.0f), new Vector3(0, 0, 1));
			vertices[9] = new myownvertexformat(new Vector3(2, 100, 1), new Vector2(0.375f, 0.0f), new Vector3(0, 0, 1));
			vertices[10] = new myownvertexformat(new Vector3(3, -10, 1), new Vector2(0.5f, 25.0f), new Vector3(0, 0, 1));
			vertices[11] = new myownvertexformat(new Vector3(3, 100, 1), new Vector2(0.5f, 0.0f), new Vector3(0, 0, 1));
			vertices[12] = new myownvertexformat(new Vector3(13, -10, 1), new Vector2(0.75f, 25.0f), new Vector3(0, 0, 1));
			vertices[13] = new myownvertexformat(new Vector3(13, 100, 1), new Vector2(0.75f, 0.0f), new Vector3(0, 0, 1));
			vertices[14] = new myownvertexformat(new Vector3(13, -10, 1), new Vector2(0.75f, 25.0f), new Vector3(-1, 0, 0));
			vertices[15] = new myownvertexformat(new Vector3(13, 100, 1), new Vector2(0.75f, 0.0f), new Vector3(-1, 0, 0));
			vertices[16] = new myownvertexformat(new Vector3(13, -10, 21), new Vector2(1.25f, 25.0f), new Vector3(-1, 0, 0));
			vertices[17] = new myownvertexformat(new Vector3(13, 100, 21), new Vector2(1.25f, 0.0f), new Vector3(-1, 0, 0));

			vb = new VertexBuffer(device, myownvertexformat.SizeInBytes * 18, ResourceUsage.WriteOnly);
			vb.SetData(vertices);
		}

		private void SetUpXNADevice()
		{
			graphics.PreferredBackBufferWidth = 500;
			graphics.PreferredBackBufferHeight = 500;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
			Window.Title = "Riemer's XNA Tutorials -- Series 3";
			device = graphics.GraphicsDevice;
			renderTarget = new RenderTarget2D(device, 512, 512, 1, SurfaceFormat.Color);
		}

		void SetToReference(object sender, PreparingDeviceSettingsEventArgs e)
		{
			e.GraphicsDeviceInformation.CreationOptions = CreateOptions.SoftwareVertexProcessing;
			e.GraphicsDeviceInformation.DeviceType = DeviceType.Reference;
			e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType = MultiSampleType.None;
		}

		private void LoadEffect()
		{

			effect = content.Load<Effect>("OurHLSLfile");
			effect.Parameters["xMaxDepth"].SetValue(60);
			effect.Parameters["xCarLightTexture"].SetValue(CarLight);
			effect.Parameters["xAmbient"].SetValue(0.4f);

			CameraPos = new Vector3(-25, -18, 13);
			viewMatrix = Matrix.CreateLookAt(CameraPos, new Vector3(0, 12, 2), new Vector3(0, 0, 1));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 1.0f, 200.0f);
		}

		protected override void LoadGraphicsContent(bool loadAllContent)
		{
			if (loadAllContent)
			{

				StreetTexture = content.Load<Texture2D>("streettexture");

				CarLight = content.Load<Texture2D>("carlight");

				SetUpXNADevice();
				LoadEffect();
				LoadModels();
				SetUpVertices();
			}
		}

		private void LoadModels()
		{

			LamppostModel = content.Load<Model>("lamppost");
			LamppostTextures = new Texture2D[7];
			int i = 0;
			foreach (ModelMesh mesh in LamppostModel.Meshes)
				foreach (BasicEffect currenteffect in mesh.Effects)
					LamppostTextures[i++] = StreetTexture;
			foreach (ModelMesh modmesh in LamppostModel.Meshes)
				foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
					modmeshpart.Effect = effect.Clone(device);


			CarModel = content.Load<Model>("racer");
			i = 0;
			CarTextures = new Texture2D[7];
			foreach (ModelMesh mesh in CarModel.Meshes)
				foreach (BasicEffect currenteffect in mesh.Effects)
					CarTextures[i++] = currenteffect.Texture;
			foreach (ModelMesh modmesh in CarModel.Meshes)
				foreach (ModelMeshPart modmeshpart in modmesh.MeshParts)
					modmeshpart.Effect = effect.Clone(device);
		}

		protected override void UnloadGraphicsContent(bool unloadAllContent)
		{
			if (unloadAllContent == true)
			{
				content.Unload();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();
			UpdateLightData();
			base.Update(gameTime);
		}

		private void UpdateLightData()
		{
			LightPos = new Vector4(-18, 2, 5, 1);
			LightPower = 1.7f;
			lightViewProjectionMatrix = Matrix.CreateLookAt(new Vector3(LightPos.X, LightPos.Y, LightPos.Z), new Vector3(-2, 10, -3), new Vector3(0, 0, 1)) * Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height, 1f, 100f);

			LamppostPos = new Vector4[2];
			LamppostPos[0] = new Vector4(4.0f, 5.0f, 11.5f, 1);
			LamppostPos[1] = new Vector4(4.0f, 35.0f, 11.5f, 1);
		}

		private void DrawModel(string technique, Model currentmodel, Matrix worldMatrix, Texture2D[] textures, bool useBrownInsteadOfTextures)
		{
			int i = 0;
			foreach (ModelMesh modmesh in currentmodel.Meshes)
			{
				foreach (Effect currenteffect in modmesh.Effects)
				{


					currenteffect.CurrentTechnique = effect.Techniques[technique];
					currenteffect.Parameters["xCameraViewProjection"].SetValue(viewMatrix * projectionMatrix);
					currenteffect.Parameters["xLightViewProjection"].SetValue(lightViewProjectionMatrix);
					currenteffect.Parameters["xWorld"].SetValue(worldMatrix);

					currenteffect.Parameters["xColoredTexture"].SetValue(textures[i++]);
					currenteffect.Parameters["xLightPos"].SetValue(LightPos);
					currenteffect.Parameters["xLightPower"].SetValue(LightPower);
					currenteffect.Parameters["xShadowMap"].SetValue(texturedRenderedTo);
					currenteffect.Parameters["xLamppostPos"].SetValue(LamppostPos);
					currenteffect.Parameters["xCameraPos"].SetValue(new Vector4(CameraPos.X, CameraPos.Y, CameraPos.Z, 1));
					currenteffect.Parameters["xUseBrownInsteadOfTextures"].SetValue(useBrownInsteadOfTextures);
				}
				modmesh.Draw();
			}
		}

		private void DrawScene(string technique)
		{
			effect.CurrentTechnique = effect.Techniques[technique];

			effect.Parameters["xCameraViewProjection"].SetValue(viewMatrix * projectionMatrix);
			effect.Parameters["xLightViewProjection"].SetValue(lightViewProjectionMatrix);
			effect.Parameters["xWorld"].SetValue(Matrix.Identity);

			effect.Parameters["xColoredTexture"].SetValue(StreetTexture);
			effect.Parameters["xLightPos"].SetValue(LightPos);
			effect.Parameters["xLightPower"].SetValue(LightPower);
			effect.Parameters["xShadowMap"].SetValue(texturedRenderedTo);
			effect.Parameters["xLamppostPos"].SetValue(LamppostPos);
			effect.Parameters["xCameraPos"].SetValue(new Vector4(CameraPos.X, CameraPos.Y, CameraPos.Z, 1));

			effect.Begin();
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				device.VertexDeclaration = new VertexDeclaration(device, myownvertexformat.Elements);
				device.Vertices[0].SetSource(vb, 0, myownvertexformat.SizeInBytes);
				device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 18);
				pass.End();
			}
			effect.End();

			Matrix worldMatrix = Matrix.CreateScale(4f, 4f, 4f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateTranslation(-3, 15, 0);
			DrawModel(technique, CarModel, worldMatrix, CarTextures, false);
			worldMatrix = Matrix.CreateScale(4f, 4f, 4f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.Pi * 5.0f / 8.0f) * Matrix.CreateTranslation(-28, -1.9f, 0);
			DrawModel(technique, CarModel, worldMatrix, CarTextures, false);
			worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) * Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateTranslation(4.0f, 35, 1);
			DrawModel(technique, LamppostModel, worldMatrix, LamppostTextures, true);
			worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) * Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateTranslation(4.0f, 5, 1);
			DrawModel(technique, LamppostModel, worldMatrix, LamppostTextures, true);
		}

		protected override void Draw(GameTime gameTime)
		{
			device.SetRenderTarget(0, renderTarget);
			device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
			DrawScene("ShadowMap");
			device.ResolveRenderTarget(0);
			texturedRenderedTo = renderTarget.GetTexture();

			device.SetRenderTarget(0, null);
			device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
			DrawScene("ShadowedScene");

			base.Draw(gameTime);
		}
	}
}