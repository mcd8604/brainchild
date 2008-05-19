using System;
using System.Collections.Generic;
using System.Text;
using Physics2;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
	/// <summary>
	/// This class is intended only to update the VertexBuffer of a model.
	/// </summary>
	public class DrawableBody : Physics2.BodyRigid
	{
		private StaticModel m_DrawableModel;

		public DrawableBody(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, StaticModel p_Model)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks)
		{
			m_DrawableModel = p_Model;
		}

		public override void update(float TotalElapsedSeconds)
		{
			base.update(TotalElapsedSeconds);
			updateVertices();
		}

		private void updateVertices()
		{
			try
			{
				for (int i = m_DrawableModel.Vertices.Length; i > 0; --i)
				{
					m_DrawableModel.Vertices[i].Position = points[i].ExternalPosition;
				}
			}
			catch (IndexOutOfRangeException )
			{
	
			}
		}

		public StaticModel getModel()
		{
			return m_DrawableModel;
		}

		// temporary, until this can be configured properly
		public override bool canCollide()
		{
			return false;
		}

	}
}
