using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace Dino_Game_with_MonoGame
{
    public class Game1 : Game
    {
        private bool GameIsActive = true;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        string FrameName = "tile0";
        float frameStopwatch = 0;
        float frameDuration = 47;
        int CurrentFrame = 0;

        float SpikeStopWatch = 0;
        float SpikeSpawnRate = 2000;

        float dt = 0;

        float StartPos = 0;

        float PlayerExtraY = 0;
        float JumpHeldStopWatch = 0;
        float MaxJumpHeld = 200;
        float fallingStopwatch = 0;

        float score = 0;

        List<SpikeObject> spikeObjects = new List<SpikeObject>();

        SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            font = Content.Load<SpriteFont>("File");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(GameIsActive)
            {
                float dt = gameTime.ElapsedGameTime.Milliseconds;

                if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)) && JumpHeldStopWatch < MaxJumpHeld)
                {
                    JumpHeldStopWatch += dt;
                    PlayerExtraY = PlayerExtraY - 10 + JumpHeldStopWatch / 25;
                }
                else if (PlayerExtraY < 0)
                {
                    fallingStopwatch = fallingStopwatch + dt;
                    PlayerExtraY += (fallingStopwatch / 25) - 5;

                    if (PlayerExtraY > 0)
                    {
                        fallingStopwatch = 0;
                        PlayerExtraY = 0;
                        JumpHeldStopWatch = 0;
                    }
                }

                //---------------------------------------------------------------

                foreach (var item in spikeObjects)
                {
                    if (item.x < 16 && item.x > -16)
                    {
                        if (PlayerExtraY > -16)
                        {
                            GameEnded();
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        void GameEnded()
        {
            GameIsActive = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float GroundSpeed = dt / 7.5f;

            GraphicsDevice.Clear(new Color(55, 55, 55));

            //---------------------------------------------------------------

            if (GameIsActive)
                score += dt / 10;

            _spriteBatch.Begin();
            // Finds the center of the string in coordinates inside the text rectangle
            Vector2 textMiddlePoint = font.MeasureString("Score: " + score.ToString("f0")) / 2;
            // Places text in center of the screen
            Vector2 position = new Vector2(Window.ClientBounds.Width / 2, 30);
            _spriteBatch.DrawString(font, "Score: " + score.ToString("f0"), position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();

            //---------------------------------------------------------------

            if (GameIsActive)
                StartPos -= GroundSpeed;
            if (StartPos < -100)
            {
                StartPos += 96;
            }
            for (int i = 0; i < 20; i++)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(Content.Load<Texture2D>("Terrain (16x16)"), new Vector2(48 * i + StartPos, 432), Color.White);
                _spriteBatch.End();
            }

            //---------------------------------------------------------------

            SpikeStopWatch += dt;
            if(SpikeStopWatch > SpikeSpawnRate)
            {
                SpikeStopWatch = 0;
                spikeObjects.Add(new SpikeObject());
            }
            foreach(var spikeObject in spikeObjects)
            {
                if (GameIsActive)
                    spikeObject.x -= GroundSpeed;

                _spriteBatch.Begin();
                _spriteBatch.Draw(Content.Load<Texture2D>("Idle"), new Vector2(spikeObject.x, spikeObject.y), Color.White);
                _spriteBatch.End();
            }

            //---------------------------------------------------------------

            frameStopwatch += dt;
            if(frameStopwatch > frameDuration)
            {
                frameStopwatch = 0;

                if (GameIsActive)
                    CurrentFrame++;
                if (CurrentFrame > 11)
                    CurrentFrame = 0;

                FrameName = "tile" + CurrentFrame;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(Content.Load<Texture2D>(FrameName), new Vector2(0, 410 + PlayerExtraY), Color.White);
            _spriteBatch.End();

            //---------------------------------------------------------------

            base.Draw(gameTime);
        }
    }
}