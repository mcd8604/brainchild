using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using Engine;

namespace Project_blob
{
    class PortalScene
    {
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

		private int _currSector = 0;
        public int CurrSector
		{
			get { return _currSector; }
			set { _currSector = value; }
		}

		private int _prevSector = 0;
		public int PrevSector
		{
			get { return _prevSector; }
			set { _prevSector = value; }
		}

		private int _currentRecursiveSector = 0;
		public int CurrentRecursiveSector
		{
			get { return _currentRecursiveSector; }
			set { _currentRecursiveSector = value; }
		}

		private int _previousRecursiveSector = 0;
		public int PreviousRecursiveSector
		{
			get { return _previousRecursiveSector; }
			set { _previousRecursiveSector = value; }
		}

        public PortalScene()
        {
            _sectors = new SortedDictionary<int, Sector>();
        }

        public void DistributeDrawableObjects(List<Drawable> scene)
        {
            foreach (StaticModel obj in scene)
            {
                if (false)//(obj.Rooms != null)
                {
                    if (obj.Rooms.Count != 0)
                    {
                        foreach (int sectorNum in obj.Rooms)
                        {
                            if (_sectors.ContainsKey(sectorNum))
                            {
                                _sectors[sectorNum].AddObjectToSector(obj);
                            }
                            else
                            {
                                _sectors.Add(sectorNum, new Sector(sectorNum));
                                _sectors[sectorNum].AddObjectToSector(obj);
                            }
                        }
                    }
                    else
                    {
                        if (_sectors.ContainsKey(1))
                        {
                            _sectors[1].AddObjectToSector(obj);
                        }
                        else
                        {
                            _sectors.Add(1, new Sector(1));
                            _sectors[1].AddObjectToSector(obj);
                        }
                    }
                }
                else
                {
                    if (_sectors.ContainsKey(1))
                    {
                        _sectors[1].AddObjectToSector(obj);
                    }
                    else
                    {
                        _sectors.Add(1, new Sector(1));
                        _sectors[1].AddObjectToSector(obj);
                    }
                }

                //_worldBox = BoundingBox.CreateMerged(_worldBox, obj.GetBoundingBox());
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
                        _sectors.Add(sectorNum, new Sector(sectorNum));
                        _sectors[sectorNum].AddPortalToSector(portal);
                    }
                }
            }
        }

        public void DrawVisible(GameTime gameTime)
        {
            //Test if camera is within the worldbox before checking all the sectors
            //foreach (Sector sector in _sectors.Values)
            //{
            //    //if(sector.ContainerBox.Contains(CameraManager.getSingleton.ActiveCamera.Position) == ContainmentType.Contains)
            //    if(sector.ContainerBox.Contains(CameraManager.getSingleton.GetCamera("test").Position) == ContainmentType.Contains)
            //    {
            //        sector.DrawVisible(gameTime);
            //        break;
            //    }
            //}

            
            if (_sectors[_currSector].ContainerBox.Contains(
                CameraManager.getSingleton.ActiveCamera.Position) == ContainmentType.Disjoint)
            {
                foreach(KeyValuePair<int, Sector> kvp in _sectors)
                {
                    if (kvp.Value.ContainerBox.Contains(CameraManager.getSingleton.ActiveCamera.Position) == ContainmentType.Contains)
                    {
						_prevSector = _currSector;
                        _currSector = kvp.Key;
                        break;
                    }
                }
            }
			_previousRecursiveSector = -1;
			_currentRecursiveSector = _currSector;
			_sectors[_currSector].DrawVisible(gameTime);
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
