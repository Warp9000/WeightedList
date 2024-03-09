using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WeightedList.BossFight;

public class Game1 : Game
{
    public static Game1 Instance { get; private set; }

    private GraphicsDeviceManager graphics;
    public SpriteBatch spriteBatch;
    public Random rng;

    public Texture2D pixel;
    public SpriteFont font;

    public Player Player { get; private set; }
    public Boss Boss { get; private set; }
    public List<Bullet> bullets = new();
    public List<Laser> lasers = new();

    public Game1()
    {
        Instance = this;
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720
        };
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        rng = new Random();
    }

    protected override void Initialize()
    {
        Player = new Player() { Position = new Vector2(100, 100) };
        Boss = new Boss() { position = new Vector2(500, 500) };

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
        font = Content.Load<SpriteFont>("font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Player.Update(gameTime);
        Boss.Update(gameTime);
        for (var i = 0; i < bullets.Count; i++)
        {
            bullets[i].Update(gameTime);

            if (bullets[i].Position.X < 0 || bullets[i].Position.X > GraphicsDevice.Viewport.Width
            || bullets[i].Position.Y < 0 || bullets[i].Position.Y > GraphicsDevice.Viewport.Height)
            {
                bullets.RemoveAt(i);
                i--;
            }
        }

        for (var i = 0; i < lasers.Count; i++)
        {
            if (lasers[i].Remove)
            {
                lasers.RemoveAt(i);
                i--;
            }
            else
            {
                lasers[i].Update(gameTime);
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGray);

        spriteBatch.Begin();

        Player.Draw(gameTime);
        Boss.Draw(gameTime);
        foreach (var bullet in bullets)
            bullet.Draw();

        foreach (var laser in lasers)
            laser.Draw();

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
