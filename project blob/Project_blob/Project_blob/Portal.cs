using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob {
	[Serializable]
	public class Portal {
		// Is this necessary?
		private String _name;
		[Obsolete]
		public String Name {
			get { return _name; }
			set { _name = value; }
		}

		private BoundingBox _boundingBox;
		public BoundingBox BoundingBox {
			get { return _boundingBox; }
			set { _boundingBox = value; }
		}

		private BoundingSphere _boundingSphere;
		[Obsolete]
		public BoundingSphere BoundingSphere {
			get { return _boundingSphere; }
			set { _boundingSphere = value; }
		}

		private List<int> _connectedSectors;
		public List<int> ConnectedSectors {
			get { return _connectedSectors; }
			set { _connectedSectors = value; }
		}

		private Vector3 _position;
		public Vector3 Position {
			get { return _position; }
			set { _position = value; }
		}

		private Vector3 _scale;
		public Vector3 Scale {
			get { return _scale; }
			set { _scale = value; }
		}

		private Vector3 _rotation;
		public Vector3 Rotation {
			get { return _rotation; }
			set { _rotation = value; }
		}

		private Matrix rot;

		public Portal() {
			_name = this.GetType().Name;
			_connectedSectors = new List<int>();

			_scale = Vector3.Zero;
			_position = Vector3.Zero;

			CreateBoundingBox();
		}

		public Portal(Vector3 size, Vector3 position) {
			_name = this.GetType().Name;
			_connectedSectors = new List<int>();

			_scale = size;
			_position = position;

			CreateBoundingBox();
		}

		private void makeRot() {
			rot = Matrix.Identity;

			if (_name == "portal1") {
				//rot = ;
			} else if (_name == "portal2") {
				rot = new Matrix(-0.00000004371139f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -0.00000004371139f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f);
			} else if (_name == "portal3") {
				rot = new Matrix(-0.00000004371139f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -0.00000004371139f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f);
			} else if (_name == "portal4") {
				rot = new Matrix(-0.00000004371139f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, -0.00000004371139f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f);
			} else if (_name == "portal5") {
				//rot = ;
			} else if (_name == "portal6") {
				//rot = ;
			}
		}

		//private void CreateBoundingSphere()
		//{
		//    _boundingSphere = BoundingSphere.CreateFromBoundingBox(_boundingBox);
		//}

		public override string ToString() {
			return _name;
		}

		//
		public void CreateBoundingBox()
			//public BoundingBox GetBoundingBox()
		{
			Matrix m_position = Matrix.CreateTranslation(_position);
			Matrix m_scale = Matrix.CreateScale(_scale);
			////Matrix m_rotation = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(_rotation.X))), Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(Convert.ToSingle(RotationYValue.Text))), Matrix.CreateRotationZ(MathHelper.ToRadians(Convert.ToSingle(RotationZValue.Text)))));
			Matrix m_rotation = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(_rotation.X)), Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(_rotation.Y)), Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation.Z))));
			Matrix transformMatrix = Matrix.Identity;
			Stack<Matrix> drawStack = new Stack<Matrix>();

			_boundingBox = new BoundingBox(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));

			//Temprorary method
			makeRot();


			//for (int j = 0; j < 4; ++j)
			//{
			//    for (int i = 0; i < 3; ++i)
			//    {
			//        if (m_PriorityArray[i] == j)
			//        {
			//            switch (i)
			//            {
			//                case 0:
			//                    if (this.Position != null)
			//                        drawStack.Push(this.Position);
			//                    break;
			//                case 1:
			//                    if (this.Rotation != null)
			//                        drawStack.Push(this.Rotation);
			//                    break;
			//                case 2:
			//                    if (this.Scale != null)
			//                        drawStack.Push(this.Scale);
			//                    break;
			//                default:
			//                    break;
			//            }
			//        }
			//    }
			//}
			if (m_scale != null)
				drawStack.Push(m_scale);
			if (m_rotation != null)
				//drawStack.Push(m_rotation);
				drawStack.Push(rot);
			if (m_position != null)
				drawStack.Push(m_position);

			while (drawStack.Count > 0) {
				transformMatrix = Matrix.Multiply(drawStack.Pop(), transformMatrix);
			}
			Vector3 min = _boundingBox.Min;
			Vector3 max = _boundingBox.Max;

			min = Vector3.Transform(min, transformMatrix);
			max = Vector3.Transform(max, transformMatrix);

			if (min.X > max.X) {
				float temp = min.X;
				min.X = max.X;
				max.X = temp;
			}
			if (min.Y > max.Y) {
				float temp = min.Y;
				min.Y = max.Y;
				max.Y = temp;
			}
			if (min.Z > max.Z) {
				float temp = min.Z;
				min.Z = max.Z;
				max.Z = temp;
			}

			if (min.X > max.X || min.Y > max.Y || min.Z > max.Z) {
				throw new Exception("Min GREATER than Max!!!");
			}

			_boundingBox = new BoundingBox(min, max);
			//return new BoundingBox(min, max) ;
		}
	}
}
