using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

public class SettingsState : GameState
{
	ImageFont arcadeFont;

	public SettingsState()
	{
		arcadeFont = TetrisGame.Load<ImageFont>(Assets.Fonts.Arcade);
	}

	public override void Update(float delta)
	{
		if (TetrisGame.Input.KeyPressed(Keys.D1))
		{
			PlayingState.MultiplayerMode = false;
			TetrisGame.ChangeState(new PlayingState());
		}
		if (TetrisGame.Input.KeyPressed(Keys.D2))
		{
			PlayingState.MultiplayerMode = true;
			TetrisGame.ChangeState(new PlayingState());
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, -30), "press 1 for singleplayer", Color.White, 2);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, 30), "press 2 for multiplayer", Color.White, 2);
	}
}
