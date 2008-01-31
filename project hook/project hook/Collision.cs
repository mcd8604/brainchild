using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

namespace project_hook
{
	public static class Collision
	{
		private static MultiDictionary<Collidable.Factions, Collidable> sorter = new MultiDictionary<Collidable.Factions, Collidable>(true);

		/// <summary>
		/// Check Everything in lists to see if any collisions have occured.
		/// </summary>
		/// <param name="list">the list to check</param>
		public static void CheckCollisions(params List<Sprite>[] list)
		{
			sorter.Clear();

			foreach (List<Sprite> l in list)
			{

				foreach (Sprite s in l)
				{
					AddToSorter(s);
				}

			}

			List<Collidable.Factions> keys = new List<Collidable.Factions>(sorter.Keys);
			for (int i = 0; i < keys.Count - 1; ++i)
			{
				foreach (Collidable c in sorter[keys[i]])
				{
					for (int j = i + 1; j < keys.Count; ++j)
					{
						foreach (Collidable x in sorter[keys[j]])
						{
							if (DoesIntersect(c, x))
							{
								c.RegisterCollision(x);
								x.RegisterCollision(c);
							}
						}
					}
				}
			}
		}

		private static void AddToSorter(Sprite s)
		{
			if (s.Enabled)
			{
				Collidable temp = s as Collidable;
				if (temp != null && temp.Faction != Collidable.Factions.None)
				{
					sorter.Add(temp.Faction, temp);

				}
				if (s.Parts != null)
				{
					foreach (Sprite x in s.Parts)
					{
						AddToSorter(x);
					}
				}
			}
		}

		private static bool DoesIntersect(Collidable one, Collidable two)
		{
			if (one.Bound == Collidable.Boundings.Circle)
			{
				if (two.Bound == Collidable.Boundings.Circle)
				{
					return DoesIntersectCircles(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Diamond)
				{
					return DoesIntersectCircleDiamond(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Square)
				{
					return DoesIntersectCircleSquare(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Rectangle)
				{
					return DoesIntersectCircleRectangle(one.Center, one.Radius, two.Center, two.Width, two.Height);
				}
				else if (two.Bound == Collidable.Boundings.Triangle)
				{
					// TODO
				}
			}
			else if (one.Bound == Collidable.Boundings.Diamond)
			{
				if (two.Bound == Collidable.Boundings.Circle)
				{
					return DoesIntersectCircleDiamond(two.Center, two.Radius, one.Center, one.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Diamond)
				{
					return DoesIntersectDiamonds(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Square)
				{
					return DoesIntersectDiamondSquare(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Rectangle)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Triangle)
				{
					// TODO
				}
			}
			else if (one.Bound == Collidable.Boundings.Square)
			{
				if (two.Bound == Collidable.Boundings.Circle)
				{
					return DoesIntersectCircleSquare(two.Center, two.Radius, one.Center, one.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Diamond)
				{
					return DoesIntersectDiamondSquare(two.Center, two.Radius, one.Center, one.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Square)
				{
					return DoesIntersectSquares(one.Center, one.Radius, two.Center, two.Radius);
				}
				else if (two.Bound == Collidable.Boundings.Rectangle)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Triangle)
				{
					// TODO
				}
			}
			else if (one.Bound == Collidable.Boundings.Rectangle)
			{
				if (two.Bound == Collidable.Boundings.Circle)
				{
					return DoesIntersectCircleRectangle(two.Center, two.Radius, one.Center, one.Width, one.Height);
				}
				else if (two.Bound == Collidable.Boundings.Diamond)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Square)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Rectangle)
				{
					DoesIntersectRectangles(one.Center, one.Width, one.Height, two.Center, two.Width, two.Height);
				}
				else if (two.Bound == Collidable.Boundings.Triangle)
				{
					// TODO
				}
			}
			else if (one.Bound == Collidable.Boundings.Triangle)
			{
				if (two.Bound == Collidable.Boundings.Circle)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Diamond)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Square)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Rectangle)
				{
					// TODO
				}
				else if (two.Bound == Collidable.Boundings.Triangle)
				{
					// TODO
				}
			}
			throw new NotImplementedException("unimplemented bounds collision exception in CheckCollisions");
		}

		private static bool DoesIntersectCircles(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
		{
			return (Math.Pow((radiustopoint1 + radiustopoint2), 2) > Math.Pow((pos1.X - pos2.X), 2) + Math.Pow((pos1.Y - pos2.Y), 2));
		}
		private static bool DoesIntersectCircleDiamond(Vector2 circ, float circrad, Vector2 diamond, float diamondrad)
		{
			// TODO:
			return DoesIntersectCircles(circ, circrad, diamond, diamondrad);
		}
		private static bool DoesIntersectCircleSquare(Vector2 circ, float circrad, Vector2 square, float squarerad)
		{

			//if (!DoesIntersectSquares(circ, circrad, square, squarerad))
			//{
			//    return false;
			//}

			//if (DoesIntersectCircles(circ, circrad, square, squarerad))
			//{
			//    return true;
			//}

			if (circ.X > square.X + squarerad)
			{

				// 1, 2, 8 

				if (circ.Y > square.Y + squarerad)
				{

					// 2
					float dsquared = Vector2.DistanceSquared(new Vector2(square.X + (squarerad), square.Y + (squarerad)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else if (circ.Y < square.Y - squarerad)
				{

					// 8
					float dsquared = Vector2.DistanceSquared(new Vector2(square.X + (squarerad), square.Y - (squarerad)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else
				{

					// 1
					return (circ.X - circrad < square.X + squarerad);

				}

			}
			else if (circ.X < square.X - squarerad)
			{

				// 4, 5, 6

				if (circ.Y > square.Y + squarerad)
				{

					// 4
					float dsquared = Vector2.DistanceSquared(new Vector2(square.X - (squarerad), square.Y + (squarerad)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else if (circ.Y < square.Y - squarerad)
				{

					// 6
					float dsquared = Vector2.DistanceSquared(new Vector2(square.X - (squarerad), square.Y - (squarerad)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else
				{

					// 5
					return (circ.X + circrad > square.X - squarerad);

				}

			}
			else
			{

				//3, 7

				if (circ.Y > square.Y + squarerad)
				{

					// 3
					return (circ.Y - circrad < square.Y + squarerad);

				}
				else if (circ.Y < square.Y - squarerad)
				{

					// 7
					return (circ.Y + circrad > square.Y - squarerad);

				}
				else
				{
					//throw new ArithmeticException("Math failed");
					// This could occur in very rare cases, when something is spawned on top of something else (bad)
					return true;
				}

			}
		}
		private static bool DoesIntersectDiamonds(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
		{
			return (radiustopoint1 + radiustopoint2 > Math.Abs(pos1.X - pos2.X) + Math.Abs(pos1.Y - pos2.Y));
		}
		private static bool DoesIntersectDiamondSquare(Vector2 diamond, float diamondrad, Vector2 square, float squarerad)
		{
			// TODO:
			return DoesIntersectCircles(diamond, diamondrad, square, squarerad);
		}
		private static bool DoesIntersectSquares(Vector2 pos1, float radiustopoint1, Vector2 pos2, float radiustopoint2)
		{
			if (pos1.X + radiustopoint1 < pos2.X - radiustopoint2)
				return false;
			if (pos2.X + radiustopoint2 < pos1.X - radiustopoint1)
				return false;
			if (pos1.Y + radiustopoint1 < pos2.Y - radiustopoint2)
				return false;
			if (pos2.Y + radiustopoint2 < pos1.Y - radiustopoint1)
				return false;
			return true;
		}

		private static bool DoesIntersectCircleRectangle(Vector2 circ, float circrad, Vector2 rect, float width, float height)
		{

			if (circ.X > rect.X + (width * 0.5f))
			{

				// 1, 2, 8 

				if (circ.Y > rect.Y + (height * 0.5f))
				{

					// 2
					float dsquared = Vector2.DistanceSquared(new Vector2(rect.X + (width * 0.5f), rect.Y + (height * 0.5f)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else if (circ.Y < rect.Y - (height * 0.5f))
				{

					// 8
					float dsquared = Vector2.DistanceSquared(new Vector2(rect.X + (width * 0.5f), rect.Y - (height * 0.5f)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else
				{

					// 1
					return (circ.X - circrad < rect.X + (width * 0.5f));

				}

			}
			else if (circ.X < rect.X - (width * 0.5f))
			{

				// 4, 5, 6

				if (circ.Y > rect.Y + (height * 0.5f))
				{

					// 4
					float dsquared = Vector2.DistanceSquared(new Vector2(rect.X - (width * 0.5f), rect.Y + (height * 0.5f)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else if (circ.Y < rect.Y - (height * 0.5f))
				{

					// 6
					float dsquared = Vector2.DistanceSquared(new Vector2(rect.X - (width * 0.5f), rect.Y - (height * 0.5f)), circ);
					return dsquared < Math.Pow(circrad, 2);

				}
				else
				{

					// 5
					return (circ.X + circrad > rect.X - (width * 0.5f));

				}

			}
			else
			{

				//3, 7

				if (circ.Y > rect.Y + (height * 0.5f))
				{

					// 3
					return (circ.Y - circrad < rect.Y + (height * 0.5f));

				}
				else if (circ.Y < rect.Y - (height * 0.5f))
				{

					// 7
					return (circ.Y + circrad > rect.Y - (height * 0.5f));

				}
				else
				{
					//throw new ArithmeticException("Math failed");
					// This could occur in very rare cases, when something is spawned on top of something else (bad)
					return true;
				}

			}
		}

		private static bool DoesIntersectRectangles(Vector2 pos1, float width1, float height1, Vector2 pos2, float width2, float height2)
		{
			if (pos1.X + (width1 * 0.5f) < pos2.X - (width2 * 0.5f))
				return false;
			if (pos2.X + (width2 * 0.5f) < pos1.X - (width1 * 0.5f))
				return false;
			if (pos1.Y + (height1 * 0.5f) < pos2.Y - (height2 * 0.5f))
				return false;
			if (pos2.Y + (height2 * 0.5f) < pos1.Y - (height1 * 0.5f))
				return false;
			return true;
		}

		public static Vector2 GetMinNonCollidingCenter(Collidable movable, Collidable solid)
		{

			if (movable.Bound == Collidable.Boundings.Circle)
			{
				if (solid.Bound == Collidable.Boundings.Circle)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Diamond)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Square)
				{

					Vector2 temp = movable.Center;

					if (movable.Center.X > solid.Center.X + solid.Radius)
					{

						// 1, 2, 8 

						if (movable.Center.Y > solid.Center.Y + solid.Radius)
						{

							// 2
							Vector2 cornerpoint = new Vector2(solid.Center.X + (solid.Radius), solid.Center.Y + (solid.Radius));
							//float d = Vector2.Distance(cornerpoint, movable.Center);
							Vector2 dir = movable.Center - cornerpoint;
							dir.Normalize();
							temp = cornerpoint + Vector2.Multiply(dir, movable.Radius);

						}
						else if (movable.Center.Y < solid.Center.Y - solid.Radius)
						{

							// 8
							Vector2 cornerpoint = new Vector2(solid.Center.X + (solid.Radius), solid.Center.Y - (solid.Radius));
							//float d = Vector2.Distance(cornerpoint, movable.Center);
							Vector2 dir = movable.Center - cornerpoint;
							dir.Normalize();
							temp = cornerpoint + Vector2.Multiply(dir, movable.Radius);

						}
						else
						{

							// 1
							//return (circ.X - circrad < square.X + squarerad);
							temp.X = solid.Center.X + solid.Radius + movable.Radius;

						}

					}
					else if (movable.Center.X < solid.Center.X - solid.Radius)
					{

						// 4, 5, 6

						if (movable.Center.Y > solid.Center.Y + solid.Radius)
						{

							// 4
							Vector2 cornerpoint = new Vector2(solid.Center.X - (solid.Radius), solid.Center.Y + (solid.Radius));
							//float d = Vector2.Distance(cornerpoint, movable.Center);
							Vector2 dir = movable.Center - cornerpoint;
							dir.Normalize();
							temp = cornerpoint + Vector2.Multiply(dir, movable.Radius);

						}
						else if (movable.Center.Y < solid.Center.Y - solid.Radius)
						{

							// 6
							Vector2 cornerpoint = new Vector2(solid.Center.X - (solid.Radius), solid.Center.Y - (solid.Radius));
							//float d = Vector2.Distance(cornerpoint, movable.Center);
							Vector2 dir = movable.Center - cornerpoint;
							dir.Normalize();
							temp = cornerpoint + Vector2.Multiply(dir, movable.Radius);

						}
						else
						{

							// 5
							//return (circ.X + circrad > square.X - squarerad);
							temp.X = solid.Center.X - (solid.Radius + movable.Radius);

						}

					}
					else
					{

						//3, 7

						if (movable.Center.Y > solid.Center.Y + solid.Radius)
						{

							// 3
							//return (circ.Y - circrad < square.Y + squarerad);
							temp.Y = solid.Center.Y + solid.Radius + movable.Radius;

						}
						else if (movable.Center.Y < solid.Center.Y - solid.Radius)
						{

							// 7
							//return (circ.Y + circrad > square.Y - squarerad);
							temp.Y = solid.Center.Y - (solid.Radius + movable.Radius);

						}
						else
						{
							// This would only occur if the circle was inside the square..
							//throw new ArithmeticException("Math failed");
							temp.Y = solid.Center.Y + solid.Radius + movable.Radius;

						}

					}


					return temp;



				}
			}
			else if (movable.Bound == Collidable.Boundings.Diamond)
			{
				if (solid.Bound == Collidable.Boundings.Circle)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Diamond)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Square)
				{

				}
			}
			else if (movable.Bound == Collidable.Boundings.Square)
			{
				if (solid.Bound == Collidable.Boundings.Circle)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Diamond)
				{

				}
				else if (solid.Bound == Collidable.Boundings.Square)
				{
					Vector2 temp = movable.Center;
					float deltaX = movable.Center.X - solid.Center.X;
					float deltaY = movable.Center.Y - solid.Center.Y;

					if (Math.Abs(deltaX) > Math.Abs(deltaY))
					{

						// horizontal collision
						if (temp.X > solid.Center.X)
						{
							temp.X = solid.Center.X + movable.Radius + solid.Radius;
						}
						else
						{
							temp.X = solid.Center.X - (movable.Radius + solid.Radius);
						}

					}
					else
					{

						// vertical
						if (temp.Y > solid.Center.Y)
						{
							temp.Y = solid.Center.Y + movable.Radius + solid.Radius;
						}
						else
						{
							temp.Y = solid.Center.Y - (movable.Radius + solid.Radius);
						}

					}
					return temp;
				}
			}
			throw new NotImplementedException("unimplemented bounds collision exception in GetMinNonCollidingCenter");


		}


		/// <summary>
		/// attach visual collision bounds to every collidable in the list, that doesn't already have bounds on it.
		/// </summary>
		/// <param name="list"></param>
		public static void DevEnableCollisionDisplay(params List<Sprite>[] list)
		{

			TextureLibrary.LoadTexture("debugcirc");
			TextureLibrary.LoadTexture("debugdiamond");
			TextureLibrary.LoadTexture("debugsquare");

			foreach (List<Sprite> l in list)
			{

				foreach (Sprite s in l)
				{
					checkAndAddCollisionDisplay( s );
				}
			}
		}

		private static void checkAndAddCollisionDisplay(Sprite s)
		{
			bool skip = false;
			Collidable temp = s as Collidable;
			if (temp != null)
			{
				if (temp.Parts != null)
				{
					foreach (Sprite x in temp.Parts)
					{
						if (x.Name == "bound")
						{
							skip = true;
							x.Height = (int)(temp.Radius * 2f);
							x.Width = (int)(temp.Radius * 2f);
							if (temp.Faction != Collidable.Factions.None)
							{
								x.Enabled = true;
							}
							else
							{
								x.Enabled = false;
							}
						}
						else
						{
							checkAndAddCollisionDisplay(x);
						}
					}
				}

				if (!skip)
				{
					if (temp.Faction != Collidable.Factions.None)
					{
						if (temp.Bound == Collidable.Boundings.Circle)
						{
							Sprite sprite = new Sprite("bound", temp.Position, (int)(temp.Radius * 2), (int)(temp.Radius * 2), TextureLibrary.getGameTexture("debugcirc", ""));
							sprite.Task = new TaskAttach(temp);
							temp.attachSpritePart(sprite);
						}
						else if (temp.Bound == Collidable.Boundings.Diamond)
						{
							Sprite sprite = new Sprite("bound", temp.Position, (int)(temp.Radius * 2), (int)(temp.Radius * 2), TextureLibrary.getGameTexture("debugdiamond", ""));
							sprite.Task = new TaskAttach(temp);
							temp.attachSpritePart(sprite);
						}
						else if (temp.Bound == Collidable.Boundings.Square)
						{
							Sprite sprite = new Sprite("bound", temp.Position, (int)(temp.Radius * 2), (int)(temp.Radius * 2), TextureLibrary.getGameTexture("debugsquare", ""));
							sprite.Task = new TaskAttach(temp);
							temp.attachSpritePart(sprite);
						}
						else if (temp.Bound == Collidable.Boundings.Rectangle)
						{
							Sprite sprite = new Sprite("bound", temp.Position, temp.Height, temp.Width, TextureLibrary.getGameTexture("debugsquare", ""));
							sprite.Task = new TaskAttach(temp);
							temp.attachSpritePart(sprite);
						}
					}
				}
			}
		}


		public static void DevDisableCollisionDisplay(params List<Sprite>[] list)
		{
			foreach (List<Sprite> l in list)
			{
				foreach (Sprite s in l)
				{
					checkAndRemoveCollisionDisplay(s);
				}
			}
		}

		private static void checkAndRemoveCollisionDisplay(Sprite s)
		{
			Collidable temp = s as Collidable;
			if (temp != null)
			{

				if (temp.Parts != null)
				{
					foreach (Sprite x in temp.Parts)
					{
						if (x.Name == "bound")
						{
							x.Enabled = false;
						}
						else
						{
							checkAndRemoveCollisionDisplay(x);
						}
					}
				}
			}
		}

		public static void SelfTest()
		{

			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
			timer.Start();

			// small, disparate objects

			List<Collidable> list = new List<Collidable>();

			Collidable smallcirc = new Collidable();
			smallcirc.Bound = Collidable.Boundings.Circle;
			smallcirc.Center = new Vector2(100, 100);
			smallcirc.Radius = 1;
			list.Add(smallcirc);

			Collidable smalldia = new Collidable();
			smalldia.Bound = Collidable.Boundings.Diamond;
			smalldia.Center = new Vector2(200, 200);
			smalldia.Radius = 1;
			list.Add(smalldia);

			Collidable smallsqa = new Collidable();
			smallsqa.Bound = Collidable.Boundings.Square;
			smallsqa.Center = new Vector2(300, 300);
			smallsqa.Radius = 1;
			list.Add(smallsqa);

			foreach (Collidable one in list)
			{
				foreach (Collidable two in list)
				{
					if (one != two)
					{
						System.Diagnostics.Debug.Assert(!DoesIntersect(one, two), "Collision Self Test Failure", one.ToString() + " " + two.ToString());
					}
				}
			}


			// large, overlapping objects

			list.Clear();

			Collidable largecirc = new Collidable();
			largecirc.Bound = Collidable.Boundings.Circle;
			largecirc.Center = new Vector2(1, 1);
			largecirc.Radius = 100;
			list.Add(largecirc);

			Collidable largedia = new Collidable();
			largedia.Bound = Collidable.Boundings.Diamond;
			largedia.Center = new Vector2(2, 2);
			largedia.Radius = 100;
			list.Add(largedia);

			Collidable largesqa = new Collidable();
			largesqa.Bound = Collidable.Boundings.Square;
			largesqa.Center = new Vector2(3, 3);
			largesqa.Radius = 100;
			list.Add(largesqa);

			foreach (Collidable one in list)
			{
				foreach (Collidable two in list)
				{
					if (one != two)
					{
						System.Diagnostics.Debug.Assert(DoesIntersect(one, two), "Collision Self Test Failure", one.ToString() + " " + two.ToString());
					}
				}
			}


			Collidable Circle = new Collidable();
			Circle.Bound = Collidable.Boundings.Circle;
			Circle.Radius = 10;

			Collidable Square = new Collidable();
			Square.Bound = Collidable.Boundings.Square;
			Square.Radius = 10;


			Circle.Center = new Vector2(0, 0);
			Square.Center = new Vector2(20, 0);

			System.Diagnostics.Debug.Assert(!DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());

			Circle.Center = new Vector2(0, 0);
			Square.Center = new Vector2(17, 17);

			System.Diagnostics.Debug.Assert(DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			Vector2 result = GetMinNonCollidingCenter(Circle, Square);
			System.Diagnostics.Debug.Assert(result != new Vector2(0, 0), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			

			Circle.Center = new Vector2(0, 0);
			Square.Center = new Vector2(18, 18);

			System.Diagnostics.Debug.Assert(!DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());


			Circle.Center = new Vector2(0, 0);
			Square.Center = new Vector2(15, 0);
			System.Diagnostics.Debug.Assert(DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			result = GetMinNonCollidingCenter(Circle, Square);
			System.Diagnostics.Debug.Assert(result == new Vector2(-5,0), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());


			Circle.Center = new Vector2(0, 0);
			Square.Center = new Vector2(15, 15);
			System.Diagnostics.Debug.Assert(DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			result = GetMinNonCollidingCenter(Circle, Square);
			System.Diagnostics.Debug.Assert(result != new Vector2(0, 0), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			
			


			// Circle Square just barely colliding

			Circle.Center = new Vector2(964.6991f, -61.95084f);
			Circle.Radius = 8;
			Square.Center = new Vector2(992f, -31.06205f);
			Square.Radius = 32;
			System.Diagnostics.Debug.Assert(DoesIntersect(Circle, Square), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());
			result = GetMinNonCollidingCenter(Circle, Square);
			System.Diagnostics.Debug.Assert(result != new Vector2(0, 0), "Collision Self Test Failure", Circle.ToString() + " " + Square.ToString());


			timer.Stop();

			Console.WriteLine("Collision Self Test Completed in " + timer.Elapsed.TotalMilliseconds.ToString() + " Milliseconds.");

		}

	}
}