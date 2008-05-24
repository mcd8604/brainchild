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
	internal class DrawableBody : Body
	{
		private StaticModel m_DrawableModel;

        private Dictionary<int, PhysicsPoint> pointMap;

        public DrawableBody(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, StaticModel p_Model, Dictionary<int, PhysicsPoint> p_pointMap)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks, false)
		{
			m_DrawableModel = p_Model;
            pointMap = p_pointMap;
		}

		public override void update(float TotalElapsedSeconds)
		{
            base.update(TotalElapsedSeconds);

            //update vertices
            for (int i = 0; i < m_DrawableModel.Vertices.Length; ++i)
            {
                m_DrawableModel.Vertices[i].Position = pointMap[i].ExternalPosition;
            }

            //update bounding box
            m_DrawableModel.SetBoundingBox(getBoundingBox().GetXNABoundingBox());
            
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
