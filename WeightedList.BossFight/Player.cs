using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WeightedList.BossFight
{
    public class Player
    {
        public const float speed = 200f;

        public Vector2 Position;

        public Vector2 Velocity { get; private set; }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            Vector2 direction = Vector2.Zero;

            if (kb.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (kb.IsKeyDown(Keys.Down))
                direction.Y += 1;
            if (kb.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (kb.IsKeyDown(Keys.Right))
                direction.X += 1;

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Velocity = direction * speed;
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Velocity = Vector2.Zero;
            }

            if (Position.X < 0)
                Position.X = 0;
            if (Position.X > Game1.Instance.GraphicsDevice.Viewport.Width)
                Position.X = Game1.Instance.GraphicsDevice.Viewport.Width;
            if (Position.Y < 0)
                Position.Y = 0;
            if (Position.Y > Game1.Instance.GraphicsDevice.Viewport.Height)
                Position.Y = Game1.Instance.GraphicsDevice.Viewport.Height;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Game1.Instance.pixel, Position, null, Color.Green, 0f, Vector2.One / 2, 16f, SpriteEffects.None, 0f);
        }
    }
}