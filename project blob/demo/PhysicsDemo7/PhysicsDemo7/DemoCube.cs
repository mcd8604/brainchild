using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo7
{
    class DemoCube : Physics.PressureBody, Drawable
    {

        public readonly List<Physics.Point> points = new List<Physics.Point>();
        public readonly List<Physics.Spring> springs = new List<Physics.Spring>();
        public readonly List<T> collidables = new List<T>();

        Physics.Point ftr;
        Physics.Point ftl;
        Physics.Point fbr;
        Physics.Point fbl;
        Physics.Point btr;
        Physics.Point btl;
        Physics.Point bbr;
        Physics.Point bbl;

        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[16];

        private GraphicsDevice theDevice;
        private VertexBuffer myVertexBuffer;

        //bad volume
        private Vector3 min;
        private Vector3 max;
        private float idealVolume;

        public Vector3 getCenter()
        {
            Vector3 ret = Vector3.Zero;
            foreach (Physics.Point p in points)
            {
                ret += p.CurrentPosition;
            }
            return ret / points.Count;
        }

        public DemoCube(Vector3 center, float radius)
        {
            initCube(center, radius);
        }

        private void initCube(Vector3 center, float radius)
        {

            ftr = new Physics.Point(center + new Vector3(radius, radius, radius));
            ftl = new Physics.Point(center + new Vector3(-radius, radius, radius));
            fbr = new Physics.Point(center + new Vector3(radius, -radius, radius));
            fbl = new Physics.Point(center + new Vector3(-radius, -radius, radius));
            btr = new Physics.Point(center + new Vector3(radius, radius, -radius));
            btl = new Physics.Point(center + new Vector3(-radius, radius, -radius));
            bbr = new Physics.Point(center + new Vector3(radius, -radius, -radius));
            bbl = new Physics.Point(center + new Vector3(-radius, -radius, -radius));

            List<Physics.Point> tempList = new List<Physics.Point>();

            tempList.Add(ftr); tempList.Add(ftl); tempList.Add(fbr); tempList.Add(fbl); tempList.Add(btr); tempList.Add(btl); tempList.Add(bbr); tempList.Add(bbl);

            foreach (Physics.Point t in tempList)
            {
                foreach (Physics.Point p in points)
                {
                    //float d = Vector3.Distance(t.CurrentPosition, p.CurrentPosition);
                    //if (d > 0 && d <= 2 * radius)
                    //{
                        springs.Add(new Physics.Spring(t, p, Vector3.Distance(t.CurrentPosition, p.CurrentPosition), 100));
                    //}
                }
                points.Add(t);
            }


            collidables.Add(new TriAA(ftr, fbr, bbr, Color.White));
            collidables.Add(new TriAA(ftr, bbr, btr, Color.White));
            collidables.Add(new TriAA(ftr, btr, btl, Color.White));
            collidables.Add(new TriAA(ftr, btl, ftl, Color.White));
            collidables.Add(new TriAA(ftr, ftl, fbl, Color.White));
            collidables.Add(new TriAA(ftr, fbl, fbr, Color.White));

            collidables.Add(new TriAA(bbl, bbr, fbr, Color.White));
            collidables.Add(new TriAA(bbl, fbr, fbl, Color.White));
            collidables.Add(new TriAA(bbl, fbl, ftl, Color.White));
            collidables.Add(new TriAA(bbl, ftl, btl, Color.White));
            collidables.Add(new TriAA(bbl, btl, btr, Color.White));
            collidables.Add(new TriAA(bbl, btr, bbr, Color.White));


            vertices[0] = new VertexPositionNormalTexture(ftr.CurrentPosition, Vector3.Up, Vector2.Zero);
            vertices[1] = new VertexPositionNormalTexture(fbr.CurrentPosition, Vector3.Up, new Vector2(0f, 1f));
            vertices[2] = new VertexPositionNormalTexture(bbr.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[3] = new VertexPositionNormalTexture(btr.CurrentPosition, Vector3.Up, new Vector2(1f, 0f));
            vertices[4] = new VertexPositionNormalTexture(btl.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[5] = new VertexPositionNormalTexture(ftl.CurrentPosition, Vector3.Up, new Vector2(0f, 1f));
            vertices[6] = new VertexPositionNormalTexture(fbl.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[7] = new VertexPositionNormalTexture(fbr.CurrentPosition, Vector3.Up, new Vector2(1f, 0f));
            vertices[8] = new VertexPositionNormalTexture(bbl.CurrentPosition, Vector3.Up, Vector2.Zero);
            vertices[9] = new VertexPositionNormalTexture(bbr.CurrentPosition, Vector3.Up, new Vector2(0f, 1f));
            vertices[10] = new VertexPositionNormalTexture(fbr.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[11] = new VertexPositionNormalTexture(fbl.CurrentPosition, Vector3.Up, new Vector2(1f, 0f));
            vertices[12] = new VertexPositionNormalTexture(ftl.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[13] = new VertexPositionNormalTexture(btl.CurrentPosition, Vector3.Up, new Vector2(0f, 1f));
            vertices[14] = new VertexPositionNormalTexture(btr.CurrentPosition, Vector3.Up, Vector2.One);
            vertices[15] = new VertexPositionNormalTexture(bbr.CurrentPosition, Vector3.Up, new Vector2(1f, 0f));

        }

        public VertexPositionNormalTexture[] getTriangleVertexes()
        {

            // ----- normals?

            Vector3 normal0;
            Vector3 normal1;
            Vector3 normal2;
            Vector3 normal3;
            Vector3 normal4;
            Vector3 normal5;
            Vector3 normal6;
            Vector3 normal7;
            Vector3 normal8;
            Vector3 normal9;
            Vector3 normal10;
            Vector3 normal11;

            normal0 = new Plane(ftl.CurrentPosition, fbl.CurrentPosition, ftr.CurrentPosition).Normal;
            normal1 = new Plane(fbl.CurrentPosition, fbr.CurrentPosition, ftr.CurrentPosition).Normal;
            normal2 = new Plane(btl.CurrentPosition, btr.CurrentPosition, bbl.CurrentPosition).Normal;
            normal3 = new Plane(bbl.CurrentPosition, btr.CurrentPosition, bbr.CurrentPosition).Normal;
            normal4 = new Plane(ftl.CurrentPosition, btr.CurrentPosition, btl.CurrentPosition).Normal;
            normal5 = new Plane(ftl.CurrentPosition, ftr.CurrentPosition, btr.CurrentPosition).Normal;
            normal6 = new Plane(fbl.CurrentPosition, bbl.CurrentPosition, bbr.CurrentPosition).Normal;
            normal7 = new Plane(fbl.CurrentPosition, bbr.CurrentPosition, fbr.CurrentPosition).Normal;
            normal8 = new Plane(ftl.CurrentPosition, bbl.CurrentPosition, fbl.CurrentPosition).Normal;
            normal9 = new Plane(btl.CurrentPosition, bbl.CurrentPosition, ftl.CurrentPosition).Normal;
            normal10 = new Plane(ftr.CurrentPosition, fbr.CurrentPosition, bbr.CurrentPosition).Normal;
            normal11 = new Plane(btr.CurrentPosition, ftr.CurrentPosition, bbr.CurrentPosition).Normal;

            //sum the normals of each plane that a vector is a part of, then normalize the result
            //this allows for gradual lighting over a plane
            Vector3 normal_ftl = Vector3.Normalize(Vector3.Add(normal9, Vector3.Add(normal4, Vector3.Add(normal5, Vector3.Add(normal0, normal8)))));
            Vector3 normal_fbl = Vector3.Normalize(Vector3.Add(normal6, Vector3.Add(normal7, Vector3.Add(normal1, Vector3.Add(normal0, normal8)))));
            Vector3 normal_ftr = Vector3.Normalize(Vector3.Add(normal5, Vector3.Add(normal11, Vector3.Add(normal10, Vector3.Add(normal0, normal1)))));
            Vector3 normal_fbr = Vector3.Normalize(Vector3.Add(normal7, Vector3.Add(normal1, normal10)));

            Vector3 normal_bbl = Vector3.Normalize(Vector3.Add(normal9, Vector3.Add(normal8, Vector3.Add(normal2, Vector3.Add(normal6, normal3)))));
            Vector3 normal_bbr = Vector3.Normalize(Vector3.Add(normal3, Vector3.Add(normal11, Vector3.Add(normal10, Vector3.Add(normal6, normal7)))));
            Vector3 normal_btl = Vector3.Normalize(Vector3.Add(normal4, Vector3.Add(normal2, normal9)));
            Vector3 normal_btr = Vector3.Normalize(Vector3.Add(normal5, Vector3.Add(normal4, Vector3.Add(normal11, Vector3.Add(normal3, normal2)))));

            // ----- end normals

            vertices[0].Position = ftr.CurrentPosition;
            vertices[0].Normal = normal_ftr;
            vertices[1].Position = fbr.CurrentPosition;
            vertices[1].Normal = normal_fbr;
            vertices[2].Position = bbr.CurrentPosition;
            vertices[2].Normal = normal_bbr;
            vertices[3].Position = btr.CurrentPosition;
            vertices[3].Normal = normal_btr;
            vertices[4].Position = btl.CurrentPosition;
            vertices[4].Normal = normal_btl;
            vertices[5].Position = ftl.CurrentPosition;
            vertices[5].Normal = normal_ftl;
            vertices[6].Position = fbl.CurrentPosition;
            vertices[6].Normal = normal_fbl;
            vertices[7].Position = fbr.CurrentPosition;
            vertices[7].Normal = normal_fbr;
            vertices[8].Position = bbl.CurrentPosition;
            vertices[8].Normal = normal_bbl;
            vertices[9].Position = bbr.CurrentPosition;
            vertices[9].Normal = normal_bbr;
            vertices[10].Position = fbr.CurrentPosition;
            vertices[10].Normal = normal_fbr;
            vertices[11].Position = fbl.CurrentPosition;
            vertices[11].Normal = normal_fbl;
            vertices[12].Position = ftl.CurrentPosition;
            vertices[12].Normal = normal_ftl;
            vertices[13].Position = btl.CurrentPosition;
            vertices[13].Normal = normal_btl;
            vertices[14].Position = btr.CurrentPosition;
            vertices[14].Normal = normal_btr;
            vertices[15].Position = bbr.CurrentPosition;
            vertices[15].Normal = normal_bbr;

            return vertices;
        }

        public VertexBuffer getVertexBuffer()
        {
            myVertexBuffer.SetData<VertexPositionNormalTexture>(getTriangleVertexes());
            return myVertexBuffer;
        }

        public void setGraphicsDevice(GraphicsDevice device)
        {
            theDevice = device;
            myVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.SizeInBytes * 16, BufferUsage.None);
        }

        public int getVertexStride()
        {
            return VertexPositionNormalTexture.SizeInBytes;
        }

        public void DrawMe()
        {
            theDevice.DrawPrimitives(PrimitiveType.TriangleFan, 0, 6);
            theDevice.DrawPrimitives(PrimitiveType.TriangleFan, 8, 6);
        }


        public IEnumerable<Physics.Point> getPoints()
        {
            return points;
        }

        public IEnumerable<Physics.Collidable> getCollidables()
        {
            List<Physics.Collidable> temp = new List<Physics.Collidable>();
            foreach( T t in collidables ) {
                temp.Add( (Physics.Collidable) t );
            }
            return temp;
        }

        public IEnumerable<Drawable> getDrawables()
        {
            List<Drawable> temp = new List<Drawable>();
            foreach (T t in collidables)
            {
                temp.Add((Drawable)t);
            }
            return temp;
        }

        public IEnumerable<Physics.Spring> getSprings()
        {
            return springs;
        }

        public float getVolume()
        {
            float totalVolume = 0;
            Vector3 centerOfCube = getCenter();
            min = centerOfCube;
            max = centerOfCube;

            for (int i = 0; i < vertices.Length; i++)
            {

                if (vertices[i].Position.X < min.X)
                {
                    min.X = vertices[i].Position.X;
                }
                if (vertices[i].Position.Y < min.Y)
                {
                    min.Y = vertices[i].Position.Y;
                }
                if (vertices[i].Position.Z < min.Z)
                {
                    min.Z = vertices[i].Position.Z;
                }

                if (vertices[i].Position.X > max.X)
                {
                    max.X = vertices[i].Position.X;
                }
                if (vertices[i].Position.Y > max.Y)
                {
                    max.Y = vertices[i].Position.Y;
                }
                if (vertices[i].Position.Z > max.Z)
                {
                    max.Z = vertices[i].Position.Z;
                }

            }

            totalVolume = (max.X - min.X) * (max.Y - min.Y) * (max.Z - min.Z);

            return totalVolume;
        }

        public float getIdealVolume()
        {
            return idealVolume;
        }
        public void setIdealVolume( float v )
        {
            idealVolume = v;
        }
    }
}
