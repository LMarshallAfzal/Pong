using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong {
    public class Paddle {
        public Rectangle rect;
        public float moveSpeed;

        public bool isSecondPlayer;

        public Paddle(bool isSecondPlayer) 
        {
            this.isSecondPlayer = isSecondPlayer;
            rect = new Rectangle((this.isSecondPlayer ? Globals.WIDTH - 40 : 0), 140, 40, 200);
            moveSpeed = 500f;
        }
        public void Update(GameTime gameTime) 
        {
            // Get current state of keyboard (info on what keys are being pressed)
            KeyboardState keyboardState = Keyboard.GetState();

            // Check if player is pressing 'W' and that the paddle is not being clipped of the top
            if ((this.isSecondPlayer ? keyboardState.IsKeyDown(Keys.Up) : keyboardState.IsKeyDown(Keys.W)) && rect.Y > 0) {
                // Change the Y position of the paddle, so that it can go up basedon the moveSpeed times the time between the last fram and the current frame (delta time)
                // Delta time helps smooth movements.
                rect.Y -= (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Check to see if player is pressing 'S' and the paddle is not being clipped off the bottom of the screen
            if ((this.isSecondPlayer ? keyboardState.IsKeyDown(Keys.Down) : keyboardState.IsKeyDown(Keys.S)) && rect.Y < Globals.HEIGHT - rect.Height) {
                // Change the Y position of the paddle so that it can do down
                rect.Y += (int)(moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public void Draw() 
        {
            Globals.spriteBatch.Draw(Globals.pixel, rect, Color.White);
        }
    }
}
