using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Physics2;
using Audio;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Project_blob {
	public enum CollisionTypes {
		None,
		BoundingBox,
		Body
	}

	[Serializable]
	public class StaticModel : Drawable {
		private string _modelName;

		private string _audioName;
		[TypeConverter(typeof(TypeConverterAudio))]
		public string AudioName {
			get { return _audioName; }
			set { _audioName = value; }
		}

		/*private TextureInfo m_TextureKey;
		public TextureInfo TextureKey
		{
			get { return m_TextureKey; }
			set { m_TextureKey = value; updateTextureCoords(); }
		}*/

		private bool m_RepeatingTexture = false;
		public bool RepeatingTexture {
			get {
				return m_RepeatingTexture;
			}
			set {
				m_RepeatingTexture = value;
				updateTextureCoords();
			}
		}
		private float m_TextureScaleX = 1f;
		public float TextureScaleX {
			get {
				return m_TextureScaleX;
			}
			set {
				m_TextureScaleX = value;
				updateTextureCoords();
			}
		}
		private float m_TextureScaleY = 1f;
		public float TextureScaleY {
			get {
				return m_TextureScaleY;
			}
			set {
				m_TextureScaleY = value;
				updateTextureCoords();
			}
		}

		private float m_TextureOffsetX = 0f;
		public float TextureOffsetX {
			get {
				return m_TextureOffsetX;
			}
			set {
				m_TextureOffsetX = value;
				updateTextureCoords();
			}
		}
		private float m_TextureOffsetY = 0f;
		public float TextureOffsetY {
			get {
				return m_TextureOffsetY;
			}
			set {
				m_TextureOffsetY = value;
				updateTextureCoords();
			}
		}

		private MaterialType m_MaterialType = MaterialType.Default;
		public MaterialType MyMaterialType {
			get {
				return m_MaterialType;
			}
			set {
				m_MaterialType = value;
			}
		}

		private EventTrigger m_Event;
		public EventTrigger Event {
			get {
				return m_Event;
			}
			set {
				m_Event = value;
			}
		}

		private bool m_Visible = true;
		public bool Visible {
			get {
				return m_Visible;
			}
			set {
				m_Visible = value;
			}
        }

        private BlendModes m_BlendMode = BlendModes.None;
        public BlendModes BlendMode
        {
            get
            {
                return m_BlendMode;
            }
            set
            {
                m_BlendMode = value;
            }
        }

        [NonSerialized]
        private bool m_Drawn = false;
        public bool Drawn
        {
            get
            {
                return m_Drawn;
            }
            set
            {
                m_Drawn = value;
            }
        }

		private CollisionTypes m_CollisionType = CollisionTypes.Body;
		public CollisionTypes CollisionType {
			get {
				return m_CollisionType;
			}
			set {
				m_CollisionType = value;
			}
		}

		/*public void updateVertexBuffer()
		{
			Model m = ModelManager.getSingleton.GetModel(_modelName);
			if (m != null)
			{
				foreach (ModelMesh mesh in m.Meshes)
				{
					m_VertexBuffers = new Dictionary<string, VertexBuffer>();

					// Vertices
					int numVertices = 0;
					foreach (ModelMeshPart part in mesh.MeshParts)
					{
						numVertices += part.NumVertices;
					}

					VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[numVertices];
					mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

					if (m_RepeatingTexture)
					{
						//scaleVector used to scale texture coordinates
						Vector3 scaleVector = Vector3.Zero;
						Quaternion rotVector = Quaternion.Identity;
						Vector3 transVector = Vector3.Zero;
						m_Scale.Decompose(out scaleVector, out rotVector, out transVector);

						Texture2D texture = TextureManager.getSingleton.GetTexture(TextureKey.TextureName);

						for (int i = 0; i < vertices.Length; ++i)
						{
							//scale the texture coordinates
							if (vertices[i].Normal.Equals(Vector3.Up))
							{
								vertices[i].TextureCoordinate.X *= (scaleVector.X / (m_TextureScaleX * texture.Width));
								vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (m_TextureScaleY * texture.Height));
							}
							else if (vertices[i].Normal.Equals(Vector3.Down))
							{
								vertices[i].TextureCoordinate.X *= (scaleVector.Z / (m_TextureScaleX * texture.Width));
								vertices[i].TextureCoordinate.Y *= (scaleVector.X / (m_TextureScaleY * texture.Height));
							}
							else if (vertices[i].Normal.Equals(Vector3.Left))
							{
								//vertices[i].TextureCoordinate.X *= (scaleVector.Y / (m_TextureScaleX * texture.Width));
								//vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (m_TextureScaleY * texture.Height));
							}
							else if (vertices[i].Normal.Equals(Vector3.Right))
							{
								vertices[i].TextureCoordinate.X *= (scaleVector.Z / (m_TextureScaleX * texture.Width));
								vertices[i].TextureCoordinate.Y *= (scaleVector.Y / (m_TextureScaleY * texture.Height));
							}
							else if (vertices[i].Normal.Equals(Vector3.Forward))
							{
								vertices[i].TextureCoordinate.X *= (scaleVector.Z / (m_TextureScaleX * texture.Width));
								vertices[i].TextureCoordinate.Y *= (scaleVector.Y / (m_TextureScaleY * texture.Height));
							}
							else if (vertices[i].Normal.Equals(Vector3.Backward))
							{
								//vertices[i].TextureCoordinate.X *= (scaleVector.Y/ (m_TextureScaleX * texture.Width));
								//vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (m_TextureScaleY * texture.Height));
							}
						}
					}
					m_VertexBuffers[mesh.Name] = new VertexBuffer(mesh.VertexBuffer.GraphicsDevice, mesh.VertexBuffer.SizeInBytes, mesh.VertexBuffer.BufferUsage);
					m_VertexBuffers[mesh.Name].SetData<VertexPositionNormalTexture>(vertices);
				}
			}
		}*/

		private BoundingBox m_BoundingBox;
		//private BoundingSphere m_BoundingSphere;

		public BoundingBox GetBoundingBox() {
			return m_BoundingBox;
		}
		public void SetBoundingBox(BoundingBox bb) {
			m_BoundingBox = bb;
		}

		[NonSerialized]
		private VertexPositionNormalTexture[] m_Vertices;
		public VertexPositionNormalTexture[] Vertices {
			get {
				return m_Vertices;
			}
			set {
				m_Vertices = value;
			}
		}

		[NonSerialized]
		private VertexBuffer m_VertexBuffer;
		public VertexBuffer getVertexBuffer() {
			return m_VertexBuffer;
		}
		[NonSerialized]
		private IndexBuffer m_IndexBuffer;
		public IndexBuffer getIndexBuffer() {
			return m_IndexBuffer;
		}
		[NonSerialized]
		private VertexDeclaration m_VertexDeclaration;
		public int getVertexStride() {
			return m_VertexStride;
		}
		[NonSerialized]
		private int m_VertexStride;
		[NonSerialized]
		private int m_StreamOffset;
		[NonSerialized]
		private int m_BaseVertex;
		[NonSerialized]
		private int m_MinVertexIndex;
		[NonSerialized]
		private int m_NumVertices;
		public int NumVertices {
			get {
				return m_NumVertices;
			}
		}
		[NonSerialized]
		private int m_StartIndex;
		[NonSerialized]
		private int m_PrimitiveCount;

		[NonSerialized]
		private VertexPositionNormalTexture[] origVertices;

		//supports one Mesh per Model
		public void initialize() {
			updateTransform();

			m_TextureID = TextureManager.GetTextureID(_textureName);

			Model m = ModelManager.getSingleton.GetModel(_modelName);

			// Crash: Object reference not set to instance of an object.
			ModelMesh mesh = m.Meshes[0];
			ModelMeshPart part = mesh.MeshParts[0];

			m_VertexBuffer = new VertexBuffer(mesh.VertexBuffer.GraphicsDevice, mesh.VertexBuffer.SizeInBytes, mesh.VertexBuffer.BufferUsage);
			m_IndexBuffer = mesh.IndexBuffer;

			m_VertexDeclaration = part.VertexDeclaration;
			m_VertexStride = part.VertexStride;
			m_StreamOffset = part.StreamOffset;
			m_BaseVertex = part.BaseVertex;
			m_MinVertexIndex = 0;
			m_NumVertices = part.NumVertices;
			m_StartIndex = part.StartIndex;
			m_PrimitiveCount = part.PrimitiveCount;

			origVertices = new VertexPositionNormalTexture[m_NumVertices];
			mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(origVertices);
			m_Vertices = new VertexPositionNormalTexture[m_NumVertices];
			mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(m_Vertices);

			//transform points
			for (int i = 0; i < m_Vertices.Length; ++i) {
				m_Vertices[i].Position = Vector3.Transform(m_Vertices[i].Position, Transform);
				m_Vertices[i].Normal = Vector3.TransformNormal(m_Vertices[i].Normal, Transform);
			}
			updateTextureCoords();
			try {
				updateVertexBuffer();
			} catch (InvalidOperationException) {
				m_VertexBuffer.GraphicsDevice.Vertices[0].SetSource(null, 0, 0);
				updateVertexBuffer();
			}
		}

		public void updateVertexBuffer() {
			m_VertexBuffer.SetData<VertexPositionNormalTexture>(m_Vertices);
		}

		/*public void updateVertexBuffer(VertexPositionNormalTexture[] vertices)
		{
			// Crash: Invalid Operation Exception was unhandled
			m_VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
		}*/

		/*public VertexPositionNormalTexture[] getVertices()
		{
			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[m_NumVertices];
			if (m_VertexBuffer != null)
			{
				m_VertexBuffer.GraphicsDevice.Vertices[0].SetSource(null, 0, 0);
				m_VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);
			}
			return vertices;
		}*/

		public void updateTextureCoords() {
			//VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[m_NumVertices];
			//Model m = ModelManager.getSingleton.GetModel(_modelName);

			//if (m != null)
			//{
			//m.Meshes[0].VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

			//VertexPositionNormalTexture[] myVertices = new VertexPositionNormalTexture[m_NumVertices];
			//m_VertexBuffer.GraphicsDevice.Vertices[0].SetSource(null, 0, 0);
			//m_VertexBuffer.GetData<VertexPositionNormalTexture>(myVertices);

			if (m_RepeatingTexture) {
				//scaleVector used to scale texture coordinates
				Vector3 scaleVector = Vector3.Zero;
				Quaternion rotVector = Quaternion.Identity;
				Vector3 transVector = Vector3.Zero;
				m_Scale.Decompose(out scaleVector, out rotVector, out transVector);

				//Texture2D texture = TextureManager.getSingleton.GetTexture(TextureKey.TextureName);
				Texture2D texture = TextureManager.GetTexture(m_TextureID);

				for (int i = 0; i < origVertices.Length; ++i) {
					//scale the texture coordinates
					m_Vertices[i].TextureCoordinate.X = (origVertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
					m_Vertices[i].TextureCoordinate.Y = (origVertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;

					//scale the texture coordinates
					/*if (vertices[i].Normal.Equals(Vector3.Up))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}
					else if (vertices[i].Normal.Equals(Vector3.Down))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}
					else if (vertices[i].Normal.Equals(Vector3.Left))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}
					else if (vertices[i].Normal.Equals(Vector3.Right))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}
					else if (vertices[i].Normal.Equals(Vector3.Forward))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}
					else if (vertices[i].Normal.Equals(Vector3.Backward))
					{
						myVertices[i].TextureCoordinate.X = (vertices[i].TextureCoordinate.X * (scaleVector.X / (m_TextureScaleX * texture.Width))) + TextureOffsetX;
						myVertices[i].TextureCoordinate.Y = (vertices[i].TextureCoordinate.Y * (scaleVector.Z / (m_TextureScaleY * texture.Height))) + TextureOffsetY;
					}*/
				}
			}
			//m_VertexBuffer.SetData<VertexPositionNormalTexture>( m_Vertices );
			//m_VertexBuffer.GraphicsDevice.Vertices[0].SetSource(m_VertexBuffer, m_StreamOffset, m_VertexStride);
			//}
		}

		/*
        [NonSerialized]
        private Dictionary<string, VertexBuffer> m_VertexBuffers;
        
        //[NonSerialized]
        //private List<Physics.Collidable> m_collidables;
        public List<Collidable> createCollidables(Area areaRef)
        {
            Model m = ModelManager.getSingleton.GetModel(_modelName);
            List<Collidable> collidables = new List<Collidable>();
            foreach (ModelMesh mesh in m.Meshes)
            {
                // Vertices
                int numVertices = 0;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    numVertices += part.NumVertices;
                }
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[numVertices];
                mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                //-------------------------------------------
                if (_modelName.Equals("plane"))
                {
                    m_VertexBuffers = new Dictionary<string, VertexBuffer>();

                    //scaleVector used to scale texture coordinates
                    Vector3 scaleVector = Vector3.Zero;
                    Quaternion rotVector = Quaternion.Identity;
                    Vector3 transVector = Vector3.Zero;
                    m_Scale.Decompose(out scaleVector, out rotVector, out transVector);

                    Texture2D texture = TextureManager.getSingleton.GetTexture(TextureKey.TextureName);

                    for (int i = 0; i < vertices.Length; ++i)
                    {
                        //scale the texture coordinates
                        vertices[i].TextureCoordinate.X *= (scaleVector.X / (texture.Width * 0.5f));
                        vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (texture.Height * 0.5f));
                    }

                    m_VertexBuffers[mesh.Name] = new VertexBuffer(mesh.VertexBuffer.GraphicsDevice, mesh.VertexBuffer.SizeInBytes, mesh.VertexBuffer.BufferUsage);
                    m_VertexBuffers[mesh.Name].SetData<VertexPositionNormalTexture>(vertices);
                }
                //-------------------------------------------

                Matrix transformMatrix = Matrix.Identity;
                Stack<Matrix> drawStack = new Stack<Matrix>();
                for (int j = 0; j < 4; ++j)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        if (m_PriorityArray[i] == j)
                        {
                            switch (i)
                            {
                                case 0:
                                    if (this.Position != null)
                                        drawStack.Push(this.Position);
                                    break;
                                case 1:
                                    if (this.Rotation != null)
                                        drawStack.Push(this.Rotation);
                                    break;
                                case 2:
                                    if (this.Scale != null)
                                        drawStack.Push(this.Scale);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                while (drawStack.Count > 0)
                {
                    transformMatrix = Matrix.Multiply(drawStack.Pop(), transformMatrix);
                }

                for (int i = 0; i < vertices.Length; ++i)
                {
                    //transform points to correct position
                    vertices[i].Position = Vector3.Transform(vertices[i].Position, transformMatrix);
                    vertices[i].Normal = Vector3.TransformNormal(vertices[i].Normal, transformMatrix);
                }

                // Indices
                int[] indices;
                if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                {
                    indices = new int[(mesh.IndexBuffer.SizeInBytes) >> 1];
                    short[] temp = new short[(mesh.IndexBuffer.SizeInBytes) >> 1];
                    mesh.IndexBuffer.GetData<short>(temp);
                    for (int i = 0; i < temp.Length; ++i)
                        indices[i] = temp[i];
                }
                else
                {
                    indices = new int[(mesh.IndexBuffer.SizeInBytes) >> 2];
                    mesh.IndexBuffer.GetData<int>(indices);
                }

                // Collidables
                int numCol = 0;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    for (int i = 0; i < indices.Length; i += 3)
                    {
                        if(vertices[indices[i]].Position != vertices[indices[i + 1]].Position && vertices[indices[i + 2]].Position != vertices[indices[i]].Position && vertices[indices[i + 1]].Position != vertices[indices[i + 2]].Position) {
                            if(m_TextureKey.TextureName.Equals("event")) {
                                collidables.Add(new Trigger(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]], areaRef.Events[Name]));
                            } else {
                                collidables.Add(new CollidableTri(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]]));
                            }
                            ++numCol;
                        }
                    }
                }
            }
            return collidables;
        }*/

		public string getName() {
			return m_Name;
		}

		private string _textureName;
		[TypeConverter(typeof(TypeConverterTexture))]
		public string TextureName {
			get {
				return _textureName;
			}
			set {
				_textureName = value;
				m_TextureID = TextureManager.GetTextureID(value);
			}
		}

		private string m_Name;
		public string Name {
			get {
				return m_Name;
			}
			set {
				m_Name = value;
			}
		}

		Matrix m_Position, m_Rotation, m_Scale;

		//priority for translation, rotation, and scale
		//index 0 = translation
		//index 1 = rotation
		//index 2 = scale
		int[] m_PriorityArray = new int[3];

		public int[] PriorityArray {
			get {
				return m_PriorityArray;
			}
			set {
				m_PriorityArray = value;
			}
		}
		public int TranslationPriority {
			get {
				return m_PriorityArray[0];
			}
			set {
				m_PriorityArray[0] = value;
			}
		}

		public int RotationPriority {
			get {
				return m_PriorityArray[1];
			}
			set {
				m_PriorityArray[1] = value;
			}
		}

		public int ScalePriority {
			get {
				return m_PriorityArray[2];
			}
			set {
				m_PriorityArray[2] = value;
			}
		}

		public Matrix Position {
			get {
				return m_Position;
			}
			set {
				m_Position = value;
				updateTransform();
			}
		}

		public Matrix Rotation {
			get {
				return m_Rotation;
			}
			set {
				m_Rotation = value;
				updateTransform();
			}
		}

		public Matrix Scale {
			get {
				return m_Scale;
			}
			set {
				m_Scale = value;
				updateTransform();
				//updateTextureCoords();
			}
		}

		private Matrix m_Transform;
		public Matrix Transform {
			get {
				return m_Transform;
			}
		}

		private void updateTransform() {
			m_Transform = Matrix.Identity;
			Stack<Matrix> drawStack = new Stack<Matrix>();
			for (int i = 0; i < 3; ++i) {
				switch (m_PriorityArray[i]) {
					case 0:
						if (this.Position != null)
							drawStack.Push(this.Position);
						break;
					case 1:
						if (this.Rotation != null)
							drawStack.Push(this.Rotation);
						break;
					case 2:
						if (this.Scale != null)
							drawStack.Push(this.Scale);
						break;
					default:
						break;
				}
			}

			while (drawStack.Count > 0) {
				m_Transform = Matrix.Multiply(drawStack.Pop(), m_Transform);
			}
			if (m_Vertices != null) {
				//transform points
				for (int i = 0; i < m_Vertices.Length; ++i) {
					m_Vertices[i].Position = Vector3.Transform(origVertices[i].Position, Transform);
					m_Vertices[i].Normal = Vector3.TransformNormal(origVertices[i].Normal, Transform);
				}
			}
		}

		[TypeConverter(typeof(TypeConverterModel))]
		public string ModelName {
			get {
				return _modelName;
			}
			set {
				_modelName = value;
			}
		}

		bool m_ShowVertices = false;
		public bool ShowVertices {
			get {
				return m_ShowVertices;
			}
			set {
				m_ShowVertices = value;
			}
		}

		private List<short> _rooms;
		public List<short> Rooms {
			get { return _rooms; }
			set { _rooms = value; }
		}
		public void AddRoom(short room) {
			if (_rooms == null) {
				_rooms = new List<short>();
			}
			if (!_rooms.Contains(room)) {
				_rooms.Add(room);
			}
		}
		public void RemoveRoom(short room) {
			_rooms.Remove(room);
		}

		public StaticModel() { }

		public StaticModel(StaticModel p_Model) {
			/*foreach (System.Reflection.PropertyInfo p in p_Model.GetType().GetProperties())
			{
         
			}*/
			this._audioName = p_Model.AudioName;
			this._modelName = p_Model.ModelName;
			this.m_Name = p_Model.Name;
			this.m_Position = p_Model.Position;
			this.m_PriorityArray = p_Model.PriorityArray;
			this.m_RepeatingTexture = p_Model.RepeatingTexture;
			this._rooms = p_Model.Rooms;
			this.m_Rotation = p_Model.Rotation;
			this.RotationPriority = p_Model.RotationPriority;
			this.m_Scale = p_Model.Scale;
			this.ScalePriority = p_Model.ScalePriority;
			this.m_ShowVertices = p_Model.ShowVertices;
			//this.m_TextureKey = p_Model.TextureKey;
			this._textureName = p_Model.TextureName;
			this.m_TextureScaleX = p_Model.TextureScaleX;
			this.m_TextureScaleY = p_Model.TextureScaleY;
			this.TranslationPriority = p_Model.TranslationPriority;
			initialize();
		}

		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public StaticModel(SerializationInfo info, StreamingContext ctxt) {
			updateTransform();
		}

		/*public StaticModel(string p_Name, string fileName, string audioName, List<short> rooms)
		{
			m_Name = p_Name;
			_modelName = fileName;
			_audioName = audioName;
			TranslationPriority = 2;
			RotationPriority = 1;
			ScalePriority = 0;

			_rooms = rooms;

			m_Position = Matrix.CreateTranslation(Vector3.Zero);
			m_Rotation = Matrix.CreateRotationZ(0);
			m_Scale = Matrix.CreateScale(1);

			//m_TextureKey = null;
		}*/

		public StaticModel(string p_Name, string fileName, string audioName, string p_TextureName, List<short> rooms) {
			m_Name = p_Name;
			_modelName = fileName;
			_audioName = audioName;
			_textureName = p_TextureName;
			TranslationPriority = 2;
			RotationPriority = 1;
			ScalePriority = 0;

			_rooms = rooms;

			m_Position = Matrix.CreateTranslation(Vector3.Zero);
			m_Rotation = Matrix.CreateRotationZ(0);
			m_Scale = Matrix.CreateScale(1);

			//m_TextureKey = p_TextureKey;
		}

		public virtual void DrawMe() { }

		public virtual void DrawMe(GraphicsDevice graphicsDevice, Effect effect, bool gameMode) {
			graphicsDevice.Indices = m_IndexBuffer;
			effect.Begin();

			// Loop through each pass in the effect like we do elsewhere
			foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
				pass.Begin();
				// Change the device settings for each part to be rendered
				graphicsDevice.VertexDeclaration = m_VertexDeclaration;
				graphicsDevice.Vertices[0].SetSource(m_VertexBuffer, m_StreamOffset, m_VertexStride);
				// Finally draw the actual triangles on the screen
				graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, m_BaseVertex, m_MinVertexIndex, m_NumVertices, m_StartIndex, m_PrimitiveCount);
				if (this.ShowVertices && !gameMode) {
					Texture2D temp = (Texture2D)graphicsDevice.Textures[0];
					graphicsDevice.Textures[0] = TextureManager.GetTexture("point_text");
					graphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, m_NumVertices);
					graphicsDevice.Textures[0] = temp;
				}
				pass.End();
			}
			effect.End();
		}

		/*public void DrawMe(ModelMesh mesh, GraphicsDevice graphicsDevice, bool gameMode)
		{
			foreach (ModelMeshPart part in mesh.MeshParts)
			{                
				// Change the device settings for each part to be rendered
				graphicsDevice.VertexDeclaration = part.VertexDeclaration;
				if (m_VertexBuffers == null)
				{
					updateVertexBuffer();
				}
				try
				{
					graphicsDevice.Vertices[0].SetSource(m_VertexBuffers[mesh.Name], part.StreamOffset, part.VertexStride);
					// Finally draw the actual triangles on the screen
					graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
					if (this.ShowVertices && !gameMode)
					{
						Texture2D temp = (Texture2D)graphicsDevice.Textures[0];
						graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture("point_text");
						graphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, part.NumVertices);
						graphicsDevice.Textures[0] = temp;
					}
				}
				catch (KeyNotFoundException knfe)
				{
					Log.Out.WriteLine("KEY NOT FOUND IN STATIC MODEL: " + this.Name);
				}
			}
		}*/

		#region Drawable Members


		/*public TextureInfo GetTextureKey()
		{
			return m_TextureKey;
		}*/

		[NonSerialized]
		private int m_TextureID;
		public int GetTextureID() {
			return m_TextureID;
		}
		public void SetTextureID(int id) {
			m_TextureID = id;
		}

		/*public BoundingBox GetBoundingBox()
		{
			if (m_BoundingSphere.Radius == 0)
			{
				Model m = ModelManager.getSingleton.GetModel(_modelName);
				if (m != null)
				{
					foreach (ModelMesh mesh in ModelManager.getSingleton.GetModel(_modelName).Meshes)
					{
						m_BoundingSphere = BoundingSphere.CreateMerged(m_BoundingSphere, mesh.BoundingSphere);
					}
					m_BoundingBox = BoundingBox.CreateFromSphere(m_BoundingSphere);
				}
			}

			Matrix transformMatrix = Matrix.Identity;
			Stack<Matrix> drawStack = new Stack<Matrix>();
			for (int j = 0; j < 4; ++j)
			{
				for (int i = 0; i < 3; ++i)
				{
					if (m_PriorityArray[i] == j)
					{
						switch (i)
						{
							case 0:
								if (this.Position != null)
									drawStack.Push(this.Position);
								break;
							case 1:
								if (this.Rotation != null)
									drawStack.Push(this.Rotation);
								break;
							case 2:
								if (this.Scale != null)
									drawStack.Push(this.Scale);
								break;
							default:
								break;
						}
					}
				}
			}

			while (drawStack.Count > 0)
			{
				transformMatrix = Matrix.Multiply(drawStack.Pop(), transformMatrix);
			}
			Vector3 min = m_BoundingBox.Min;
			Vector3 max = m_BoundingBox.Max;

			min = Vector3.Transform(min, transformMatrix);
			max = Vector3.Transform(max, transformMatrix);

			if (min.X > max.X)
			{
				float temp = min.X;
				min.X = max.X;
				max.X = temp;
			}
			if (min.Y > max.Y)
			{
				float temp = min.Y;
				min.Y = max.Y;
				max.Y = temp;
			}
			if (min.Z > max.Z)
			{
				float temp = min.Z;
				min.Z = max.Z;
				max.Z = temp;
			}

			if (min.X > max.X || min.Y > max.Y || min.Z > max.Z)
			{
				throw new Exception("Min GREATER than Max!!!");
			}

			return new BoundingBox(min, max) ;
		}*/


		//public BoundingSphere GetBoundingSphere()
		//{
		//    Matrix transformMatrix = Matrix.Identity;
		//    Stack<Matrix> drawStack = new Stack<Matrix>();
		//    for (int j = 0; j < 4; ++j)
		//    {
		//        for (int i = 0; i < 3; ++i)
		//        {
		//            if (m_PriorityArray[i] == j)
		//            {
		//                switch (i)
		//                {
		//                    case 0:
		//                        if (this.Position != null)
		//                            drawStack.Push(this.Position);
		//                        break;
		//                    case 1:
		//                        if (this.Rotation != null)
		//                            drawStack.Push(this.Rotation);
		//                        break;
		//                    case 2:
		//                        if (this.Scale != null)
		//                            drawStack.Push(this.Scale);
		//                        break;
		//                    default:
		//                        break;
		//                }
		//            }
		//        }
		//    }

		//    while (drawStack.Count > 0)
		//    {
		//        transformMatrix = Matrix.Multiply(drawStack.Pop(), transformMatrix);
		//    }
		//    Vector3 center = m_BoundingSphere.Center;
		//    //float radius;

		//    throw new Exception("Fix This");
		//    //return m_BoundingSphere;
		//}
		#endregion

		public override string ToString() {
			return m_Name;
		}
	}
}
