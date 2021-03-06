using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class LevelHandler
	{
		private Dictionary<int, List<Event>> m_Events;
		internal Dictionary<int, List<Event>> Events
		{
			get
			{
				return m_Events;
			}
			set
			{
				m_Events = value;
			}
		}

		private World m_Game;
		internal World Game
		{
			get
			{
				return m_Game;
			}
			set
			{
				m_Game = value;
			}
		}

		private int m_EventDistance = 0;

		internal LevelHandler()
		{
		}
		internal LevelHandler(Dictionary<int, List<Event>> p_Events, World p_Game)
		{
			Events = p_Events;
			Game = p_Game;
		}

		internal void CheckEvents(float p_UpToDistance)
		{
			int CurrentPosition = Convert.ToInt32(Math.Ceiling(p_UpToDistance));

			for (; m_EventDistance < CurrentPosition; m_EventDistance++)
			{
				if (m_Events.ContainsKey(m_EventDistance))
				{
					foreach (Event e in m_Events[m_EventDistance])
					{
						switch (e.Type)
						{
							case Event.Types.CreateSprite:
								CreateSprite(e.Sprite);
								break;
							case Event.Types.CreateSprites:
								foreach (Sprite s in e.Sprites)
								{
									CreateSprite(s);
								}
								break;
							case Event.Types.ChangeFile:
								ChangeFile(e.FileName);
								break;
							case Event.Types.ChangeSpeed:
								ChangeSpeed(e.Speed);
								break;
							case Event.Types.LoadBMP:
								LoadBMP(e.FileName);
								break;
							case Event.Types.PleaseLoadBMP:
								PleaseLoadBMP(e.FileName);
								break;
							case Event.Types.EndGame:
								EndGame();
								break;
						}
					}
				}
			}
		}

		internal void EndGame()
		{
			if (!World.PlayerDead)
			{
				m_Game.changeState(World.GameState.Won);
				World.DestroyWorld = true;
				//m_Game.State = World.GameState.Won;
				Menus.setCurrentMenu(Menus.MenuScreens.Credits);
			}
		}

		internal void resetLevel()
		{
			m_EventDistance = 0;
		}

		internal void CreateSprite(Sprite p_Sprite)
		{
			//add the sprtie to the sprtiebatch in the game class
			//Sprite.addSprite(p_Collidable);
			m_Game.AddSprite(p_Sprite);
		}

		internal void ChangeSpeed(int p_Speed)
		{
			//change the speed in the game file
			//World.Position.setSpeed(p_Speed);
			m_Game.ChangeSpeed(p_Speed);
		}

		internal void ChangeFile(String p_FileName)
		{
			//change the fileName in the levelReader class
			m_Game.ChangeFile(p_FileName);
			//tell it to read the file
		}
		internal void LoadBMP(String p_FileName)
		{
			m_Game.LoadBMP(p_FileName);
		}
		internal void PleaseLoadBMP(String p_FileName)
		{
			m_Game.PleaseLoadBMP(p_FileName);
		}
	}
}
