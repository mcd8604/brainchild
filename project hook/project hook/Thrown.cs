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
		private float m_CollideDelay = .00001f;
		public float CollideDelay
		{
			get
			{
				return m_CollideDelay;
			}
		}

		private double m_LastCollision = 0;
		private Collidable m_LastCollidable = null;

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
			setDamageEffect("Explosion", "3", "Explosion", 23);
			setDeathEffect("ExplosionBig", "0");
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			m_LastCollision += (float)p_Time.ElapsedGameTime.TotalSeconds;
			//m_TimeOut -= (float)p_Time.ElapsedGameTime.TotalSeconds;
			//if (m_TimeOut < 0)
			//	Enabled = false;

		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Collidable.Factions.Environment)
			{
				if (m_LastCollision >= m_CollideDelay && m_LastCollidable != p_Other)
				{
					try
					{
						TaskStraightAngle tempTask = null;
						TaskRotateToAngle tempRotateTask = null;
						foreach (Task t in Task.getSubTasks())
						{
							if (t is TaskStraightAngle)
							{
								tempTask = (TaskStraightAngle)t;
							}
							else if (t is TaskRotateToAngle)
							{
								tempRotateTask = (TaskRotateToAngle)t;
							}
						}

						if (p_Other.Bound == Collidable.Boundings.Square)
						{
							while (tempTask.AngleDegrees <= 0)
								tempTask.AngleDegrees += 360;

							Vector2 tempVect = this.Position;

							if (Math.Abs(Math.Abs(Center.Y) - Math.Abs(p_Other.Center.Y)) < Math.Abs(Math.Abs(Center.X) - Math.Abs(p_Other.Center.X)))
							{
								if (Center.X < p_Other.Center.X)
									tempVect.X = p_Other.Position.X - (this.Texture.Width + 1);
								else
									tempVect.X = p_Other.Position.X + (p_Other.Texture.Width + 1);

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
							else if (Math.Abs(Math.Abs(Center.Y) - Math.Abs(p_Other.Center.Y)) > Math.Abs(Math.Abs(Center.X) - Math.Abs(p_Other.Center.X)))
							{
								if (Center.Y < p_Other.Center.Y)
									tempVect.Y = p_Other.Position.Y - (this.Texture.Height +  1);
								else
									tempVect.Y = p_Other.Position.Y + (p_Other.Texture.Height + 1);
								
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

							//this.Position = tempVect;
							tempRotateTask.AngleDegrees = tempTask.AngleDegrees;
						}

						m_LastCollision = 0;
						m_LastCollidable = p_Other;
					}
					catch (NullReferenceException)
					{
						Console.WriteLine("Error: Object does not have straight angle task");
					}
				}
			}
			else
			{
				didCollide = p_Other;
			}
		}
	}
}
