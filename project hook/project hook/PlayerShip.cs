using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;

/*
 * Description: This class contains the information regarding the player ship,
 *              i.e. power up effects
 * 
 * TODO:
 *  
 */

namespace project_hook
{
	public class PlayerShip : Ship
	{

		public PlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Shield, p_DamageEffect, p_Radius)
		{

		}

		//stores the current power up effects
		List<Effect> m_EffectsArray = new List<Effect>();

		/*
		 * Description: Adds a new power up effec to the player ship.
		 */
		public void AddEffect(Effect p_Effect)
		{
			m_EffectsArray.Add(p_Effect);
		}

		/*
		 * Description: This removes any effects that have expired.
		 */
		public void CheckEffects()
		{
			foreach (Effect i_Effect in m_EffectsArray)
			{
				if (i_Effect.Expired())
					m_EffectsArray.Remove(i_Effect);
			}
		}
	}
}
