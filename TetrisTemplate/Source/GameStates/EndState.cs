using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

public class EndState: GameState
{
	ImageFont arcadeFont;

	public EndState()
	{
		arcadeFont = TetrisGame.Load<ImageFont>(Assets.Fonts.Arcade);
	}

	public override void Update(float delta)
	{
		if (TetrisGame.Input.KeyPressed(Keys.Enter))
			TetrisGame.ChangeState(new MenuState());
	}

	public override void Draw(SpriteBatch batch)
	{
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, -50), $"your score", Color.White, 4);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0), $"{PlayingState.Score}", Color.Orange, 4);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0.5f), "press enter to return", Color.White, 2, true);
	}
}
