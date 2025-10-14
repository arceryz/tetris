using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

public class PlayingState: GameState
{
	// Game settings.
	public static float NumPreviewBlocks = 3;
	public static float BlockFallInterval = 0.75f;
	public static float BlockFallIntervalFast = 0.05f;

	public static TetrisGrid TetrisGrid;
	public static FallingTetrisBlock Player1;
	public static int Score { get; private set; } = 0;

	static float scoreFlashTimer = 0;
	static string scoreFlashText = "";
	static List<TetrisBlock> previewBlocks;

	ImageFont arcadeFont;

    public PlayingState()
	{
		TetrisGrid = new TetrisGrid();
		TetrisGrid.Position = TetrisGame.GetAnchor(0, -1, -5 * 30, 0);
		Score = 0;
		arcadeFont = TetrisGame.Load<ImageFont>(Assets.Fonts.Arcade);

		previewBlocks = new();
		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			previewBlocks.Add(TetrisBlock.GetRandom());
		}
		CreateFallingBlock(0);
    }

	public override void Update(float delta)
	{
		Player1.Update(delta);
		if (scoreFlashTimer > 0) scoreFlashTimer -= delta;
	}

	public override void Draw(SpriteBatch batch)
	{
        TetrisGrid.Draw(batch);
		Player1.Draw(batch);

		// Draw upcoming blocks.
		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			TetrisBlock block = previewBlocks[i];
			Vector2 pos = TetrisGame.GetAnchor(0.65f, -0.5f + 0.5f * i);
			block.Draw(batch, pos);
		}

		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, -1, 20, 20), $"{Score:D6}", Color.White, 4, false);
		if (scoreFlashTimer > 0 && scoreFlashTimer % 0.2f < 0.1f)
		{
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, -3, -3), scoreFlashText, Color.Orange, 6, true);
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, 0), scoreFlashText, Color.White, 6, true);
		}

		Color controlColor = new Color(50, 50, 50);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -100), "arrows: move", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -80), "down: faster", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -50), "z : rotate l", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -30), "up: rotate r", controlColor, 2, false);
	}

	public static void GameOver()
	{
		TetrisGame.ChangeState(new EndState());
	}

	public static void CreateFallingBlock(int playerIndex)
	{
		if (playerIndex == 0)
		{
			Player1 = new FallingTetrisBlock(0, GetNextBlock());
		}
	}

	public static void AddScore(int score)
	{
		if (score <= 0) return;

		string[] goodWords = { "good", "nice", "got em", "well done", "try harder", "ok", "not bad", "cool" };
		string[] betterWords = { "sublime", "crazy", "master", "pro", "damn", "wow", "impressive", "genius" };
		string[] ultraWords = { "insanity", "godlike", "perfect", "omg", "lucky", "god", "ultra" };

		string word = goodWords[TetrisGame.Random.Next(goodWords.Length)];
		if (score > 200) word = betterWords[TetrisGame.Random.Next(betterWords.Length)];
		if (score > 600) word = ultraWords[TetrisGame.Random.Next(ultraWords.Length)];

		scoreFlashText = $"{word} {score}";
		scoreFlashTimer = 1;
		Score += score;
	}

	static TetrisBlock GetNextBlock()
	{
		TetrisBlock next = previewBlocks[0];
		previewBlocks.RemoveAt(0);
		previewBlocks.Add(TetrisBlock.GetRandom());
		return next;
	}
}
