using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

class TetrisGame : Game
{
    public static InputHelper Input;
    public static Random Random = new Random();
    public static float ElapsedTime = 0;

	public static int GameWidth = 800;
	public static int GameHeight = 600;

	const float cameraShakeMax = 30;
	const float cameraShakeFalloff = 5;

	static ContentManager content;
	static SpriteBatch batch;
	static GameState currentState;
    static Dictionary<string, Object> customAssets = new();
	static float cameraShake = 0;

	Rectangle Viewport;
	RenderTarget2D renderTarget;
	RenderTarget2D effectTarget;
	GraphicsDeviceManager graphics;
	Effect crtEffect;

	[STAThread]
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run();
    }

    public TetrisGame()
    {        
		graphics = new GraphicsDeviceManager(this);
        content = Content;
        Content.RootDirectory = "Content";

		graphics.PreferredBackBufferWidth = GameWidth;
		graphics.PreferredBackBufferHeight = GameHeight;
		graphics.HardwareModeSwitch = false;
		Window.AllowUserResizing = true;

		Input = new InputHelper();
    }

    protected override void LoadContent()
    {
		renderTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight, false, SurfaceFormat.Color, DepthFormat.None);
		effectTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight, false, SurfaceFormat.Color, DepthFormat.None);
		customAssets[Assets.Fonts.Arcade] = new ImageFont(Assets.Fonts.Arcade, "abcdefghijklmnopqrstuvwxyz0123456789-:");
		crtEffect = Load<Effect>(Assets.Shaders.CRT);
		TetrisBlock.LoadContent();

		MediaPlayer.Volume = 0.25f;
		MediaPlayer.IsRepeating = true;

		batch = new SpriteBatch(GraphicsDevice);
		currentState = new MenuState();
	}

    protected override void Update(GameTime gameTime)
    {
        Input.Update();
		ElapsedTime = (float)gameTime.TotalGameTime.TotalSeconds;
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        currentState.Update(delta);
		cameraShake *= MathHelper.Max(1.0f - cameraShakeFalloff * delta, 0);
		if (Input.KeyPressed(Keys.F)) 
			graphics.ToggleFullScreen();
	}

    protected override void Draw(GameTime gameTime)
    {
		ComputeViewport();

		//***********************************************//
		// 1. Render the game itself.
		//***********************************************//
		GraphicsDevice.SetRenderTarget(renderTarget);
		GraphicsDevice.Clear(Color.Black);
		batch.Begin(samplerState: SamplerState.PointClamp);
		currentState.Draw(batch);
		batch.End();

		//***********************************************//
		// 2. Apply the CRT effect.

		// We have to use a seperate render target for the effect to
		// avoid graphical glitches from reading/writing the same texture.
		//***********************************************//
		GraphicsDevice.SetRenderTarget(effectTarget);
		GraphicsDevice.Clear(Color.Black);
		crtEffect.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);

		float ang = Random.NextSingle() * MathF.Tau;
		Vector2 vec = new Vector2(MathF.Cos(ang), MathF.Sin(ang));
		crtEffect.Parameters["Shake"]?.SetValue(vec * MathHelper.Min(cameraShakeMax, cameraShake) * 0.001f);
		batch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, effect: crtEffect);
		batch.Draw(renderTarget, new Rectangle(0, 0, GameWidth, GameHeight), Color.White);
		batch.End();

		//***********************************************//
		// 3. Draw the render target to the screen.
		//***********************************************//
		GraphicsDevice.SetRenderTarget(null);
		GraphicsDevice.Clear(Color.Black);
		batch.Begin(samplerState: SamplerState.PointClamp);
		batch.Draw(effectTarget, Viewport, Color.White);
		batch.End();
	}

    public static void ChangeState(GameState state)
    {
        currentState = state;
    }

	public static Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
	{
		return new Vector2(GameWidth * (xs + 1) / 2 + xo, GameHeight * (ys + 1) / 2 + yo);
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

	#region Viewport
	public void ComputeViewport()
	{
		// Calculate scaling rectangle to maintain aspect ratio.
		// By setting the scale to the smallest of the two ratios, we ensure the game fits on the screen.
		// Black bars will be present on the sides that don't fit.
		float windowWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
		float windowHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
		float scale = Math.Min(windowWidth / GameWidth, windowHeight / GameHeight);

		// The viewport is centered on the screen.
		int vpWidth = (int)(GameWidth * scale);
		int vpHeight = (int)(GameHeight * scale);
		int vpX = (int)((windowWidth - vpWidth) / 2);
		int vpY = (int)((windowHeight - vpHeight) / 2);
		Viewport = new Rectangle(vpX, vpY, vpWidth, vpHeight);
	}

	public static void AddCameraShake(float strength)
	{
		cameraShake += strength;
	}

	#endregion
}

