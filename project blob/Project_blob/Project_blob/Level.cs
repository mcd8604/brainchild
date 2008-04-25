using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Project_blob
{
    public static class Level
    {
        private static Dictionary<String, Area> _areas = new Dictionary<string, Area>();
        private static String _name = "";

        public static Dictionary<String, Area> Areas
        {
            get { return _areas; }
            set { _areas = value; }
        }

        public static String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public static Area GetArea(String areaName)
        {
            if (_areas.ContainsKey(areaName))
            {
                return _areas[areaName];
            }
            return null;
        }

        public static void RemoveArea(String areaName)
        {
            if (_areas.ContainsKey(areaName))
            {
                _areas.Remove(areaName);
            }
        }

        public static void AddArea(String areaName, Area area)
        {
            if (!_areas.ContainsKey(areaName))
            {
                _areas.Add(areaName, area);
            }
        }

        public static void SaveLevel(String levelName)
        {
            _name = levelName;
            Stream s = File.Create(System.Environment.CurrentDirectory + "\\Content\\Levels\\" + levelName + ".lev");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, _areas);
            s.Close();
            Console.WriteLine("Level Saved");

#if DEBUG
			Stream s2 = File.Create(System.Environment.CurrentDirectory + "\\..\\..\\..\\..\\Project_blob\\Content\\Levels\\" + levelName + ".lev");
			BinaryFormatter bf2 = new BinaryFormatter();
			bf2.Serialize(s2, _areas);
			s2.Close();
			Console.WriteLine("Level Saved");
#endif
        }

        public static void LoadLevel(String levelName, String effectName)
        {
            _name = levelName;
            Stream s ;
            BinaryFormatter bf;
            try {
                s = File.Open( System.Environment.CurrentDirectory + "\\Content\\Levels\\" + levelName + ".lev", FileMode.Open );
                bf = new BinaryFormatter( );
                _areas = (Dictionary<String, Area>)bf.Deserialize( s );
                s.Close();
                Console.WriteLine( "Level Loaded" );
                foreach( Area area in _areas.Values ) {
                    area.Display.EffectName = effectName;
                }
            } catch( SerializationException se ) {
                string msg = "Could not deserialize: " + levelName + " : " + se;
                Console.WriteLine(msg);
                System.Windows.Forms.MessageBox.Show( msg ); ;
            }
        }
    }
}
