using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WeightedList.BossFight
{
    public class Laser
    {
        public Vector2 Position;
        public Vector2 Direction;

        private double timer;

        private bool damage = false;

        public const double chargeTime = 0.5f;

        public bool Remove;

        public void Update(GameTime gameTime)
        {
            if (Remove)
            {
                return;
            }

            timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= chargeTime)
            {
                damage = true;
            }

            if (timer >= chargeTime * 2)
            {
                Remove = true;
            }
        }

        public void Draw()
        {
            if (Remove)
            {
                return;
            }

            if (damage)
            {
                Game1.Instance.spriteBatch.Draw(Game1.Instance.pixel, Position, null, Color.Red, (float)Math.Atan2(Direction.Y, Direction.X), Vector2.Zero, new Vector2(1000, 8), SpriteEffects.None, 0f);
            }
            else
            {
                Game1.Instance.spriteBatch.Draw(Game1.Instance.pixel, Position, null, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Vector2.Zero, new Vector2(1000, 4), SpriteEffects.None, 0f);
            }
        }
    }
}