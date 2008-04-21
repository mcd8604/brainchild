using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class PortalScene
    {
        //private List<Sector> _sectors;
        //public List<Sector> Sectors
        //{
        //    get { return _sectors; }
        //    set { _sectors = value; }
        //}

        private SortedDictionary<int, Sector> _sectors;
        public SortedDictionary<int, Sector> Sectors
        {
            get { return _sectors; }
            set { _sectors = value; }
        }

        private BoundingBox _worldBox;
        public BoundingBox WorldBox
        {
            get { return _worldBox; }
            set { _worldBox = value; }
        }

        public PortalScene()
        {
            _sectors = new SortedDictionary<int, Sector>();
        }

        public void Distribute(List<SceneObject> scene)
        {
            foreach (SceneObject obj in scene)
            {
                if (_sectors.ContainsKey(obj.SectorNum))
                {
                    _sectors[obj.SectorNum].AddObjectToSector(obj);
                }
                else
                {
                    _sectors.Add(obj.SectorNum, new Sector());
                    _sectors[obj.SectorNum].AddObjectToSector(obj);
                }

                _worldBox = BoundingBox.CreateMerged(_worldBox, obj.GetBoundingBoxTransformed());
            }
        }

        public void DrawVisible(GameTime gameTime)
        {
            //Test if camera is within the worldbox before checking all the sectors

            foreach (Sector sector in _sectors.Values)
            {
                //if(sector.ContainerBox.Contains(CameraManager.getSingleton.ActiveCamera.Position) == ContainmentType.Contains)
                if(sector.ContainerBox.Contains(CameraManager.getSingleton.GetCamera("test").Position) == ContainmentType.Contains)
                {
                    sector.DrawVisible(gameTime);
                    break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Sector sector in _sectors.Values)
            {
                sector.Draw(gameTime);
            }
        }
    }
}
