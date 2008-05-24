using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
{
	internal class HighScores
	{
		internal const int count = 10;
		private const string filename = "score.dat";

		private static readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

		private Dictionary<string, Score[]> Map = new Dictionary<string, Score[]>();

		internal HighScores()
		{
			if (System.IO.File.Exists(filename))
			{
				try
				{
					load();
					return;
				}
				catch (Exception e)
				{
					Log.Out.WriteLine("Error loading from High Score file: " + e);
				}
			}
		}

		internal void addScore(string area, string name, float time)
		{
			if (!Map.ContainsKey(area))
			{
				fill(area);
			}
			Score[] List = Map[area];
			Score toAdd = new Score(name, time);
			Score temp;
			bool changed = false;

			for (int i = 0; i < count; ++i)
			{
				if (List[i] == null)
				{
					List[i] = toAdd;
					changed = true;
					break;
				}
				if (toAdd.Time < List[i].Time)
				{
					temp = List[i];
					List[i] = toAdd;
					toAdd = temp;
					changed = true;
				}
			}

			if (changed)
			{
				save();
			}
		}

		internal Score[] getScores(string area)
		{
			if (!Map.ContainsKey(area))
			{
				fill(area);
			}
			return Map[area];
		}

		private void fill(string NewArea)
		{
			Map[NewArea] = new Score[count];
			for (int i = 0; i < count; ++i)
			{
				Map[NewArea][i] = null;
			}
		}

		private void load()
		{
			using (System.IO.FileStream input = System.IO.File.OpenRead(filename))
			{
				Map = (Dictionary<string, Score[]>)b.Deserialize(input);
			}
		}

		private void save()
		{
			using (System.IO.FileStream output = System.IO.File.OpenWrite(filename))
			{
				b.Serialize(output, Map);
			}
		}
	}

	[Serializable]
	internal class Score
	{
		internal string Name;
		internal float Time;
		internal Score(string name, float time)
		{
			Name = name;
			Time = time;
		}
		public override string ToString()
		{
			return Name + ": " + Time;
		}
	}
}
