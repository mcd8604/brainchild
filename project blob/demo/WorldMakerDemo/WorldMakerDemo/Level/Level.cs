using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorldMakerDemo.Level
{
    public static class Level
    {
        private static List<Area> _areas;
        private static String _name;

        public static List<Area> Areas
        {
            get { return _areas; }
            set { _areas = value; }
        }

        public static String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public static void SaveLevel(String levelName)
        {
            _name = levelName;
            Stream s = File.Create(levelName + ".lev");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, _areas);
            s.Close();
        }

        public static void LoadLevel(String levelName)
        {
            _name = levelName;
            Stream s = File.Open(levelName + ".lev", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            _areas = (List<Area>)bf.Deserialize(s);
            Console.WriteLine("Level Loaded");
            s.Close();
        }
    }
}
