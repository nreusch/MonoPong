﻿using System;
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

        Texture2D t_player,t_wall,t_goal;
        Player player1, player2;
        Ball ball;

        private KeyboardState keyState;
        public static readonly Rectangle boundingBoxBottom = new Rectangle(0, HEIGHT-5, WIDTH, 5);
        public static readonly Rectangle boundingBoxLeft = new Rectangle(0, 0, 3, HEIGHT);
        public static readonly Rectangle boundingBoxRight = new Rectangle(0, WIDTH-3, 3, HEIGHT);
        public static readonly Rectangle boundingBoxTop = new Rectangle(0,0,WIDTH,5);

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

            t_wall = new Texture2D(GraphicsDevice,1,1);
            t_wall.SetData(new Color[] {Color.Black});

            t_goal = new Texture2D(GraphicsDevice, 1, 1);
            t_goal.SetData(new Color[] { Color.Pink });

            Dictionary<Keys, Command> p1Dict = new Dictionary<Keys, Command>();            
            player1 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(0, 0), 1,p1Dict);
            player1.Position = new Vector2(boundingBoxLeft.Width, boundingBoxTop.Height);
            p1Dict.Add(Keys.W, new MoveUpCommand(player1));
            p1Dict.Add(Keys.S, new MoveDownCommand(player1));

            Dictionary<Keys, Command> p2Dict = new Dictionary<Keys, Command>();        
            player2 = new Player(Content.Load<Texture2D>(@"png/bar"), new Vector2(0, 0), 2, p2Dict);
            player2.Position = new Vector2(WIDTH-boundingBoxRight.Width-player2.BoundingBox.Width,boundingBoxTop.Height);
            p2Dict.Add(Keys.Up, new MoveUpCommand(player2));
            p2Dict.Add(Keys.Down, new MoveDownCommand(player2));
           
            ball = new Ball(Content.Load<Texture2D>(@"png/ball"),new Vector2(300,100),new Vector2(1,0));
            ball.setToStartPosition();
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

            ball.Position += ball.Velocity * ball.Speed;

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
            if (ball.BoundingBox.Intersects(boundingBoxLeft))
            {
                player1.Score++;
                ball.setToStartPosition();
                ball.Velocity = new Vector2(-1,0);
            }
            if (ball.BoundingBox.Intersects(boundingBoxLeft))
            {
                player2.Score++;
                ball.setToStartPosition();
                ball.Velocity = new Vector2(1, 0);
            }

            if (ball.BoundingBox.Intersects(player1.BoundingBox))
            {
                float relIntersY = (int) ((player1.Position.Y + player1.BoundingBox.Height/2) - (ball.Position.Y + ball.BoundingBox.Height/2));
                float normalizedInters = relIntersY/ player1.BoundingBox.Height / 2;
                float bounceangle = (normalizedInters*((5*MathHelper.Pi)/12)); // 75 Grad
                ball.Velocity = new Vector2((float) (1*Math.Cos(bounceangle)),(float) (-1 * Math.Sin(bounceangle)));
            }
            if (ball.BoundingBox.Intersects(player2.BoundingBox))
            {

            
               
                float relIntersY = (int)((player2.Position.Y + player2.BoundingBox.Height / 2) - (ball.Position.Y + ball.BoundingBox.Height / 2));
                float normalizedInters = relIntersY / player1.BoundingBox.Height / 2;
                float bounceangle = (normalizedInters * ((5 * MathHelper.Pi) / 12)); // 75 Grad
                ball.Velocity = new Vector2((float)(-1 * Math.Cos(bounceangle)), (float)(-1 * Math.Sin(bounceangle)));
                
            }

            if(ball.BoundingBox.Intersects(boundingBoxTop) || ball.BoundingBox.Intersects(boundingBoxBottom))
            {
                ball.Velocity = new Vector2(ball.Velocity.X,ball.Velocity.Y*-1);
            }

            


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            this.Window.Title = ball.Velocity.ToString();

            spriteBatch.Begin();

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
  
            spriteBatch.End();
            base.Draw(gameTime);
        }

       
    }
}
