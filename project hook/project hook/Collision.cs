using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

namespace project_hook
{
    public static class Collision
    {
        /// <summary>
        /// Check Everything in list to see if any collisions have occured.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="gametime"></param>
        public static void CheckCollisions(List<Sprite> list)
        {
            MultiDictionary<Collidable.Factions, Collidable> sorter = new MultiDictionary<Collidable.Factions, Collidable>(true);
            foreach (Sprite s in list)
            {
                Collidable temp = s as Collidable;
                if (temp != null)
                {
                    sorter.Add(temp.Faction, temp);
                }
            }

            List<Collidable.Factions> keys = new List<Collidable.Factions>(sorter.Keys);
            for (int i = 0; i < keys.Count - 1; ++i)
            {
                foreach (Collidable c in sorter[keys[i]])
                {
                    for (int j = i + 1; j < keys.Count; ++j)
                    {
                        foreach (Collidable x in sorter[keys[j]])
                        {
                            if (DoesIntersect(c, x))
                            {
                                c.RegisterCollision(x);
                                x.RegisterCollision(c);
                            }
                        }
                    }
                }
            }
        }
        private static bool DoesIntersect(Collidable one, Collidable two)
        {
            if (one.Bound == Collidable.Boundings.Circle)
            {
                if (two.Bound == Collidable.Boundings.Circle)
                {
                    return DoesIntersectCircles(one.Center, one.Radius, two.Center, two.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Diamond)
                {
					return DoesIntersectCircleDiamond(one.Center, one.Radius, two.Center, two.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Square)
                {
                    return DoesIntersectCircleSquare(one.Center, one.Radius, two.Center, two.Radius);
                }
            }
            else if (one.Bound == Collidable.Boundings.Diamond)
            {
                if (two.Bound == Collidable.Boundings.Circle)
                {
					return DoesIntersectCircleDiamond(two.Center, two.Radius, one.Center, one.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Diamond)
                {
                    return DoesIntersectDiamonds(one.Center, one.Radius, two.Center, two.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Square)
                {
					return DoesIntersectDiamondSquare(one.Center, one.Radius, two.Center, two.Radius);
                }
            }
            else if (one.Bound == Collidable.Boundings.Square)
            {
                if (two.Bound == Collidable.Boundings.Circle)
                {
                    return DoesIntersectCircleSquare(two.Center, two.Radius, one.Center, one.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Diamond)
                {
					return DoesIntersectDiamondSquare(two.Center, two.Radius, one.Center, one.Radius);
                }
                else if (two.Bound == Collidable.Boundings.Square)
                {
                    return DoesIntersectSquares(one.Center, one.Radius, two.Center, two.Radius);
                }
            }
            throw new Exception("unimplemented bounds collision exception");
        }

        private static bool DoesIntersectCircles(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
        {
            return (Math.Pow((radiustopoint1 + radiustopoint2), 2) > Math.Pow((pos1.X - pos2.X), 2) + Math.Pow((pos1.Y - pos2.Y), 2));
        }
		private static bool DoesIntersectCircleDiamond(Vector2 circ, float circrad, Vector2 diamond, float diamondrad)
		{
			// TODO
			return DoesIntersectCircles(circ, circrad, diamond, diamondrad);
		}
        private static bool DoesIntersectCircleSquare(Vector2 circ, float circrad, Vector2 square, float squarerad)
        {
            return (DoesIntersectSquares(circ, circrad, square, squarerad)) && (Math.Pow((circrad + ((float)(squarerad * (1 / Math.Cos((float)Math.Atan2((circ.Y - square.Y), (circ.X - square.X))))))), 2) > Math.Pow((circ.X - square.X), 2) + Math.Pow((circ.Y - square.Y), 2));
        }
        private static bool DoesIntersectDiamonds(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
        {
            return (radiustopoint1 + radiustopoint2 > Math.Abs(pos1.X - pos2.X) + Math.Abs(pos1.Y - pos2.Y));
        }
		private static bool DoesIntersectDiamondSquare(Vector2 diamond, float diamondrad, Vector2 square, float squarerad)
		{
			// TODO
			return DoesIntersectCircles(diamond, diamondrad, square, squarerad);
		}
        private static bool DoesIntersectSquares(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
        {
            if (pos1.X + radiustopoint1 < pos2.X - radiustopoint2)
                return false;
            if (pos2.X + radiustopoint2 < pos1.X - radiustopoint1)
                return false;
            if (pos1.Y + radiustopoint1 < pos2.Y - radiustopoint2)
                return false;
            if (pos2.Y + radiustopoint2 < pos1.Y - radiustopoint1)
                return false;
            return true;
        }
    }
}