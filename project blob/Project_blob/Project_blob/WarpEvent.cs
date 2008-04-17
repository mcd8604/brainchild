using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;

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

        public void PerformEvent(GameplayScreen gameRef)
        {
            Vector3[] offsets = new Vector3[gameRef.Player.points.Count];
            for(int i = 0; i < offsets.Length; i++) {
                offsets[i] = gameRef.Player.points[i].CurrentPosition - gameRef.Player.getCenter();
            }
            for(int j = 0; j < offsets.Length; j++) {
                gameRef.Player.points[j].CurrentPosition = offsets[j] + _moveToPos;
                gameRef.Player.points[j].CurrentVelocity = _moveToVel;
            }

            //Vector3 diff = _moveToPos - trigger.ParentBody.getCenter();

            //foreach (Physics.Point p in trigger.ParentBody.getPoints())
            //{
            //    // Physics is going to be changing soon, this will not be neccessary once events as 'handled' post collision.
            //    p.PotientialPosition = p.CurrentPosition + diff;
            //    p.NextPosition = p.CurrentPosition + diff;

            //    p.PotientialVelocity = _moveToVel;
            //    p.NextVelocity = _moveToVel;
            //}

        }
    }
}
