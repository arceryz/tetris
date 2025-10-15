using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public class MenuState: GameState
{
	ImageFont arcadeFont;

	public MenuState()
	{
		arcadeFont = TetrisGame.Load<ImageFont>(Assets.Fonts.Arcade);
		MediaPlayer.Play(TetrisGame.Load<Song>(Assets.Music.Tetris));
	}

	public override void Update(float delta)
	{
		if (TetrisGame.Input.KeyPressed(Keys.Enter))
		{
			TetrisGame.ChangeState(new SettingsState());
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		Color[] colors = { 
			Color.Red, Color.Orange, Color.Yellow, Color.Green, 
			Color.Blue, Color.Purple, Color.Magenta, Color.White 
		};
		int end = 32;
		for (int i = 0; i < end; i++)
		{
			float t = 3 * TetrisGame.ElapsedTime + i * 0.1f;
			Vector2 pos = TetrisGame.GetAnchor(0, 0f);
			pos += new Vector2(0, -MathF.Abs(60.0f * MathF.Sin(t)));
			arcadeFont.DrawString(batch, pos, "tetris", colors[i % colors.Length], 1 + 8 * MathF.Pow(i / (float)end, 2), true);
		}

		if (TetrisGame.ElapsedTime % 0.5f < 0.25f)
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0.5f), "press enter to play", Color.White, 3, true);

		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, 50), "created by nils and timothy", Color.Gray, 2, true);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 1, 0, -30), "press f for fullscreen", Color.Gray, 2, true);
	}
}
