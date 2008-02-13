using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class BloodCellGenerator
	{
		float m_BloodCellDelay = 3.0f;
		float m_LastRelease = 0;
		Random m_RanX = new Random();
		List<Sprite> m_BloodCellList = new List<Sprite>();
		public List<Sprite> BloodCells
		{
			get
			{
				return m_BloodCellList;
			}
		}

		public BloodCellGenerator()
		{
			for (int i = 0; i < 4; i++)
			{
				Collidable t_Blood = new Collidable("BloodCell", new Vector2(0, 0), 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"),
					0.75f, true, -MathHelper.PiOver2, Depth.BackGroundLayer.Upper, Collidable.Factions.Blood, 100, 25);
				t_Blood.setAnimation("bloodcell", 60);
				t_Blood.BlendMode = SpriteBlendMode.Additive;
				t_Blood.Enabled = false;
				m_BloodCellList.Add(t_Blood);
			}
		}

		public void Update(GameTime p_Time)
		{
			foreach (Sprite s in m_BloodCellList)
			{
				s.Update(p_Time);
			}

			m_LastRelease += (float)p_Time.ElapsedGameTime.TotalSeconds;
			if (m_LastRelease >= m_BloodCellDelay)
			{
				foreach (Collidable c in m_BloodCellList)
				{
					if (c.ToBeRemoved || !c.Enabled)
					{
						c.Enabled = true;
						c.ToBeRemoved = false;

						c.Center = new Vector2(m_RanX.Next(0, Game.graphics.GraphicsDevice.Viewport.Width), 0);
						c.Task = new TaskStraightVelocity(new Vector2(0, 100));
						c.Faction = Collidable.Factions.Blood;
						c.Rotation = -MathHelper.PiOver2;
						c.Health = c.MaxHealth;
						c.Animation.StartAnimation();

						m_LastRelease = 0;
						return;
					}
				}
			}
		}

		public void Draw(SpriteBatch p_SpriteBatch)
		{
			foreach (Sprite c in m_BloodCellList)
			{
				c.Draw(p_SpriteBatch);
			}
		}
	}
}