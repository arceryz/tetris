using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class PlayingState: GameState
{
	TetrisGrid grid;
	TetrisBlock block;

	List<TetrisBlock> nextBlocks = new();

    public PlayingState()
	{
		block = new TetrisBlock(TetrisBlockData.GetRandom());
        grid = new TetrisGrid();
        grid.Position = new Vector2(250, 0);

		for (int i = 0; i < 5; i++)
		{
			nextBlocks.Add(new TetrisBlock(TetrisBlockData.GetRandom()));
		}
    }

	public override void Update(float delta)
	{
		if (TetrisGame.Input.KeyPressed(Keys.X))
		{
			block = nextBlocks[0];
			nextBlocks.RemoveAt(0);
			nextBlocks.Add(new TetrisBlock(TetrisBlockData.GetRandom()));
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		block.Position = new Vector2(250, 100);

		grid.Draw(batch);
		block.Draw(batch);

		for (int i = 0; i < nextBlocks.Count; i++)
		{
			TetrisBlock block = nextBlocks[i];
			block.Position = new Vector2(650, 100 + 100 * i);
			block.Draw(batch);
		}
    }
}
