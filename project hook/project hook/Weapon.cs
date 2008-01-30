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

		// this is how long the delay is between shots in seconds
		protected float m_Delay = 100;
		public virtual float Delay
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
				for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt(Math.Pow(Game.graphics.GraphicsDevice.Viewport.Height, 2) + Math.Pow(Game.graphics.GraphicsDevice.Viewport.Width, 2))) / m_Speed) / m_Delay)); i++)
				{
					m_Shots.Add(new Shot(value));
				}
			}
		}

		protected IList<Shot> m_Shots;
		protected int m_NextShot = 0;

		#endregion // End of variables and Properties Region

		public Weapon() { }

		public Weapon(Shot p_Shot, float p_Delay, float p_Speed)
		{
			Delay = p_Delay;
			Speed = p_Speed;
			ShotType = p_Shot;
		}

		//this function will create a Shot at the current location
		// Only a single shot? 
		public virtual void CreateShot(Ship who)
		{
			if (m_Cooldown <= 0)
			{
				Fire(who);
				m_Cooldown = m_Delay;
				m_NextShot = (m_NextShot + 1) % m_Shots.Count;
			}
		}

		public abstract void Fire(Ship who);

		public void Update(GameTime p_Time)
		{
			m_Cooldown -= (float)p_Time.ElapsedGameTime.TotalSeconds;
		}

		public IList<Shot> changeShotType(Shot type)
		{
			m_Shots.Clear();
			for (int i = 0; i < (int)Math.Ceiling((((Math.Sqrt(Math.Pow(Game.graphics.GraphicsDevice.Viewport.Height, 2) + Math.Pow(Game.graphics.GraphicsDevice.Viewport.Width, 2))) / m_Speed) / m_Delay)); i++)
			{
				m_Shots.Add(new Shot(type));
			}
			return m_Shots;
		}

		public IList<Shot> getShots()
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
			if ( task is TaskStraightAngle ) {
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