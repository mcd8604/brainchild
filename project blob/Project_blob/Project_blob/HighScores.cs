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

		private Score[] List = new Score[count];

		internal Score[] Scores
		{
			get
			{
				return List;
			}
		}

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

			for (int i = 0; i < count; ++i)
			{
				List[i] = new Score("Nobody", i * 1000f);
			}
		}

		internal void addScore(string name, float time)
		{
			Score toAdd = new Score(name, time);
			Score temp;
			bool changed = false;

			for (int i = 0; i < count; ++i)
			{
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

		internal void load()
		{
			using (System.IO.FileStream input = System.IO.File.OpenRead(filename))
			{
				List = (Score[])b.Deserialize(input);
			}
		}

		internal void save()
		{
			using ( System.IO.FileStream output = System.IO.File.OpenWrite(filename) )
			{
				b.Serialize(output, List);
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
	}
}
