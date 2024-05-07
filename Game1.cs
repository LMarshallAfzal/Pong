using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    public Paddle paddle;
    public Paddle paddle2;
    public Ball ball;
    private SpriteFont _font;
    private GameState _currentState;
    private float _countdownTimer;

    public enum GameState
    {
        Countdown,
        Playing,
        GameOver
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Globals.WIDTH;
        _graphics.PreferredBackBufferHeight = Globals.HEIGHT;
        _currentState = GameState.Countdown;
        _countdownTimer = 3f;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        paddle = new Paddle(false);
        paddle2 = new Paddle(true);
        ball = new Ball(Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        Globals.pixel = new Texture2D(GraphicsDevice, 1, 1);
        Globals.pixel.SetData<Color>(new Color[] {Color.White});

        _font = Content.Load<SpriteFont>("Score");

        ball.LoadContent();

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_currentState)
        {
            case GameState.Countdown:
                _countdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_countdownTimer <= 0)
                {
                    _currentState = GameState.Playing;
                }
                break;
            case GameState.Playing:
                paddle.Update(gameTime);
                paddle2.Update(gameTime);
                ball.Update(gameTime, paddle, paddle2);

                if (Globals.player1_score == 5 || Globals.player1_score == 5)
                {
                    _currentState = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    ResetGame();
                }
                break;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch.Begin();

        switch (_currentState)
        {
            case GameState.Countdown:
                string countdownText = _countdownTimer > 0 ? Math.Ceiling(_countdownTimer).ToString() : "Go!";
                Vector2 countdownTextSize = _font.MeasureString(countdownText);
                Vector2 countdownTextPosition = new Vector2((Globals.WIDTH - countdownTextSize.X) / 2, (Globals.HEIGHT - countdownTextSize.Y) / 2);
                Globals.spriteBatch.DrawString(_font, countdownText, countdownTextPosition, Color.White);
                break;
            case GameState.Playing:
                paddle.Draw();
                paddle2.Draw();
                ball.Draw();
                Globals.spriteBatch.DrawString(_font, Globals.player1_score.ToString(), new Vector2(100, 50), Color.White);
                Globals.spriteBatch.DrawString(_font, Globals.player2_score.ToString(), new Vector2(Globals.WIDTH - 112, 50), Color.White);
                break;
            case GameState.GameOver:
                string gameOverText = "Game Over";
                string enterText = "Press Enter to Restart!";
                string winnerText;

                if (Globals.player1_score > Globals.player2_score) 
                {
                    winnerText = "Player 1 Wins!";
                }
                else
                {
                    winnerText = "Player 2 Wins!";
                }

                // Measure the size of the text
                Vector2 gameOverTextSize = _font.MeasureString(gameOverText);
                Vector2 winnerTextSize = _font.MeasureString(winnerText);
                Vector2 enterTextSize = _font.MeasureString(enterText);

                // Calculate the position to center the text
                Vector2 gameOverTextPosition = new((Globals.WIDTH - gameOverTextSize.X) / 2, (Globals.HEIGHT - gameOverTextSize.Y) / 2 - 40);
                Vector2 winnerTextPosition = new((Globals.WIDTH - winnerTextSize.X) / 2, (Globals.HEIGHT - winnerTextSize.Y) / 2);
                Vector2 enterTextPosition = new((Globals.WIDTH - enterTextSize.X) / 2, (Globals.HEIGHT + enterTextSize.Y) / 2 + 40);

                Globals.spriteBatch.DrawString(_font, gameOverText, gameOverTextPosition, Color.White);
                Globals.spriteBatch.DrawString(_font, winnerText, winnerTextPosition, Color.White);
                Globals.spriteBatch.DrawString(_font, enterText, enterTextPosition, Color.White);
                break;
        }

        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ResetGame()
    {
        Globals.player1_score = 0;
        Globals.player2_score = 0;
        Globals.game_ended = false;
        _currentState = GameState.Countdown;
        _countdownTimer = 3f;
        ball.resetGame();
    }
}
