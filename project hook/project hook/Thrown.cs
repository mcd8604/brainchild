using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Thrown : Collidable
	{
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
			this.StartPosition = p_Collidable.StartPosition;
			this.Task = p_Collidable.Task;
			this.Texture = p_Collidable.Texture;
			this.ToBeRemoved = p_Collidable.ToBeRemoved;
			this.Transparency = p_Collidable.Transparency;
			this.Width = p_Collidable.Width;
			this.Z = p_Collidable.Z;
		}

		public virtual void RegisterCollision(Collidable p_Other)
		{
			TaskStraightAngle tempTask = (TaskStraightAngle)this.Task;

			if (p_Other.Bound == Collidable.Boundings.Square)
			{
				while (tempTask.Angle < 0)
					tempTask.Angle += 360;

				if (this.Position.X < p_Other.Position.X)
				{
					if (tempTask.Angle < 90 && tempTask.Angle > 0)
					{
						tempTask.Angle = 180 - MathHelper.Distance(tempTask.Angle, 0f);
					}
					else if (tempTask.Angle > 270 && tempTask.Angle < 360)
					{
						tempTask.Angle = 180 + MathHelper.Distance(tempTask.Angle, 270f);
					}
				}
			}

			this.Task = tempTask;
		}
	}
}
