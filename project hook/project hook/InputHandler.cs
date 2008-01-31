using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Wintellect.PowerCollections;

namespace project_hook
{

	/// <summary>
	/// List of Actions to which keys may be bound and states queried.
	/// </summary>
	public enum Actions { Up, Down, Left, Right, ShipPrimary, ShipSecondary, TailPrimary, TailSecondary, Pause, MenuAccept, MenuBack };

	public enum MouseButtons { Left, Middle, Right, XButton1, XButton2 };

	public enum GamePadButtons { Up, Down, Left, Right, A, B, Back, LeftShoulder, LeftStick, RightShoulder, RightStick, Start, X, Y };

	public static class InputHandler
	{

		private static KeyboardState lastKeyboardState = new KeyboardState();
		private static KeyboardState thisKeyboardState = new KeyboardState();
		private static MultiDictionary<Actions, Keys> KeyboardMap = new MultiDictionary<Actions, Keys>(false);

		private static MouseState lastMouseState = new MouseState();
		private static MouseState thisMouseState = new MouseState();
		private static MultiDictionary<Actions, MouseButtons> MouseMap = new MultiDictionary<Actions, MouseButtons>(false);

		private static GamePadState lastGamePadState = new GamePadState();
		private static GamePadState thisGamePadState = new GamePadState();
		private static MultiDictionary<Actions, GamePadButtons> GamePadMap = new MultiDictionary<Actions, GamePadButtons>(false);

		public static void LoadDefaultBindings()
		{

			// Default mappings for now
			KeyboardMap.Add(Actions.Down, Keys.S);
			KeyboardMap.Add(Actions.Down, Keys.Down);
			KeyboardMap.Add(Actions.Left, Keys.A);
			KeyboardMap.Add(Actions.Left, Keys.Left);
			KeyboardMap.Add(Actions.Right, Keys.D);
			KeyboardMap.Add(Actions.Right, Keys.Right);
			KeyboardMap.Add(Actions.Up, Keys.W);
			KeyboardMap.Add(Actions.Up, Keys.Up);
			KeyboardMap.Add(Actions.ShipPrimary, Keys.Space);
			KeyboardMap.Add(Actions.ShipPrimary, Keys.RightControl);
			KeyboardMap.Add(Actions.ShipSecondary, Keys.LeftShift);
			KeyboardMap.Add(Actions.ShipSecondary, Keys.RightShift);
			KeyboardMap.Add(Actions.Pause, Keys.Escape);
			KeyboardMap.Add(Actions.MenuAccept, Keys.Space);
			KeyboardMap.Add(Actions.MenuAccept, Keys.Enter);
			KeyboardMap.Add(Actions.MenuBack, Keys.Back);
			KeyboardMap.Add(Actions.MenuBack, Keys.Escape);

			MouseMap.Add(Actions.TailPrimary, MouseButtons.Left);
			MouseMap.Add(Actions.TailSecondary, MouseButtons.Right);
			MouseMap.Add(Actions.MenuAccept, MouseButtons.Left);
			MouseMap.Add(Actions.MenuBack, MouseButtons.Right);

			GamePadMap.Add(Actions.Down, GamePadButtons.Down);
			GamePadMap.Add(Actions.Left, GamePadButtons.Left);
			GamePadMap.Add(Actions.Right, GamePadButtons.Right);
			GamePadMap.Add(Actions.Up, GamePadButtons.Up);
			GamePadMap.Add(Actions.ShipPrimary, GamePadButtons.A);
			GamePadMap.Add(Actions.ShipSecondary, GamePadButtons.B);
			GamePadMap.Add(Actions.Pause, GamePadButtons.Start);
			GamePadMap.Add(Actions.MenuAccept, GamePadButtons.Start);
			GamePadMap.Add(Actions.MenuBack, GamePadButtons.Back);

		}

		/// <summary>
		/// Add a new keybinding for an action.
		/// </summary>
		/// <param name="action">The action to bind to.</param>
		/// <param name="key">The key to bind.</param>
		public static void AddBinding(Actions action, Keys key)
		{
			KeyboardMap.Add(action, key);
		}
		public static void AddBinding(Actions action, MouseButtons button)
		{
			MouseMap.Add(action, button);
		}
		public static void AddBinding(Actions action, GamePadButtons button)
		{
			GamePadMap.Add(action, button);
		}

		/// <summary>
		/// Request that the KeyHandler read the current state of the hardware devices.
		/// </summary>
		public static void Update()
		{
			lastKeyboardState = thisKeyboardState;
			thisKeyboardState = Keyboard.GetState();
			lastMouseState = thisMouseState;
			thisMouseState = Mouse.GetState();
			lastGamePadState = thisGamePadState;
			thisGamePadState = GamePad.GetState(PlayerIndex.One);
		}

		/// <summary>
		/// Returns whether a specified action is currently being pressed.
		/// </summary>
		/// <param name="action">The action to check.</param>
		/// <returns>True if any key bound to this action is pressed down.</returns>
		public static Boolean IsActionDown(Actions action)
		{
			return IsActionDownThis(action);
		}

		/// <summary>
		/// Returns whether a specified action is currently not pressed.
		/// </summary>
		/// <param name="action">The action to check.</param>
		/// <returns>True if no keys bound to this action are pressed down.</returns>
		public static Boolean IsActionUp(Actions action)
		{
			return !IsActionDownThis(action);
		}

		private static Boolean IsActionDownThis(Actions action)
		{
			return CheckState(action, thisKeyboardState) || CheckState(action, thisMouseState) || CheckState(action, thisGamePadState);
		}
		private static Boolean IsActionDownLast(Actions action)
		{
			return CheckState(action, lastKeyboardState) || CheckState(action, lastMouseState) || CheckState(action, lastGamePadState);
		}

		/// <summary>
		/// Returns if a specified action was just pressed.
		/// Formally, this is true on the transition from up to down.
		/// </summary>
		/// <param name="action">The action to check.</param>
		/// <returns>True if the action was up, and is now down.</returns>
		public static Boolean IsActionPressed(Actions action)
		{
			return !IsActionDownLast(action) && IsActionDownThis(action);
		}

		/// <summary>
		/// Returns if a specified action was just released.
		/// Formally, this is true on the transition from down to up.
		/// </summary>
		/// <param name="action">The action to check.</param>
		/// <returns>True if the action was down, and is now up.</returns>
		public static Boolean IsActionReleased(Actions action)
		{
			return IsActionDownLast(action) && !IsActionDownThis(action);
		}

		/// <summary>
		/// true if any key for 'action' is pressed in 'state'
		/// </summary>
		private static Boolean CheckState(Actions action, KeyboardState state)
		{
			foreach (Keys key in KeyboardMap[action])
			{
				if (state.IsKeyDown(key))
				{
					return true;
				}
			}
			return false;
		}
		private static Boolean CheckState(Actions action, MouseState state)
		{
			foreach (MouseButtons button in MouseMap[action])
			{
				if (IsButtonDown(button, state))
				{
					return true;
				}
			}
			return false;
		}
		private static Boolean CheckState(Actions action, GamePadState state)
		{
			if (thisGamePadState.IsConnected)
			{
				foreach (GamePadButtons button in GamePadMap[action])
				{
					if (IsButtonDown(button, state))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static Boolean IsButtonDown(MouseButtons button, MouseState state)
		{
			switch (button)
			{
				case MouseButtons.Left:
					return state.LeftButton.Equals(ButtonState.Pressed);
				case MouseButtons.Middle:
					return state.MiddleButton.Equals(ButtonState.Pressed);
				case MouseButtons.Right:
					return state.RightButton.Equals(ButtonState.Pressed);
				case MouseButtons.XButton1:
					return state.XButton1.Equals(ButtonState.Pressed);
				case MouseButtons.XButton2:
					return state.XButton2.Equals(ButtonState.Pressed);
				default:
					return false;
			}
		}
		private static Boolean IsButtonDown(GamePadButtons button, GamePadState state)
		{
			switch (button)
			{
				case GamePadButtons.A:
					return state.Buttons.A.Equals(ButtonState.Pressed);
				case GamePadButtons.B:
					return state.Buttons.B.Equals(ButtonState.Pressed);
				case GamePadButtons.Back:
					return state.Buttons.Back.Equals(ButtonState.Pressed);
				case GamePadButtons.Down:
					return state.DPad.Down.Equals(ButtonState.Pressed);
				case GamePadButtons.Left:
					return state.DPad.Left.Equals(ButtonState.Pressed);
				case GamePadButtons.LeftShoulder:
					return state.Buttons.LeftShoulder.Equals(ButtonState.Pressed);
				case GamePadButtons.LeftStick:
					return state.Buttons.LeftStick.Equals(ButtonState.Pressed);
				case GamePadButtons.Right:
					return state.DPad.Right.Equals(ButtonState.Pressed);
				case GamePadButtons.RightShoulder:
					return state.Buttons.RightShoulder.Equals(ButtonState.Pressed);
				case GamePadButtons.RightStick:
					return state.Buttons.RightStick.Equals(ButtonState.Pressed);
				case GamePadButtons.Start:
					return state.Buttons.Start.Equals(ButtonState.Pressed);
				case GamePadButtons.Up:
					return state.DPad.Up.Equals(ButtonState.Pressed);
				case GamePadButtons.X:
					return state.Buttons.X.Equals(ButtonState.Pressed);
				case GamePadButtons.Y:
					return state.Buttons.Y.Equals(ButtonState.Pressed);
				default:
					return false;
			}
		}

		public static Boolean HasMouseMoved()
		{
			return (thisMouseState.X != lastMouseState.X) || (thisMouseState.Y != lastMouseState.Y);
		}

		public static Vector2 MousePosition
		{
			get
			{
				return new Vector2(thisMouseState.X, thisMouseState.Y);
			}
		}

		public static Boolean HasLeftStickMoved()
		{
			return thisGamePadState.ThumbSticks.Left != lastGamePadState.ThumbSticks.Left;
		}
		public static Vector2 LeftStickPosition
		{
			get
			{
				return thisGamePadState.ThumbSticks.Left;
			}
		}
		public static Boolean HasRightStickMoved()
		{
			return thisGamePadState.ThumbSticks.Left != lastGamePadState.ThumbSticks.Left;
		}
		public static Vector2 RightStickPosition
		{
			get
			{
				return thisGamePadState.ThumbSticks.Right;
			}
		}




		/// <summary>
		/// Returns whether a specified key is currently being pressed.
		/// </summary>
		/// <param name="Key">Enumerated value that specifies the key to query.</param>
		/// <returns>true if the key specified by key is being held down; false otherwise.</returns>
		public static Boolean IsKeyDown(Keys Key)
		{
			return thisKeyboardState.IsKeyDown(Key);
		}

		/// <summary>
		/// Returns whether a specified key is currently not pressed.
		/// </summary>
		/// <param name="Key">Enumerated value that specifies the key to query.</param>
		/// <returns>true if the key specified by key is not pressed; false otherwise.</returns>
		public static Boolean IsKeyUp(Keys Key)
		{
			return thisKeyboardState.IsKeyUp(Key);
		}

		/// <summary>
		/// Returns whether a specified key was just pressed.
		/// </summary>
		/// <param name="Key">Enumerated value that specifies the key to query.</param>
		/// <returns>true if the key specified by key was just pressed; false otherwise.</returns>
		public static Boolean IsKeyPressed(Keys Key)
		{
			return lastKeyboardState.IsKeyUp(Key) && thisKeyboardState.IsKeyDown(Key);
		}

		/// <summary>
		/// Returns whether a specified key was just released.
		/// </summary>
		/// <param name="Key">Enumerated value that specifies the key to query.</param>
		/// <returns>true if the key specified by key was just released; false otherwise.</returns>
		public static Boolean IsKeyReleased(Keys Key)
		{
			return lastKeyboardState.IsKeyDown(Key) && thisKeyboardState.IsKeyUp(Key);
		}


	}
}
