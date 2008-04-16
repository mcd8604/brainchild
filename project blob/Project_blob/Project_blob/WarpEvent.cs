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

        public void PerformEvent(GameState.GameplayScreen gameRef)
        {
            Vector3[] offsets = new Vector3[gameRef.Player.points.Count];
            for(int i = 0; i < offsets.Length; i++) {
                offsets[i] = gameRef.Player.points[i].CurrentPosition - gameRef.Player.getCenter();
            }
            for(int j = 0; j < offsets.Length; j++) {
                gameRef.Player.points[j].NextPosition = offsets[j] + _moveToPos;
                //gameRef.Player.points[j].PotientialVelocity = _moveToVel;
            }
        }
    }
}
