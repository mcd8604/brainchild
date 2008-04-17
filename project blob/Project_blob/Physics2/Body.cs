using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
    public class Body : Actor
    {

        public List<Point> points = new List<Point>();
        public List<Collidable> collidables = new List<Collidable>();
        public List<Spring> springs = new List<Spring>();

        public Body parentBody = null;
        public List<Body> childBodies = null;

        public AxisAlignedBoundingBox boundingBox = new AxisAlignedBoundingBox();

        public Vector3 center;

        public Body() { }

        public Body(Body ParentBody)
        {
            ParentBody.addChild(this);
        }

        public void addChild(Body childBody)
        {
            if (childBody.parentBody != null)
            {
                throw new Exception();
            }
            if (childBodies == null)
            {
                childBodies = new List<Body>();
            }
            childBody.parentBody = this;
            childBodies.Add(childBody);
        }

        public virtual AxisAlignedBoundingBox getBoundingBox()
        {
            return boundingBox;
        }

        public virtual Vector3 getCenter() {
            return center;
        }

        public virtual IEnumerable<Collidable> getCollidables()
        {
            return collidables;
        }

        public virtual IEnumerable<Point> getPoints()
        {
            return points;
        }

        public virtual IEnumerable<Spring> getSprings()
        {
            return springs;
        }

        public virtual void update()
        {

            // Predict potiential position
            foreach (Point p in points)
            {
                //p.PotientialPosition = p.CurrentPosition + (p.CurrentVelocity * TotalElapsedSeconds);
            }

        }

        public virtual void updatePosition()
        {

            boundingBox.clear();
            foreach (Body child in childBodies)
            {
                child.update();
                boundingBox.expandToInclude(child.getBoundingBox());
            }

            center = Vector3.Zero;
            foreach (Point p in points)
            {
                center += p.CurrentPosition;
            }
            center /= points.Count;

        }

    }
}
