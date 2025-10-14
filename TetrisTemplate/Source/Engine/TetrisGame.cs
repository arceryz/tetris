using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

class TetrisGame : Game
{
    public static InputHelper Input;
    public static Random Random = new Random();
    public static Point ScreenSize { get; private set; }

    static ContentManager content;
	static SpriteBatch batch;
	static GameState currentState;
    static Dictionary<string, Object> customAssets = new();

	[STAThread]
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run();
    }

    public TetrisGame()
    {        
        GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
        content = Content;
        Content.RootDirectory = "Content";

        ScreenSize = new Point(800, 600);
        graphics.PreferredBackBufferWidth = ScreenSize.X;
        graphics.PreferredBackBufferHeight = ScreenSize.Y;

        Input = new InputHelper();
    }

    protected override void LoadContent()
    {
        batch = new SpriteBatch(GraphicsDevice);
        currentState = new MenuState();
        TetrisBlock.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update();

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        currentState.Update(delta);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        batch.Begin();
        currentState.Draw(batch);
        batch.End();
    }

    public static void ChangeState(GameState state)
    {
        currentState = state;
    }

    /// <summary>
    /// Laad een asset van de custom assets of van de content manager.
    /// Hiermee kunnen we custom data laden.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T Load<T>(string path)
    {
        if (customAssets.ContainsKey(path))
            return (T)customAssets[path];
        else
            return content.Load<T>(path);
    }
}

