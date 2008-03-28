using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo7
{
    public interface Drawable
    {

        VertexBuffer getVertexBuffer();

        void setGraphicsDevice(GraphicsDevice device);

        int getVertexStride();

        void DrawMe();

    }
}