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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Globals.WIDTH;
        _graphics.PreferredBackBufferHeight = Globals.HEIGHT;
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

        // TODO: Add your update logic here
        paddle.Update(gameTime);
        paddle2.Update(gameTime);
        ball.Update(gameTime, paddle, paddle2);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch.Begin();

        if (!Globals.game_ended)
        {
            paddle.Draw();
            paddle2.Draw();
            ball.Draw();
            Globals.spriteBatch.DrawString(_font, Globals.player1_score.ToString(), new Vector2(100, 50), Color.White);
            Globals.spriteBatch.DrawString(_font, Globals.player2_score.ToString(), new Vector2(Globals.WIDTH - 112, 50), Color.White);
        }
        else
        {
            string gameOverText = "Game Over";
            string winnerText;

            if(Globals.player1_score > Globals.player2_score) {
                winnerText = "Player 1 Wins!";
            }
            else
            {
                winnerText = "Player 2 Wins!";
            }

            // Measure the size of the text
            Vector2 gameOverTextSize = _font.MeasureString(gameOverText);
            Vector2 winnerTextSize = _font.MeasureString(winnerText);

            // Calculate the position to center the text
            Vector2 gameOverTextPosition = new((Globals.WIDTH - gameOverTextSize.X) / 2, (Globals.HEIGHT - gameOverTextSize.Y) / 2 - 20);
            Vector2 winnerTextPosition = new((Globals.WIDTH - winnerTextSize.X) / 2, (Globals.HEIGHT - winnerTextSize.Y) / 2 + 20);

            Globals.spriteBatch.DrawString(_font, gameOverText, gameOverTextPosition, Color.White);
            Globals.spriteBatch.DrawString(_font, winnerText, winnerTextPosition, Color.White);

        }

        

        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}
