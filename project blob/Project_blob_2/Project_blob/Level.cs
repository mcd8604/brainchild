using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Project_blob {
	public static class Level {
		private static Dictionary<string, Area> _areas = new Dictionary<string, Area>();
		private static string _name = string.Empty;

		public static Area CurrentArea;

		public static Dictionary<string, Area> Areas {
			get { return _areas; }
			set { _areas = value; }
		}

		public static string Name {
			get { return _name; }
			set { _name = value; }
		}

		public static Area GetArea(string areaName) {
			if (_areas.ContainsKey(areaName)) {
				return _areas[areaName];
			}
			return null;
		}

		public static string GetAreaName(Area p_Area) {
			foreach (string s in _areas.Keys) {
				if (_areas[s] == p_Area)
					return s;
			}
			return null;
		}

		public static string[] GetAreaNames() {
			string[] areaNames = new string[_areas.Count];
			_areas.Keys.CopyTo(areaNames, 0);
			return areaNames;
		}

		public static void RemoveArea(string areaName) {
			if (_areas.ContainsKey(areaName)) {
				_areas.Remove(areaName);
			}
		}

		public static void AddArea(string areaName, Area area) {
			if (!_areas.ContainsKey(areaName)) {
				_areas.Add(areaName, area);
			}
		}

		public static void SaveLevel(string levelName) {
			_name = levelName;
			Stream s = File.Create(System.Environment.CurrentDirectory + "\\Content\\Levels\\" + levelName + ".lev");
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(s, _areas);
			s.Close();
			Log.Out.WriteLine("Level Saved");

#if DEBUG
			Stream s2 = File.Create(System.Environment.CurrentDirectory + "\\..\\..\\..\\..\\Project_blob\\Content\\Levels\\" + levelName + ".lev");
			BinaryFormatter bf2 = new BinaryFormatter();
			bf2.Serialize(s2, _areas);
			s2.Close();
			Log.Out.WriteLine("Level Saved");
#endif
		}

		public static void LoadLevel(string levelName, string effectName) {
			_name = levelName;
			Stream s;
			BinaryFormatter bf;
			try {
				s = File.Open(System.Environment.CurrentDirectory + "\\Content\\Levels\\" + levelName + ".lev", FileMode.Open);
				bf = new BinaryFormatter();
				_areas = (Dictionary<string, Area>)bf.Deserialize(s);
				s.Close();
#if DEBUG
				Log.Out.WriteLine("Level Loaded");
#endif

				// Conversion Loop
				/*foreach (Area area in _areas.Values)
				{
					Dictionary<string, Drawable> temp = new Dictionary<string, Drawable>();
					foreach ( string key in area.Drawables.Keys )
					{
						Drawable d = area.Drawables[key];
						if ( d is StaticModel )
						{
							StaticModel sm = d as StaticModel;
							if ( sm.TextureName.Equals( "speed" ) )
							{
								temp[key] = new StaticModelSpeed( sm ); ;
							}
						}
					}
					foreach ( string key in temp.Keys )
					{
						area.Drawables[key] = temp[key];
					}
				}*/
			} catch (SerializationException se) {
				string msg = "Could not deserialize: " + levelName + " : " + se;
				Log.Out.WriteLine(msg);
				System.Windows.Forms.MessageBox.Show(msg);
			}
		}
	}
}
