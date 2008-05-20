using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob {
	public class EffectManager {
		// Objects to ensure singleton design
		private static volatile EffectManager _instance;
		private static object _syncRoot = new Object();

		// Dictionary of effects
		private static Dictionary<string, Effect> _effects;

		/// <summary>
		/// Constructor
		/// </summary>
		private EffectManager() {
			_effects = new Dictionary<string, Effect>();
		}

		//! Instance
		public static EffectManager getSingleton {
			get {
				if (_instance == null) {
					lock (_syncRoot) {
						if (_instance == null)
							_instance = new EffectManager();
					}
				}

				return _instance;
			}
		}

		public Effect GetEffect(string effectName) {
			//if (_effects.ContainsKey(effectName))
			//{
			return _effects[effectName];
			//}
			//return null;
		}

		public void AddEffect(string effectName, Effect effect) {
			if (!_effects.ContainsKey(effectName)) {
				_effects.Add(effectName, effect);
			}
		}

		public void RemoveEffect(string effectName) {
			if (_effects.ContainsKey(effectName)) {
				_effects.Remove(effectName);
			}
		}
	}
}
