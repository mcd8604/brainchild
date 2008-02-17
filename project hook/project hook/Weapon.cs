using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{

	public abstract class Weapon
	{
		#region Variables and Properties

		protected Ship m_Ship;
		public Ship BaseShip
		{
			get
			{ 
				return m_Ship; 
			}
			set
			{
				m_Ship = value;
				if (m_Shots != null)
				{
					foreach (Shot s in m_Shots)
						s.m_Ship = m_Ship;
				}
			}
		}
		// this is how long the delay is between shots in seconds
		protected float m_Delay = 100;
		public float Delay
		{
			get
			{
				return m_Delay;
			}
			set
			{
				m_Delay = value;
			}
		}

		// this will hold the time of the last shot
		protected float m_Cooldown = 0;
		public virtual float Cooldown
		{
			get
			{
				return m_Cooldown;
			}
			set { m_Cooldown += value; }
		}

		protected float m_LastSpeed = float.NaN;

		// this is how fast the shot will travel, pixels per second
		protected float m_Speed = 1000;
		public virtual float Speed
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

		public Shot ShotType
		{
			set
			{
				m_Shots = new List<Shot>();
				for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt((Game.graphics.GraphicsDevice.Viewport.Height * Game.graphics.GraphicsDevice.Viewport.Height) + (Game.graphics.GraphicsDevice.Viewport.Width * Game.graphics.GraphicsDevice.Viewport.Width))) / m_Speed) / m_Delay)); i++)
				{
					Shot tmp = new Shot(value);

					if (m_Ship != null)
					{
						tmp.m_Ship = m_Ship;
					}
					m_Shots.Add(tmp);
				}
			}
		}

		public IList<Shot> m_Shots;
		public int m_NextShot = 0;

		#endregion // End of variables and Properties Region

		public Weapon(){}

		public Weapon(Ship p_Ship) 
		{
			m_Ship = p_Ship;	
		}

		public Weapon(Ship p_Ship, Shot p_Shot, float p_Delay, float p_Speed)
		{
			Delay = p_Delay;
			Speed = p_Speed;
			ShotType = p_Shot;
			m_Ship = p_Ship;
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public virtual void CreateShot()
		{			
			if (m_Cooldown <= 0)
			{
				Fire(m_Ship);
				if (m_Ship.ShootAnimation != null)
				{
					//
					m_Ship.ShootAnimation.Reset();
					m_Ship.ShootAnimation.StartAnimation();
				}
				m_Cooldown = m_Delay;
				m_NextShot = (m_NextShot + 1) % m_Shots.Count;
				//Sound.Play("fire_shot");
			}
		}

		public abstract void Fire(Ship who);

		public void Update(GameTime p_Time)
		{
			m_Cooldown -= (float)p_Time.ElapsedGameTime.TotalSeconds;
		}

		public virtual IList<Shot> changeShotType(Shot type)
		{
			m_Shots.Clear();
			for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt((Game.graphics.GraphicsDevice.Viewport.Height * Game.graphics.GraphicsDevice.Viewport.Height) + (Game.graphics.GraphicsDevice.Viewport.Width * Game.graphics.GraphicsDevice.Viewport.Width))) / m_Speed) / m_Delay)); i++)
			{
				m_Shots.Add(new Shot(type));
			}
			return m_Shots;
		}

		public virtual IList<Shot> getShots()
		{
			return m_Shots;
		}



		public static Task ConvertWeaponTaskTarget(Task task, Sprite toTarget)
		{
			Task retTask = task.copy();
			ChangeWeaponTaskTarget(retTask, toTarget);
			return retTask;
		}
		protected static void ChangeWeaponTaskTarget(Task task, Sprite toTarget)
		{
			if (task is TaskSeekTarget)
			{
				((TaskSeekTarget)task).Target = toTarget;
			}
			else if (task is TaskRotateFaceTarget)
			{
				((TaskRotateFaceTarget)task).Target = toTarget;
			}
			else
			{
				if (task.getSubTasks() != null)
				{
					foreach (Task t in task.getSubTasks())
					{
						if (t != null)
						{
							ChangeWeaponTaskTarget(t, toTarget);
						}
					}
				}
			}
		}

		public static Task ConvertWeaponTaskAngle(Task task, float toAngle)
		{
			Task retTask = task.copy();
			ChangeWeaponTaskAngle(retTask, toAngle);
			return retTask;
		}
		protected static void ChangeWeaponTaskAngle(Task task, float toAngle)
		{
			if (task is TaskStraightAngle)
			{
				((TaskStraightAngle)task).Angle = toAngle;
			}
			else if (task is TaskRotateToAngle)
			{
				((TaskRotateToAngle)task).Angle = toAngle;
			}
			else
			{
				if (task.getSubTasks() != null)
				{
					foreach (Task t in task.getSubTasks())
					{
						if (t != null)
						{
							ChangeWeaponTaskAngle(t, toAngle);
						}
					}
				}
			}
		}
	}
}
