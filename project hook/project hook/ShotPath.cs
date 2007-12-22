using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class ShotPath:PathStrategy
    {
           Shot m_Base;
        Vector2 m_Start;
        Vector2 m_End;
        Vector2 m_Delta;

        public ShotPath(Dictionary<ValueKeys, Object> p_Values)
			:base(p_Values){

            m_Base = (Shot)m_Values[ValueKeys.Base];
            m_Start = (Vector2)m_Values[ValueKeys.Start];
            m_End = (Vector2)m_Values[ValueKeys.End];
            
            m_Delta = new Vector2(0.0f,m_Base.Speed);
            

		}

        public override void CalculateMovement(GameTime p_gameTime)
        {
            Vector2 t_Cur = m_Base.Center;
            if (t_Cur.Y >= m_End.Y)
             {
                t_Cur.Y -= m_Delta.Y ;
                m_Base.Center = t_Cur;
                
            }           
            else
            {
                m_Done = true;
            }


        }


    }
}
