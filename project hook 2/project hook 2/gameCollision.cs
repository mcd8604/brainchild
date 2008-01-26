using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/*
	 * This class will check collisions between all objects given to it by world
	 * This is just the start, will will work on it later
	 */
	[Obsolete]
	class gameCollision
	{
		public gameCollision(Array colArray)
		{
			Array collidables = colArray;
		}

		[Obsolete]
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

		[Obsolete]
		public static void QuickCheckCollision(List<Sprite> temp, GameTime gameTime, Player player)
		{

			for (int a = 0; a < temp.Count; a++)
			{
				if (temp[a] is Collidable)
				{

					Collidable item = (Collidable)temp[a];


					//temp.Remove( item ); // Todo: fix this later
					for (int b = 0; b < temp.Count; b++)
					{
						if (temp[b] is Collidable)
						{

							Collidable item2 = (Collidable)temp[b];
							if (item.Faction != item2.Faction)
							{
								if (item != item2 && Intersection.DoesIntersectDiamond(item.Position + item.Center, item.Height / 2.5f, item2.Position + item2.Center, item2.Height / 2.5f))
								{

									//explosion.Position = (item.Position + item2.Position) / 2;
									item.RegisterCollision(item2);
									item2.RegisterCollision(item);
									player.Score.RegisterHit(gameTime);
								}
							}
						}
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