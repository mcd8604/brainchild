using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Thrown : Collidable
	{
		private float m_TimeOut = 6f;
		private float m_CollideDelay = .005f;
		public float CollideDelay
		{
			get
			{
				return m_CollideDelay;
			}
		}

		private double m_LastCollision = 0;

		public Thrown(Collidable p_Collidable)
		{
			this.Alpha = p_Collidable.Alpha;
			this.Animation = p_Collidable.Animation;
			this.BlendMode = p_Collidable.BlendMode;
			this.Bound = p_Collidable.Bound;
			this.CollisonEffect = p_Collidable.CollisonEffect;
			this.Color = p_Collidable.Color;
			this.Damage = p_Collidable.Damage;
			this.DamageEffect = p_Collidable.DamageEffect;
			this.Enabled = p_Collidable.Enabled;
			this.Faction = p_Collidable.Faction;
			this.Health = p_Collidable.Health;
			this.Height = p_Collidable.Height;
			this.MaxHealth = p_Collidable.MaxHealth;
			this.Name = p_Collidable.Name;
			this.Position = p_Collidable.Position;
			this.Radius = p_Collidable.Radius;
			this.Rotation = p_Collidable.Rotation;
			this.RotationDegrees = p_Collidable.RotationDegrees;
			this.Scale = p_Collidable.Scale;
			this.Task = p_Collidable.Task;
			this.Texture = p_Collidable.Texture;
			this.ToBeRemoved = p_Collidable.ToBeRemoved;
			this.Transparency = p_Collidable.Transparency;
			this.Width = p_Collidable.Width;
			this.Z = p_Collidable.Z;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			m_LastCollision += (float)p_Time.ElapsedGameTime.TotalSeconds;
			//m_TimeOut -= (float)p_Time.ElapsedGameTime.TotalSeconds;
			//if (m_TimeOut < 0)
			//	this.Enabled = false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Collidable.Factions.Environment)
			{
				if (m_LastCollision >= m_CollideDelay)
				{
					try
					{
						TaskStraightAngle tempTask = null;
						foreach (Task t in this.Task.getSubTasks())
						{
							if (t is TaskStraightAngle)
								tempTask = (TaskStraightAngle)t;
						}

						if (p_Other.Bound == Collidable.Boundings.Square)
						{
							while (tempTask.AngleDegrees <= 0)
								tempTask.AngleDegrees += 360;

							if (Math.Abs(Math.Abs(this.Center.Y) - Math.Abs(p_Other.Center.Y)) < Math.Abs(Math.Abs(this.Center.X) - Math.Abs(p_Other.Center.X)))
							{
								if (tempTask.AngleDegrees > 270 && tempTask.AngleDegrees < 360)
								{
									tempTask.AngleDegrees = 270 - MathHelper.Distance(tempTask.AngleDegrees, 270f);
								}
								else if (tempTask.AngleDegrees > 0 && tempTask.AngleDegrees < 90)
								{
									tempTask.AngleDegrees = 90 + MathHelper.Distance(tempTask.AngleDegrees, 90f);
								}
								else if (tempTask.AngleDegrees > 90 && tempTask.AngleDegrees <= 180)
								{
									tempTask.AngleDegrees = 90 - MathHelper.Distance(tempTask.AngleDegrees, 90f);
								}
								else if (tempTask.AngleDegrees > 180 && tempTask.AngleDegrees < 270)
								{
									tempTask.AngleDegrees = 270 + MathHelper.Distance(tempTask.AngleDegrees, 270f);
								}
								//else if (tempTask.AngleDegrees > 270 && tempTask.AngleDegrees < 360)
								//{
								//	tempTask.AngleDegrees = 180 + MathHelper.Distance(tempTask.AngleDegrees, 270f);
								//}
							}
							else if (Math.Abs(Math.Abs(this.Center.Y) - Math.Abs(p_Other.Center.Y)) > Math.Abs(Math.Abs(this.Center.X) - Math.Abs(p_Other.Center.X)))
							{
								if (tempTask.AngleDegrees > 270 && tempTask.AngleDegrees < 360)
								{
									tempTask.AngleDegrees = 0 + MathHelper.Distance(tempTask.AngleDegrees, 360f);
								}
								else if (tempTask.AngleDegrees > 0 && tempTask.AngleDegrees < 90)
								{
									tempTask.AngleDegrees = 360 - MathHelper.Distance(tempTask.AngleDegrees, 0f);
								}
								else if (tempTask.AngleDegrees >= 90 && tempTask.AngleDegrees < 180)
								{
									tempTask.AngleDegrees = 180 + MathHelper.Distance(tempTask.AngleDegrees, 180f);
								}
								else if (tempTask.AngleDegrees > 180 && tempTask.AngleDegrees <= 270)
								{
									tempTask.AngleDegrees = 180 - MathHelper.Distance(tempTask.AngleDegrees, 180f);
								}
							}
						}

						m_LastCollision = 0;
						//this.Task = tempTask;
					}
					catch (NullReferenceException)
					{
						Console.WriteLine("Error: Object does not have straight angle task");
					}
				}
			}
			else
			{
				this.didCollide = p_Other;
			}
		}
	}
}
