using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace MonoPong
{
    

    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static int WIDTH = 800;
        public static int HEIGHT = 400;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;

        Texture2D t_player,t_wall,t_goal;
        Player player1, player2;
        Ball ball;

        private KeyboardState keyState;
        public static readonly Rectangle boundingBoxBottom = new Rectangle(0, HEIGHT-5, WIDTH, 5);
        public static readonly Rectangle boundingBoxLeft = new Rectangle(0, 0, 3, HEIGHT);
        public static readonly Rectangle boundingBoxRight = new Rectangle(WIDTH - 3, 0, 3, HEIGHT);
        public static readonly Rectangle boundingBoxTop = new Rectangle(0,45,WIDTH,5);
        private readonly bool WAITFORENTER;
        private bool _waitingforenter = true;
        private bool _pressspacepositonset;
        private SpriteFont font_big;
        private Vector2 _pressspacepositon;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            
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

            font = Content.Load<SpriteFont>("Font");
            font_big = Content.Load<SpriteFont>("font_big");

            t_wall = new Texture2D(GraphicsDevice,1,1);
            t_wall.SetData(new Color[] {Color.Black});

            t_goal = new Texture2D(GraphicsDevice, 1, 1);
            t_goal.SetData(new Color[] { Color.Pink });

            Dictionary<Keys, Command> p1Dict = new Dictionary<Keys, Command>();            
            player1 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(0, 0), 1,p1Dict);
            player1.Position = new Vector2(boundingBoxLeft.Width, boundingBoxTop.Y + boundingBoxTop.Height);
            //player1.Position = new Vector2(boundingBoxLeft.Width, boundingBoxTop.Y + boundingBoxTop.Height + HEIGHT / 2 - player1.BoundingBox.Height);
            p1Dict.Add(Keys.W, new MoveUpCommand(player1));
            p1Dict.Add(Keys.S, new MoveDownCommand(player1));

            Dictionary<Keys, Command> p2Dict = new Dictionary<Keys, Command>();        
            player2 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(0, 0), 2, p2Dict);
            player2.Position = new Vector2(WIDTH - boundingBoxRight.Width - player2.BoundingBox.Width, boundingBoxTop.Y + boundingBoxTop.Height);
            // player2.Position = new Vector2(WIDTH-boundingBoxRight.Width-player2.BoundingBox.Width, boundingBoxTop.Y + boundingBoxTop.Height + HEIGHT/2 - player2.BoundingBox.Height );
            p2Dict.Add(Keys.Up, new MoveUpCommand(player2));
            p2Dict.Add(Keys.Down, new MoveDownCommand(player2));
           
            ball = new Ball(Content.Load<Texture2D>(@"png/ball"),new Vector2(300, boundingBoxTop.Y + boundingBoxTop.Height + player1.BoundingBox.Height/2 - 10),300f);
            ball.setToStartPosition(player1);
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
            
            keyState = Keyboard.GetState();

            if (!_waitingforenter)
            {
                
                ball.Position += ball.Velocity* (float)gameTime.ElapsedGameTime.TotalSeconds;

                // TODO: Change, dirty fix for having the ball jump over the player and goal bounding box
                if (ball.Position.X < boundingBoxLeft.X) ball.Position = new Vector2(boundingBoxLeft.X + boundingBoxLeft.Width - 1 + player1.BoundingBox.Width, ball.Position.Y);
                else if (ball.Position.X > boundingBoxRight.X + boundingBoxRight.Width) ball.Position = new Vector2(boundingBoxRight.X - ball.BoundingBox.Width + 1 - player1.BoundingBox.Width, ball.Position.Y);

                if (ball.Position.Y < boundingBoxTop.Y) ball.Position = new Vector2(ball.Position.X, boundingBoxTop.Y + boundingBoxTop.Height-1);
                else if (ball.Position.Y > boundingBoxBottom.Y + boundingBoxBottom.Height) ball.Position = new Vector2(ball.Position.X, boundingBoxBottom.Y - ball.BoundingBox.Height + 1);

                foreach (KeyValuePair<Keys, Command> pair in player1.getKeyDict())
                {
                    if (keyState.IsKeyDown(pair.Key))
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
            }
            else
            {
                if (keyState.IsKeyDown(Keys.Enter) || keyState.IsKeyDown(Keys.Space))
                {
                    _waitingforenter = false;
                }
            }

            base.Update(gameTime);
        }

        private void CheckBallCollision()
        {
            if (ball.BoundingBox.Intersects(boundingBoxLeft))
            {
                if (ball.BoundingBox.Intersects(player1.BoundingBox)) // TODO: Change, dirty fix for having the ball jump over the player and goal bounding box
                {
                    hitPaddleFrom(player1);
                }
                else
                {
                    goalShotBy(player1);
                }
            }
            if (ball.BoundingBox.Intersects(boundingBoxRight))
            {
                if (ball.BoundingBox.Intersects(player2.BoundingBox)) // TODO: Change, dirty fix for having the ball jump over the player and goal bounding box
                {
                    hitPaddleFrom(player2);
                }
                else
                {
                    goalShotBy(player2);
                }
            }

            if (ball.BoundingBox.Intersects(player1.BoundingBox))
            {
                hitPaddleFrom(player1);

            }
            if (ball.BoundingBox.Intersects(player2.BoundingBox))
            {

                hitPaddleFrom(player2);


            }

            if(ball.BoundingBox.Intersects(boundingBoxTop) || ball.BoundingBox.Intersects(boundingBoxBottom))
            {
                ball.Velocity = new Vector2(ball.Velocity.X,ball.Velocity.Y*-1);
            }

            


        }

        private void hitPaddleFrom(Player player)
        {
            float relIntersY = (int)((player.Position.Y + player.BoundingBox.Height / 2) - (ball.Position.Y + ball.BoundingBox.Height / 2));
            float normalizedInters = relIntersY / player.BoundingBox.Height / 2;
            float bounceangle = (normalizedInters * ((5 * MathHelper.Pi) / 12)); // 75 Grad
            ball.Speed *= 1.2f; // Increase Ball Speed by 20 % TODO: Maybe not exponential growth

            if (player.Id == 1)
            {
                ball.Velocity = new Vector2((float) (ball.Speed*Math.Cos(bounceangle)),
                    (float) (-ball.Speed*Math.Sin(bounceangle)));
            }
            else
            {
                ball.Velocity = new Vector2((float)(-ball.Speed * Math.Cos(bounceangle)), (float)(-ball.Speed * Math.Sin(bounceangle)));
            }
            
        }

        private void goalShotBy(Player scorer)
        {
            scorer.Score++;           
            ball.setToStartPosition(scorer);

            if (!this.WAITFORENTER == true)
            {
                _waitingforenter = true;
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

            if (_waitingforenter)
            {
                if (_pressspacepositonset == false)
                {
                    _pressspacepositon = new Vector2(WIDTH/2 - font_big.MeasureString("PRESS SPACE").X/2,
                        HEIGHT/2 - font_big.MeasureString("PRESS SPACE").Y/2);
                }
                spriteBatch.DrawString(font_big, "PRESS SPACE", _pressspacepositon, Color.Black);
            }

            // Draw Walls & Goals
            spriteBatch.Draw(t_wall, boundingBoxTop, boundingBoxTop, Color.White);
            spriteBatch.Draw(t_wall, boundingBoxBottom, boundingBoxBottom,Color.White);

            spriteBatch.Draw(t_goal, boundingBoxLeft, boundingBoxLeft, Color.White);
            spriteBatch.Draw(t_goal, boundingBoxRight, boundingBoxRight, Color.White);

            // Draw Player
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);

            // Draw Ball
            ball.Draw(spriteBatch);
            
            // Draw Text
            string text = "Score: " + player1.Score + ":" + player2.Score;
            spriteBatch.DrawString(font,text ,new Vector2(WIDTH/2-font.MeasureString(text).X/2,20),Color.Black );
            spriteBatch.End();
            base.Draw(gameTime);
        }

       
    }
}
