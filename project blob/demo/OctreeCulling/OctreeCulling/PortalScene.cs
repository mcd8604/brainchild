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

		private Sector _currSector;
		public Sector CurrSector
		{
			get { return _currSector; }
			set { _currSector = value; }
		}

        public PortalScene()
        {
            _sectors = new SortedDictionary<int, Sector>();
        }

        public void DistributeDrawableObjects(List<SceneObject> scene)
        {
            foreach (SceneObject obj in scene)
            {
				foreach (int sectorNum in obj.SectorNums)
				{
					if(_sectors.ContainsKey(sectorNum))
					//if (_sectors.ContainsKey(obj.SectorNums))
					{
						_sectors[sectorNum].AddObjectToSector(obj);
						//_sectors[obj.SectorNums].AddObjectToSector(obj);
					}
					else
					{
						_sectors.Add(sectorNum, new Sector());
						_sectors[sectorNum].AddObjectToSector(obj);
						//_sectors.Add(obj.SectorNums, new Sector());
						//_sectors[obj.SectorNums].AddObjectToSector(obj);
					}
				}

                _worldBox = BoundingBox.CreateMerged(_worldBox, obj.GetBoundingBoxTransformed());
            }
        }

        public void DistributePortals(List<Portal> portals)
        {
            foreach (Portal portal in portals)
            {
                foreach (int sectorNum in portal.ConnectedSectors)
                {
                    if(_sectors.ContainsKey(sectorNum))
                    {
                        _sectors[sectorNum].AddPortalToSector(portal);
                    }
                    else
                    {
                        _sectors.Add(sectorNum, new Sector());
                        _sectors[sectorNum].AddPortalToSector(portal);
                    }
                }
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

			//_currSector.DrawVisible(gameTime);
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
