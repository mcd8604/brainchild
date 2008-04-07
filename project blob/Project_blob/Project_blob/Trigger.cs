using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Physics;

namespace Project_blob
{
    public class Trigger : Collidable
    {
        public Trigger()
            :base(null)
        {
        }

        /// <summary>
        /// Could this collidable possibly be hit by Point p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool couldIntersect(Physics.Point p)
        {
            return false;
        }

        /// <summary>
        /// Does the line segment from start to end intersect with this collidable.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public override float didIntersect(Vector3 start, Vector3 end)
        {
            return -1;
        }

        /// <summary>
        /// Should this point p, which hit this collidable, be blocked.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool shouldPhysicsBlock(Physics.Point p)
        {
            return false;
        }

        /// <summary>
        /// The Dot Product to the normal of this collidable surface, if applicable.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override float DotNormal(Vector3 pos)
        {
            return -1;
        }

        /// <summary>
        /// The Normal to this collidable surface, if applicable.
        /// </summary>
        /// <returns></returns>
        public override Vector3 Normal()
        {
            return Vector3.Zero;
        }

        /// <summary>
        /// This collidable was hit by a point, and this force was transferred into the collidable.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="f"></param>
        public override void ApplyForce(Vector3 at, Vector3 f)
        {
        }

        /// <summary>
        /// This collidable was hit by a point, and this velocity was transferred into the collidable.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="v"></param>
        public override void ImpartVelocity(Vector3 at, Vector3 v)
        {
        }

        public Vector3[] getCollisionVerticies()
        {
            throw new Exception("Not used");
        }

        public override Vector3[] getNextCollisionVerticies()
        {
            throw new Exception("Not used");
        }

        public override void test(Physics.Point p)
        {

            throw new Exception("do nothing!");
        }

        public override bool inBoundingBox(Vector3 v)
        {
            throw new Exception("do nothing");
        }

        //public override Physics.Material getMaterial()
        //{
        //    return new Physics.MaterialBasic();// NormalMaterial();
        //}
    }
}
