using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Point : Actor
	{

        public Vector3 CurrentPosition;

        public Vector3 PotientialPosition;

        public Collidable LastCollision;

        public Vector3 ForceThisFrame;

        public virtual void update() { }

	}
}
