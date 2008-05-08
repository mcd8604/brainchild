using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wintellect.PowerCollections;

/// <summary>
/// List of Actions to which keys may be bound and states queried.
/// </summary>
internal enum Actions { Reset, ToggleStickiness, ToggleElasticity };
internal enum MouseButtons { Left, Middle, Right, XButton1, XButton2 };
internal enum AnalogActions { Movement, Camera, MouseLook };
internal delegate Vector2 AnalogFunction();

internal static class InputHandler
{
	private static KeyboardState lastKeyboardState = new KeyboardState();
	private static KeyboardState thisKeyboardState = Keyboard.GetState();
	private static MultiDictionary<Actions, Keys> KeyboardMap = new MultiDictionary<Actions, Keys>(false);
	private static GamePadState lastGamePadState = new GamePadState();
	private static GamePadState thisGamePadState = GamePad.GetState(PlayerIndex.One);
	private static MultiDictionary<Actions, Buttons> GamePadMap = new MultiDictionary<Actions, Buttons>(false);
	private static MouseState lastMouseState = new MouseState();
	private static MouseState thisMouseState = Mouse.GetState();
	private static MultiDictionary<Actions, MouseButtons> MouseMap = new MultiDictionary<Actions, MouseButtons>(false);
	private static MultiDictionary<AnalogActions, AnalogFunction> AnalogMap = new MultiDictionary<AnalogActions, AnalogFunction>(false);

	internal static void LoadDefaultBindings()
	{
		KeyboardMap.Add(Actions.Reset, Keys.Space);
		GamePadMap.Add(Actions.Reset, Buttons.X);

		GamePadMap.Add(Actions.ToggleElasticity, Buttons.RightShoulder);
		GamePadMap.Add(Actions.ToggleStickiness, Buttons.LeftShoulder);

		AnalogMap.Add(AnalogActions.Movement, delegate { return InputHandler.thisGamePadState.ThumbSticks.Left; });
		AnalogMap.Add(AnalogActions.Movement, delegate { if (InputHandler.IsKeyDown(Keys.Up)) { return new Vector2(0, 1); } else { return Vector2.Zero; } });
		AnalogMap.Add(AnalogActions.Movement, delegate { if (InputHandler.IsKeyDown(Keys.Down)) { return new Vector2(0, -1); } else { return Vector2.Zero; } });
		AnalogMap.Add(AnalogActions.Movement, delegate { if (InputHandler.IsKeyDown(Keys.Right)) { return new Vector2(1, 0); } else { return Vector2.Zero; } });
		AnalogMap.Add(AnalogActions.Movement, delegate { if (InputHandler.IsKeyDown(Keys.Left)) { return new Vector2(-1, 0); } else { return Vector2.Zero; } });

		AnalogMap.Add(AnalogActions.Camera, delegate { return InputHandler.thisGamePadState.ThumbSticks.Right; });

		AnalogMap.Add(AnalogActions.MouseLook, InputHandler.getMouseDeltaPosition);
	}

	internal static Vector2 GetAnalogAction(AnalogActions action)
	{

		Vector2 Result = Vector2.Zero;
		foreach (AnalogFunction func in AnalogMap[action])
		{
			Result += func.Invoke();
		}
		Vector2.Clamp(Result, new Vector2(-1, -1), new Vector2(1, 1));
		return Result;

	}

	/// <summary>
	/// Add a new keybinding for an action.
	/// </summary>
	/// <param name="action">The action to bind to.</param>
	/// <param name="key">The key to bind.</param>
	internal static void AddBinding(Actions action, Keys key)
	{
		KeyboardMap.Add(action, key);
	}

	internal static void AddBinding(Actions action, Buttons button)
	{
		GamePadMap.Add(action, button);
	}

	internal static void AddBinding(Actions action, MouseButtons button)
	{
		MouseMap.Add(action, button);
	}

	/// <summary>
	/// Request that the KeyHandler read the current state of the hardware devices.
	/// </summary>
	internal static void Update()
	{
		lastKeyboardState = thisKeyboardState;
		thisKeyboardState = Keyboard.GetState();

		lastGamePadState = thisGamePadState;
		thisGamePadState = GamePad.GetState(PlayerIndex.One);

		lastMouseState = thisMouseState;
		thisMouseState = Mouse.GetState();

	}

	/// <summary>
	/// Returns whether a specified action is currently being pressed.
	/// </summary>
	/// <param name="action">The action to check.</param>
	/// <returns>True if any key bound to this action is pressed down.</returns>
	internal static bool IsActionDown(Actions action)
	{
		return IsActionDownThis(action);
	}

	/// <summary>
	/// Returns whether a specified action is currently not pressed.
	/// </summary>
	/// <param name="action">The action to check.</param>
	/// <returns>True if no keys bound to this action are pressed down.</returns>
	internal static bool IsActionUp(Actions action)
	{
		return !IsActionDownThis(action);
	}

	private static bool IsActionDownThis(Actions action)
	{
		return CheckState(action, thisKeyboardState) || CheckState(action, thisGamePadState) || CheckState(action, thisMouseState);
	}
	private static bool IsActionDownLast(Actions action)
	{
		return CheckState(action, lastKeyboardState) || CheckState(action, lastGamePadState) || CheckState(action, lastMouseState);
	}

	/// <summary>
	/// Returns if a specified action was just pressed.
	/// Formally, this is true on the transition from up to down.
	/// </summary>
	/// <param name="action">The action to check.</param>
	/// <returns>True if the action was up, and is now down.</returns>
	internal static bool IsActionPressed(Actions action)
	{
		return !IsActionDownLast(action) && IsActionDownThis(action);
	}

	/// <summary>
	/// Returns if a specified action was just released.
	/// Formally, this is true on the transition from down to up.
	/// </summary>
	/// <param name="action">The action to check.</param>
	/// <returns>True if the action was down, and is now up.</returns>
	internal static bool IsActionReleased(Actions action)
	{
		return IsActionDownLast(action) && !IsActionDownThis(action);
	}

	/// <summary>
	/// true if any key for 'action' is pressed in 'state'
	/// </summary>
	private static bool CheckState(Actions action, KeyboardState state)
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

	private static bool CheckState(Actions action, GamePadState state)
	{
		if (thisGamePadState.IsConnected)
		{
			foreach (Buttons button in GamePadMap[action])
			{
				if (state.IsButtonDown(button))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool CheckState(Actions action, MouseState state)
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

	private static bool IsButtonDown(MouseButtons button, MouseState state)
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

	internal static bool HasLeftStickMoved()
	{
		return thisGamePadState.ThumbSticks.Left != lastGamePadState.ThumbSticks.Left;
	}
	internal static Vector2 LeftStickPosition
	{
		get
		{
			return thisGamePadState.ThumbSticks.Left;
		}
	}
	internal static bool HasRightStickMoved()
	{
		return thisGamePadState.ThumbSticks.Right != lastGamePadState.ThumbSticks.Right;
	}
	internal static Vector2 RightStickPosition
	{
		get
		{
			return thisGamePadState.ThumbSticks.Right;
		}
	}

	internal static void SetVibration(float low, float high)
	{
		GamePad.SetVibration(PlayerIndex.One, low, high);
	}

	internal static bool HasMouseMoved()
	{
		return (thisMouseState.X != lastMouseState.X) || (thisMouseState.Y != lastMouseState.Y);
	}

	internal static Vector2 MousePosition
	{
		get
		{
			return new Vector2(thisMouseState.X, thisMouseState.Y);
		}
	}

	internal static Vector2 getMousePosition()
	{
		return new Vector2(thisMouseState.X, thisMouseState.Y);
	}

	internal static Vector2 getMouseDeltaPosition()
	{
		return new Vector2(thisMouseState.X, thisMouseState.Y) - new Vector2(lastMouseState.X, lastMouseState.Y);
	}

	/// <summary>
	/// Returns whether a specified key is currently being pressed.
	/// </summary>
	/// <param name="Key">Enumerated value that specifies the key to query.</param>
	/// <returns>true if the key specified by key is being held down; false otherwise.</returns>
	internal static bool IsKeyDown(Keys Key)
	{
		return thisKeyboardState.IsKeyDown(Key);
	}

	/// <summary>
	/// Returns whether a specified key is currently not pressed.
	/// </summary>
	/// <param name="Key">Enumerated value that specifies the key to query.</param>
	/// <returns>true if the key specified by key is not pressed; false otherwise.</returns>
	internal static bool IsKeyUp(Keys Key)
	{
		return thisKeyboardState.IsKeyUp(Key);
	}

	/// <summary>
	/// Returns whether a specified key was just pressed.
	/// </summary>
	/// <param name="Key">Enumerated value that specifies the key to query.</param>
	/// <returns>true if the key specified by key was just pressed; false otherwise.</returns>
	internal static bool IsKeyPressed(Keys Key)
	{
		return lastKeyboardState.IsKeyUp(Key) && thisKeyboardState.IsKeyDown(Key);
	}

	/// <summary>
	/// Returns whether a specified key was just released.
	/// </summary>
	/// <param name="Key">Enumerated value that specifies the key to query.</param>
	/// <returns>true if the key specified by key was just released; false otherwise.</returns>
	internal static bool IsKeyReleased(Keys Key)
	{
		return lastKeyboardState.IsKeyDown(Key) && thisKeyboardState.IsKeyUp(Key);
	}
}