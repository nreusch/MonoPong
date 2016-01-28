using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace MonoPong
{
    internal class Ball
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;

        public Ball(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this._texture = texture;
            this._position = position;
            this._velocity = velocity;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    _texture.Width,
                    _texture.Height);
            }
        }

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }

            set
            {
                _texture = value;
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position,Color.White);
        }
    }

    internal class Player
    {
        private Texture2D _tex;
        private Vector2 _pos;
        private byte _id;

        // Dict Keys -> Command
        private Dictionary<Keys, Command> _keydict;

        public Dictionary<Keys,Command> getKeyDict()
        {
            return _keydict;
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)_pos.X,
                    (int)_pos.Y,
                    _tex.Width,
                    _tex.Height);
            }
        }

        public Texture2D getTexture()
        {
            return _tex;
        }

        public void setPosition(Vector2 pos)
        {
            _pos = pos;
        }

        public Vector2 getPosition()
        {
            return _pos;
        }

        public byte getID()
        {
            return _id;
        }

        public Player(Texture2D tex, Vector2 pos, byte id,Dictionary<Keys,Command> keyDict)
        {
            this._tex = tex;
            this._pos = pos;
            this._id = id;
            this._keydict = keyDict;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_tex,_pos,Color.White);
        }
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D t_player;
        Player player1, player2;
        Ball ball;
        private KeyboardState keyState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Dictionary<Keys, Command> p1dict = new Dictionary<Keys, Command>();            
            player1 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(0, 0), 1,p1dict);
            p1dict.Add(Keys.W, new MoveUpCommand(player1));
            p1dict.Add(Keys.S, new MoveDownCommand(player1));

            Dictionary<Keys, Command> p2dict = new Dictionary<Keys, Command>();        
            player2 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(graphics.PreferredBackBufferWidth-50, 0), 2, p2dict);
            p2dict.Add(Keys.Up, new MoveUpCommand(player2));
            p2dict.Add(Keys.Down, new MoveDownCommand(player2));
           
            ball = new Ball(Content.Load<Texture2D>(@"png/ball"),new Vector2(100,100),new Vector2(2,0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyState = Keyboard.GetState();

            ball.Position += ball.Velocity;

            foreach (KeyValuePair<Keys, Command> pair in player1.getKeyDict())
            {
                if(keyState.IsKeyDown(pair.Key))
                {
                    pair.Value.execute();
                }
            }

            foreach (KeyValuePair<Keys, Command> pair in player2.getKeyDict())
            {
                if (keyState.IsKeyDown(pair.Key))
                {
                    pair.Value.execute();
                }
            }

            CheckBallCollision();

            base.Update(gameTime);
        }

        private void CheckBallCollision()
        {
            if (ball.BoundingBox.Intersects(player1.BoundingBox))
            {
                int relIntersY = (int) ((player1.getPosition().Y + 75) - (ball.Position.Y + 10));
                float normalizedInters = relIntersY/75;
                float bounceangle = (normalizedInters*((5*MathHelper.Pi)/12)); // 75 Grad
                ball.Velocity = new Vector2((float) (2*Math.Cos(bounceangle)),(float) (-2*Math.Sin(bounceangle)));
                ball.Position += ball.Velocity;
            }

            if (ball.BoundingBox.Intersects(player2.BoundingBox))
            {
                int relIntersY = (int)((player2.getPosition().Y + 75) - (ball.Position.Y + 10));
                float normalizedInters = relIntersY / 75;
                float bounceangle = (normalizedInters * ((5 * MathHelper.Pi) / 12)); // 75 Grad
                ball.Velocity = new Vector2((float)(2 * Math.Cos(bounceangle)), (float)(-2 * Math.Sin(bounceangle)));
                ball.Position += ball.Velocity;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            ball.Draw(spriteBatch);
  
            spriteBatch.End();
            base.Draw(gameTime);
        }

       
    }
}
