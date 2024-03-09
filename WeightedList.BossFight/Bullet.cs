using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WeightedList.BossFight
{
    public class Bullet
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public const float Speed = 400f;

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
        }

        public void Draw()
        {
            Game1.Instance.spriteBatch.Draw(Game1.Instance.pixel, Position, null, Color.Red, 0f, Vector2.One / 2, 8f, SpriteEffects.None, 0f);
        }
    }
}