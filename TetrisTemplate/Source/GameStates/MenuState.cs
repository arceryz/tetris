using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class MenuState: GameState
{
	SpriteFont textFont;

	public MenuState()
	{
		textFont = TetrisGame.Load<SpriteFont>("SpelFont");
	}

	public override void Update(float delta)
	{
		if (TetrisGame.Input.KeyPressed(Keys.Enter))
		{
			TetrisGame.ChangeState(new PlayingState());
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		batch.DrawString(textFont, "Tetris", Vector2.Zero, Color.Black);
	}
}
