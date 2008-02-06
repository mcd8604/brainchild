using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Thrown : Collidable
	{
		//private float m_TimeOut = 6f;
		//private float m_CollideDelay = .00001f;
		//public float CollideDelay
		//{
		//    get
		//    {
		//        return m_CollideDelay;
		//    }
		//}

		//private double m_LastCollision = 0;

		public Thrown(Collidable p_Collidable)
		{
			if (p_Collidable.Animation != null)
			{
				Animation = new VisualEffect(p_Collidable.Animation.Name, this, p_Collidable.Animation.FramesPerSecond);
			}
			BlendMode = p_Collidable.BlendMode;
			Bound = p_Collidable.Bound;
			//CollisonEffect = p_Collidable.CollisonEffect;
			Color = p_Collidable.Color;
			Damage = p_Collidable.Damage;
			//DamageEffect = p_Collidable.DamageEffect;
			Enabled = p_Collidable.Enabled;
			Faction = p_Collidable.Faction;
			Height = p_Collidable.Height;
			MaxHealth = p_Collidable.MaxHealth;
			Health = p_Collidable.Health;
			Name = p_Collidable.Name;
			Position = p_Collidable.Position;
			Radius = p_Collidable.Radius;
			Rotation = p_Collidable.Rotation;
			RotationDegrees = p_Collidable.RotationDegrees;
			Scale = p_Collidable.Scale;
			Task = p_Collidable.Task;
			Texture = p_Collidable.Texture;
			ToBeRemoved = p_Collidable.ToBeRemoved;
			Transparency = p_Collidable.Transparency;
			Width = p_Collidable.Width;
			Z = p_Collidable.Z;

			// TODO
			// temp
			//setDamageEffect("Explosion", "3", "Explosion", 23);
			//setDeathEffect("ExplosionBig", "0");
			//m_DamageEffect = p_Collidable.m_DamageEffect;
			//m_DeathEffect = p_Collidable.m_DeathEffect;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			//m_LastCollision += (float)p_Time.ElapsedGameTime.TotalSeconds;
			//m_TimeOut -= (float)p_Time.ElapsedGameTime.TotalSeconds;
			//if (m_TimeOut < 0)
			//	Enabled = false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Collidable.Factions.Environment)
			{
				//if (m_LastCollision >= m_CollideDelay)
				//{
					checkTaskAngle(Task, p_Other);
						
					//m_LastCollision = 0;
				//}
			}
			else
			{
				didCollide.Enqueue(p_Other);
			}
		}

		private void checkTaskAngle(Task t, Collidable p_Other)
		{
			if (t is TaskStraightAngle)
			{
				changeTaskAngle( (TaskStraightAngle)t, p_Other);
			}
			//else if (t is TaskRotateToAngle)
			//{
			//    changeTaskAngle((TaskRotateToAngle)t, p_Other);
			//}
			else
			{
				if (t.getSubTasks() != null)
				{
					foreach (Task t2 in t.getSubTasks())
					{
						checkTaskAngle(t2, p_Other);
					}
				}
			}
		}
		
		private void changeTaskAngle(Task t, Collidable p_Other)
		{
			TaskStraightAngle task = null;

			if (t is TaskStraightAngle)
			{
				task = t as TaskStraightAngle;
			}
			//else if (t is TaskRotateToAngle)
			//{
			//
			//}

			if (p_Other.Bound == Collidable.Boundings.Square || p_Other.Bound == Collidable.Boundings.Rectangle)
			{
				while (task.AngleDegrees <= 0)
					task.AngleDegrees += 360;

				Vector2 tempVect = this.Position;

				if (Math.Abs(Math.Abs(Center.Y) - Math.Abs(p_Other.Center.Y)) < Math.Abs(Math.Abs(Center.X) - Math.Abs(p_Other.Center.X)))
				{
					if (((Center.X < p_Other.Center.X && (task.AngleDegrees < 90 || task.AngleDegrees > 270))) ||
						(Center.X > p_Other.Center.X && (task.AngleDegrees > 90 && task.AngleDegrees < 270)))
					{

						if (task.AngleDegrees > 270 && task.AngleDegrees < 360)
						{
							task.AngleDegrees = 270 - MathHelper.Distance(task.AngleDegrees, 270f);
						}
						else if (task.AngleDegrees > 0 && task.AngleDegrees < 90)
						{
							task.AngleDegrees = 90 + MathHelper.Distance(task.AngleDegrees, 90f);
						}
						else if (task.AngleDegrees > 90 && task.AngleDegrees <= 180)
						{
							task.AngleDegrees = 90 - MathHelper.Distance(task.AngleDegrees, 90f);
						}
						else if (task.AngleDegrees > 180 && task.AngleDegrees < 270)
						{
							task.AngleDegrees = 270 + MathHelper.Distance(task.AngleDegrees, 270f);
						}
					}
					else
					{
					}
				}
				else if (Math.Abs(Math.Abs(Center.Y) - Math.Abs(p_Other.Center.Y)) > Math.Abs(Math.Abs(Center.X) - Math.Abs(p_Other.Center.X)))
				{

					if (((Center.Y < p_Other.Center.Y && task.AngleDegrees < 180)) ||
						(Center.Y > p_Other.Center.Y && task.AngleDegrees > 180))
					{
						if (task.AngleDegrees > 270 && task.AngleDegrees < 360)
						{
							task.AngleDegrees = 0 + MathHelper.Distance(task.AngleDegrees, 360f);
						}
						else if (task.AngleDegrees > 0 && task.AngleDegrees < 90)
						{
							task.AngleDegrees = 360 - MathHelper.Distance(task.AngleDegrees, 0f);
						}
						else if (task.AngleDegrees >= 90 && task.AngleDegrees < 180)
						{
							task.AngleDegrees = 180 + MathHelper.Distance(task.AngleDegrees, 180f);
						}
						else if (task.AngleDegrees > 180 && task.AngleDegrees <= 270)
						{
							task.AngleDegrees = 180 - MathHelper.Distance(task.AngleDegrees, 180f);
						}
					}
				}
			}

		}

	}
}
