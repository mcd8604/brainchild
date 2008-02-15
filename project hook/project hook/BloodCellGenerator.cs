using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class BloodCellGenerator : Sprite
	{
		float m_BloodCellDelay = 3.0f;
		public float BloodCellDelay
		{
			get
			{
				return m_BloodCellDelay;
			}
			set
			{
				m_BloodCellDelay = value;
			}
		}

		int m_BloodCellMinSpawnRange = 100;
		public int MinSpawnRange
		{
			get
			{
				return m_BloodCellMinSpawnRange;
			}
			set
			{
				m_BloodCellMinSpawnRange = value;
			}
		}

		int m_BloodCellMaxSpawnRange = Game.graphics.GraphicsDevice.Viewport.Width - 100;
		public int MaxSpawnRange
		{
			get
			{
				return m_BloodCellMaxSpawnRange;
			}
			set
			{
				m_BloodCellMaxSpawnRange = value;
			}
		}

		float m_LastRelease = 0;

		public BloodCellGenerator(int p_BloodCellMax)
		{
			for (int i = 0; i < p_BloodCellMax; i++)
			{
				Collidable t_Blood = new Collidable(
#if !FINAL
					"BloodCell",
#endif
					Vector2.Zero, 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"), 0.75f, true, -MathHelper.PiOver2, Depth.BackGroundLayer.Blood, Collidable.Factions.Blood, 100, 25);
				t_Blood.setAnimation("bloodcell", 60);
				t_Blood.Enabled = false;
				attachSpritePart(t_Blood);
			}
		}

		public override void Update(GameTime p_Time)
		{
			foreach (Sprite s in m_Parts)
			{
				s.Update(p_Time);
			}

			m_LastRelease += (float)p_Time.ElapsedGameTime.TotalSeconds;
			if (m_LastRelease >= m_BloodCellDelay)
			{
				foreach (Collidable c in m_Parts)
				{
					if (c.ToBeRemoved || !c.Enabled)
					{
						c.Enabled = true;
						c.ToBeRemoved = false;

						c.Center = new Vector2(Game.Random.Next(m_BloodCellMinSpawnRange, m_BloodCellMaxSpawnRange), 0);
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
	}
}