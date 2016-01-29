using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong
{
    internal class Player
    {
        private Texture2D _tex;
        private Vector2 _position;
        private byte _id;
        private float _speed = 3f;
        // Dict Keys -> Command
        private Dictionary<Keys, Command> _keydict;
        private int _score = 0;

        public Dictionary<Keys, Command> getKeyDict()
        {
            return _keydict;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _tex.Width,
                    _tex.Height);
            }
        }

        public Texture2D Texture
        {
            get { return _tex; }
            set { _tex = value; }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public byte Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public float Speed
        {
            get
            {
                return _speed;
            }

            set
            {
                _speed = value;
            }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public Player(Texture2D tex, Vector2 position, byte id, Dictionary<Keys, Command> keyDict)
        {
            this._tex = tex;
            this._position = position;
            this._id = id;
            this._keydict = keyDict;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_tex, _position, Color.White);
        }
    }
}
