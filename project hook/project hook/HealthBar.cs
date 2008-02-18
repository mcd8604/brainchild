using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class HealthBar : Sprite
	{
		//the ship who's shield and health will be displayed
		Collidable m_Target;
		public Collidable Target
		{
			set
			{
				m_Target = value;
				bg.Texture = m_Target.Texture;
			}
		}

		//The poistion the shields and heatlh will be drawn
		Sprite shields;
		Sprite blackS;
		Sprite bg;

		Sprite health;
		Sprite blackH;

		WeaponUpgradeBar wp;

		int width;
		int height;
		Vector2 offset;
		//  bool attach = false;

		public HealthBar(Collidable p_Ship)
		{
			m_Target = p_Ship;
			//  attach = true;
			TaskAttach t = new TaskAttach(m_Target);
			this.Task = t;

			width = m_Target.Width / 2;

			height = m_Target.Height / 25;
			if (height < 5)
			{
				height = 5;
			}

			offset = new Vector2(0, m_Target.Radius / 2);

			ini(p_Ship.Width, p_Ship.Height);
			setBars();

			m_Target.attachSpritePart(this);
		}

		public HealthBar(Collidable p_Ship, Vector2 pos, int p_Width, int p_Height, int p_BGWidth, int p_BGHeight)
		{

			m_Target = p_Ship;
			width = p_Width;
			height = p_Height;
			Position = pos;
#if !FINAL
			Name = "Health Bar - " + m_Target.Name;
#endif
			ini(p_BGWidth, p_BGHeight);
			setBars();

		}

		private void ini(int p_BGWidth, int p_BGHeight)
		{
			//if(m_Target is Ship && ((Ship)m_Target).MaxShield > 0)
			shields = new Sprite(
#if !FINAL
				"HealthBar",
#endif
				new Vector2(this.Center.X, this.Center.Y), height, width, TextureLibrary.getGameTexture("shieldBar", ""), 200, true, 0.0f, Depth.HUDLayer.Foreground);

			if (m_Target is PlayerShip)
			{
				wp = new WeaponUpgradeBar((PlayerShip)m_Target, new Vector2(this.Center.X, this.Center.Y - height * 2), width, height);
				wp.Enabled = true;
			}

			health = new Sprite(
#if !FINAL
				"HealthBar",
#endif
				new Vector2(this.Center.X, this.Center.Y - height), height, width, TextureLibrary.getGameTexture("healthBar", ""), 200, true, 0.0f, Depth.HUDLayer.Foreground);

			blackS = new Sprite();
			blackS.Texture = TextureLibrary.getGameTexture("black", "");
			blackS.Z = Depth.HUDLayer.Midground;
			blackS.Enabled = true;
			blackS.Width = width;
			blackS.Height = height;
			blackS.Alpha = 255;

			blackH = new Sprite();
			blackH.Texture = TextureLibrary.getGameTexture("black", "");
			blackH.Z = Depth.HUDLayer.Midground;
			blackH.Enabled = true;
			blackH.Width = width;
			blackH.Height = height;
			blackH.Alpha = 255;

			bg = new Sprite();
			bg.Texture = m_Target.Texture;
			bg.Center = this.Center;
			bg.Transparency = .5f;
			bg.Z = Depth.HUDLayer.Background;
			bg.Enabled = true;
			bg.Width = p_BGWidth;
			bg.Height = p_BGHeight;
			bg.Transparency = .75f;

		}

		private void setBars()
		{
			Vector2 c;
			if (m_Target is Ship)
			{


				Ship t_Ship = (Ship)m_Target;
				if (t_Ship.MaxShield > 0)
				{
					if (!shields.Enabled)
					{
						shields.Enabled = true;
						blackS.Enabled = true;
					}

					c = shields.Center;
					c.X = this.Center.X;
					c.Y = this.Center.Y + offset.Y;
					shields.Center = c;

					shields.Width = (int)(width * t_Ship.Shield / t_Ship.MaxShield);
					blackS.Center = shields.Center;
					shields.Position = blackS.Position;
				}
				else
				{
					shields.Enabled = false;
					blackS.Enabled = false;
				}
			}
			else
			{
				shields.Enabled = false;
				blackS.Enabled = false;
			}

			health.Width = (int)(width * m_Target.Health / m_Target.MaxHealth);
			c = health.Center;
			c.X = this.Center.X;// -m_Target.Radius / 2;
			c.Y = this.Center.Y + offset.Y + height;// + height; ;
			health.Center = c;



			blackH.Center = health.Center;
			health.Position = blackH.Position;

			if (wp != null)
			{
				Vector2 wpP = blackH.Position;
				wpP.Y += offset.Y + height;
				wp.Position = wpP;
			}

		}


		internal override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (wp != null)
			{
				wp.Update(p_Time);
			}
			setBars();


		}

		internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			if (health != null)
			{
				health.Draw(p_SpriteBatch);
				blackH.Draw(p_SpriteBatch);
			}

			if (shields != null)
			{
				shields.Draw(p_SpriteBatch);
				blackS.Draw(p_SpriteBatch);
			}

			if (wp != null)
			{
				wp.Draw(p_SpriteBatch);
			}
			bg.Draw(p_SpriteBatch);
		}
	}
}
