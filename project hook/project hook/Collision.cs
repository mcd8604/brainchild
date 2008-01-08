using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

namespace project_hook
{
    public static class Collision
    {

        public static void CheckCollisions(List<Sprite> list, GameTime gametime)
        {
            MultiDictionary<Collidable.Factions, Collidable> sorter = new MultiDictionary<Collidable.Factions,Collidable>(true);
            foreach (Sprite s in list)
            {
                Collidable temp = s as Collidable;
                if (temp != null)
                {
                    sorter.Add(temp.Faction, temp);
                }
            }
            
            List<Collidable.Factions> keys = new List<Collidable.Factions>( sorter.Keys );
            for( int i = 0; i < keys.Count - 1; ++i ) {
                foreach( Collidable c in sorter[keys[i]] ) {
                    for( int j = i + 1; j < keys.Count; ++j ) {
                        foreach( Collidable x in sorter[keys[j]] ) {
                            // since the collidables don't have defined bounds yet
                            if ( DoesIntersectDiamonds( c, x ) ) {
                                c.RegisterCollision(x, gametime);
                                x.RegisterCollision(c, gametime);
                            }
                        }
                    }
                }
            }
        }
        private static bool DoesIntersectDiamonds(Collidable one, Collidable two)
        {
            return DoesIntersectDiamonds(one.Center, one.Radius, two.Center, two.Radius);
        }
        private static bool DoesIntersectDiamonds(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
        {
            return (radiustopoint1 + radiustopoint2 > Math.Abs(pos1.X - pos2.X) + Math.Abs(pos1.Y - pos2.Y));
        }
        private static bool DoesIntersectCircles(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
        {
            return (Math.Pow((radiustopoint1 + radiustopoint2), 2) > Math.Pow((pos1.X - pos2.X), 2) + Math.Pow((pos1.Y - pos2.Y), 2));
        }
        private static bool DoesIntersectSquares()
        {
            return false;
        }
    }
}