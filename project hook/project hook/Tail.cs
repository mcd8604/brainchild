using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class Tail : Collidable
    {
        private Vector2 m_TailTarget;
        public Vector2 TailTarget
        {
            get
            {
                return m_TailTarget;
            }
            set
            {
                m_TailTarget = value;
            }
        }

        private Ship m_EnemyCaught;
        public Ship EnemyCaught
        {
            get
            {
                return m_EnemyCaught;
            }
            set
            {
                m_EnemyCaught = value;
            }
        }

        private PlayerShip m_PlayerShip;
        public PlayerShip PlayerShip
        {
            get
            {
                return m_PlayerShip;
            }
            set
            {
                m_PlayerShip = value;
            }
        }

		private double m_TailAttackDelay;
		public double TailAttackDelay
		{
			get
			{
				return m_TailAttackDelay;
			}
			set
			{
				m_TailAttackDelay = value;
			}
		}

        private Vector2 m_TailSpeed = new Vector2(0, 0);
		private double m_LastTailAttack = 0;
		private bool m_TailReturned = true;

		public Tail(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
							float p_Degree, float p_Z, Factions p_Faction, int p_Health, Path p_Path, int p_Speed, GameTexture p_DamageEffect, float p_Radius, Ship p_AttachShip, double p_TailAttackDelay)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z, p_Faction, -1,p_Path,p_Speed,p_DamageEffect,p_Radius )
        {
			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Target, p_AttachShip);
			this.PlayerShip = (PlayerShip)p_AttachShip;
			dic.Add(PathStrategy.ValueKeys.Base, this);
			this.Path = new Path(Path.Paths.Tether, dic);
            m_TailTarget = new Vector2(-1, -1);
            m_EnemyCaught = null;
			m_TailAttackDelay = p_TailAttackDelay;
        }

		public void TailAttack(Vector2 p_Target, GameTime p_GameTime)
		{
			if (m_EnemyCaught == null && m_TailReturned && p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastTailAttack + m_TailAttackDelay)
			{
				m_TailTarget = p_Target;
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.End, p_Target);
				dic.Add(PathStrategy.ValueKeys.Start, this.Center);
				dic.Add(PathStrategy.ValueKeys.Base, this);
				dic.Add(PathStrategy.ValueKeys.Speed, 1250f);
				dic.Add(PathStrategy.ValueKeys.Target, this.PlayerShip);
				this.Path = new Path(Path.Paths.TailAttack, dic);
				//gets the current time in milliseconds
				m_LastTailAttack = p_GameTime.TotalGameTime.TotalMilliseconds;
				m_TailReturned = false;
			}
			else if (m_EnemyCaught != null && m_TailReturned && p_GameTime.TotalGameTime.TotalMilliseconds >= m_LastTailAttack + m_TailAttackDelay)
			{
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.End, p_Target);
				dic.Add(PathStrategy.ValueKeys.Start, m_EnemyCaught.Center);
				dic.Add(PathStrategy.ValueKeys.Base, m_EnemyCaught);
				dic.Add(PathStrategy.ValueKeys.Speed, 500f);
				m_EnemyCaught.Path = new Path(Path.Paths.Shot, dic);
				m_EnemyCaught.Faction = Factions.Player;
				m_EnemyCaught = null;
				m_LastTailAttack = p_GameTime.TotalGameTime.TotalMilliseconds;
			}
		}

		public void TailReturned()
		{
			m_TailTarget.X = -1f;
			m_TailTarget.Y = -1f;
			Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Target, this.PlayerShip);
			dic.Add(PathStrategy.ValueKeys.Base, this);
			this.Path = new Path(Path.Paths.Bother, dic);
			m_TailReturned = true;
		}

        public override void RegisterCollision(Collidable p_Other, GameTime p_GameTime)
        {
			//base.RegisterCollision(p_Other);
			if (p_Other.Faction == Factions.Enemy && m_EnemyCaught == null && m_EnemyCaught == null)
			{
				TailReturned();
				Dictionary<PathStrategy.ValueKeys, object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Target, p_Other);
				dic.Add(PathStrategy.ValueKeys.Base, this);
				m_EnemyCaught = (Ship)p_Other;
				m_EnemyCaught.Path = new Path(Path.Paths.TailAttach, dic);
			}
        }

        private void UpdateEnemyCaught()
        {
            if (m_EnemyCaught != null)
            {
                m_EnemyCaught.Position = this.Position;
                m_EnemyCaught.Degree = this.Degree;
            }
        }

        /// <summary>
        /// Calculates the angle that an object should face, given its position, its
        /// target's position, its current angle, and its maximum turning speed.
        /// </summary>
        private static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
        {
            // consider this diagram:
            //         B 
            //        /|
            //      /  |
            //    /    | y
            //  / o    |
            // A--------
            //     x
            // 
            // where A is the position of the object, B is the position of the target,
            // and "o" is the angle that the object should be facing in order to 
            // point at the target. we need to know what o is. using trig, we know that
            //      tan(theta)       = opposite / adjacent
            //      tan(o)           = y / x
            // if we take the arctan of both sides of this equation...
            //      arctan( tan(o) ) = arctan( y / x )
            //      o                = arctan( y / x )
            // so, we can use x and y to find o, our "desiredAngle."
            // x and y are just the differences in position between the two objects.
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            // we'll use the Atan2 function. Atan will calculates the arc tangent of 
            // y / x for us, and has the added benefit that it will use the signs of x
            // and y to determine what cartesian quadrant to put the result in.
            // http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
            float desiredAngle = (float)Math.Atan2(y, x);

            // so now we know where we WANT to be facing, and where we ARE facing...
            // if we weren't constrained by turnSpeed, this would be easy: we'd just 
            // return desiredAngle.
            // instead, we have to calculate how much we WANT to turn, and then make
            // sure that's not more than turnSpeed.

            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Returns the angle expressed in radians between -Pi and Pi.
        /// <param name="radians">the angle to wrap, in radians.</param>
        /// <returns>the input value expressed in radians from -Pi to Pi.</returns>
        /// </summary>
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}
