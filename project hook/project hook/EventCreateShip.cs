using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
	class EventCreateShip : Event
	{
		String m_Name;
		public String Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		Vector2 m_StartPos;
		public Vector2 StartPos
		{
			get
			{
				return m_StartPos;
			}
			set
			{
				m_StartPos = value;
			}
		}

		int m_Height;
		public int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}

		int m_Width;
		public int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		GameTexture m_Texture;
		String m_TextureName;
		public String TextureName
		{
			get
			{
				return m_TextureName;
			}
			set
			{
				m_TextureName = value;
			}
		}

		float m_Alpha;
		public float Alpha
		{
			get
			{
				return m_Alpha;
			}
			set
			{
				m_Alpha = value;
			}
		}

		bool m_Visible;
		public bool Visible
		{
			get
			{
				return m_Visible;
			}
			set
			{
				m_Visible = value;
			}
		}

		float m_Degree;
		public float Degree
		{
			get
			{
				return m_Degree;
			}
			set
			{
				m_Degree = value;
			}
		}


		float m_ZBuff;
		String m_ZBuffSpecs;
		public String ZBuffSpecs
		{
			get
			{
				return m_ZBuffSpecs;
			}
			set
			{
				m_ZBuffSpecs = value;
			}
		}

		Collidable.Factions m_Faction;
		String m_FactionString;
		public String Faction
		{
			get
			{
				return m_FactionString;
			}
			set
			{
				m_FactionString = value;
			}
		}

		int m_Health;
		public int Health
		{
			get
			{
				return m_Health;
			}
			set
			{
				m_Health = value;
			}
		}

		int m_Shield;
		public int Shield
		{
			get
			{
				return m_Shield;
			}
			set
			{
				m_Shield = value;
			}
		}

		PathGroup m_PathGroup;
		public PathGroup PathGroup
		{
			get
			{
				return m_PathGroup;
			}
			set
			{
				m_PathGroup = value;
			}
		}

		int m_Speed;
		public int Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		GameTexture m_DamageTexture;
		String m_DamageTextureName;
		public String DamageTextureName
		{
			get
			{
				return m_DamageTextureName;
			}
			set
			{
				m_DamageTextureName = value;
			}
		}

		float m_Radius;
		public float Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}

		public void Load(ContentManager content)
		{
			m_Texture = TextureLibrary.getGameTexture(m_TextureName,"");

			String [] zBuffTokens = m_ZBuffSpecs.Split((' '));
			if(zBuffTokens[0].Equals("ForeGround"))
			{
				if(zBuffTokens[1].Equals("Top"))
					m_ZBuff = Depth.ForeGround.Top;
				if(zBuffTokens[1].Equals("Mid"))
					m_ZBuff = Depth.ForeGround.Mid;
				if(zBuffTokens[1].Equals("Bottom"))
					m_ZBuff = Depth.ForeGround.Bottom;
			}
			else if(zBuffTokens[0].Equals("MidGround"))
			{
				if(zBuffTokens[1].Equals("Top"))
					m_ZBuff = Depth.MidGround.Top;
				if(zBuffTokens[1].Equals("Mid"))
					m_ZBuff = Depth.MidGround.Mid;
				if(zBuffTokens[1].Equals("Bottom"))
					m_ZBuff = Depth.MidGround.Bottom;
			}
			else if(zBuffTokens[0].Equals("BackGround"))
			{
				if(zBuffTokens[1].Equals("Top"))
					m_ZBuff = Depth.BackGround.Top;
				if(zBuffTokens[1].Equals("Mid"))
					m_ZBuff = Depth.BackGround.Mid;
				if(zBuffTokens[1].Equals("Bottom"))
					m_ZBuff = Depth.BackGround.Bottom;
			}
		}
	}

	class EventCreateShipReader : ContentTypeReader<EventCreateShip>
	{
		protected override EventCreateShip Read(
				ContentReader input,
				EventCreateShip existingInstance)
		{
			EventCreateShip m_Event = new EventCreateShip();

			m_Event.Distance = input.ReadInt32();
			m_Event.Name = input.ReadString();
			m_Event.StartPos = input.ReadVector2();
			m_Event.Height = input.ReadInt32();
			m_Event.Width = input.ReadInt32();
			m_Event.TextureName = input.ReadString();
			m_Event.Alpha = input.ReadInt32();
			m_Event.Visible = input.ReadBoolean();
			m_Event.Degree = input.ReadInt32();
			m_Event.ZBuffSpecs = input.ReadString();
			m_Event.Faction = input.ReadString();

			m_Event.Load(input.ContentManager);

			return m_Event;
		}
	}
}