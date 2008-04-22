using Microsoft.Xna.Framework;

namespace Physics2
{
	public abstract class CollidableStatic : Collidable
	{

		public CollidableStatic(BodyStatic parentBody)
			: base(parentBody)
		{ }

		public override bool isStatic()
		{
			return true;
		}

	}
}
