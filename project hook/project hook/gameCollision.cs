using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	/*
	 * This class will check collisions between all objects given to it by world
	 * This is just the start, will will work on it later
	 */
	class gameCollision
	{
		public gameCollision(Array colArray)
		{
			Array collidables = colArray;
		}

		public virtual void checkCollisions()
		{
			//for (int i = 0; i < collidables.length; i++)
			{
				//Collidable firstCol = collidables[i];
				//for (int j = 0; j < collidables.length; j++)
				{
                  //  Collidable secondCol = collidables[j];
                   // if (firstCol.faction != secondCol.faction && Intersection.DoesIntersectDiamond(firstCol.Position + firstCol.Center, firstCol.Height / 2.5f, secondCol.Position + secondCol.Center, secondCol.Height / 2.5f))
					{
                        //Here be collisions
					}
				}
			}
		}


	}
}
/*
  * Class: Game Collision
  * Author: Matt
  * Date Created: 12/19/2007
  * 
  * Change Log:
  *     12/19/2007 - Started class 
  *     12/29/2007 - Continued
  */