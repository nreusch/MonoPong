using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace MonoPong
{


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
            p1dict.Add(Keys.W, new MoveUpCommand(player1));
            p1dict.Add(Keys.S, new MoveDownCommand(player1));
            player1 = new Player(Content.Load<Texture2D>("bar.png"), new Vector2(0, 0), 1,p1dict);

            Dictionary<Keys, Command> p2dict = new Dictionary<Keys, Command>();
            p2dict.Add(Keys.Up, new MoveUpCommand(player2));
            p2dict.Add(Keys.Down, new MoveDownCommand(player2));
            player2 = new Player(Content.Load<Texture2D>("bar.png"), new Vector2(0, 0), 2, p2dict);
            // TODO: use this.Content to load your game content here
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

       
    }
}
