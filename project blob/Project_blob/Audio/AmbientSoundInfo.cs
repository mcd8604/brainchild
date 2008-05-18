using System;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace Audio
{

	[Serializable]
	public class AmbientSoundInfo
	{

		private string name;
		[TypeConverter(typeof(TypeConverterAudio))]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private Vector3 position;
		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}

		public override string ToString()
		{
			return "Ambient Sound";
		}

	}
}
