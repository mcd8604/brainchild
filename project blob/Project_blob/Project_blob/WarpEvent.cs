using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Project_blob
{
    [Serializable]
    public class WarpEvent : EventTrigger
    {
        private Vector3 _moveToPos;
        private Vector3 _moveToVel;

        public WarpEvent(float xPos, float yPos, float zPos, float xVel, float yVel, float zVel)
        {
            _moveToPos = new Vector3( xPos, yPos, zPos );
            _moveToVel = new Vector3( xVel, yVel, zVel );
        }

        public void PerformEvent()
        {
        }
    }
}
