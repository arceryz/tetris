using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

class PlayingState: GameState
{
	// Game settings.
	public static float NumPreviewBlocks = 3;
	public static float BlockFallInterval = 0.75f;
	public static float BlockFallIntervalFast = 0.05f;

	public static TetrisGrid TetrisGrid;
	public static FallingTetrisBlock Player1;

	static List<TetrisBlock> previewBlocks = new();

    public PlayingState()
	{
		TetrisGrid = new TetrisGrid();
        TetrisGrid.Position = new Vector2(250, 0);

		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			previewBlocks.Add(TetrisBlock.GetRandom());
		}
		CreateFallingBlock(0);
    }

	public override void Update(float delta)
	{
		Player1.Update(delta);
	}

	public override void Draw(SpriteBatch batch)
	{
        TetrisGrid.Draw(batch);
		Player1.Draw(batch);

		// Draw upcoming blocks.
		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			TetrisBlock block = previewBlocks[i];
			Vector2 pos = new Vector2(660, 150 + i * 15*8);
			block.Draw(batch, pos);
		}
    }

	public static void CreateFallingBlock(int playerIndex)
	{
		if (playerIndex == 0)
		{
			Player1 = new FallingTetrisBlock(0, GetNextBlock());
		}
	}

	public static void GameOver()
	{

	}

	static TetrisBlock GetNextBlock()
	{
		TetrisBlock next = previewBlocks[0];
		previewBlocks.RemoveAt(0);
		previewBlocks.Add(TetrisBlock.GetRandom());
		return next;
	}
}
