using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeightedList.BossFight.BossStates;

namespace WeightedList.BossFight
{
    public class Boss
    {
        private WeightedList<Location> locations = new()
        {
            { Location.Top, 10f },   // ~47.6%
            { Location.Bottom, 5f }, // ~23.8%
            { Location.Follow, 5f }, // ~23.8%
            { Location.Random, 1f }  // ~4.8%
        };
        private WeightedList<AttackPattern> patterns = new()
        {
            { AttackPattern.None, 1f },     // ~5.4%
            { AttackPattern.Circle, 7.5f }, // ~40.5%
            { AttackPattern.Line, 10f }     // ~54%
        };
        private WeightedList<AttackType> types = new()
        {
            { AttackType.Bullet, 10f }, // ~66.7%
            { AttackType.Laser, 5f }    // ~33.3%
        };
        private WeightedList<bool> predict = new()
        {
            { true, 1f }, // 25% chance
            { false, 3f } // 75% chance
        };

        private Location location;
        private AttackPattern pattern;
        private AttackType type;
        private bool predictShot = false;

        private double timer;
        private double attackTimer;

        public Vector2 position;
        public Vector2 targetPosition;

        public void UpdateStates()
        {
            location = locations.GetRandom();
            pattern = patterns.GetRandom();
            type = types.GetRandom();
            predictShot = predict.GetRandom();
        }

        bool newState = true;
        public void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer < 0)
            {
                UpdateStates();
                timer = Random.Shared.Next(5, 10);
                if (pattern == AttackPattern.None)
                {
                    timer /= 2;
                }
                newState = true;
            }

            var screenSize = new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height);

            switch (location)
            {
                case Location.Top:
                    MoveTowards(new Vector2(screenSize.X / 2, screenSize.Y / 4), gameTime);
                    break;
                case Location.Bottom:
                    MoveTowards(new Vector2(screenSize.X / 2, screenSize.Y / 4 * 3), gameTime);
                    break;
                case Location.Follow:
                    MoveTowards(Game1.Instance.Player.Position, gameTime);
                    break;
                case Location.Random:
                    if (newState)
                    {
                        targetPosition = new Vector2(Game1.Instance.rng.Next((int)screenSize.X), Game1.Instance.rng.Next((int)screenSize.Y));
                    }
                    MoveTowards(targetPosition, gameTime);
                    break;
                default:
                    break;
            }

            Vector2 targetPos = Game1.Instance.Player.Position;
            if (predictShot)
            {
                if (type == AttackType.Laser)
                {
                    targetPos = Game1.Instance.Player.Position + Game1.Instance.Player.Velocity * (float)Laser.chargeTime;
                }
                else
                {
                    var distance = Vector2.Distance(Game1.Instance.Player.Position, position);
                    targetPos = Game1.Instance.Player.Position + Game1.Instance.Player.Velocity * (distance / Bullet.Speed);
                }
            }

            switch (pattern)
            {
                case AttackPattern.None:
                    break;
                case AttackPattern.Circle:
                    if (type == AttackType.Bullet)
                    {
                        if (attackTimer >= 1)
                        {
                            ShootBulletCircle(Random.Shared.Next(10, 25));
                            attackTimer = 0;
                        }
                    }
                    else if (type == AttackType.Laser)
                    {
                        if (attackTimer >= 2)
                        {
                            ShootLaserCircle(Random.Shared.Next(5, 10));
                            attackTimer = 0;
                        }
                    }
                    break;
                case AttackPattern.Line:
                    if (type == AttackType.Bullet)
                    {
                        if (attackTimer >= 0.25f)
                        {
                            ShootBullet(Vector2.Normalize(targetPos - position));
                            attackTimer = 0;
                        }
                    }
                    else if (type == AttackType.Laser)
                    {
                        if (attackTimer >= 1.5f)
                        {
                            ShootLaser(Vector2.Normalize(targetPos - position));
                            attackTimer = 0;
                        }
                    }
                    break;
                default:
                    break;
            }

            newState = false;
        }

        public void MoveTowards(Vector2 position, GameTime gameTime)
        {
            var direction = Vector2.Normalize(position - this.position);
            if (Vector2.Distance(position, this.position) < 100f * (float)gameTime.ElapsedGameTime.TotalSeconds)
                this.position = position;
            else
                this.position += direction * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void ShootBullet(Vector2 direction)
        {
            Game1.Instance.bullets.Add(new Bullet { Position = position, Velocity = direction });
        }

        public void ShootBulletCircle(int amount)
        {
            var random = (float)Random.Shared.NextDouble() * MathF.PI * 2;
            for (int i = 0; i < amount; i++)
            {
                var angle = MathHelper.TwoPi / amount * i + random;
                var velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                ShootBullet(velocity);
            }
        }

        public void ShootLaser(Vector2 direction)
        {
            Game1.Instance.lasers.Add(new Laser { Position = position, Direction = direction });
        }

        public void ShootLaserCircle(int amount)
        {
            var random = (float)Random.Shared.NextDouble() * MathF.PI * 2;
            for (int i = 0; i < amount; i++)
            {
                var angle = MathHelper.TwoPi / amount * i + random;
                var direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                ShootLaser(direction);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Game1.Instance.pixel, position, null, Color.Gray, 0f, Vector2.One / 2, 32f, SpriteEffects.None, 0f);

            string debug = $"Location: {location}\nPattern: {pattern}\nType: {type}\nPredict: {predictShot}";
            debug += $"\nTimer: {timer:0.00}\nAttack Timer: {attackTimer:0.00}";
            Game1.Instance.spriteBatch.DrawString(Game1.Instance.font, debug, new Vector2(10, 10), Color.White);
        }
    }
}