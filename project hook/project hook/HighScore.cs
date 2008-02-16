using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class HighScore
	{
		public const int size = 10;
		private int[] List = new int[size];
		public int[] Scores
		{
			get
			{
				return List;
			}
		}

		private const string filename = "score.dat";

		public HighScore()
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
					Game.Out.WriteLine("Error loading from High Score file: " + e);
				}
			}

			List[0] = 50000;
			List[1] = 40000;
			List[2] = 30000;
			List[3] = 20000;
			List[4] = 10000;
			for (int i = 5; i < size; i++)
			{
				List[i] = 20000 - (i * 2000);
			}

		}

		public void addScore(int Score)
		{

			int toAdd = Score;
			int temp;

			for (int i = 0; i < size; i++)
			{
				if (toAdd > List[i])
				{
					temp = List[i];
					List[i] = toAdd;
					toAdd = temp;
				}
			}

			if (toAdd != Score)
			{
				save();
			}
		}

		public void load()
		{
			// TODO, I/O Errors, etc
			using (System.IO.TextReader reader = new System.IO.StreamReader(filename))
			{
				for (int i = 0; i < size; i++)
				{
					List[i] = int.Parse(reader.ReadLine());
				}
				if (!check(int.Parse(reader.ReadLine())))
				{
					throw new Exception("High Score file invalid.");
				}
			}
		}

		public void save()
		{
			using (System.IO.TextWriter writer = new System.IO.StreamWriter(filename))
			{
				for (int i = 0; i < size; i++)
				{
					writer.WriteLine(List[i].ToString());
				}
				writer.WriteLine(gen().ToString());
			}
		}


		const int a = 12345;
		const int b = 123456;

		private bool check(int v)
		{
			return gen() == v;
		}
		private int gen()
		{
			int v = a;
			for (int i = 0; i < size; i++)
			{
				v += List[i];
			}
			return v % b;
		}

	}
}
