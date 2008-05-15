using System;
using Microsoft.Xna.Framework;

namespace Audio {

    [Serializable]
    public class AmbientSoundInfo {

        private string name;
        public string Name {
            get { return name; }
            set { name = value; }
        }

        private Vector3 position;
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        public override string ToString() {
            return "Ambient Sound";
        }

    }
}
