using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    class CinematicCamera : Camera
    {
        private bool _running = false;
        public bool Running
        {
            get { return _running; }
            set { _running = value; }
        }

        private List<Vector3> _lookAts;
        public List<Vector3> LookAts
        {
            get { return _lookAts; }
            set { _lookAts = value; }
        }

        private List<Vector3> _positions;
        public List<Vector3> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        private List<Vector3> _ups;
        public List<Vector3> Ups
        {
            get { return _ups; }
            set { _ups = value; }
        }

        private int _currentIndex = 0;

        private float _lerpAmt = 0.0f;

        private float _lerpSpeed = 0.001f;
        public float LerpSpeed
        {
            get { return _lerpSpeed; }
            set { _lerpSpeed = value; }
        }

        public CinematicCamera()
        {
            _positions = new List<Vector3>();
            _lookAts = new List<Vector3>();
            _ups = new List<Vector3>();
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            if (_running)
            {
                //Run cinematics
                if (_lerpAmt == 1.0f)
                {
                    ++_currentIndex;
                    _lerpAmt = 0.0f;
                }

                /*
                if (_currentIndex >= _positions.Count)
                {
                    _currentIndex = 0;
                }

                int nextIndex = _currentIndex + 1;

                if (nextIndex >= _positions.Count)
                {
                    nextIndex = 0;
                }
                 * */

                Position = Vector3.Lerp(Position, _positions[_currentIndex + 1], _lerpAmt);
                Target = Vector3.Lerp(Target, _lookAts[_currentIndex + 1], _lerpAmt);
                Up = Vector3.Lerp(Up, _ups[_currentIndex + 1], _lerpAmt);

                UpdateMatrices();

                _lerpAmt += _lerpSpeed;
            }
        }

        public override void UpdateMatrices()
        {
            //base.UpdateMatrices();

            View = Matrix.CreateLookAt(Position, Target, Up);

            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);

            Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));
        }
    }
}
