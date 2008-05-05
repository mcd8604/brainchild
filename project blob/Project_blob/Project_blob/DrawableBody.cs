using System;
using System.Collections.Generic;
using System.Text;
using Physics2;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    public abstract class DrawableBody : Physics2.Body
    {

        private StaticModel m_DrawableModel;
        private VertexPositionNormalTexture[] vertices;

        public DrawableBody(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, StaticModel p_Model)
			:base(ParentBody,p_points,p_collidables,p_springs,p_tasks)
        {
            m_DrawableModel = p_Model;
            m_DrawableModel.updateTextureCoords();
            vertices = m_DrawableModel.getVertices();
        }

        public override void update(float TotalElapsedSeconds)
        {
            base.update(TotalElapsedSeconds);
        }

        private void updateVertices()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = points[i].ExternalPosition;
            }
            m_DrawableModel.updateVertexBuffer(vertices);
        }
    }
}
